using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class CityState
{
    public SegmentSO[] segments;
    public float[] proportions; // セグメント比率（合計1）
    public Season season;
    System.Random rng;

    public CityState(SegmentSO[] segs, Season s, int seed = 123)
    {
        segments = segs;
        season = s;
        rng = new System.Random(seed);
        proportions = Dirichlet(segs.Length, 1.0f, rng);
    }

    public int SampleSegmentIndex(float dayNoise = 0.2f)
    {
        // log(P) + N(0,σ) → softmax
        int n = proportions.Length;
        double[] logits = new double[n];
        for (int i = 0; i < n; i++)
        {
            double g = Math.Log(Math.Max(1e-9, proportions[i])) + PreferenceModel.NextGaussian(rng, 0, dayNoise);
            logits[i] = g;
        }
        // softmax sample
        double max = logits.Max();
        double sum = 0; double[] exps = new double[n];
        for (int i = 0; i < n; i++) { exps[i] = Math.Exp(logits[i] - max); sum += exps[i]; }
        double r = rng.NextDouble() * sum; double acc = 0;
        for (int i = 0; i < n; i++) { acc += exps[i]; if (r <= acc) return i; }
        return n - 1;
    }

    public void UpdateByInfluence(float[] influence, float inertia = 0.95f, float eta = 1.0f, float lambda = 0.05f)
    {
        // influence: セグメント別に「今日刺さった」重み
        int n = proportions.Length;
        float[] pt = proportions;
        float[] target = new float[n];
        float sum = 0;
        for (int i = 0; i < n; i++) { target[i] = pt[i] + eta * influence[i]; sum += target[i]; }
        for (int i = 0; i < n; i++) { target[i] = target[i] / Math.Max(1e-6f, sum); }
        // 遅延反映
        for (int i = 0; i < n; i++) { proportions[i] = (1f - lambda) * pt[i] + lambda * target[i]; }
    }

    static float[] Dirichlet(int k, float alpha, System.Random rng)
    {
        double[] g = new double[k];
        double sum = 0;
        for (int i = 0; i < k; i++) { g[i] = Gamma(alpha, 1, rng); sum += g[i]; }
        float[] p = new float[k];
        for (int i = 0; i < k; i++) p[i] = (float)(g[i] / sum);
        return p;
    }
    static double Gamma(double shape, double scale, System.Random rng)
    {
        // 形状k>1用 Marsaglia-Tsang。alpha=1でも問題ない程度。
        double d = shape < 1 ? shape + (1.0 / 3.0) : shape - (1.0 / 3.0);
        double c = 1.0 / Math.Sqrt(9.0 * d);
        while (true)
        {
            double x, v;
            do
            {
                x = PreferenceModel.NextGaussian(rng, 0, 1);
                v = 1.0 + c * x;
            } while (v <= 0);
            v = v * v * v;
            double u = rng.NextDouble();
            if (u < 1 - 0.0331 * (x * x) * (x * x)) return d * v * scale;
            if (Math.Log(u) < 0.5 * x * x + d * (1 - v + Math.Log(v))) return d * v * scale;
        }
    }
}
