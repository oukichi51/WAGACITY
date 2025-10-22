using UnityEngine;

public enum AttentionType { None, Teiban, Ichioshi }

[CreateAssetMenu(menuName = "Wagashi/MenuItem")]
public class MenuItemSO : ScriptableObject
{
    public RecipeSO recipe;
    public int priceYen = 300;
    public AttentionType attention;
    public string[] tags; // "HighCalorie" ‚È‚Ç
}
