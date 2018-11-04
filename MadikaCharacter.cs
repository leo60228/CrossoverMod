using Monocle;
using Microsoft.Xna.Framework;

namespace Madika {
    public class MadikaCharacter {
        public MTexture Sprite { get; private set; }
        public MTexture Body { get; private set; }
        public MTexture LeftFoot { get; private set; }
        public MTexture RightFoot { get; private set; }
        public int FootHeight { get; private set; }

        public MadikaCharacter(MTexture sprite, int footHeight = 2) {
            Sprite = sprite;
            FootHeight = footHeight;
            LeftFoot = new MTexture(sprite, new Rectangle(0, sprite.Height - footHeight, sprite.Width / footHeight, footHeight));
            RightFoot = new MTexture(sprite, new Rectangle(sprite.Width / footHeight, sprite.Height - footHeight, sprite.Width / footHeight, footHeight));
            Body = new MTexture(sprite, new Rectangle(0, 0, sprite.Width, sprite.Height - footHeight));
        }
    }
}
