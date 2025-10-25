using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Wagashi/Ingredient")]
public class IngredientSO : ScriptableObject
{
    [FormerlySerializedAs("ingredientName")] public string IngredientName;
    [FormerlySerializedAs("sweet")][Range(0, 2f)] public float Sweet;
    [FormerlySerializedAs("bitter")][Range(0, 2f)] public float Bitter;
    [FormerlySerializedAs("chewy")][Range(0, 2f)] public float Chewy;
    [FormerlySerializedAs("bean")][Range(0, 2f)] public float Bean;
    [FormerlySerializedAs("fruit")][Range(0, 2f)] public float Fruit;

    [FormerlySerializedAs("sugarGramsPerUnit")]
    public float SugarGramsPerUnit; // 1íPà Ç†ÇΩÇËçªìú[g]
}
