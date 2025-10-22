using UnityEngine;

[CreateAssetMenu(menuName = "Wagashi/UniqueNPC")]
public class UniqueNPCSO : ScriptableObject
{
    public string npcName;
    public SegmentSO baseSegment;
    public float loyalty = 0f;
    public float spawnDailyProb = 0.02f;
}
