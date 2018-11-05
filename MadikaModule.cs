using Celeste.Mod;
using System;
using Celeste;
using Monocle;
using Microsoft.Xna.Framework;

namespace Madika {

    public class MadikaModule : EverestModule {
        public static MadikaModule Instance;
        public override Type SettingsType => typeof(MadikaModuleSettings);

        public static MadikaModuleSettings Settings => (MadikaModuleSettings)Instance._Settings;

        public static MadikaCharacter Character => Settings.Character;

        public static MadikaCharacter Kris { get; private set; }
        public static MadikaCharacter Ralsei { get; private set; }
        public static MadikaCharacter Monika { get; private set; }
        public static MadikaCharacter Niko { get; private set; }
        public static MadikaCharacter WorldMachine { get; private set; }

        public static bool OffsetFeet = false;
        public static float OffsetCounter = 0f;

        public static bool AirDuck = false;

        public MadikaModule() {
            Instance = this;
        }

        public override void Load() {
            On.Celeste.PlayerHair.Render += RenderHair;
            On.Celeste.PlayerSprite.Render += RenderPlayerSprite;
        }

        public override void LoadContent(bool firstLoad) {
            Kris = new MadikaCharacter(GFX.Game["characters/player/kris"], 3, 0);
            Ralsei = new MadikaCharacter(GFX.Game["characters/player/ralsei"], 2, 0);
            Monika = new MadikaCharacter(GFX.Game["characters/player/monika"], 3, 11);
            Niko = new MadikaCharacter(GFX.Game["characters/player/niko"], 3, 7);
            WorldMachine = new MadikaCharacter(GFX.Game["characters/player/worldmachine"], 3, 7);
        }

        public override void Unload() {
            On.Celeste.PlayerHair.Render -= RenderHair;
            On.Celeste.PlayerSprite.Render -= RenderPlayerSprite;
        }

        public static void RenderHair(On.Celeste.PlayerHair.orig_Render orig, PlayerHair self) {
            Player player = self.Entity as Player;

            if (player == null || self.GetSprite().Mode == PlayerSpriteMode.Badeline || Character == null) {
                orig(self);
                return;
            }
        }

        public static void RenderPlayerSprite(On.Celeste.PlayerSprite.orig_Render orig, PlayerSprite self) {
            Player player = self.Entity as Player;

            if (player == null || self.Mode == PlayerSpriteMode.Badeline || Character == null) {
                orig(self);
                return;
            }

            if (Settings.Mode == MadikaModuleChar.Invisible) return;

            if (player.Ducking && !player.OnSafeGround) {
                AirDuck = true;
            }

            if (player.OnSafeGround || Input.MoveY != 1) AirDuck = false;

            bool ducking = player.Ducking || AirDuck;

            int widthFix = (Character.Sprite.Width / 2 + 2) * (player.Facing == Facings.Left ? 1 : -1);

            if (player.Speed.X != 0 && player.OnSafeGround) {
                Character.Body.Draw(
                  self.RenderPosition.Floor() + new Vector2(widthFix, -Character.Sprite.Height),
                  Vector2.Zero, Color.White,
                  self.Scale
                );

                if (player.Facing == Facings.Left) {
                    Character.LeftFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(widthFix + (OffsetFeet ? 0 : 1), -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                    Character.RightFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(widthFix + (OffsetFeet ? 1 : 0) - Character.RightFootX, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                } else {
                    Character.LeftFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(widthFix - (OffsetFeet ? 1 : 0), -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                    Character.RightFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(widthFix - (OffsetFeet ? 0 : 1) + Character.RightFootX, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                }
            } else {
                Character.Sprite.Draw(
                  self.RenderPosition.Floor() + new Vector2(widthFix, -Character.Sprite.Height * (ducking ? 0.3f : 1f)),
                  Vector2.Zero, Color.White,
                  new Vector2(self.Scale.X, ducking ? 0.3f : self.Scale.Y)
                );
            }

            OffsetCounter += Math.Abs(player.Speed.X) * Engine.DeltaTime;
            if (OffsetCounter > 10) {
                OffsetCounter = 0;
                OffsetFeet = !OffsetFeet;
            }
        }
    }
}
