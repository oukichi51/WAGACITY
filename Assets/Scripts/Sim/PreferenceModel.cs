using UnityEngine;
using UnityEngine.InputSystem;

public static class PreferenceModel
{
    public static Vector5 SeasonBias(Season s)
    {
        return s switch
        {
            Season.Spring => new Vector5(0.0f, 0f, 0f, 0f, 0.2f),
            Season.Summer => new Vector5(-0.1f, 0f, 0f, 0f, 0.3f),
            Season.Autumn => new Vector5(0.0f, 0.1f, 0f, 0.2f, 0.0f),
            Season.Winter => new Vector5(0.2f, 0f, 0f, 0.2f, 0.0f),
            _ => Vector5.Zero
        };
    }

    public static Vector5 TasteFor(SegmentSO Seg, Season season, System.Random Rng, float noise = 0.2f)
    {
        var basePref = new Vector5(Seg.Sweet, Seg.Bitter, Seg.Chewy, Seg.Bean, Seg.Fruit);
        var seasonB = SeasonBias(season);
        var n = new Vector5(
            NextGaussian(Rng, 0, noise),
            NextGaussian(Rng, 0, noise),
            NextGaussian(Rng, 0, noise),
            NextGaussian(Rng, 0, noise),
            NextGaussian(Rng, 0, noise)
        );
        return (basePref + seasonB + n).Normalized();
    }

    public static float NextGaussian(System.Random Rng, float mean = 0f, float std = 1f)
    {
        double u1 = 1.0 - Rng.NextDouble();
        double u2 = 1.0 - Rng.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
                               System.Math.Sin(2.0 * System.Math.PI * u2);
        return (float)(mean + std * randStdNormal);
    }
}
