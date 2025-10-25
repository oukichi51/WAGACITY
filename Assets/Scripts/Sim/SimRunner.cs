// SimRunner.cs�i�������s�Łj
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
        RunDays(DaysToRun); // �� �����ŉ�
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
                Debug.Log($"[Season] �� {Simulator.City.Season}");
            }
            var r = Simulator.SimulateOneDay();
            totalRevenue += r.Revenue; totalItems += r.SoldCount; totalSets += r.SoldSetCount;
            LogDay(d, r); LogTown();
        }
        Debug.Log($"=== ���v({n}��) === ���� {totalRevenue} �~ / �P�i {totalItems} �� / �Z�b�g {totalSets} ��");
    }

    void LogDay(int day, DaySimulator.DayResult r)
    {
        Debug.Log($"=== Day {day} === ����: {r.Revenue} �~ / �P�i {r.SoldCount} �� / �Z�b�g {r.SoldSetCount} �� / ���ϖ����x {r.AvgSatisfaction:0.00}");
        // ���R�T���v���i�����j
        if (r.SampleReasons != null && r.SampleReasons.Count > 0)
        {
            for (int i = 0; i < r.SampleReasons.Count; i++)
            {
                Debug.Log($"  ���R{i + 1}: {r.SampleReasons[i]}");
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
        if (Simulator == null) { Debug.LogError("[SimRunner] Simulator ���ݒ�"); return false; }
        if (!Simulator.enabled) { Debug.LogError("[SimRunner] DaySimulator �� Disabled"); return false; }
        if (Simulator.Segments == null || Simulator.Segments.Length == 0) { Debug.LogError("[SimRunner] Segments ����"); return false; }
        if ((Simulator.TodaysItems == null || Simulator.TodaysItems.Count == 0) &&
            (Simulator.TodaysSets == null || Simulator.TodaysSets.Count == 0))
        {
            Debug.LogWarning("[SimRunner] ���j���[����ł��i����0�ɂȂ�\���j");
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
