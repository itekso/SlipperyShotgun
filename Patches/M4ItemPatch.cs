using HarmonyLib;
using PiggyVarietyMod.Patches;
using UnityEngine;
using Random = SlipperyShotgun.Logic.Random;
using SlipperyShotgun.Logic;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(M4Item))]
    public static class M4ItemPatch
    {
        [HarmonyPatch("ShootGun")]
        [HarmonyPostfix]
        public static void PostfixShootGun(M4Item __instance, Vector3 gunPosition, Vector3 gunForward)
        {
            if (__instance.playerHeldBy == null || __instance.playerHeldBy.currentlyHeldObjectServer == null) return;

            if (Random.ShouldDropItem(SlipperyOptions.RifleDropChance.Value, __instance.itemProperties.itemId, StartOfRound.Instance.currentLevelID, StartOfRound.Instance.randomMapSeed))
            {
                __instance.isFiring = false;
                __instance.isReloading = false;
                __instance.cantFire = false;
                __instance.isInspecting = false;

                __instance.playerHeldBy.DiscardHeldObject();

                Effects.PlaySillyExtrasServerRpc(__instance.transform.position);
                Effects.PlaySoundEffectServerRpc(__instance.transform.position);

                if (SlipperyOptions.LogLevelConfig.Value != SlipperyOptions.LogLevel.None)
                {
                    SlipperyShotgun.Logger.LogInfo("You call yourself a Scavenger?!");
                }
            }

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyOptions.RifleDropChance.Value}");
            }
        }
    }
}