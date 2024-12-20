using UnityEngine;
using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Logic
{
    public static class Effects
    {
        public static void PlaySillyExtras(Vector3 position)
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

        public static void PlaySoundEffect(Vector3 position)
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