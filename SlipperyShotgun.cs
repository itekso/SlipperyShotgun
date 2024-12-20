using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SlipperyShotgun.Patches;
using UnityEngine;

namespace SlipperyShotgun
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

    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("Piggy.PiggyVarietyMod", BepInDependency.DependencyFlags.SoftDependency)]
    public class SlipperyShotgun : BaseUnityPlugin
    {
        private const string modGUID = "com.itekso.SlipperyShotgun";
        private const string modName = "SlipperyShotgun";
        private const string modVersion = "1.2.0";

        public static SlipperyShotgun Instance { get; private set; } = null!;
        public new static ManualLogSource Logger { get; private set; } = null!;
        private static Harmony? Harmony { get; set; }

        public static ConfigEntry<int> ShotgunDropChance { get; private set; } = null!;
        public static ConfigEntry<int> RifleDropChance { get; private set; } = null!;
        public static ConfigEntry<int> RevolverDropChance { get; private set; } = null!;
        public static ConfigEntry<LogLevel> LogLevelConfig { get; private set; } = null!;
        public static ConfigEntry<SillyExtrasOption> SillyExtras { get; private set; } = null!;
        public static ConfigEntry<SoundEffectOption> SoundEffect { get; private set; } = null!;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            ShotgunDropChance = Config.Bind("General", "ShotgunDropChance", 45, "Percent chance to drop the shotgun (0-100)");
            RifleDropChance = Config.Bind("General", "RifleDropChance", 25, "Percent chance to drop the Rifle (0-100)");
            RevolverDropChance = Config.Bind("General", "RevolverDropChance", 35, "Percent chance to drop the Revolver (0-100)");
            LogLevelConfig = Config.Bind("General", "LogLevel", LogLevel.Info, "Log level (None, Info, Debug)");
            SillyExtras = Config.Bind("General", "SillyExtras", SillyExtrasOption.None, "Enable silly extras (None, Confetti, Explosion, CarBomb)");
            SoundEffect = Config.Bind("General", "SoundEffect", SoundEffectOption.None, "Sound effect to play (None, Bonk, Boo, Slip)");

            PatchShotgun();

            if (LogLevelConfig?.Value != LogLevel.None)
            {
                Logger.LogInfo($"{modName} v{modVersion} has loaded!");
            }

            if (Chainloader.PluginInfos?.Keys.Any(k => k == "Piggy.PiggyVarietyMod") == true)
            {
                Logger.LogInfo("Piggy's Variety Mod found. Applying patches for RevolverItem and M4Item.");
                Harmony?.PatchAll(typeof(RevolverItemPatch));
                Harmony?.PatchAll(typeof(M4ItemPatch));
            }
            else
            {
                Logger.LogInfo("Piggy's Variety Mod not found. RevolverItem and M4Item patches skipped.");
            }
        }

        private static void PatchShotgun()
        {
            Harmony ??= new Harmony(modGUID);

            if (LogLevelConfig.Value == LogLevel.Debug)
            {
                Logger.LogDebug("Patching ShotgunItem...");
            }

            Harmony.PatchAll(typeof(ShotgunItemPatch));

            if (LogLevelConfig.Value == LogLevel.Debug)
            {
                Logger.LogDebug("Finished patching ShotgunItem!");
            }
        }

        internal static void Unpatch()
        {
            if (LogLevelConfig.Value == LogLevel.Debug)
            {
                Logger.LogDebug("Unpatching...");
            }

            Harmony?.UnpatchSelf();

            if (LogLevelConfig.Value == LogLevel.Debug)
            {
                Logger.LogDebug("Finished unpatching!");
            }
        }

        public static GameObject FindPrefabByName(string prefabName)
        {
            var prefab = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == prefabName);
            if (prefab == null)
            {
                Logger.LogError($"Prefab '{prefabName}' not found.");
            }
            return prefab!;
        }

        public static AudioClip LoadSoundEffect(string soundName)
        {
            var bundle = AssetBundleManager.LoadEmbeddedAssetBundle();
            if (bundle == null)
            {
                Logger.LogError("Failed to load asset bundle.");
                return null!;
            }

            var clip = bundle.LoadAsset<AudioClip>(soundName);
            if (clip == null)
            {
                Logger.LogError($"Failed to load sound effect: {soundName}");
            }

            return clip!;
        }
    }
}