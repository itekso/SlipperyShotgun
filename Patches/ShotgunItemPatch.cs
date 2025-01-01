using HarmonyLib;
using SlipperyShotgun.Logic;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(ShotgunItem))]
    public static class ShotgunItemPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void PostfixStart(ShotgunItem __instance)
        {
            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>() ?? __instance.gameObject.AddComponent<SlipperyBehaviour>();
            slippery.RandomSeed = __instance.itemProperties.itemId + StartOfRound.Instance.currentLevelID + StartOfRound.Instance.randomMapSeed;
        }

        [HarmonyPatch("ShootGunAndSync")]
        [HarmonyPrefix]
        public static bool PrefixShootGunAndSync(ShotgunItem __instance, bool heldByPlayer)
        {
            // Ensure the local player is firing the gun
            if (!heldByPlayer) return true;

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyOptions.ShotgunDropChance.Value}");
            }

            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>();
            if (!slippery.ShouldDropItem(SlipperyOptions.ShotgunDropChance.Value)) return true;

            __instance.playerHeldBy.StartCoroutine(__instance.playerHeldBy.waitToEndOfFrameToDiscard());

            SlipperyManager.PlaySillyExtrasServerRpc(slippery.gameObject);
            SlipperyManager.PlaySoundEffectServerRpc(slippery.gameObject);

            if (SlipperyOptions.LogLevelConfig.Value != SlipperyOptions.LogLevel.None)
            {
                SlipperyShotgun.Logger.LogInfo("Try putting more stats in to handling!");
            }

            return !SlipperyOptions.ShotgunDropPreventsFiring.Value;
        }
    }
}