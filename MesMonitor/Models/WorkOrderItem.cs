namespace MesMonitor.Models;

/// <summary>
/// 작업지시(Work Order)의 진행 단계를 정의하는 열거형
/// </summary>
public enum OrderStatus
{
    /// <summary>
    /// 작업 대기 (라인 배정 완료 및 투입 대기 상태)
    /// </summary>
    Waiting,

    /// <summary>
    /// 생산 진행 중
    /// </summary>
    InProgress,

    /// <summary>
    /// 목표 수량 생산 완료
    /// </summary>
    Completed,

    /// <summary>
    /// 라인 정지 또는 자재 부족으로 인한 일시 중단
    /// </summary>
    Paused
}

/// <summary>
/// 생산 작업지시(Work Order)의 정보 및 진행 실적을 관리하는 모델 클래스
/// </summary>
public class WorkOrderItem
{
    /// <summary>
    /// 작업지시 식별 번호 (예: WO-261105-01)
    /// </summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 배정된 생산 라인 코드 (예: L01, L02)
    /// </summary>
    public string LineId { get; set; } = string.Empty;

    /// <summary>
    /// 생산 품목 코드 (예: PRD-PCB-03)
    /// </summary>
    public string ProductCode { get; set; } = string.Empty;

    /// <summary>
    /// 생산 품목 명칭
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 지시 계획 수량 (EA)
    /// </summary>
    public int PlanQty { get; set; }

    /// <summary>
    /// 현재까지 생산된 실적 수량 (EA)
    /// </summary>
    public int ActualQty { get; set; }

    /// <summary>
    /// 발생한 불량 수량 (EA)
    /// </summary>
    public int DefectQty { get; set; }

    /// <summary>
    /// 작업지시 진행 상태 (Waiting, InProgress, Completed, Paused)
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// 작업 시작 일시
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// 작업 완료 납기 일시
    /// </summary>
    public DateTime DueDate { get; set; }

    /// <summary>
    /// 계획 대비 실적 진행률 (최대 100%, 소수점 1자리 %)
    /// </summary>
    public double ProgressRate => PlanQty > 0 ? Math.Min(100.0, Math.Round((double)ActualQty / PlanQty * 100, 1)) : 0;
}

