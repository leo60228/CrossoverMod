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

        public static bool OffsetFeet = false;
        public static float OffsetCounter = 0f;

        public MadikaModule() {
            Instance = this;
        }

        public override void Load() {
            On.Celeste.PlayerHair.Render += RenderHair;
            On.Celeste.PlayerSprite.Render += RenderPlayerSprite;
        }

        public override void LoadContent(bool firstLoad) {
            Kris = new MadikaCharacter(GFX.Game["characters/player/kris"], 3);
            Ralsei = new MadikaCharacter(GFX.Game["characters/player/ralsei"]);
        }

        public override void Unload() {
            On.Celeste.PlayerHair.Render -= RenderHair;
            On.Celeste.PlayerSprite.Render -= RenderPlayerSprite;
        }

        public static void RenderHair(On.Celeste.PlayerHair.orig_Render orig, PlayerHair self) {
            Player player = self.Entity as Player;

            if (player == null || self.GetSprite().Mode == PlayerSpriteMode.Badeline || Settings.Mode == MadikaModuleChar.Off) {
                orig(self);
                return;
            }
        }

        public static void RenderPlayerSprite(On.Celeste.PlayerSprite.orig_Render orig, PlayerSprite self) {
            Player player = self.Entity as Player;

            if (player == null || self.Mode == PlayerSpriteMode.Badeline || Settings.Mode == MadikaModuleChar.Off) {
                orig(self);
                return;
            }

            if (player.Speed.X != 0 && player.OnSafeGround) {
                Character.Body.Draw(
                  self.RenderPosition.Floor() + new Vector2(player.Facing == Facings.Left ? 6 : -6, -Character.Sprite.Height),
                  Vector2.Zero, Color.White,
                  self.Scale
                );

                if (player.Facing == Facings.Left) {
                    Character.LeftFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(OffsetFeet ? 6 : 7, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                    Character.RightFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2((OffsetFeet ? 7 : 6) - Character.Sprite.Width / Character.FootHeight, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                } else {
                    Character.LeftFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2(OffsetFeet ? -7 : -6, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                    Character.RightFoot.Draw(
                      self.RenderPosition.Floor() + new Vector2((OffsetFeet ? -6 : -7) + Character.Sprite.Width / Character.FootHeight, -Character.FootHeight),
                      Vector2.Zero, Color.White,
                      self.Scale
                    );
                }
            } else {
                Character.Sprite.Draw(
                  self.RenderPosition.Floor() + new Vector2(player.Facing == Facings.Left ? 6 : -6, -Character.Sprite.Height),
                  Vector2.Zero, Color.White,
                  self.Scale
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
