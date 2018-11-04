using Celeste.Mod;
using YamlDotNet.Serialization;

namespace Madika {
    public class MadikaModuleSettings : EverestModuleSettings {
        public MadikaModuleChar Mode { get; set; } = MadikaModuleChar.Off;

        [SettingIgnore]
        [YamlIgnore]
        public MadikaCharacter Character {
            get {
                if (Mode == MadikaModuleChar.Kris || Mode == MadikaModuleChar.Invisible) {
                    return MadikaModule.Kris;
                } else if (Mode == MadikaModuleChar.Ralsei) {
                    return MadikaModule.Ralsei;
                } else if (Mode == MadikaModuleChar.Monika) {
                    return MadikaModule.Monika;
                }

                return null;
            }
        }
    }

    public enum MadikaModuleChar {
        Off,
        Kris,
        Ralsei,
        Monika,
        Invisible
    }
}
