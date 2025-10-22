using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class DemandModel
{
    public static float AttentionBonus(AttentionType a)
        => a == AttentionType.Ichioshi ? 0.6f : (a == AttentionType.Teiban ? 0.3f : 0f);

    public static float UtilityForItem(
        Vector5 taste, MenuItemSO m, SegmentSO seg)
    {
        float baseDot = taste.Dot(m.recipe.FlavorVector());
        float attn = AttentionBonus(m.attention);
        float pricePen = seg.priceSensitivity * m.priceYen;
        float kcal = m.recipe.Kcal();
        float calPen = Mathf.Max(0f, (kcal - seg.kcalOk) / Mathf.Max(1f, seg.kcalSpan));
        float novelty = (m.attention == AttentionType.Ichioshi ? seg.noveltyLove : 0f);
        float U = baseDot + attn + novelty - pricePen - 0.7f * calPen;
        return U;
    }

    public static float UtilityForSet(
        Vector5 taste, SetMenuSO s, SegmentSO seg)
    {
        float baseDot = taste.Dot(s.FlavorVector());
        float attn = AttentionBonus(s.attention);
        float pricePen = seg.priceSensitivity * s.setPriceYen;
        float kcal = s.Kcal();
        float calPen = Mathf.Max(0f, (kcal - seg.kcalOk) / Mathf.Max(1f, seg.kcalSpan));
        float novelty = (s.attention == AttentionType.Ichioshi ? seg.noveltyLove : 0f);
        float U = baseDot + attn + novelty - pricePen - 0.7f * calPen;
        // 高カロリータグが好きな層を少し上げる（例）
        if (s.tags != null && s.tags.Contains("HighCalorie")) U += 0.15f;
        return U;
    }

    public static int ChooseIndexSoftmax(float[] utilities, System.Random rng, float beta = 2.0f, float noneBias = 0.0f)
    {
        // utilitiesに「買わない」も追加してsoftmaxで選ぶ
        var list = new List<float>(utilities) { noneBias }; // 最後が「買わない」
        float max = list.Max();
        double sum = 0;
        double[] exps = new double[list.Count];
        for (int i = 0; i < list.Count; i++) { exps[i] = System.Math.Exp(beta * (list[i] - max)); sum += exps[i]; }
        double r = rng.NextDouble() * sum;
        double acc = 0;
        for (int i = 0; i < exps.Length; i++) { acc += exps[i]; if (r <= acc) return i; }
        return exps.Length - 1;
    }
}
