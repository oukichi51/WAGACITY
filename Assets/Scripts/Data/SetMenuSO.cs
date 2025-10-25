using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Wagashi/SetMenu")]
public class SetMenuSO : ScriptableObject
{
    [FormerlySerializedAs("itemA")] public MenuItemSO ItemA;
    [FormerlySerializedAs("itemB")] public MenuItemSO ItemB;
    [FormerlySerializedAs("setPriceYen")] public int SetPriceYen = 600;
    [FormerlySerializedAs("attention")] public AttentionType Attention;
    [FormerlySerializedAs("tags")] public string[] Tags;

    public Vector5 FlavorVector() => (ItemA.Recipe.FlavorVector() + ItemB.Recipe.FlavorVector()).Normalized();
    public float Kcal() => ItemA.Recipe.Kcal() + ItemB.Recipe.Kcal();
}
