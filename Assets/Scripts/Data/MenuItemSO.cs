using UnityEngine;
using UnityEngine.Serialization;

public enum AttentionType { None, Teiban, Ichioshi }

[CreateAssetMenu(menuName = "Wagashi/Menu Item", fileName = "MenuItem_")]
public class MenuItemSO : ScriptableObject
{
    [Header("Display")]
    [Tooltip("�Q�[�����ɏo���\�����B��Ȃ� Asset ���i���̃A�Z�b�g�� name�j���g���܂��B")]
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

    // ---------- �֗��v���p�e�B ----------
    /// <summary>UI�p�̕\�����iDisplayName����Ȃ�A�Z�b�g���j</summary>
    public string NameToShow => string.IsNullOrEmpty(DisplayName) ? name : DisplayName;

    /// <summary>����J�����[�ikcal�j�BRecipe ���ݒ�Ȃ� 0�B</summary>
    public float Kcal => Recipe ? Recipe.Kcal() : 0f;

    /// <summary>���x�N�g���BRecipe ���ݒ�Ȃ�[���B</summary>
    public Vector5 FlavorVector() => Recipe ? Recipe.FlavorVector() : Vector5.Zero;
}
