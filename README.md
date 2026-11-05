# 🏭 Smart MES Operations Center (MesMonitor)

C# (.NET 8 WPF)과 **LiveCharts2 (v2.0)**, **CommunityToolkit.Mvvm**을 기반으로 개발된 스마트 제조 실행 시스템(MES: Manufacturing Execution System) 실시간 관제 솔루션 포트폴리오입니다.

---

## 🛠️ 기술 스택 (Tech Stack)

| 구분 | 기술 / 라이브러리 | 설명 |
| :--- | :--- | :--- |
| **Framework** | .NET 8.0 (WPF) | 고성능 Windows 데스크톱 UI 애플리케이션 |
| **Chart Library** | **LiveChartsCore.SkiaSharpView.WPF (v2.0.5)** | 실시간 GPU 가속 데이터 시각화 차트 엔진 |
| **Architecture** | **MVVM Pattern** (`CommunityToolkit.Mvvm`) | 느슨한 결합의 유지보수 용이한 아키텍처 |
| **Theme / Design** | **Dark Industrial Operations Theme** | 생산 관제 센터 실무 표준 다크 모드 UI |
| **Data Engine** | **MesDataSimulator (Local Engine)** | 1.5초 주기로 설비 센서, 생산 수량, 알람을 갱신하는 시뮬레이터 |

---

## 🖥️ 구현된 5대 핵심 화면 구성

```
[MainWindow]
 ├── 📊 1. 실시간 생산 대시보드 (Dashboard & Andon)
 ├── ⚙️ 2. 설비 센서 모니터링 (IoT Sensor & Diagnostics)
 ├── 📈 3. 품질 & SPC 관리도 (Pareto & Xbar Control Chart)
 ├── 📋 4. 작업지시 및 실적 관리 (Work Order Management)
 └── 🔔 5. 알람 이력 & 기준정보 설정 (Alarm Logs & Master Configuration)
```

### 1. 실시간 공정 모니터링 대시보드 (`DashboardView`)
* **핵심 KPI 요약:** 당일 생산 목표, 실시간 생산 실적, 불량 수량, 종합설비효율(OEE).
* **OEE 도넛 차트 (LiveCharts2 `PieChart`):** 시간 가동률(Availability), 성능 가동률(Performance), 양품률(Quality) 3대 지표 분해.
* **시간대별 생산 추이 (LiveCharts2 `CartesianChart`):** 시간대별 목표 vs 실적 비교 막대 차트.
* **안돈(Andon) 라인 현황 카드:** 라인별 가동/비가동/경보 상태 배지, 실시간 수량 및 프로그레스 바.
* **실시간 알람 티커:** 미확인 알람 카운트 및 이벤트 피드.

### 2. 설비 센서 모니터링 & 상태 분석 (`EquipmentSensorView`)
* **설비 선택 셀렉터:** 대상 설비(SMT 1호기, CNC 2호기, 사출 3호기, 조립 4호기) 전환.
* **실시간 멀티 센서 트렌드 차트 (LiveCharts2 Multi-Y Axis `CartesianChart`):**
  * 온도(°C) - 좌측 Y축
  * 압력(bar) 및 진동(G) - 우측 Y축
  * 1.5초 주기로 슬라이딩 윈도우 큐를 통해 실시간 데이터가 유입되는 부드러운 애니메이션 구현.
* **설비 상태 제어 시뮬레이터:** [정상 가동], [일시 대기], [경보 발생] 버튼을 통해 상태 및 센서 변동 시뮬레이션 지원.
* **예지보전 임계치 가이드:** 온도/진동/압력의 안전/경고/위험 기준선 표기.

### 3. 품질 및 SPC 공정 분석 화면 (`QualitySpcView`)
* **불량 파레토 차트 (Pareto Chart - 80/20 법칙):**
  * `ColumnSeries`: 불량 유형별 발생 건수 (내림차순).
  * `LineSeries`: 누적 백분율 (오른쪽 Y축, 0~100%).
* **Xbar-R 관리도 (Control Chart):**
  * UCL (상한선: 25.45mm 점선), CL (중심선: 25.00mm 점선), LCL (하한선: 24.55mm 점선).
  * 실시간 샘플링 타점 및 규격 이탈(Out of Spec) 검출.
* **품질 지표 카드:** 총 검사 수량, 불량률(%), PPM, 공정능력지수(Cpk: 1.33, Cp: 1.48).

### 4. 생산 계획 및 실적 관리 (`WorkOrderView`)
* **상태별 필터링:** 전체 / 진행중 / 대기 / 완료 조건 검색.
* **작업지시 그리드 (DataGrid):** 지시번호, 라인, 품목코드, 품목명, 상태 태그, 계획/실적 수량, 프로그레스 바.
* **신규 작업지시 발행 폼:** 품목 및 수량 입력 후 원클릭 지시 생성 및 실시간 목록 반영.
* **작업 제어 액션:** [완료 처리] 버튼을 통한 오더 마감 시뮬레이션.
* **오더 상태 점유 도넛 차트:** 진행/대기/완료 비율 시각화.

### 5. 알람 이력 및 기준정보 설정 (`HistoryAlarmView`)
* **이상 심각도(Severity) 필터:** Critical / Warning / Info 별 검색.
* **알람 조치 기능:** 미확인 알람에 대한 [확인] 처리 및 상태 변경.
* **CSV 내보내기 기능:** 바탕화면으로 알람 이력 즉시 CSV 파일 추출.
* **인터페이스 마스터 정보:** 통신 프로토콜 현황(OPC-UA, FANUC FOCAS, Euromap 63, Mitsubishi MC) 및 시스템 주기 설정 확인.

---

## 📂 프로젝트 구조 (Project Directory)

```
MesMonitor/
 ├── Converters/
 │    └── StatusToBrushConverter.cs     # 상태값(가동,경보,심각도 등) 색상 변환기
 ├── Models/
 │    ├── LineStatusItem.cs             # 설비 라인 모델
 │    ├── SensorDataPoint.cs            # 실시간 센서 시계열 모델
 │    ├── WorkOrderItem.cs              # 작업 지시 모델
 │    ├── DefectItem.cs                 # 불량 통계 모델
 │    ├── SpcDataPoint.cs               # SPC 관리도 샘플 모델
 │    └── AlarmItem.cs                  # 알람 로그 모델
 ├── Services/
 │    ├── IMesDataSimulator.cs          # 시뮬레이터 인터페이스
 │    └── MesDataSimulator.cs           # 백그라운드 실시간 데이터 생성 엔진
 ├── ViewModels/
 │    ├── ViewModelBase.cs              # ObservableObject 기반 공통 베이스
 │    ├── MainViewModel.cs              # 내비게이션, 시계, 글로벌 상태 제어
 │    ├── DashboardViewModel.cs         # 메인 대시보드 뷰모델
 │    ├── EquipmentSensorViewModel.cs   # 센서 모니터링 뷰모델
 │    ├── QualitySpcViewModel.cs        # 파레토 및 SPC 뷰모델
 │    ├── WorkOrderViewModel.cs         # 작업지시 뷰모델
 │    └── HistoryAlarmViewModel.cs      # 알람 및 설정 뷰모델
 ├── Views/
 │    ├── DashboardView.xaml            # 대시보드 화면
 │    ├── EquipmentSensorView.xaml      # 센서 트렌드 화면
 │    ├── QualitySpcView.xaml           # 품질 & SPC 화면
 │    ├── WorkOrderView.xaml            # 작업지시 화면
 │    └── HistoryAlarmView.xaml         # 알람 이력 화면
 ├── App.xaml                           # 전역 테마, 스타일, DataTemplate 정의
 ├── MainWindow.xaml                    # 메인 프레임 (좌측 메뉴 + 상단 헤더)
 └── MesMonitor.csproj                  # .NET 8 WPF 프로젝트 파일
```

---

## 🚀 실행 방법 (How to Run)

### Visual Studio에서 실행:
1. `MesMonitor.sln` 또는 `MesMonitor.slnx` 파일을 Visual Studio 2022 이상에서 오픈합니다.
2. 상단 시작 버튼 (`F5` 또는 `Ctrl+F5`)을 누르면 즉시 빌드 및 실행됩니다.

