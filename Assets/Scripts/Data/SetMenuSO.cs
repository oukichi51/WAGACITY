using UnityEngine;

[CreateAssetMenu(menuName = "Wagashi/SetMenu")]
public class SetMenuSO : ScriptableObject
{
    public MenuItemSO itemA;
    public MenuItemSO itemB;
    public int setPriceYen = 600; // Š„ˆøŒã
    public AttentionType attention;
    public string[] tags;
    public Vector5 FlavorVector() => (itemA.recipe.FlavorVector() + itemB.recipe.FlavorVector()).Normalized();
    public float Kcal() => itemA.recipe.Kcal() + itemB.recipe.Kcal();
}
