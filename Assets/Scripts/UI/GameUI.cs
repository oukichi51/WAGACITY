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
        Planning.BuildList(); // �����̌���\��
    }

    void OnStartDay()
    {
        var selected = Planning.GetSelectedItems();
        if (selected.Count == 0)
        {
            Debug.LogWarning("[GameUI] �����͉����I�΂�Ă��܂���B���Ȃ��Ƃ�1�i�I��ł��������B");
            return;
        }
        // DaySimulator�ɍ����̃��j���[���Z�b�g�i�Z�b�g�̓t�F�[�Y1�ł͖��Ή��j
        Simulator.TodaysItems = selected;
        Simulator.TodaysSets.Clear();

        var r = Simulator.SimulateOneDay();

        // ���ʉ�ʂ�
        Results.Show(r, Simulator);
        PlanningPanel.SetActive(false);
        ResultsPanel.SetActive(true);
    }
}
