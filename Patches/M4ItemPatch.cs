using HarmonyLib;
using PiggyVarietyMod.Patches;
using UnityEngine;
using Random = SlipperyShotgun.Logic.Random;
using SlipperyShotgun.Logic;

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

            if (Random.ShouldDropItem(SlipperyShotgun.RifleDropChance.Value, __instance.itemProperties.itemId, StartOfRound.Instance.currentLevelID, StartOfRound.Instance.randomMapSeed))
            {
                __instance.isFiring = false;
                __instance.isReloading = false;
                __instance.cantFire = false;
                __instance.isInspecting = false;

                __instance.playerHeldBy.DiscardHeldObject();

                Effects.PlaySillyExtras(__instance.transform.position);
                Effects.PlaySoundEffect(__instance.transform.position);

                if (SlipperyShotgun.LogLevelConfig.Value != LogLevel.None)
                {
                    SlipperyShotgun.Logger.LogInfo("You call yourself a Scavenger?!");
                }
            }

            if (SlipperyShotgun.LogLevelConfig.Value == LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyShotgun.RifleDropChance.Value}");
            }
        }
    }
}