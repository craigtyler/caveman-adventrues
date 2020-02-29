using System;

[Serializable]
public class IntRange
{
    public int Min;
    public int Max;

    public IntRange(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public int Random
    {
        get { return UnityEngine.Random.Range(Min, Max); }
    }
}
