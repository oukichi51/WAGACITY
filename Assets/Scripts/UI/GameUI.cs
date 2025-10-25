using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUI : MonoBehaviour
{
    [Header("Refs")]
    public DaySimulator Simulator;
    public GameObject PlanningPanel;
    public GameObject ResultsPanel;
    public PlanningUI Planning;
    public ResultsUI Results;
    public Button StartDayButton;
    public Button NextDayButton;

    void Start()
    {
        ShowPlanning();
        StartDayButton.onClick.AddListener(OnStartDay);
        NextDayButton.onClick.AddListener(ShowPlanning);
    }

    void ShowPlanning()
    {
        ResultsPanel.SetActive(false);
        PlanningPanel.SetActive(true);
        Planning.BuildList(); // 当日の候補を表示
    }

    void OnStartDay()
    {
        var selected = Planning.GetSelectedItems();
        if (selected.Count == 0)
        {
            Debug.LogWarning("[GameUI] 今日は何も選ばれていません。少なくとも1品選んでください。");
            return;
        }
        // DaySimulatorに今日のメニューをセット（セットはフェーズ1では未対応）
        Simulator.TodaysItems = selected;
        Simulator.TodaysSets.Clear();

        var r = Simulator.SimulateOneDay();

        // 結果画面へ
        Results.Show(r, Simulator);
        PlanningPanel.SetActive(false);
        ResultsPanel.SetActive(true);
    }
}
