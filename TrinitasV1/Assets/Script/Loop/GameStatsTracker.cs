using UnityEngine;

public static class GameStatsTracker
{
    public static int TotalBirths { get; private set; } = 0;
    public static int LastDeathYear { get; private set; } = 0;
    public static int WorldCount { get; private set; } = 1;

    public static void RegisterBirth()
    {
        TotalBirths++;
    }

    public static void RegisterDeath(int year)
    {
        if (year > LastDeathYear)
        {
            LastDeathYear = year;
        }
    }

    public static void ResetForNewWorld()
    {
        TotalBirths = 0;
        LastDeathYear = 0;
        WorldCount++;
    }
}
