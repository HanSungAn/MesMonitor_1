namespace MesMonitor.Models;

/// <summary>
/// 설비 알람 및 이벤트의 위험도/심각도를 나타내는 열거형
/// </summary>
public enum AlarmSeverity
{
    /// <summary>
    /// 일반 정보성 알림 (예: 자재 부족 사전 알림, 작업 시작 안내)
    /// </summary>
    Info,

    /// <summary>
    /// 주의 및 경고 수준 (설비 가동은 유지되나 사전 점검이 필요한 상태)
    /// </summary>
    Warning,

    /// <summary>
    /// 긴급 위험 수준 (설비 이상 인터록 발생 또는 즉각 조치가 필요한 상태)
    /// </summary>
    Critical
}

/// <summary>
/// 설비 경보(Alarm) 및 이상 발생 이벤트 이력을 관리하는 모델 클래스
/// </summary>
public class AlarmItem
{
    /// <summary>
    /// 알람 발생 시각
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 발생 설비/라인 코드 (예: L01, L03)
    /// </summary>
    public string EquipmentId { get; set; } = string.Empty;

    /// <summary>
    /// 발생 설비 명칭 (예: Line 03 - 사출기 Noズル)
    /// </summary>
    public string EquipmentName { get; set; } = string.Empty;

    /// <summary>
    /// 알람 심각도 (Info, Warning, Critical)
    /// </summary>
    public AlarmSeverity Severity { get; set; }

    /// <summary>
    /// 알람 상세 내용 및 원인 메시지
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 작업자 또는 관리자의 알람 인지/확인(Acknowledge) 처리 여부
    /// </summary>
    public bool IsAcknowledged { get; set; }
}

