using UnityEngine;

namespace SlipperyShotgun.Logic
{
    public static class Effects
    {
        public static void PlaySillyExtras(Vector3 position)
        {
            if (SlipperyShotgun.SillyExtras.Value == SillyExtrasOption.None) return;

            var prefabName = SlipperyShotgun.SillyExtras.Value switch
            {
                SillyExtrasOption.Confetti => "EasterEggExplosionParticle",
                SillyExtrasOption.Explosion => "ExplosionEffect",
                SillyExtrasOption.CarBomb => "VehicleExplosionEffect",
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
            if (SlipperyShotgun.SoundEffect.Value == SoundEffectOption.None) return;

            var soundName = SlipperyShotgun.SoundEffect.Value switch
            {
                SoundEffectOption.Bonk => "Bonk",
                SoundEffectOption.Boo => "Boo",
                SoundEffectOption.Slip => "Slip",
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