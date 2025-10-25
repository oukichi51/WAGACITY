using UnityEngine;
public class IngredientTest : MonoBehaviour
{
    public IngredientSO Sugar, ShiratamaFlour, Koshian, Matcha, Strawberry;
    void Start()
    {
        Debug.Log($"[Ingredient] {Sugar.IngredientName} sweet={Sugar.Sweet} sugar={Sugar.SugarGramsPerUnit}g/unit");
        Debug.Log($"[Ingredient] {Koshian.IngredientName} bean={Koshian.Bean} sweet={Koshian.Sweet}");
    }
}
