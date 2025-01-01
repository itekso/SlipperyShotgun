using HarmonyLib;
using PiggyVarietyMod.Patches;
using SlipperyShotgun.Logic;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(RevolverItem))]
    public static class RevolverItemPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void PostfixStart(RevolverItem __instance)
        {
            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>() ?? __instance.gameObject.AddComponent<SlipperyBehaviour>();
            slippery.RandomSeed = __instance.itemProperties.itemId + StartOfRound.Instance.currentLevelID + StartOfRound.Instance.randomMapSeed;
        }

        [HarmonyPatch("ShootGunAndSync")]
        [HarmonyPrefix]
        public static bool PrefixShootGunAndSync(RevolverItem __instance, bool heldByPlayer)
        {
            // Ensure the local player is firing the gun
            if (!heldByPlayer) return true;

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyOptions.RevolverDropChance.Value}");
            }
            
            SlipperyBehaviour slippery = __instance.gameObject.GetComponent<SlipperyBehaviour>();
            if (!slippery.ShouldDropItem(SlipperyOptions.RevolverDropChance.Value)) return true;
            
            __instance.isReloading = false;
            __instance.cantFire = false;
            __instance.isCylinderMoving = false;

            __instance.playerHeldBy.StartCoroutine(__instance.playerHeldBy.waitToEndOfFrameToDiscard());

            SlipperyManager.PlaySillyExtrasServerRpc(slippery.gameObject);
            SlipperyManager.PlaySoundEffectServerRpc(slippery.gameObject);

            if (SlipperyOptions.LogLevelConfig.Value != SlipperyOptions.LogLevel.None)
            {
                SlipperyShotgun.Logger.LogInfo("This town ain't big enough for the two of us!");
            }

            return !SlipperyOptions.RevolverDropPreventsFiring.Value;
        }
    }
}