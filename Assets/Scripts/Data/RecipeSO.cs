using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

[System.Serializable]
public struct IngredientAmount
{
    public IngredientSO Ingredient;
    public float Units;
}

[CreateAssetMenu(menuName = "Wagashi/Recipe")]
public class RecipeSO : ScriptableObject
{
    [FormerlySerializedAs("recipeName")] public string RecipeName;
    [FormerlySerializedAs("ingredients")] public List<IngredientAmount> Ingredients = new();

    public Vector5 FlavorVector()
    {
        if (Ingredients == null || Ingredients.Count == 0)
        {
            Debug.LogWarning($"[RecipeSO] Ingredients is empty on '{RecipeName}'.");
            return Vector5.Zero;
        }
        var sum = Vector5.Zero;
        int used = 0;
        foreach (var ia in Ingredients)
        {
            if (ia.Ingredient == null)
            {
                Debug.LogWarning($"[RecipeSO] Null Ingredient in '{RecipeName}'. Skipped.");
                continue;
            }
            var i = ia.Ingredient;
            sum += new Vector5(i.Sweet, i.Bitter, i.Chewy, i.Bean, i.Fruit) * ia.Units;
            used++;
        }
        if (used == 0)
        {
            Debug.LogWarning($"[RecipeSO] No valid ingredients in '{RecipeName}'.");
            return Vector5.Zero;
        }
        return sum.Normalized();
    }
    public float SugarGrams()
    {
        float g = 0f;
        if (Ingredients != null)
            foreach (var ia in Ingredients) g += ia.Ingredient.SugarGramsPerUnit * ia.Units;
        return g;
    }
    public float Kcal() => SugarGrams() * 4f;
}
