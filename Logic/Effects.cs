using Unity.Netcode;
using UnityEngine;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Logic
{
    public static class Effects
    {
        [ServerRpc(RequireOwnership = false)]
        public static void PlaySillyExtrasServerRpc(Vector3 position)
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null || !networkManager.IsListening)
                return;

            PlaySillyExtrasClientRpc(position);
            PlaySillyExtrasLocal(position);
        }

        [ClientRpc]
        private static void PlaySillyExtrasClientRpc(Vector3 position)
        {
            PlaySillyExtrasLocal(position);
        }

        private static void PlaySillyExtrasLocal(Vector3 position)
        {
            if (SlipperyOptions.SillyExtras.Value == SlipperyOptions.SillyExtrasOption.None) return;

            var prefabName = SlipperyOptions.SillyExtras.Value switch
            {
                SlipperyOptions.SillyExtrasOption.Confetti => "EasterEggExplosionParticle",
                SlipperyOptions.SillyExtrasOption.Explosion => "ExplosionEffect",
                SlipperyOptions.SillyExtrasOption.CarBomb => "VehicleExplosionEffect",
                _ => null
            };

            if (prefabName != null)
            {
                var effectPrefab = SlipperyShotgun.FindPrefabByName(prefabName);
                if (effectPrefab != null)
                {
                    Object.Instantiate(effectPrefab, position, Quaternion.identity);
                }
                else
                {
                    SlipperyShotgun.Logger.LogDebug($"{prefabName} prefab is null.");
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public static void PlaySoundEffectServerRpc(Vector3 position)
        {
            var networkManager = NetworkManager.Singleton;
            if (networkManager == null || !networkManager.IsListening)
                return;

            PlaySoundEffectClientRpc(position);
            PlaySoundEffectLocal(position);
        }

        [ClientRpc]
        private static void PlaySoundEffectClientRpc(Vector3 position)
        {
            PlaySoundEffectLocal(position);
        }

        private static void PlaySoundEffectLocal(Vector3 position)
        {
            if (SlipperyOptions.SoundEffect.Value == SlipperyOptions.SoundEffectOption.None) return;

            var soundName = SlipperyOptions.SoundEffect.Value switch
            {
                SlipperyOptions.SoundEffectOption.Bonk => "Bonk",
                SlipperyOptions.SoundEffectOption.Boo => "Boo",
                SlipperyOptions.SoundEffectOption.Slip => "Slip",
                _ => null
            };

            if (soundName != null)
            {
                var clip = SlipperyShotgun.LoadSoundEffect(soundName);
                if (clip != null)
                {
                    AudioSource.PlayClipAtPoint(clip, position);
                }
            }
        }
    }
}