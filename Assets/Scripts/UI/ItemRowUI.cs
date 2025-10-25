// Assets/Scripts/UI/ItemRowUI.cs
using UnityEngine;
using UnityEngine.UI;

public class ItemRowUI : MonoBehaviour
{
    [Header("Refs")]
    public Toggle Toggle;                 // �s�̃g�O���i�K�{�j
    public RectTransform Container;       // ���x������ׂ�e�iHGroup�����j
    public Text Label;                    // ���O/���i��\������ Text�i�����⊮�j

    // �Ăяo�����FInstantiate��Ɉ�x����
    public void InitIfNeeded()
    {
        // 1) Toggle
        if (!Toggle) Toggle = GetComponentInChildren<Toggle>(true);

        // 2) Container�i�D�揇�F�����w�� �� �q��"HGroup" �� �q�� HorizontalLayoutGroup �������� �� �����j
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

        // 3) Label�F������Ȃ���� Container �����ɐV�K�쐬
        if (!Label)
        {
            Label = GetComponentInChildren<Text>(true);
            if (!Label)
            {
                var go = new GameObject("Label", typeof(RectTransform), typeof(Text), typeof(LayoutElement));
                var rt = go.GetComponent<RectTransform>();
                rt.SetParent(Container, false);
                // �A���J�[�����E�X�g���b�`�E�㉺����
                rt.anchorMin = new Vector2(0, 0.5f);
                rt.anchorMax = new Vector2(1, 0.5f);
                rt.pivot = new Vector2(0, 0.5f);
                rt.offsetMin = new Vector2(0, -12);
                rt.offsetMax = new Vector2(0, 12);

                var le = go.GetComponent<LayoutElement>();
                le.flexibleWidth = 1;     // �c�蕝��S���g��

                Label = go.GetComponent<Text>();
                Label.alignment = TextAnchor.MiddleLeft;
                Label.horizontalOverflow = HorizontalWrapMode.Wrap;
                Label.verticalOverflow = VerticalWrapMode.Overflow;
                Label.raycastTarget = false;
                Label.fontSize = 20;
                // �O�̂��߁F�t�H���g���ݒ�ł�������悤�Ɂi����Arial�j
                if (Label.font == null)
                {
                    Label.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                }
                Label.color = Color.black;
            }
        }
        // ���x���͕K�� Active
        if (!Label.gameObject.activeSelf) Label.gameObject.SetActive(true);
    }

    public void Set(MenuItemSO item)
    {
        InitIfNeeded();
        if (!Label) { Debug.LogError("[ItemRowUI] Label ������"); return; }

        var jp = string.IsNullOrEmpty(item.DisplayName) ? item.name : item.DisplayName;
        var en = item.Recipe ? item.Recipe.RecipeName : "";
        Label.text = string.IsNullOrEmpty(en)
            ? $"{jp} {item.PriceYen}�~  [{item.Attention}]"
            : $"{jp} / {en} {item.PriceYen}�~  [{item.Attention}]";
    }
}
