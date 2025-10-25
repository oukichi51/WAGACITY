using System;
using System.Collections.Generic;
using UnityEngine;

public struct UtilityBreakdown
{
    public float FlavorDot;   // +  ����v
    public float Attention;   // +  �C�`�I�V/���
    public float Novelty;     // +  �V�
    public float PricePen;    // -  ���i�y�i���e�B
    public float CalPen;      // -  �J�����[�y�i���e�B

    public float Sum(float calWeight = 0.7f)
        => FlavorDot + Attention + Novelty - PricePen - calWeight * CalPen;

    public (ReasonTag tag, float val) TopContributor()
    {
        // ���̗v���g�b�v�ƕ��̗v���g�b�v�����āA��Βl�ő��Ԃ�
        float bestPos = 0; ReasonTag bestPosTag = ReasonTag.Flavor;
        if (FlavorDot > bestPos) { bestPos = FlavorDot; bestPosTag = ReasonTag.Flavor; }
        if (Attention > bestPos) { bestPos = Attention; bestPosTag = ReasonTag.Attention; }
        if (Novelty > bestPos) { bestPos = Novelty; bestPosTag = ReasonTag.Novelty; }

        float worstNeg = 0; ReasonTag worstNegTag = ReasonTag.Price;
        if (PricePen > worstNeg) { worstNeg = PricePen; worstNegTag = ReasonTag.Price; }
        if (CalPen > worstNeg) { worstNeg = CalPen; worstNegTag = ReasonTag.Calorie; }

        // �ǂ��炪���������͐�Βl��r
        if (worstNeg > bestPos) return (worstNegTag, -worstNeg);
        return (bestPosTag, bestPos);
    }
}

public static class DemandModel
{
    public static float AttentionBonus(AttentionType A)
        => A == AttentionType.Ichioshi ? 0.6f : (A == AttentionType.Teiban ? 0.3f : 0f);

    // ���i�y�i���e�B�́u100�~�P�ʁv�ŃX�P�[��
    static float PricePenaltyYenScaled(float priceYen, float priceSensitivityPer100Yen)
        => priceSensitivityPer100Yen * (priceYen / 100f);

    public static float UtilityForItem(Vector5 Taste, MenuItemSO M, SegmentSO Seg)
        => EvaluateItem(Taste, M, Seg).Sum();

    public static float UtilityForSet(Vector5 Taste, SetMenuSO S, SegmentSO Seg)
        => EvaluateSet(Taste, S, Seg).Sum();

    public static UtilityBreakdown EvaluateItem(Vector5 Taste, MenuItemSO M, SegmentSO Seg)
    {
        if (M == null || M.Recipe == null)
        {
            Debug.LogWarning("[DemandModel] MenuItem or Recipe is null");
            return default;
        }
        float flavor = Taste.Dot(M.Recipe.FlavorVector());
        float attn = AttentionBonus(M.Attention);
        float novelty = (M.Attention == AttentionType.Ichioshi ? Seg.NoveltyLove : 0f);
        float price = PricePenaltyYenScaled(M.PriceYen, Seg.PriceSensitivity);
        float kcal = M.Recipe.Kcal();
        float calPen = Mathf.Max(0f, (kcal - Seg.KcalOk) / Mathf.Max(1f, Seg.KcalSpan));
        return new UtilityBreakdown
        {
            FlavorDot = flavor,
            Attention = attn,
            Novelty = novelty,
            PricePen = price,
            CalPen = calPen
        };
    }

    public static UtilityBreakdown EvaluateSet(Vector5 Taste, SetMenuSO S, SegmentSO Seg)
    {
        if (S == null || S.ItemA == null || S.ItemB == null || S.ItemA.Recipe == null || S.ItemB.Recipe == null)
        {
            Debug.LogWarning("[DemandModel] Set or its items/recipes are null");
            return default;
        }
        float flavor = Taste.Dot(S.FlavorVector());
        float attn = AttentionBonus(S.Attention);
        float novelty = (S.Attention == AttentionType.Ichioshi ? Seg.NoveltyLove : 0f);
        float price = PricePenaltyYenScaled(S.SetPriceYen, Seg.PriceSensitivity);
        float kcal = S.Kcal();
        float calPen = Mathf.Max(0f, (kcal - Seg.KcalOk) / Mathf.Max(1f, Seg.KcalSpan));

        // �^�O�␳�i��FHighCalorie �D�ݑw���������_�j
        if (S.Tags != null && Array.IndexOf(S.Tags, "HighCalorie") >= 0) novelty += 0.15f;

        return new UtilityBreakdown
        {
            FlavorDot = flavor,
            Attention = attn,
            Novelty = novelty,
            PricePen = price,
            CalPen = calPen
        };
    }

    /// Softmax�I���i�Ō�̃C���f�b�N�X���u����Ȃ��v�j
    public static int ChooseIndexSoftmax(
        float[] Utilities,
        System.Random Rng,
        float Beta = 2.0f,
        float NoneBias = 0.0f)
    {
        if (Utilities == null || Utilities.Length == 0) return 0;

        var list = new List<float>(Utilities);
        list.Add(NoneBias); // �u����Ȃ��v

        double max = double.NegativeInfinity;
        for (int i = 0; i < list.Count; i++)
            if (list[i] > max) max = list[i];

        double sum = 0.0;
        double[] exps = new double[list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            exps[i] = Math.Exp(Beta * (list[i] - max));
            sum += exps[i];
        }
        double r = Rng.NextDouble() * sum, acc = 0.0;
        for (int i = 0; i < exps.Length; i++) { acc += exps[i]; if (r <= acc) return i; }
        return exps.Length - 1;
    }
}
