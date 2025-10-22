using UnityEngine;

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
            _ => Vector5.zero
        };
    }

    public static Vector5 TasteFor(SegmentSO seg, Season season, System.Random rng, float noise = 0.2f)
    {
        Vector5 basePref = new(seg.sweet, seg.bitter, seg.chewy, seg.bean, seg.fruit);
        Vector5 seasonB = SeasonBias(season);
        Vector5 n = new(
            NextGaussian(rng, 0, noise),
            NextGaussian(rng, 0, noise),
            NextGaussian(rng, 0, noise),
            NextGaussian(rng, 0, noise),
            NextGaussian(rng, 0, noise)
        );
        return (basePref + seasonB + n).Normalized();
    }

    // ïWèÄê≥ãKóêêî
    public static float NextGaussian(System.Random rng, float mean = 0f, float std = 1f)
    {
        // Box?Mullerñ@
        double u1 = 1.0 - rng.NextDouble();
        double u2 = 1.0 - rng.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) *
                               System.Math.Sin(2.0 * System.Math.PI * u2);
        return (float)(mean + std * randStdNormal);
    }
}

public enum Season { Spring, Summer, Autumn, Winter }
