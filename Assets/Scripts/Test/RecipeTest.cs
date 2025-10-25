using UnityEngine;
public class RecipeTest : MonoBehaviour
{
    public RecipeSO StrawberryDaifuku;
    void Start()
    {
        var f = StrawberryDaifuku.FlavorVector();
        Debug.Log($"[{StrawberryDaifuku.RecipeName}] kcal={StrawberryDaifuku.Kcal():0}  flavor=({f.X:0.00},{f.Y:0.00},{f.Z:0.00},{f.W:0.00},{f.V:0.00})");
    }
}
