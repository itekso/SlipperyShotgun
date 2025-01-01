using SlipperyShotgun.Configuration;
using UnityEngine;
using Random = System.Random;

namespace SlipperyShotgun.Logic
{
    public class SlipperyBehaviour : MonoBehaviour
    {
        public int RandomSeed = 0;
        private int LocalRolls = 0;
        private int NetworkedRolls = 0;

        public int RollRandomAndSync()
        {
            var randomNumber = new Random(RandomSeed + NetworkedRolls + LocalRolls).Next(0, 100);

            LocalRolls++;
            SlipperyManager.IncrementRollsServerRpc(gameObject);

            return randomNumber;
        }

        public void IncrementRolls()
        {
            NetworkedRolls++;
            if (LocalRolls > 0) LocalRolls--;
        }

        public bool ShouldDropItem(int dropChance)
        {
            var randomNumber = RollRandomAndSync();

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Generated random number: {randomNumber}");
            }

            return randomNumber < dropChance;
        }

        public void PlaySillyExtras(SlipperyOptions.SillyExtrasOption sillyExtrasOption)
        {
            var prefabName = sillyExtrasOption switch
            {
                SlipperyOptions.SillyExtrasOption.Confetti => "EasterEggExplosionParticle",
                SlipperyOptions.SillyExtrasOption.Explosion => "ExplosionEffect",
                SlipperyOptions.SillyExtrasOption.CarBomb => "VehicleExplosionEffect",
                _ => null
            };

            if (prefabName == null) return;

            var effectPrefab = SlipperyShotgun.FindPrefabByName(prefabName);
            if (effectPrefab == null) return;

            Object.Instantiate(effectPrefab, transform.position, Quaternion.identity);
        }

        public void PlaySoundEffect(SlipperyOptions.SoundEffectOption soundEffectOption)
        {
            var soundName = soundEffectOption switch
            {
                SlipperyOptions.SoundEffectOption.Bonk => "Bonk",
                SlipperyOptions.SoundEffectOption.Boo => "Boo",
                SlipperyOptions.SoundEffectOption.Slip => "Slip",
                _ => null
            };

            if (soundName == null) return;

            var effectClip = SlipperyShotgun.LoadSoundEffect(soundName);
            if (effectClip == null) return;

            AudioSource.PlayClipAtPoint(effectClip, transform.position);
        }
    }
}