using Celeste.Mod;
using System;
using Celeste;
using Monocle;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Madika
{

	public class MadikaModule : EverestModule
	{
		public static MadikaModule Instance;
		public override Type SettingsType => typeof(MadikaModuleSettings);

		public static MadikaModuleSettings Settings => (MadikaModuleSettings)Instance._Settings;

		public static MadikaCharacter Character => Settings.Character;

		public static MadikaCharacter Kris { get; private set; }
		public static MadikaCharacter Ralsei { get; private set; }
		public static MadikaCharacter Monika { get; private set; }
		public static MadikaCharacter MonikaHair { get; private set; }
		public static MadikaCharacter Niko { get; private set; }
		public static MadikaCharacter WorldMachine { get; private set; }
		public static MadikaCharacter PirahnaPlant { get; private set; }
		public static MadikaCharacter O { get; private set; }
		public static MadikaCharacter Kirby { get; private set; }
		public static MadikaCharacter Frosch { get; private set; }

		public static bool OffsetFeet;
		public static float OffsetCounter;

		public static bool AirDuck;
		private bool spriteflashing;

		public float timePassed;

		public MadikaModule()
		{
			Instance = this;
		}
        
		public override void Load()
		{
			On.Celeste.PlayerHair.Render += RenderHair;
			On.Celeste.PlayerSprite.Render += RenderPlayerSprite;
			On.Celeste.Player.Update += UpdatePlayer;
		}

		public override void LoadContent(bool firstLoad)
		{
			Kris = new MadikaCharacter("characters/player/kris", 3, 0);
			Ralsei = new MadikaCharacter("characters/player/ralsei", 2, 0);
			Monika = new MadikaCharacter("characters/player/monika", 3, 11);
			MonikaHair = new MadikaCharacter("characters/player/baldemonika", 3, 11, true, new Vector2(-1, -3));
			Niko = new MadikaCharacter("characters/player/niko", 3, 7);
			WorldMachine = new MadikaCharacter("characters/player/worldmachine", 3, 7);
			PirahnaPlant = new MadikaCharacter("characters/player/pirahnaplant_walk", 2, 3);
			O = new MadikaCharacter("characters/player/o", 0, 0);
			Frosch = new MadikaCharacter("characters/player/frosch", 4, 0);
			Kirby = new MadikaCharacter("characters/player/kirby", 2, 8);
		}

		public override void Unload()
		{
			On.Celeste.PlayerHair.Render -= RenderHair;
			On.Celeste.PlayerSprite.Render -= RenderPlayerSprite;
			On.Celeste.Player.Update -= UpdatePlayer;
		}

		public void UpdatePlayer(On.Celeste.Player.orig_Update orig, Player self) 
		{
			timePassed += Engine.DeltaTime;         
            OffsetCounter += Math.Abs(self.Speed.X) * Engine.DeltaTime;

            if (OffsetCounter > 10)
            {
                OffsetCounter = 0;
                OffsetFeet = !OffsetFeet;
            }

			if (self.Scene.OnInterval(0.05f))
            {
				spriteflashing = !spriteflashing;
            }         

			orig(self);
		}

		public void RenderHair(On.Celeste.PlayerHair.orig_Render orig, PlayerHair self)
		{
			Player player = self.Entity as Player;         

			if (player == null || self.GetSprite().Mode == PlayerSpriteMode.Badeline || Character == null)
			{            
				orig(self);
				return;
			}
			if (Character.HasHair) 
			{
				if (self.Sprite.HasHair)
                {               
                    Vector2 origin = new Vector2(5f, 5f);
                    Color color = self.Border * self.Alpha;

					Vector2 offset = new Vector2(Character.HairOffset.X * (self.GetHairScale(0).X > 0 ? 1 : -1), Character.HairOffset.Y);

                    if (self.DrawPlayerSpriteOutline)
                    {
						Color color2 = self.Sprite.Color;
						Vector2 position = self.Sprite.Position;
						self.Sprite.Color = color;

						self.Sprite.Position = position + new Vector2(0f, -1f) + offset;
						self.Sprite.Render();
						self.Sprite.Position = position + new Vector2(0f, 1f) + offset;
						self.Sprite.Render();
						self.Sprite.Position = position + new Vector2(-1f, 0f) + offset;
						self.Sprite.Render();
						self.Sprite.Position = position + new Vector2(1f, 0f) + offset;
						self.Sprite.Render();
						self.Sprite.Color = color2;
						self.Sprite.Position = position;
                    }
					self.Nodes[0] = self.Nodes[0].Floor();
                    if (color.A > 0)
                    {
						for (int i = 0; i < self.Sprite.HairCount; i++)
                        {
							MTexture hairTexture = self.GetHairTexture(i);
							Vector2 hairScale = self.GetHairScale(i);
							hairTexture.Draw(self.Nodes[i] + new Vector2(-1f, 0f) + offset, origin, color, hairScale);
							hairTexture.Draw(self.Nodes[i] + new Vector2(1f, 0f) + offset, origin, color, hairScale);
							hairTexture.Draw(self.Nodes[i] + new Vector2(0f, -1f) + offset, origin, color, hairScale);
							hairTexture.Draw(self.Nodes[i] + new Vector2(0f, 1f) + offset, origin, color, hairScale);
                        }
                    }
					for (int num = self.Sprite.HairCount - 1; num >= 0; num--)
                    {
						self.GetHairTexture(num).Draw(self.Nodes[num] + offset, origin, self.GetHairColor(num), self.GetHairScale(num));                  
                    }
					if (Character.SpriteName.Contains("baldemonika"))
                    {
						GFX.Game["characters/player/baldemonikaribbon"].Draw(self.Nodes[0] + offset - new Vector2(self.GetHairScale(0).X > 0 ? 1 : -1, 2), origin, Color.White, self.GetHairScale(0));
                    }
                }
			}
		}
        
		public void RenderPlayerSprite(On.Celeste.PlayerSprite.orig_Render orig, PlayerSprite self)
		{
			Player player = self.Entity as Player;

			if (player == null || self.Mode == PlayerSpriteMode.Badeline || Character == null || player.StateMachine.State == 9 || player.StateMachine.State == 5)
			{
                // state 9 is dreamdashing, state 5 is red bubble
				orig(self);
				return;
			}         

			if (Settings.Mode == MadikaModuleChar.Invisible) return;

			AirDuck |= (player.Ducking && !player.OnSafeGround);
            
			AirDuck &= (!player.OnSafeGround && Input.MoveY == 1);

			bool ducking = player.Ducking || AirDuck;

			Color playerColor = player.Stamina < 20 && spriteflashing ? Color.Red : Color.White;

			//int widthFix = (Character.Sprite.Width / 2 + 2) * (player.Facing == Facings.Left ? 1 : -1);

			int dashes = player.Dashes;

			if (Character.HasWalkStillTextures)
			{
				Character.Sprite = GFX.Game[Character.SpriteName + "_" + (player.Speed.X == 0 ? "walk" : "still")];
			}

			if (Character.HasDashTextures) {
				Character.Sprite = GFX.Game[Character.SpriteName + "_" + dashes];
			}
            
			Vector2 renderPosition = new Vector2(self.RenderPosition.X, self.RenderPosition.Y).Floor();
         
			renderPosition.X += (1f - self.Scale.X) * (Character.Sprite.Width/2);
			renderPosition.Y += (1f - self.Scale.Y) * (Character.Sprite.Height/2);

			renderPosition.X -= Character.Sprite.Width/4 + (player.Facing == Facings.Left ? 0 : Character.Sprite.Width / 2);

			if (Character.Sprite == GFX.Game["characters/player/o"])
            {
                renderPosition.Y += (float)Math.Cos(timePassed*1.5);
            }

            

			if (player.Speed.X != 0 && player.OnSafeGround)
			{
				MTexture Body = new MTexture(Character.Sprite, new Rectangle(0, 0, Character.Sprite.Width, Character.Sprite.Height - Character.FootHeight));

				Body.Draw(
				  renderPosition + new Vector2(0, -Character.Sprite.Height),
				  Vector2.Zero, playerColor,
				  self.Scale
				);

				if (player.Facing == Facings.Left)
				{
					Character.LeftFoot.Draw(
					  renderPosition + new Vector2((OffsetFeet ? 0 : 1), -Character.FootHeight),
					  Vector2.Zero, playerColor,
					  self.Scale
					);
					Character.RightFoot.Draw(
					  renderPosition + new Vector2((OffsetFeet ? 1 : 0) - Character.RightFootX, -Character.FootHeight),
					  Vector2.Zero, playerColor,
					  self.Scale
					);
				}
				else
				{
					Character.LeftFoot.Draw(
					  renderPosition + new Vector2(-(OffsetFeet ? 1 : 0), -Character.FootHeight),
					  Vector2.Zero, playerColor,
					  self.Scale
					);
					Character.RightFoot.Draw(
					  renderPosition + new Vector2(-(OffsetFeet ? 0 : 1) + Character.RightFootX, -Character.FootHeight),
					  Vector2.Zero, playerColor,
					  self.Scale
					);
				}
			}
			else
			{            
				Character.Sprite.Draw(
				  renderPosition + new Vector2(0, -Character.Sprite.Height * (ducking ? 0.4f : 1f)),
				  Vector2.Zero, playerColor,
				  new Vector2(self.Scale.X, ducking ? 0.3f : self.Scale.Y)
				);
			}         
		}
	}
}
