using UnityEngine;
public class IngredientTest : MonoBehaviour
{
    public IngredientSO Sugar, ShiratamaFlour, Koshian, Matcha, Strawberry;
    void Start()
    {
        Debug.Log($"[Ingredient] {Sugar.ingredientName} sweet={Sugar.sweet} sugar={Sugar.sugarGramsPerUnit}g/unit");
        Debug.Log($"[Ingredient] {Koshian.ingredientName} bean={Koshian.bean} sweet={Koshian.sweet}");
    }
}
