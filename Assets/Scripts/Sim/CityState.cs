using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class CityState
{
    public SegmentSO[] Segments;
    public float[] Proportions; // çáåv1
    public Season Season;
    System.Random Rng;

    public CityState(SegmentSO[] Segs, Season S, int Seed = 123)
    {
        Segments = Segs;
        Season = S;
        Rng = new System.Random(Seed);
        Proportions = Dirichlet(Segs.Length, 1.0f, Rng);
    }

    public int SampleSegmentIndex(float DayNoise = 0.2f)
    {
        int n = (Proportions == null) ? 0 : Proportions.Length;
        if (n == 0) throw new InvalidOperationException("[CityState] Proportions empty.");
        double[] logits = new double[n];
        for (int i = 0; i < n; i++)
        {
            double g = Math.Log(Math.Max(1e-9, Proportions[i])) + PreferenceModel.NextGaussian(Rng, 0, DayNoise);
            logits[i] = g;
        }
        double max = logits.Max();
        double sum = 0; double[] exps = new double[n];
        for (int i = 0; i < n; i++) { exps[i] = Math.Exp(logits[i] - max); sum += exps[i]; }
        double r = Rng.NextDouble() * sum, acc = 0;
        for (int i = 0; i < n; i++) { acc += exps[i]; if (r <= acc) return i; }
        return n - 1;
    }

    public void UpdateByInfluence(float[] Influence, float Inertia = 0.95f, float Eta = 1.0f, float Lambda = 0.05f)
    {
        int n = Proportions.Length;
        var pt = Proportions;
        var target = new float[n];
        float sum = 0;
        for (int i = 0; i < n; i++) { target[i] = pt[i] + Eta * Influence[i]; sum += target[i]; }
        for (int i = 0; i < n; i++) { target[i] = target[i] / Math.Max(1e-6f, sum); }
        for (int i = 0; i < n; i++) { Proportions[i] = (1f - Lambda) * pt[i] + Lambda * target[i]; }
    }

    static float[] Dirichlet(int K, float Alpha, System.Random Rng)
    {
        double[] g = new double[K];
        double sum = 0;
        for (int i = 0; i < K; i++) { g[i] = Gamma(Alpha, 1, Rng); sum += g[i]; }
        float[] p = new float[K];
        for (int i = 0; i < K; i++) p[i] = (float)(g[i] / sum);
        return p;
    }
    static double Gamma(double Shape, double Scale, System.Random Rng)
    {
        double d = Shape < 1 ? Shape + (1.0 / 3.0) : Shape - (1.0 / 3.0);
        double c = 1.0 / Math.Sqrt(9.0 * d);
        while (true)
        {
            double x, v;
            do
            {
                x = PreferenceModel.NextGaussian(Rng, 0, 1);
                v = 1.0 + c * x;
            } while (v <= 0);
            v = v * v * v;
            double u = Rng.NextDouble();
            if (u < 1 - 0.0331 * (x * x) * (x * x)) return d * v * Scale;
            if (Math.Log(u) < 0.5 * x * x + d * (1 - v + Math.Log(v))) return d * v * Scale;
        }
    }
}
