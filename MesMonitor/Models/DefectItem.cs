namespace MesMonitor.Models;

/// <summary>
/// 품질 분석 및 파레토 차트(Pareto Chart) 시각화를 위한 불량 유형별 통계 모델
/// </summary>
public class DefectItem
{
    /// <summary>
    /// 불량 유형 명칭 (예: 표면 스크래치, 치수 공차 미달, 기포 발생 등)
    /// </summary>
    public string DefectType { get; set; } = string.Empty;

    /// <summary>
    /// 해당 불량 유형의 발생 건수 (EA)
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 전체 불량 건수 대비 해당 불량의 점유 비율 (%)
    /// </summary>
    public double Percentage { get; set; }

    /// <summary>
    /// 파레토 분석(80/20 법칙)용 누적 점유율 백분율 (%)
    /// </summary>
    public double CumulativePercentage { get; set; }
}

