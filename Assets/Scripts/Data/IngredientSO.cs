using UnityEngine;

[CreateAssetMenu(menuName = "Wagashi/Ingredient")]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    [Range(0, 2f)] public float sweet;
    [Range(0, 2f)] public float bitter;
    [Range(0, 2f)] public float chewy;
    [Range(0, 2f)] public float bean;
    [Range(0, 2f)] public float fruit;
    public float sugarGramsPerUnit; // 1’PˆÊ‚ ‚½‚è»“œ[g]
}
