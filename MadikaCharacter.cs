using Monocle;
using System;
using Microsoft.Xna.Framework;

namespace Madika {
    public class MadikaCharacter {
        public MTexture Sprite { get; private set; }
        public MTexture Body { get; private set; }
        public MTexture LeftFoot { get; private set; }
        public MTexture RightFoot { get; private set; }
        public int FootHeight { get; private set; }
        public int LeftFootX { get; private set; }
        public int RightFootX => (int) Math.Ceiling(LeftFootX + (Sprite.Width - LeftFootX) / 2f);

        public MadikaCharacter(MTexture sprite, int footHeight, int leftFoot) {
            Sprite = sprite;
            FootHeight = footHeight;
            LeftFootX = leftFoot;
            LeftFoot = new MTexture(sprite, new Rectangle(0, sprite.Height - footHeight, RightFootX - 1, footHeight));
            RightFoot = new MTexture(sprite, new Rectangle(RightFootX, sprite.Height - footHeight, sprite.Width - RightFootX, footHeight));
            Body = new MTexture(sprite, new Rectangle(0, 0, sprite.Width, sprite.Height - footHeight));
        }
    }
}
