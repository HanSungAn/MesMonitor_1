namespace MesMonitor.Models;

/// <summary>
/// 제조 설비 및 생산 라인의 현재 가동 상태를 나타내는 열거형
/// </summary>
public enum MachineState
{
    /// <summary>
    /// 정상 가동 중 (생산 진행 중)
    /// </summary>
    Running,

    /// <summary>
    /// 대기 중 (작업 대기 또는 무부하 상태)
    /// </summary>
    Idle,

    /// <summary>
    /// 경보/알람 발생 (이상 정지 상태)
    /// </summary>
    Alarm,

    /// <summary>
    /// 정기 보수 및 점검 중
    /// </summary>
    Maintenance
}

/// <summary>
/// 생산 라인 현황 및 안돈(Andon) 모니터링 정보를 담는 모델 클래스
/// </summary>
public class LineStatusItem
{
    /// <summary>
    /// 라인 고유 식별자 (예: L01, L02)
    /// </summary>
    public string LineId { get; set; } = string.Empty;

    /// <summary>
    /// 라인 명칭 (예: Line 01 - SMT 표면실장)
    /// </summary>
    public string LineName { get; set; } = string.Empty;

    /// <summary>
    /// 설비 가동 상태 (Running, Idle, Alarm, Maintenance)
    /// </summary>
    public MachineState State { get; set; } = MachineState.Running;

    /// <summary>
    /// 현재 라인에서 진행 중인 작업지시 번호 (예: WO-261105-01)
    /// </summary>
    public string CurrentOrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 생산 중인 대상 품목명
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 목표 생산 수량 (EA)
    /// </summary>
    public int TargetQty { get; set; }

    /// <summary>
    /// 현재까지 생산된 실적 수량 (EA)
    /// </summary>
    public int ProducedQty { get; set; }

    /// <summary>
    /// 생산 과정에서 발생한 불량 수량 (EA)
    /// </summary>
    public int DefectQty { get; set; }

    /// <summary>
    /// 생산 진행률 (목표 수량 대비 생산 수량 백분율 %)
    /// </summary>
    public double ProgressRate => TargetQty > 0 ? Math.Round((double)ProducedQty / TargetQty * 100, 1) : 0;

    /// <summary>
    /// 종합 라인 가동 효율 (%)
    /// </summary>
    public double Efficiency { get; set; }

    /// <summary>
    /// 설비 현재 측정 온도 (°C)
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    /// 주축/모터 회전 속도 (RPM)
    /// </summary>
    public double Rpm { get; set; }
}

