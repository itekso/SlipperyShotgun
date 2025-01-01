using HarmonyLib;
using PiggyVarietyMod.Patches;
using SlipperyShotgun.Logic;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(M4Item))]
    public static class M4ItemPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void PostfixStart(M4Item __instance)
        {
            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>() ?? __instance.gameObject.AddComponent<SlipperyBehaviour>();
            slippery.RandomSeed = __instance.itemProperties.itemId + StartOfRound.Instance.currentLevelID + StartOfRound.Instance.randomMapSeed;
        }

        [HarmonyPatch("ShootGunAndSync")]
        [HarmonyPrefix]
        public static bool PrefixShootGunAndSync(M4Item __instance, bool heldByPlayer)
        {
            // Ensure the local player is firing the gun
            if (!heldByPlayer) return true;

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyOptions.RifleDropChance.Value}");
            }
            
            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>();
            if (!slippery.ShouldDropItem(SlipperyOptions.RifleDropChance.Value)) return true;

            __instance.isFiring = false;
            __instance.isReloading = false;
            __instance.cantFire = false;
            __instance.isInspecting = false;

            __instance.playerHeldBy.StartCoroutine(__instance.playerHeldBy.waitToEndOfFrameToDiscard());

            SlipperyManager.PlaySillyExtrasServerRpc(slippery.gameObject);
            SlipperyManager.PlaySoundEffectServerRpc(slippery.gameObject);

            if (SlipperyOptions.LogLevelConfig.Value != SlipperyOptions.LogLevel.None)
            {
                SlipperyShotgun.Logger.LogInfo("You call yourself a Scavenger?!");
            }

            return !SlipperyOptions.RifleDropPreventsFiring.Value;
        }
    }
}