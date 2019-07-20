﻿using Celeste.Mod;
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

		private List<Vector2> originalNodes;

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
			Monika = new MadikaCharacter("characters/player/monika", 3, 11, true, new Vector2(0, -10));
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
				if (originalNodes.Count != 0)
                {
					self.Nodes = originalNodes;
                }

				orig(self);
				return;
			}
			if (Character.HasHair) {
				if (originalNodes.Count == 0) {
					originalNodes = self.Nodes;
				}

				int i = 0;

				self.Nodes.ForEach((Vector2 node) =>
				{
					self.Nodes[i] += Character.HairOffset;

					i++;
				});

				orig(self);
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
