using SlipperyShotgun.Configuration;
using StaticNetcodeLib;
using Unity.Netcode;
using UnityEngine;

namespace SlipperyShotgun.Logic
{
    [StaticNetcode]
    public static class SlipperyManager
    {
        [ServerRpc]
        public static void IncrementRollsServerRpc(NetworkObjectReference slipperyNGO)
        {
            IncrementRollsClientRpc(slipperyNGO);
        }

        [ClientRpc]
        public static void IncrementRollsClientRpc(NetworkObjectReference slipperyNGO)
        {
            SlipperyBehaviour? slippery = ((GameObject)slipperyNGO)?.GetComponent<SlipperyBehaviour>();
            if (slippery == null) return;

            slippery.IncrementRolls();
        }

        [ServerRpc]
        public static void PlaySillyExtrasServerRpc(NetworkObjectReference slipperyNGO)
        {
            PlaySillyExtrasClientRpc(slipperyNGO, SlipperyOptions.SillyExtras.Value);
        }

        [ClientRpc]
        public static void PlaySillyExtrasClientRpc(NetworkObjectReference slipperyNGO, SlipperyOptions.SillyExtrasOption sillyExtrasOption)
        {
            SlipperyBehaviour? slippery = ((GameObject)slipperyNGO)?.GetComponent<SlipperyBehaviour>();
            if (slippery == null) return;

            slippery.PlaySillyExtras(sillyExtrasOption);
        }

        [ServerRpc]
        public static void PlaySoundEffectServerRpc(NetworkObjectReference slipperyNGO)
        {
            PlaySoundEffectClientRpc(slipperyNGO, SlipperyOptions.SoundEffect.Value);
        }

        [ClientRpc]
        public static void PlaySoundEffectClientRpc(NetworkObjectReference slipperyNGO, SlipperyOptions.SoundEffectOption soundEffectOption)
        {
            SlipperyBehaviour? slippery = ((GameObject)slipperyNGO)?.GetComponent<SlipperyBehaviour>();
            if (slippery == null) return;

            slippery.PlaySoundEffect(soundEffectOption);
        }
    }
}