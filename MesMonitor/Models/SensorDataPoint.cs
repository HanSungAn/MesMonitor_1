namespace MesMonitor.Models;

/// <summary>
/// 설비에서 수집되는 실시간 시계열(Time-series) IoT 센서 계측 데이터 모델
/// </summary>
public class SensorDataPoint
{
    /// <summary>
    /// 센서 계측 시각
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// 노즐/챔버 온도 계측값 (°C)
    /// </summary>
    public double Temperature { get; set; }

    /// <summary>
    /// 유압/공압 압력 계측값 (bar)
    /// </summary>
    public double Pressure { get; set; }

    /// <summary>
    /// 주축 및 구동부 진동 가속도 계측값 (G)
    /// </summary>
    public double Vibration { get; set; }

    /// <summary>
    /// 모터 회전 속도 (RPM)
    /// </summary>
    public double Rpm { get; set; }

    /// <summary>
    /// 실시간 순시 소비 전력 (kW)
    /// </summary>
    public double PowerUsage { get; set; }
}

