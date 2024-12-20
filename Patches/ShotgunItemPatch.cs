using HarmonyLib;
using UnityEngine;
using Random = SlipperyShotgun.Logic.Random;
using SlipperyShotgun.Logic;

namespace SlipperyShotgun.Patches
{
    [HarmonyPatch(typeof(ShotgunItem))]
    public static class ShotgunItemPatch
    {
        [HarmonyPatch("ShootGunServerRpc")]
        [HarmonyPostfix]
        public static void PostfixShootGunServerRpc(ShotgunItem __instance, Vector3 shotgunPosition, Vector3 shotgunForward)
        {
            if (__instance.playerHeldBy == null || __instance.playerHeldBy.currentlyHeldObjectServer == null) return;

            if (Random.ShouldDropItem(SlipperyShotgun.ShotgunDropChance.Value, __instance.itemProperties.itemId, StartOfRound.Instance.currentLevelID, StartOfRound.Instance.randomMapSeed))
            {
                __instance.playerHeldBy.DiscardHeldObject();

                Effects.PlaySillyExtras(__instance.transform.position);
                Effects.PlaySoundEffect(__instance.transform.position);

                if (SlipperyShotgun.LogLevelConfig is not { Value: LogLevel.None })
                {
                    SlipperyShotgun.Logger.LogInfo("Try putting more stats in to handling!");
                }
            }

            if (SlipperyShotgun.LogLevelConfig is { Value: LogLevel.Debug })
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyShotgun.ShotgunDropChance.Value}");
            }
        }
    }
}