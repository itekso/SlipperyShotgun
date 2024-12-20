using BepInEx.Configuration;
using SlipperyShotgun;

namespace SlipperyShotgun.Configuration
{
    public static class SlipperyOptions
    {
        public enum LogLevel
        {
            None,
            Info,
            Debug
        }

        public enum SillyExtrasOption
        {
            None,
            Confetti,
            Explosion,
            CarBomb
        }

        public enum SoundEffectOption
        {
            None,
            Bonk,
            Boo,
            Slip
        }

        public static ConfigEntry<int> ShotgunDropChance { get; private set; } = null!;
        public static ConfigEntry<int> RifleDropChance { get; private set; } = null!;
        public static ConfigEntry<int> RevolverDropChance { get; private set; } = null!;
        public static ConfigEntry<LogLevel> LogLevelConfig { get; private set; } = null!;
        public static ConfigEntry<SillyExtrasOption> SillyExtras { get; private set; } = null!;
        public static ConfigEntry<SoundEffectOption> SoundEffect { get; private set; } = null!;

        public static void Initialize(ConfigFile config)
        {
            ShotgunDropChance = config.Bind("General", "ShotgunDropChance", 45, "Percent chance to drop the shotgun (0-100)");
            RifleDropChance = config.Bind("General", "RifleDropChance", 25, "Percent chance to drop the Rifle (0-100)");
            RevolverDropChance = config.Bind("General", "RevolverDropChance", 35, "Percent chance to drop the Revolver (0-100)");
            LogLevelConfig = config.Bind("General", "LogLevel", LogLevel.Info, "Log level (None, Info, Debug)");
            SillyExtras = config.Bind("General", "SillyExtras", SillyExtrasOption.None, "Enable silly extras (None, Confetti, Explosion, CarBomb)");
            SoundEffect = config.Bind("General", "SoundEffect", SoundEffectOption.None, "Sound effect to play (None, Bonk, Boo, Slip)");
        }
    }
}