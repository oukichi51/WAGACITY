using UnityEngine;

[CreateAssetMenu(menuName = "Wagashi/Segment")]
public class SegmentSO : ScriptableObject
{
    public string segName; // 学生/社会人/シニアなど
    [Range(0, 2f)] public float sweet, bitter, chewy, bean, fruit; // 基礎好み
    [Range(0.0f, 0.02f)] public float priceSensitivity = 0.005f; // 円→効用
    [Range(0f, 800f)] public float kcalOk = 300f; // 許容基準
    [Range(50f, 400f)] public float kcalSpan = 150f; // 越えるほどペナルティ
    public float noveltyLove = 0.1f; // 新奇性ブースト
}
