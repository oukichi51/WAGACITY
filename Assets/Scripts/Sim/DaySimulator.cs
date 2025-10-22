using UnityEngine;
using System.Collections.Generic;

public class DaySimulator : MonoBehaviour
{
    [Header("Config")]
    public CityState city; // インスペクタからは参照できないので、初期化用SOを別途作ってもOK
    public SegmentSO[] segments; // セグメント一覧
    public List<MenuItemSO> todaysItems = new();
    public List<SetMenuSO> todaysSets = new();

    [Header("Runtime")]
    public int customersPerDay = 100;
    public int seed = 42;
    public float buyNoneBias = 0.0f; // 買わないの基準効用
    public float beta = 2.0f;

    System.Random rng;

    [System.Serializable]
    public class DayResult
    {
        public int soldCount;
        public int soldSetCount;
        public int revenue;
        public float avgSatisfaction;
        public float[] segInfluence; // セグメント別刺さり
    }

    void Awake()
    {
        rng = new System.Random(seed);
        if (city == null)
        {
            // 簡易初期化
            city = new CityState(segments, Season.Spring, seed: seed + 1);
        }
    }

    public DayResult SimulateOneDay()
    {
        var res = new DayResult
        {
            segInfluence = new float[segments.Length]
        };
        float satSum = 0;

        for (int c = 0; c < customersPerDay; c++)
        {
            int si = city.SampleSegmentIndex();
            var seg = segments[si];
            var taste = PreferenceModel.TasteFor(seg, city.season, rng, noise: 0.2f);

            // ユーティリティ計算
            var utils = new List<float>();
            foreach (var m in todaysItems)
                utils.Add(DemandModel.UtilityForItem(taste, m, seg));
            foreach (var s in todaysSets)
                utils.Add(DemandModel.UtilityForSet(taste, s, seg));

            int choice = DemandModel.ChooseIndexSoftmax(utils.ToArray(), rng, beta, buyNoneBias);
            bool bought = choice < utils.Count;
            if (bought)
            {
                float u = utils[choice];
                bool isSet = choice >= todaysItems.Count;
                if (isSet)
                {
                    var sm = todaysSets[choice - todaysItems.Count];
                    res.revenue += sm.setPriceYen;
                    res.soldSetCount++;
                }
                else
                {
                    var mi = todaysItems[choice];
                    res.revenue += mi.priceYen;
                    res.soldCount++;
                }
                float sat = Sigmoid(u - 0.2f); // 体感調整
                satSum += sat;
                res.segInfluence[si] += sat; // 刺さった度合い
            }
        }
        res.avgSatisfaction = (customersPerDay > 0) ? satSum / customersPerDay : 0f;
        // 街更新（遅延）
        city.UpdateByInfluence(res.segInfluence, lambda: 0.05f, eta: 0.8f);
        return res;
    }

    float Sigmoid(float x) => 1f / (1f + Mathf.Exp(-x));
}
