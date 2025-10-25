// SimRunner.cs（自動実行版）
using UnityEngine;
using System.Text;

public class SimRunner : MonoBehaviour
{
    public DaySimulator Simulator;
    public int DaysToRun = 7;
    public bool RotateSeason = false;
    public int DaysPerSeason = 7;

    void Reset() { Simulator = GetComponent<DaySimulator>(); }

    void Start()
    {
        if (!CheckSim()) return;
        Debug.Log("[SimRunner] Auto start.");
        RunDays(DaysToRun); // ← 自動で回す
    }

    [ContextMenu("Run One Day")]
    public void RunOneDay()
    {
        if (!CheckSim()) return;
        var r = Simulator.SimulateOneDay();
        LogDay(1, r); LogTown();
    }

    [ContextMenu("Run 7 Days")]
    public void Run7Days() => RunDays(7);

    [ContextMenu("Run N Days (DaysToRun)")]
    public void RunNDays_Context() => RunDays(DaysToRun);

    public void RunDays(int n)
    {
        if (!CheckSim()) return;
        int totalRevenue = 0, totalItems = 0, totalSets = 0;
        for (int d = 1; d <= n; d++)
        {
            if (RotateSeason && DaysPerSeason > 0 && (d % DaysPerSeason) == 1 && d > 1)
            {
                Simulator.City.Season = NextSeason(Simulator.City.Season);
                Debug.Log($"[Season] → {Simulator.City.Season}");
            }
            var r = Simulator.SimulateOneDay();
            totalRevenue += r.Revenue; totalItems += r.SoldCount; totalSets += r.SoldSetCount;
            LogDay(d, r); LogTown();
        }
        Debug.Log($"=== 合計({n}日) === 売上 {totalRevenue} 円 / 単品 {totalItems} 個 / セット {totalSets} 個");
    }

    void LogDay(int day, DaySimulator.DayResult r)
    {
        Debug.Log($"=== Day {day} === 売上: {r.Revenue} 円 / 単品 {r.SoldCount} 個 / セット {r.SoldSetCount} 個 / 平均満足度 {r.AvgSatisfaction:0.00}");
        // 理由サンプル（数件）
        if (r.SampleReasons != null && r.SampleReasons.Count > 0)
        {
            for (int i = 0; i < r.SampleReasons.Count; i++)
            {
                Debug.Log($"  理由{i + 1}: {r.SampleReasons[i]}");
            }
        }
    }

    void LogTown()
    {
        var segs = Simulator.Segments;
        var p = Simulator.City.Proportions;
        var sb = new StringBuilder("[Town] ");
        for (int i = 0; i < p.Length; i++)
        {
            var name = segs[i] != null ? segs[i].SegName : $"Seg{i}";
            sb.Append($"{name}:{(p[i] * 100f):0.0}%  ");
        }
        Debug.Log(sb.ToString());
    }

    bool CheckSim()
    {
        if (Simulator == null) { Debug.LogError("[SimRunner] Simulator 未設定"); return false; }
        if (!Simulator.enabled) { Debug.LogError("[SimRunner] DaySimulator が Disabled"); return false; }
        if (Simulator.Segments == null || Simulator.Segments.Length == 0) { Debug.LogError("[SimRunner] Segments が空"); return false; }
        if ((Simulator.TodaysItems == null || Simulator.TodaysItems.Count == 0) &&
            (Simulator.TodaysSets == null || Simulator.TodaysSets.Count == 0))
        {
            Debug.LogWarning("[SimRunner] メニューが空です（売上0になる可能性）");
        }
        return true;
    }

    Season NextSeason(Season s) => s switch
    {
        Season.Spring => Season.Summer,
        Season.Summer => Season.Autumn,
        Season.Autumn => Season.Winter,
        _ => Season.Spring
    };
}
