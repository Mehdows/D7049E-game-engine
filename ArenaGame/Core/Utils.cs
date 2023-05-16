using System;

namespace ArenaGame.Core;


public static class Utils
{
    private static Random random = new Random();

    public static float GetRandomFloatInRange(float minValue, float maxValue)
    {
        return (float)random.NextDouble() * (maxValue - minValue) + minValue;
    }
}