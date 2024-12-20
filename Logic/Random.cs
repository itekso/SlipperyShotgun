namespace SlipperyShotgun.Logic
{
    
    public static class Random
    {
        public static bool ShouldDropItem(int dropChance, int itemId, int levelId, int mapSeed)
        {
            var seed = mapSeed + levelId + itemId;
            var random = new System.Random(seed);
            var randomNumber = random.Next(0, 101);

            if (SlipperyShotgun.LogLevelConfig.Value == LogLevel.Debug)
            {
                SlipperyShotgun.Logger.LogDebug($"Generated random number: {randomNumber}");
            }

            return randomNumber < dropChance;
        }
    }
}