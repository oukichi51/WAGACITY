// Assets/Scripts/UI/ItemRowUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ItemRowUI : MonoBehaviour
{
    [Header("Refs")]
    public Toggle Toggle;                 // 行のトグル（必須）
    public RectTransform Container;       // ラベルを並べる親（HGroup推奨）
    public Text Label;                    // 名前/価格を表示する Text（自動補完可）

    // 呼び出し側：Instantiate後に一度だけ
    public void InitIfNeeded()
    {
        // 1) Toggle
        if (!Toggle) Toggle = GetComponentInChildren<Toggle>(true);

        // 2) Container（優先順：明示指定 → 子の"HGroup" → 子で HorizontalLayoutGroup を持つもの → 自分）
        if (!Container)
        {
            var t = transform.Find("HGroup") as RectTransform;
            if (!t)
            {
                var hlg = GetComponentInChildren<HorizontalLayoutGroup>(true);
                if (hlg) t = hlg.transform as RectTransform;
            }
            Container = t ? t : transform as RectTransform;
        }

        // 3) Label：見つからなければ Container 直下に新規作成
        if (!Label)
        {
            Label = GetComponentInChildren<Text>(true);
            if (!Label)
            {
                var go = new GameObject("Label", typeof(RectTransform), typeof(Text), typeof(LayoutElement));
                var rt = go.GetComponent<RectTransform>();
                rt.SetParent(Container, false);
                // アンカーを左右ストレッチ・上下中央
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(1, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.offsetMin = new Vector2(0, -12);
                rt.offsetMax = new Vector2(0, 12);

                var le = go.GetComponent<LayoutElement>();
                le.flexibleWidth = 1;     // 残り幅を全部使う

                Label = go.GetComponent<Text>();
                Label.alignment = TextAnchor.MiddleLeft;
                Label.horizontalOverflow = HorizontalWrapMode.Wrap;
                Label.verticalOverflow = VerticalWrapMode.Overflow;
                Label.raycastTarget = false;
                Label.fontSize = 20;
                // 念のため：フォント未設定でも見えるように（内蔵Arial）
                if (Label.font == null)
                {
                    Label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                }
                Label.color = Color.black;
            }
        }
        // ラベルは必ず Active
        if (!Label.gameObject.activeSelf) Label.gameObject.SetActive(true);
    }

    public void Set(MenuItemSO item)
    {
        InitIfNeeded();
        if (!Label) { Debug.LogError("[ItemRowUI] Label 未生成"); return; }

        var jp = string.IsNullOrEmpty(item.DisplayName) ? item.name : item.DisplayName;
        var en = item.Recipe ? item.Recipe.RecipeName : "";
        Label.text = string.IsNullOrEmpty(en)
            ? $"{jp} {item.PriceYen}円  [{item.Attention}]"
            : $"{jp} / {en} {item.PriceYen}円  [{item.Attention}]";
    }
}
