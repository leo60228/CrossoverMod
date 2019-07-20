using Celeste.Mod;
using YamlDotNet.Serialization;

namespace Madika
{
	public class MadikaModuleSettings : EverestModuleSettings
	{
		public MadikaModuleChar Mode { get; set; } = MadikaModuleChar.Off;

		[SettingIgnore]
		[YamlIgnore]
		public MadikaCharacter Character
		{
			get
			{
				switch (Mode)
				{
					case MadikaModuleChar.Kris:
					case MadikaModuleChar.Invisible:
						return MadikaModule.Kris;
					case MadikaModuleChar.Ralsei:
						return MadikaModule.Ralsei;
					case MadikaModuleChar.Monika:
						return MadikaModule.Monika;
					case MadikaModuleChar.MonikaHair:
						return MadikaModule.MonikaHair;
					case MadikaModuleChar.Niko:
						return MadikaModule.Niko;
					case MadikaModuleChar.WorldMachine:
						return MadikaModule.WorldMachine;
					case MadikaModuleChar.PirahnaPlant:
						return MadikaModule.PirahnaPlant;
					case MadikaModuleChar.O:
						return MadikaModule.O;
					case MadikaModuleChar.Kirby:
						return MadikaModule.Kirby;
					case MadikaModuleChar.Frosch:
						return MadikaModule.Frosch;
				}

				return null;
			}
		}
	}

	public enum MadikaModuleChar
	{
		Off,
		Kris,
		Ralsei,
		Monika,
        MonikaHair,
		Niko,
		WorldMachine,
        PirahnaPlant,
        O,
        Kirby,
        Frosch,
		Invisible
	}
}
