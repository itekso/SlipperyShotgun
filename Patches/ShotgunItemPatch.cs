using HarmonyLib;
using UnityEngine;
using Random = SlipperyShotgun.Logic.Random;
using SlipperyShotgun.Logic;
using SlipperyShotgun.Configuration;

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

            if (Random.ShouldDropItem(SlipperyOptions.ShotgunDropChance.Value, __instance.itemProperties.itemId, StartOfRound.Instance.currentLevelID, StartOfRound.Instance.randomMapSeed))
            {
                __instance.playerHeldBy.DiscardHeldObject();

                Effects.PlaySillyExtrasServerRpc(__instance.transform.position);
                Effects.PlaySoundEffectServerRpc(__instance.transform.position);

                if (SlipperyOptions.LogLevelConfig is not { Value: SlipperyOptions.LogLevel.None })
                {
                    SlipperyShotgun.Logger.LogInfo("Try putting more stats in to handling!");
                }
            }

            if (SlipperyOptions.LogLevelConfig is { Value: SlipperyOptions.LogLevel.Debug })
            {
                SlipperyShotgun.Logger.LogDebug($"Configured drop chance: {SlipperyOptions.ShotgunDropChance.Value}");
            }
        }
    }
}