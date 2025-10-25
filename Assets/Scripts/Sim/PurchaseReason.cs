using UnityEngine;

public enum ReasonTag
{
    Flavor,      // ���x�N�g���̈�v�idot�j
    Attention,   // �C�`�I�V/���
    Novelty,     // �V��u�[�X�g
    Price,       // ���i�y�i���e�B�i���e���j
    Calorie      // �J�����[�y�i���e�B�i���e���j
}

public static class ReasonFormat
{
    public static string ToPretty(ReasonTag tag, float value)
    {
        switch (tag)
        {
            case ReasonTag.Flavor: return $"����������(+{value:0.00})";
            case ReasonTag.Attention: return $"���ړx(+{value:0.00})";
            case ReasonTag.Novelty: return $"�V�(+{value:0.00})";
            case ReasonTag.Price: return $"���i(-{Mathf.Abs(value):0.00})";
            case ReasonTag.Calorie: return $"�J�����[(-{Mathf.Abs(value):0.00})";
            default: return "���̑�";
        }
    }
}
