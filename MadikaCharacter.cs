using Monocle;
using Microsoft.Xna.Framework;

namespace Madika
{
	public class MadikaCharacter
	{
		public MTexture Sprite { get; private set; }
		public MTexture Body { get; private set; }
		public MTexture LeftFoot { get; private set; }
		public MTexture RightFoot { get; private set; }
		public int FootHeight { get; private set; }
		public int RightFootX { get; private set; }

		public MadikaCharacter(MTexture sprite, int footHeight, int footX)
		{
			Sprite = sprite;
			FootHeight = footHeight;
			RightFootX = footX;
			LeftFoot = new MTexture(Sprite, new Rectangle(0, Sprite.Height - FootHeight, RightFootX - 1, FootHeight));
			RightFoot = new MTexture(Sprite, new Rectangle(RightFootX, Sprite.Height - FootHeight, Sprite.Width - RightFootX, FootHeight));
			Body = new MTexture(Sprite, new Rectangle(0, 0, Sprite.Width, Sprite.Height - FootHeight));
		}
	}
}
