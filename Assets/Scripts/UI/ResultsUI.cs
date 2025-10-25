using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class ResultsUI : MonoBehaviour
{
    public Text SummaryText;  // 売上/満足度/販売数
    public Text ReasonsText;  // 理由サンプル
    public Text TownText;     // 町の構成（営業後）

    public void Show(DaySimulator.DayResult r, DaySimulator sim)
    {
        if (SummaryText)
        {
            SummaryText.text =
                $"売上: {r.Revenue} 円\n" +
                $"単品: {r.SoldCount} 個  / セット: {r.SoldSetCount} 個\n" +
                $"平均満足度: {r.AvgSatisfaction:0.00}";
        }

        if (ReasonsText)
        {
            var sb = new StringBuilder();
            if (r.SampleReasons != null && r.SampleReasons.Count > 0)
            {
                sb.AppendLine("購入理由（サンプル）");
                for (int i = 0; i < r.SampleReasons.Count; i++)
                    sb.AppendLine($"・{r.SampleReasons[i]}");
            }
            else
            {
                sb.Append("（理由ログなし）");
            }
            ReasonsText.text = sb.ToString();
        }

        if (TownText)
        {
            var p = sim.City.Proportions;
            var segs = sim.Segments;
            var sb = new StringBuilder("Town Composition (営業後)\n");
            for (int i = 0; i < p.Length; i++)
            {
                var name = segs[i] ? segs[i].SegName : $"Seg{i}";
                sb.AppendLine($"{name}: {(p[i] * 100f):0.0}%");
            }
            TownText.text = sb.ToString();
        }
    }
}
