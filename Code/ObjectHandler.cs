﻿using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKPlatformerExample
{
    class ObjectHandler
    {
        public static List<Sprite> definedSprites = new List<Sprite>();
        public static int fireballsonscreen;
        public static List<GameObject> objects = new List<GameObject>();
        public static void Load()
        {
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("coinGold.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("fireball.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("fireflower.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("coinGold.png"))); //Load it twice to avoid adding more code - Lazyness at its finest! :)
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("door.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("logo.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("cursor.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("play.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("options.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("spongesad.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("thebones.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("mysterious_man.png")));
            definedSprites.Add(new Sprite(ContentPipe.LoadTexture("shadow.png")));

        }
        public static void AddObject(ObjectType type, Vector2 position,bool left = false)
        {
            if (type == ObjectType.Fireball)
            {
                if (fireballsonscreen >= 2)
                {
                    return;
                }else
                {
                    fireballsonscreen++;
                    SoundManager.sounds[1].Play();
                }
            }
            GameObject o = new GameObject(definedSprites.ElementAt((int)type).texture, position, type,left);
            objects.Add(o);
        }
        public enum ObjectType
        {
            Coin = 0,
            Fireball = 1,
            Fireflower = 2,
            StandingCoin = 3,
            Door = 4,
            Logo = 5,
            Cursor = 6,
            Play = 7,
            Options = 8,
            SadSponge = 9,
            TheBones = 10,
            RedBrother = 11,
            Shadow = 12,
        }
    }
}
