using UnityEngine;
using System.Collections.Generic;

public class DaySimulator : MonoBehaviour
{
    [Header("Config")]
    public CityState City;
    public SegmentSO[] Segments;
    public List<MenuItemSO> TodaysItems = new();
    public List<SetMenuSO> TodaysSets = new();

    [Header("Runtime")]
    public int CustomersPerDay = 100;
    public int Seed = 42;
    public float BuyNoneBias = -0.2f;   // �� ���������u����Ȃ��v����߂�
    public float Beta = 2.0f;

    [Header("Logs")]
    public int ReasonLogCount = 8;      // �� �����܂ŗ��R���L�^���邩

    System.Random Rng;

    [System.Serializable]
    public class DayResult
    {
        public int SoldCount;
        public int SoldSetCount;
        public int Revenue;
        public float AvgSatisfaction;
        public float[] SegInfluence;
        public List<string> SampleReasons = new(); // �� �ǉ�
    }

    void Awake()
    {
        Rng = new System.Random(Seed);
        if (Segments == null || Segments.Length == 0) { Debug.LogError("[DaySimulator] Segments ���ݒ�"); enabled = false; return; }
        City = new CityState(Segments, Season.Spring, Seed: Seed + 1);
        // �y���o���f�[�V�����i�ȗ��j
        if ((TodaysItems == null || TodaysItems.Count == 0) && (TodaysSets == null || TodaysSets.Count == 0))
            Debug.LogWarning("[DaySimulator] ���j���[����ł��B");
    }

    public DayResult SimulateOneDay()
    {
        var res = new DayResult { SegInfluence = new float[Segments.Length] };
        float satSum = 0;

        for (int c = 0; c < CustomersPerDay; c++)
        {
            int si = City.SampleSegmentIndex();
            var seg = Segments[si];
            var taste = PreferenceModel.TasteFor(seg, City.Season, Rng, noise: 0.2f);

            // ����ƍ��v���[�e�B���e�B
            var utils = new List<float>();
            var breakdowns = new List<UtilityBreakdown>();

            foreach (var m in TodaysItems)
            {
                var bd = DemandModel.EvaluateItem(taste, m, seg);
                breakdowns.Add(bd);
                utils.Add(bd.Sum());
            }
            foreach (var s in TodaysSets)
            {
                var bd = DemandModel.EvaluateSet(taste, s, seg);
                breakdowns.Add(bd);
                utils.Add(bd.Sum());
            }

            int choice = DemandModel.ChooseIndexSoftmax(utils.ToArray(), Rng, Beta, BuyNoneBias);
            bool bought = choice < utils.Count;

            if (bought)
            {
                float u = utils[choice];
                bool isSet = choice >= TodaysItems.Count;
                if (isSet)
                {
                    var sm = TodaysSets[choice - TodaysItems.Count];
                    res.Revenue += sm.SetPriceYen;
                    res.SoldSetCount++;
                }
                else
                {
                    var mi = TodaysItems[choice];
                    res.Revenue += mi.PriceYen;
                    res.SoldCount++;
                }
                float sat = Sigmoid(u - 0.2f);
                satSum += sat;
                res.SegInfluence[si] += sat;

                // ���R���O
                if (res.SampleReasons.Count < ReasonLogCount)
                {
                    var (tag, val) = breakdowns[choice].TopContributor();
                    res.SampleReasons.Add(ReasonFormat.ToPretty(tag, val));
                }
            }
        }
        res.AvgSatisfaction = (CustomersPerDay > 0) ? satSum / CustomersPerDay : 0f;

        // ��F�X�����������ω�
        City.UpdateByInfluence(res.SegInfluence, Lambda: 0.05f, Eta: 0.8f);
        return res;
    }

    float Sigmoid(float x) => 1f / (1f + Mathf.Exp(-x));
}
