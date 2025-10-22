using UnityEngine;

[CreateAssetMenu(menuName = "Wagashi/Segment")]
public class SegmentSO : ScriptableObject
{
    public string segName; // �w��/�Љ�l/�V�j�A�Ȃ�
    [Range(0, 2f)] public float sweet, bitter, chewy, bean, fruit; // ��b�D��
    [Range(0.0f, 0.02f)] public float priceSensitivity = 0.005f; // �~�����p
    [Range(0f, 800f)] public float kcalOk = 300f; // ���e�
    [Range(50f, 400f)] public float kcalSpan = 150f; // �z����قǃy�i���e�B
    public float noveltyLove = 0.1f; // �V��u�[�X�g
}
