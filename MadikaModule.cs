using Celeste.Mod;
using System;
using System.Reflection;
using Celeste;

namespace Madika
{
    public class MadikaModule : EverestModule
    {
        public static MadikaModule Instance;

        public override Type SettingsType => null;

        public MadikaModule() {
            Instance = this;
        }

        public override void Load() {
            On.Celeste.PlayerHair.Render += RenderHair;
        }

        public override void Unload() {
            On.Celeste.PlayerHair.Render -= RenderHair;
        }

        public static void RenderHair(On.Celeste.PlayerHair.orig_Render orig, PlayerHair self) {
          return;
        }
    }
}
