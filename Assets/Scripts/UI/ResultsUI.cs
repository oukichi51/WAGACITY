using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ResultsUI : MonoBehaviour
{
    public Text SummaryText;  // ����/�����x/�̔���
    public Text ReasonsText;  // ���R�T���v��
    public Text TownText;     // ���̍\���i�c�ƌ�j

    public void Show(DaySimulator.DayResult r, DaySimulator sim)
    {
        if (SummaryText)
        {
            SummaryText.text =
                $"����: {r.Revenue} �~\n" +
                $"�P�i: {r.SoldCount} ��  / �Z�b�g: {r.SoldSetCount} ��\n" +
                $"���ϖ����x: {r.AvgSatisfaction:0.00}";
        }

        if (ReasonsText)
        {
            var sb = new StringBuilder();
            if (r.SampleReasons != null && r.SampleReasons.Count > 0)
            {
                sb.AppendLine("�w�����R�i�T���v���j");
                for (int i = 0; i < r.SampleReasons.Count; i++)
                    sb.AppendLine($"�E{r.SampleReasons[i]}");
            }
            else
            {
                sb.Append("�i���R���O�Ȃ��j");
            }
            ReasonsText.text = sb.ToString();
        }

        if (TownText)
        {
            var p = sim.City.Proportions;
            var segs = sim.Segments;
            var sb = new StringBuilder("Town Composition (�c�ƌ�)\n");
            for (int i = 0; i < p.Length; i++)
            {
                var name = segs[i] ? segs[i].SegName : $"Seg{i}";
                sb.AppendLine($"{name}: {(p[i] * 100f):0.0}%");
            }
            TownText.text = sb.ToString();
        }
    }
}
