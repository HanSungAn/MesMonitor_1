namespace MesMonitor.Models;

/// <summary>
/// 통계적 공정 관리(SPC)의 Xbar 관리도 작성을 위한 시료 측정 데이터 모델
/// </summary>
public class SpcDataPoint
{
    /// <summary>
    /// 검사 시료 번호 또는 차수 (예: #1, #2 ...)
    /// </summary>
    public int SampleIndex { get; set; }

    /// <summary>
    /// 실제 측정된 품질 규격 계측값 (예: 가공 치수 mm)
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// 관리 상한선 (Upper Control Limit: UCL = CL + 3σ)
    /// </summary>
    public double UCL { get; set; }

    /// <summary>
    /// 관리 중심선 (Center Line: CL, 규격 기준 목표치)
    /// </summary>
    public double CL { get; set; }

    /// <summary>
    /// 관리 하한선 (Lower Control Limit: LCL = CL - 3σ)
    /// </summary>
    public double LCL { get; set; }

    /// <summary>
    /// 관리 한계 이탈(Out of Spec) 여부 판정 (UCL 초과 또는 LCL 미만)
    /// </summary>
    public bool IsOutOfSpec => Value > UCL || Value < LCL;
}

