using UnityEngine;

public enum ReasonTag
{
    Flavor,      // 味ベクトルの一致（dot）
    Attention,   // イチオシ/定番
    Novelty,     // 新奇性ブースト
    Price,       // 価格ペナルティ（悪影響）
    Calorie      // カロリーペナルティ（悪影響）
}

public static class ReasonFormat
{
    public static string ToPretty(ReasonTag tag, float value)
    {
        switch (tag)
        {
            case ReasonTag.Flavor: return $"味が合った(+{value:0.00})";
            case ReasonTag.Attention: return $"注目度(+{value:0.00})";
            case ReasonTag.Novelty: return $"新奇性(+{value:0.00})";
            case ReasonTag.Price: return $"価格(-{Mathf.Abs(value):0.00})";
            case ReasonTag.Calorie: return $"カロリー(-{Mathf.Abs(value):0.00})";
            default: return "その他";
        }
    }
}
