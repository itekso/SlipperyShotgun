using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;

namespace SlipperyShotgun.Configuration
{
    public class SlipperyOptions : SyncedConfig2<SlipperyOptions>
    {
        public static SlipperyOptions Instance { get; private set; } = null!;
        public static void Initialize(ConfigFile config)
        {
            Instance ??= new(config);
        }

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

        // --- Synced Entries --- \\
        [field: SyncedEntryField]
        public static SyncedEntry<int> ShotgunDropChance { get; private set; } = null!;

        [field: SyncedEntryField]
        public static SyncedEntry<int> RifleDropChance { get; private set; } = null!;

        [field: SyncedEntryField]
        public static SyncedEntry<int> RevolverDropChance { get; private set; } = null!;

        [field: SyncedEntryField]
        public static SyncedEntry<bool> ShotgunDropPreventsFiring { get; private set; } = null!;

        [field: SyncedEntryField]
        public static SyncedEntry<bool> RifleDropPreventsFiring { get; private set; } = null!;
        
        [field: SyncedEntryField]
        public static SyncedEntry<bool> RevolverDropPreventsFiring { get; private set; } = null!;

        // --- Unsynced Entries --- \\
        public static ConfigEntry<SillyExtrasOption> SillyExtras { get; private set; } = null!; // Host setting enforced by SlipperyManager RPC
        public static ConfigEntry<SoundEffectOption> SoundEffect { get; private set; } = null!; // Host setting enforced by SlipperyManager RPC
        public static ConfigEntry<LogLevel> LogLevelConfig { get; private set; } = null!; // No need to sync this

        private SlipperyOptions(ConfigFile config) : base(SlipperyShotgun.modGUID)
        {
            // --- Synced Entries --- \\
            ShotgunDropChance = config.BindSyncedEntry("General", "ShotgunDropChance", 45, "Percent chance to drop the shotgun (0-100)");
            RifleDropChance = config.BindSyncedEntry("General", "RifleDropChance", 25, "Percent chance to drop the Rifle (0-100)");
            RevolverDropChance = config.BindSyncedEntry("General", "RevolverDropChance", 35, "Percent chance to drop the Revolver (0-100)");
            ShotgunDropPreventsFiring = config.BindSyncedEntry("General", "ShotgunDropPreventsFiring", false, "Whether dropping the shotgun should prevent it from firing");
            RifleDropPreventsFiring = config.BindSyncedEntry("General", "RifleDropPreventsFiring", false, "Whether dropping the Rifle should prevent it from firing");
            RevolverDropPreventsFiring = config.BindSyncedEntry("General", "RevolverDropPreventsFiring", false, "Whether dropping the Revolver should prevent it from firing");

            // --- Unsynced Entries --- \\
            SillyExtras = config.Bind("General", "SillyExtras", SillyExtrasOption.None, "Enable silly extras (None, Confetti, Explosion, CarBomb)");
            SoundEffect = config.Bind("General", "SoundEffect", SoundEffectOption.None, "Sound effect to play (None, Bonk, Boo, Slip)");
            LogLevelConfig = config.Bind("General", "LogLevel", LogLevel.Info, "Log level (None, Info, Debug)");

            ConfigManager.Register(this);
        }
    }
}