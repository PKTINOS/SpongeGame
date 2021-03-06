﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenTKPlatformerExample.ObjectHandler;

namespace OpenTKPlatformerExample
{
    class GameObject
    {
        private Texture2D sprite;
        public Vector2 position;
        public ObjectType type;
        private int spawnTime;
        private int index = 0;
        private Vector2 velocity;
        public RectangleF ColRec
        {
            get
            {
                return new RectangleF(position.X - size.X / 2f, position.Y - size.Y / 2f, size.X - 2, size.Y);
            }
        }
        private Vector2 size;

        public GameObject(Texture2D sprite, Vector2 position, ObjectHandler.ObjectType type, bool left = false)
        {
            this.sprite = sprite;
            this.position = position;
            this.type = type;
            spawnTime = Environment.TickCount;
            if (type == ObjectType.Fireball)
            {
                size = new Vector2(16, 20);
                velocity.X = 10;
                if (left)
                {
                    velocity *= -1;
                }
            }
            else if (type == ObjectType.Coin)
            {
                size = new Vector2(48, 48);
            }
            else if (type == ObjectType.Fireflower)
            {
                size = new Vector2(37, 43);
                this.position.X += 5;
                this.position.Y += size.Y / 1.2f;
                SoundManager.sounds[4].Play();
            }
            else if (type == ObjectType.StandingCoin)
            {
                size = new Vector2(48, 48);
            }
            else if (type == ObjectType.Door)
            {
                size = new Vector2(48, 96);
            }else if (type == ObjectType.Logo)
            {
                size = new Vector2(426, 188)/1.5f;
            }else if (type == ObjectType.Cursor)
            {
                size = new Vector2(24,24);
            }else if (type == ObjectType.Play)
            {
                size = new Vector2(267, 53) / 2f;

            }else if (type == ObjectType.Options)
            {
                size = new Vector2(427,53)/2f;
            }else if (type == ObjectType.SadSponge)
            {
                size = new Vector2(54, 92)/1.5f;
            }else if (type == ObjectType.TheBones)
            {
                size = new Vector2(-113, 27);
            }else if (type == ObjectType.RedBrother)
            {
                size = new Vector2(54, 92);
            }else if (type == ObjectType.Shadow)
            {
                size = new Vector2(640, 480);
            }
        }
        public void Draw(Color color)
        {

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            if (type == ObjectType.StandingCoin || type == ObjectType.Coin)
            {
                if (index < 6)
                    Spritebatch.DrawSprite(sprite, new RectangleF(position.X, position.Y, size.X, size.Y), color);
                else
                    Spritebatch.DrawSprite(sprite, new RectangleF(position.X, position.Y, size.X, size.Y), Color.FromArgb(color.A,color.R-50,color.G-50,color.B-50));
                return;
            }
            Spritebatch.DrawSprite(sprite, new RectangleF(position.X, position.Y, size.X, size.Y), color);

        }
        public void Update(Player player = null)
        {
            try
            {
                if (player.dying)
                    return;
            }
            catch { }
            if (type != ObjectType.Cursor)
            index++;
            if (index >= 12)
            {
                index = 0;
            }
            if (type == ObjectType.Coin)
            {
                if (Environment.TickCount >= spawnTime + 500)
                {
                    objects.Remove(this);
                }
                position.Y -= 2;

            }
            else if (type == ObjectType.Fireball)
            {
                if (Environment.TickCount >= spawnTime + 2000)
                {
                    objects.Remove(this);
                    fireballsonscreen -= 1;
                }
                this.velocity += new Vector2(0, 1);
                ResolveCollisions(ref Game.level);
                position += velocity;
            }
            else if (type == ObjectType.Fireflower)
            {
                if (spawnTime + 500 > Environment.TickCount)
                {
                    position.Y -= 2;
                }
                else
                {
                    float cx, cy;
                    cx = position.X + size.X / 2;
                    cy = position.Y + size.Y / 2;
                    if (Math.Sqrt(Math.Pow(player.position.X - cx, 2) + (Math.Pow(player.position.Y - cy, 2))) < size.X)
                    {
                        SoundManager.sounds[3].Play();
                        objects.Remove(this);
                        Player.fireflower = true;
                        player.isInvincible = true;
                        player.invincibletime = Environment.TickCount - 200;
                    }
                }
            }
            else if (type == ObjectType.StandingCoin)
            {
                float cx, cy;
                cx = position.X + size.X / 2;
                cy = position.Y + size.Y / 2;
                if (Math.Sqrt(Math.Pow(player.position.X - cx, 2) + (Math.Pow(player.position.Y - cy, 2))) <54)
                {
                    SoundManager.sounds[2].Play();
                    objects.Remove(this);
                    View.Coins++;
                }
            }
            else if (type == ObjectType.Door)
            {
                float cx, cy;
                cx = position.X + size.X / 2;
                cy = position.Y + size.Y / 2;
                if (Math.Sqrt(Math.Pow(player.position.X - cx, 2) + (Math.Pow(player.position.Y - cy, 2))) < size.X)
                {
                    if (Input.KeyDown(OpenTK.Input.Key.Up) || Input.up)
                    {
                        ObjectHandler.fireballsonscreen = 0;

                        if (Game.Level == 1)
                        {
                            Game.intro = new Message[]
                            {
                                new Message("Who the fuck is   calling me?       I'm busy.",Color.DarkRed),
                                new Message("*click*",Color.Gray),
                                new Message("Hello?",Color.DarkRed),
                                new Message("Oh its'a you boss.How are you-a     doing?",Color.Red),
                                new Message("Yes the new world will be done soon!",Color.Red),
                                new Message("No no, don't you-aworry about it    boss.It will be   the happiest worldever.",Color.Red),
                                new Message("*click*",Color.Gray),
                                new Message("If this degenerateof a brother      doesn't make it intime,I will make  sure he regrets   this.",Color.DarkRed),
                                new Message("I wonder whose    bright idea it wasto introduce a newbrother. Let aloneANY brother.", Color.DarkRed), //check this
                            };

                            Game.slevel = "afterstory1";
                            Game.Level = 2;
                            Game.introind = 0;
                            Game.LoadLevel("Desert.xml");
                            DialogBox.cd = Game.intro[0].color;
                            DialogBox.LoadString(Game.intro[0].msg);
                        }
                        else if (Game.Level == 2)
                        {
                            Game.intro = new Message[]
                        {
                              //fix dialog
                              new Message("Sponge has not    been good lately.",Color.DarkGreen),
                              new Message("I think replacing his princess with a fake skeleton atthe end of his    first adventure...",Color.DarkGreen),
                              new Message("was a pretty bad  idea for a prank, and-",Color.DarkGreen),
                              new Message("I don't want to   hear it.",Color.DarkRed),
                              new Message("You always take   things too        seriously.",Color.DarkRed),
                              new Message("No wonder you're  always the one    noone picks in my games.",Color.DarkRed),
                              new Message("You better pray   that your emo     brother has       already passed theghost house.",Color.DarkRed),
                              new Message("For your own good",Color.DarkRed),
                        };
                            Game.slevel = "afterstory2";
                            Game.Level = 3;
                            Game.introind = 0;
                            Game.LoadLevel("GhostHouse.xml");
                            DialogBox.cd = Game.intro[0].color;
                            DialogBox.LoadString(Game.intro[0].msg);
                        }
                        else if (Game.Level == 3)
                        {
                            Game.Level = 4;
                            Game.LoadLevel("GhostEventRoom.xml");
                        }
                        else if (Game.Level == 4) 
                        {
                            Game.intro = new Message[]
                              {
                              //fix dialog
                              new Message("He is not here    yet...",Color.DarkRed),
                              new Message("I guess it was my fault for trustinga retard like him.",Color.DarkRed),
                              new Message("Should've known   from the fact he  believes we       actually run      around and save   princesses.",Color.DarkRed),
                              new Message("Ignorant just likehis other brother.",Color.DarkRed),
                              new Message("At least I can useHIM to make money.",Color.DarkRed),
                              new Message("The other one was just a useless    space consumer.",Color.DarkRed),

                             };
                            Game.slevel = "afterstory3";
                            Game.Level = 5;
                            Game.introind = 0;
                            Game.LoadLevel("Castle.xml");
                            DialogBox.cd = Game.intro[0].color;
                            DialogBox.LoadString(Game.intro[0].msg);

                        }else if (Game.Level == 5) 
                        {
                            Game.intro = new Message[]
                              {
                              new Message("I am done..       Now please...     Give them ba-",Color.DarkOrange),
                              new Message("You are late.",Color.DarkRed),
                              new Message("I threw your candyin the lava along with your brother.",Color.DarkRed),
                              new Message("What is it Sponge?Are you going to  jump in too?",Color.DarkRed),
                              new Message("Go ahead. I've    already made      enough money of ofyou to live for   years to come.",Color.DarkRed),
                              new Message("Noone even knows  you exist. I take all the credit andmoney for your    work.",Color.FromArgb(255,80,0,0)),
                              new Message("You are worthless just like your br-",Color.DarkRed),
                              new Message("Huh.              He actually did it",Color.DarkRed),
                              new Message("Whatever. This    world isn't for   the weak.",Color.DarkRed),
                              new Message("I'll just use the other brother     Pretzel just like I used Sponge.",Color.DarkRed),

                             };
                            Game.slevel = "afterstory4";
                            Game.introind = 0;
                            Game.Level = 6;
                            Game.LoadLevel("Epilogue.xml");
                            DialogBox.cd = Game.intro[0].color;
                            DialogBox.LoadString(Game.intro[0].msg);
                        }

                    }
                }
            } else if (type == ObjectType.Cursor)
            {
                
                if ((Input.KeyDown(OpenTK.Input.Key.Up) || Input.up) && index == 1)
                {
                    index = 0;
                    position.Y -= 48;
                }
                //No "else if" because now if you hold both buttons the cursor wont move.
                if ((Input.KeyDown(OpenTK.Input.Key.Down) || Input.down) && index == 0)
                {
                    index = 1;
                    position.Y += 48;
                }
                if (Input.KeyPress(OpenTK.Input.Key.Z) || Input.A)
                {
                    if (index == 0)
                    {
                        Game.music.Stop();
                        Game.slevel = "introstory";
                        Game.Reload();
                    }else
                    {

                        new Settings().ShowDialog();
                    }
                }   
                        
           }
        }
        private void ResolveCollisions(ref Level level)
        {
            int minX = (int)Math.Floor((this.position.X - this.size.X / 2f) / Game.GRIDSIZE);
            int minY = (int)Math.Floor((this.position.Y - this.size.Y / 2f) / Game.GRIDSIZE);
            int maxX = (int)Math.Ceiling((this.position.X + this.size.X / 2f) / Game.GRIDSIZE);
            int maxY = (int)Math.Ceiling((this.position.Y + this.size.Y / 2f) / Game.GRIDSIZE);



            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    RectangleF blockRec = new RectangleF(x * Game.GRIDSIZE, y * Game.GRIDSIZE, Game.GRIDSIZE, Game.GRIDSIZE);

                    if ((level[x, y].IsSolid || level[x, y].IsPlatform) && this.ColRec.IntersectsWith(blockRec))
                    {

                        #region Resolve
                        if (this.position.Y < level[x, y].posY * Game.GRIDSIZE + 8)
                            velocity.Y = -8;
                        else
                        { objects.Remove(this); fireballsonscreen -= 1; }
                        float[] depths = new float[4]
                        {
                            blockRec.Right - ColRec.Left, //PosX
							blockRec.Bottom - ColRec.Top, //PosY
							ColRec.Right - blockRec.Left, //NegX
							ColRec.Bottom - blockRec.Top  //NegY
						};

                        if (level[x + 1, y].IsSolid)
                            depths[0] = -1;
                        if (level[x, y + 1].IsSolid || level[x, y].IsPlatform)
                            depths[1] = -1;
                        if (level[x - 1, y].IsSolid)
                            depths[2] = -1;
                        if (level[x, y - 1].IsSolid)
                            depths[3] = -1;

                        float min = float.MaxValue;
                        Vector2 minDirection = Vector2.Zero;
                        for (int i = 0; i < 4; i++)
                        {
                            if (depths[i] >= 0 && depths[i] < min)
                            {
                                min = depths[i];
                                switch (i)
                                {
                                    case 0:
                                        minDirection = new Vector2(1, 0);
                                        break;
                                    case 1:
                                        minDirection = new Vector2(0, 1);
                                        break;
                                    case 2:
                                        minDirection = new Vector2(-1, 0);
                                        break;
                                    default:
                                        minDirection = new Vector2(0, -1);
                                        break;
                                }
                            }
                        }

                        if (minDirection == Vector2.Zero)
                        {
                            //Console.WriteLine("This shouldn't really happen...");
                        }
                        else
                        {
                            this.position += minDirection * min;
                            if (this.velocity.X * minDirection.X < 0)
                                this.velocity.X = 0;
                            if (this.velocity.Y * minDirection.Y < 0)
                                this.velocity.Y = 0;
                        }

                        #endregion
                    }
                    if (this.velocity.Y > 0 && !Input.KeyDown(OpenTK.Input.Key.Down) && level[x, y].IsPlatform && this.ColRec.IntersectsWith(blockRec))
                    {
                        if (this.position.Y - this.velocity.Y + this.size.Y / 2f <= blockRec.Top) //if we were above last frame
                        {
                            this.velocity.Y = 0;
                            this.position.Y = blockRec.Top - this.size.Y / 2f;
                        }
                    }
                }
            }
        }
    }
}
