using HarmonyLib;
using PiggyVarietyMod.Patches;
using UnityEngine;
using Random = SlipperyShotgun.Logic.Random;
using SlipperyShotgun.Logic;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(RevolverItem))]
    public static class RevolverItemPatch
    {
        [HarmonyPatch("ShootGun")]
        [HarmonyPostfix]
        public static void PostfixShootGun(RevolverItem __instance, Vector3 revolverPosition, Vector3 revolverForward)
        {
            if (__instance.playerHeldBy == null || __instance.playerHeldBy.currentlyHeldObjectServer == null) return;

            if (Random.ShouldDropItem(SlipperyShotgun.RevolverDropChance.Value, __instance.itemProperties.itemId, StartOfRound.Instance.currentLevelID, StartOfRound.Instance.randomMapSeed))
            {
                __instance.isReloading = false;
                __instance.cantFire = false;
                __instance.isCylinderMoving = false;

                __instance.playerHeldBy.DiscardHeldObject();

                Effects.PlaySillyExtras(__instance.transform.position);
                Effects.PlaySoundEffect(__instance.transform.position);

                if (SlipperyShotgun.LogLevelConfig.Value != LogLevel.None)
                {
                    SlipperyShotgun.Logger.LogInfo("This town ain't big enough for the two of us!");
                }
            }

            if (SlipperyShotgun.LogLevelConfig.Value == LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyShotgun.RevolverDropChance.Value}");
            }
        }
    }
}