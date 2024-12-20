using SlipperyShotgun.Configuration;

namespace SlipperyShotgun.Logic
{
    public static class Random
    {
        public static bool ShouldDropItem(int dropChance, int itemId, int levelId, int mapSeed)
        {
            var seed = mapSeed + levelId + itemId;
            var random = new System.Random(seed);
            var randomNumber = random.Next(0, 101);

            if (SlipperyOptions.LogLevelConfig.Value == SlipperyOptions.LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Generated random number: {randomNumber}");
            }

            return randomNumber < dropChance;
        }
    }
}