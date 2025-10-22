using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct IngredientAmount
{
    public IngredientSO ingredient;
    public float units; // レシピでの使用量（単位）
}

[CreateAssetMenu(menuName = "Wagashi/Recipe")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public List<IngredientAmount> ingredients = new();
    public Vector5 FlavorVector()
    {
        Vector5 sum = Vector5.zero;
        foreach (var ia in ingredients)
        {
            var i = ia.ingredient;
            sum += new Vector5(i.sweet, i.bitter, i.chewy, i.bean, i.fruit) * ia.units;
        }
        return sum.Normalized();
    }
    public float SugarGrams()
    {
        float g = 0f;
        foreach (var ia in ingredients) g += ia.ingredient.sugarGramsPerUnit * ia.units;
        return g;
    }
    public float Kcal() => SugarGrams() * 4f; // シンプル版
}

[System.Serializable]
public struct Vector5
{
    public float x, y, z, w, v;
    public Vector5(float a, float b, float c, float d, float e) { x = a; y = b; z = c; w = d; v = e; }
    public static Vector5 zero => new(0, 0, 0, 0, 0);
    public static Vector5 operator +(Vector5 a, Vector5 b)
        => new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w, a.v + b.v);
    public static Vector5 operator *(Vector5 a, float s)
        => new(a.x * s, a.y * s, a.z * s, a.w * s, a.v * s);
    public float Dot(Vector5 b) => x * b.x + y * b.y + z * b.z + w * b.w + v * b.v;
    public float Magnitude() => Mathf.Sqrt(Dot(this));
    public Vector5 Normalized()
    {
        float m = Magnitude();
        return m > 1e-5f ? this * (1f / m) : zero;
    }
}
