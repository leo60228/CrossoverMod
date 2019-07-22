using Monocle;
using Microsoft.Xna.Framework;
using Celeste;

namespace Madika
{
	public class MadikaCharacter
	{
		public MTexture Sprite { get; set; }
		public MTexture Body { get; private set; }
		public MTexture LeftFoot { get; private set; }
		public MTexture RightFoot { get; private set; }
		public int FootHeight { get; private set; }
		public int RightFootX { get; private set; }

		public bool HasDashTextures { get; private set; }
		public bool HasWalkStillTextures { get; private set; }

		public string SpriteName { get; private set; }

		public bool HasHair { get; private set; }
		public Vector2 HairOffset { get; private set; }

		public MadikaCharacter(string spritename, int footHeight, int footX) : this(spritename, footHeight, footX, false, new Vector2(0, 0))
		{         
		}

		public MadikaCharacter(string spritename, int footHeight, int footX, bool hasHair, Vector2 hairOffset)
		{
			HasDashTextures = GFX.Game.Has(spritename + "_0") && GFX.Game.Has(spritename + "_1") && GFX.Game.Has(spritename + "_2");
			HasWalkStillTextures = GFX.Game.Has(spritename + "_walk") && GFX.Game.Has(spritename + "_still");

			HasHair = hasHair;
			HairOffset = hairOffset;

			Sprite = GFX.Game[spritename];
			SpriteName = spritename;
			FootHeight = footHeight;
			RightFootX = footX;
			LeftFoot = new MTexture(Sprite, new Rectangle(0, Sprite.Height - FootHeight, RightFootX - 1, FootHeight));
			RightFoot = new MTexture(Sprite, new Rectangle(RightFootX, Sprite.Height - FootHeight, Sprite.Width - RightFootX, FootHeight));
			Body = new MTexture(Sprite, new Rectangle(0, 0, Sprite.Width, Sprite.Height - FootHeight));         
		}
	}
}
