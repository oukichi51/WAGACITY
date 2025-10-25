using UnityEngine;
using UnityEngine.Serialization;

public enum AttentionType { None, Teiban, Ichioshi }

[CreateAssetMenu(menuName = "Wagashi/Menu Item", fileName = "MenuItem_")]
public class MenuItemSO : ScriptableObject
{
    [Header("Display")]
    [Tooltip("ゲーム内に出す表示名。空なら Asset 名（このアセットの name）を使います。")]
    public string DisplayName;

    [Header("Recipe")]
    [FormerlySerializedAs("recipe")]
    public RecipeSO Recipe;

    [Header("Sales")]
    [FormerlySerializedAs("priceYen")]
    public int PriceYen = 350;

    [FormerlySerializedAs("attention")]
    public AttentionType Attention = AttentionType.Ichioshi;

    [FormerlySerializedAs("tags")]
    public string[] Tags;

    // ---------- 便利プロパティ ----------
    /// <summary>UI用の表示名（DisplayNameが空ならアセット名）</summary>
    public string NameToShow => string.IsNullOrEmpty(DisplayName) ? name : DisplayName;

    /// <summary>推定カロリー（kcal）。Recipe 未設定なら 0。</summary>
    public float Kcal => Recipe ? Recipe.Kcal() : 0f;

    /// <summary>味ベクトル。Recipe 未設定ならゼロ。</summary>
    public Vector5 FlavorVector() => Recipe ? Recipe.FlavorVector() : Vector5.Zero;
}
