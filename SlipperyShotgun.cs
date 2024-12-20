using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using SlipperyShotgun.Configuration;
using SlipperyShotgun.Patches;
using UnityEngine;

namespace SlipperyShotgun
{
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

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            SlipperyOptions.Initialize(Config);

            PatchShotgun();

            if (SlipperyOptions.LogLevelConfig?.Value != SlipperyOptions.LogLevel.None)
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

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                Logger.LogDebug("Patching ShotgunItem...");
            }

            Harmony.PatchAll(typeof(ShotgunItemPatch));

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                Logger.LogDebug("Finished patching ShotgunItem!");
            }
        }

        internal static void Unpatch()
        {
            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                Logger.LogDebug("Unpatching...");
            }

            Harmony?.UnpatchSelf();

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
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