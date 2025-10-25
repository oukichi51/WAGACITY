using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Wagashi/Segment")]
public class SegmentSO : ScriptableObject
{
    [FormerlySerializedAs("segName")] public string SegName;

    [FormerlySerializedAs("sweet")][Range(0, 2f)] public float Sweet;
    [FormerlySerializedAs("bitter")][Range(0, 2f)] public float Bitter;
    [FormerlySerializedAs("chewy")][Range(0, 2f)] public float Chewy;
    [FormerlySerializedAs("bean")][Range(0, 2f)] public float Bean;
    [FormerlySerializedAs("fruit")][Range(0, 2f)] public float Fruit;

    [FormerlySerializedAs("priceSensitivity")][Range(0.0f, 0.02f)] public float PriceSensitivity = 0.005f;
    [FormerlySerializedAs("kcalOk")][Range(0f, 800f)] public float KcalOk = 300f;
    [FormerlySerializedAs("kcalSpan")][Range(50f, 400f)] public float KcalSpan = 150f;
    [FormerlySerializedAs("noveltyLove")] public float NoveltyLove = 0.1f;
}
