using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlanningUI : MonoBehaviour
{
    public List<MenuItemSO> AvailableItems = new();
    public int MaxItems = 4;

    public Transform ListRoot;
    public GameObject TogglePrefab;
    public Text CounterText;

    readonly List<Toggle> _toggles = new();

    public void BuildList()
    {
        // クリア
        foreach (Transform c in ListRoot) Destroy(c.gameObject);
        _toggles.Clear();

        Debug.Log($"[PlanningUI] build: items={AvailableItems?.Count ?? 0}, prefab={(TogglePrefab ? TogglePrefab.name : "null")}");

        int i = 0;
        foreach (var item in AvailableItems)
        {
            if (item == null) continue;
            // Assets/Scripts/UI/PlanningUI.cs（BuildList内の生成部分だけ差し替え）
            var go = Instantiate(TogglePrefab, ListRoot);

            // 行UIを取得/補完
            var row = go.GetComponent<ItemRowUI>();
            if (!row) row = go.AddComponent<ItemRowUI>();

            // Container が未設定なら、子の HGroup を優先して使う
            if (!row.Container)
            {
                var h = go.transform.Find("HGroup") as RectTransform;
                if (h) row.Container = h;
            }
            row.InitIfNeeded();
            row.Set(item);

            // Toggle をリスト管理
            var t = row.Toggle ? row.Toggle : go.GetComponentInChildren<Toggle>(true);
            if (t != null)
            {
                t.isOn = i < MaxItems;
                t.onValueChanged.AddListener(_ => UpdateCounter());
                _toggles.Add(t);
            }
            else
            {
                Debug.LogWarning("[PlanningUI] Toggle が見つかりません。この行は選択できません。", go);
            }

            i++;
        }
        UpdateCounter();
    }

    void UpdateCounter()
    {
        int on = 0;
        foreach (var t in _toggles) if (t != null && t.isOn) on++;
        if (on > MaxItems)
        {
            int needOff = on - MaxItems;
            for (int i = _toggles.Count - 1; i >= 0 && needOff > 0; i--)
            {
                if (_toggles[i] != null && _toggles[i].isOn) { _toggles[i].isOn = false; needOff--; }
            }
        }
        if (CounterText) CounterText.text = $"{on}/{MaxItems}";
    }

    public List<MenuItemSO> GetSelectedItems()
    {
        var list = new List<MenuItemSO>();
        int idx = 0;
        for (int i = 0; i < _toggles.Count && i < AvailableItems.Count; i++)
        {
            if (_toggles[i] != null && _toggles[i].isOn) list.Add(AvailableItems[i]);
            idx++;
        }
        return list;
    }
}
