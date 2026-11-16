# MES Solution Portfolio Presentation Generator
$ErrorActionPreference = "Stop"

function RGB([int]$r, [int]$g, [int]$b) {
    return [int]($r + ($g * 256) + ($b * 65536))
}

$outputPath = "D:\workspace\MES Sample\MesMonitor(C#)\docs\MesMonitor_Portfolio.pptx"

# Image Paths (Latest High-Res Screenshots)
$imgDashboard   = "D:\workspace\MES Sample\MesMonitor(C#)\screenshots\dashboard.png"
$imgEquipment   = "D:\workspace\MES Sample\MesMonitor(C#)\screenshots\equipment.png"
$imgQuality     = "D:\workspace\MES Sample\MesMonitor(C#)\screenshots\quality.png"
$imgWorkOrder   = "D:\workspace\MES Sample\MesMonitor(C#)\screenshots\workorder.png"
$imgAlarm       = "D:\workspace\MES Sample\MesMonitor(C#)\screenshots\history.png"

Write-Host "Starting PowerPoint Application..."
$ppt = New-Object -ComObject PowerPoint.Application
$pres = $ppt.Presentations.Add([Microsoft.Office.Core.MsoTriState]::msoFalse)
$pres.PageSetup.SlideWidth = 960
$pres.PageSetup.SlideHeight = 540

# Color Palette
$cBgDark       = RGB 11 19 43       # #0B132B (Window Background)
$cCardDark     = RGB 28 37 65       # #1C2541 (Card Background)
$cSubCardDark  = RGB 22 31 56       # #161F38 (SubCard Background)
$cBorder       = RGB 58 80 107      # #3A506B (Border)
$cAccentBlue   = RGB 2 132 199      # #0284C7 (Brand Accent)
$cAccentCyan   = RGB 56 189 248     # #38BDF8 (Cyan Accent)
$cTextWhite    = RGB 255 255 255    # #FFFFFF (White)
$cTextSlate    = RGB 203 213 225    # #CBD5E1 (Light Slate)
$cTextMuted    = RGB 148 163 184    # #94A3B8 (Muted Gray)
$cSuccess      = RGB 16 185 129     # #10B981 (Success Green)
$cWarning      = RGB 245 158 11     # #F59E0B (Warning Amber)
$cDanger       = RGB 239 68 68      # #EF4444 (Danger Red)

# Helper: Create Header
function Add-SlideHeader($slide, [string]$badge, [string]$title) {
    # Top decorative bar
    $bar = $slide.Shapes.AddShape(1, 0, 0, 960, 4)
    $bar.Fill.Solid()
    $bar.Fill.ForeColor.RGB = $cAccentCyan
    $bar.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse

    # Badge text
    $bBox = $slide.Shapes.AddTextbox(1, 40, 24, 600, 20)
    $bBox.TextFrame.MarginLeft = 0; $bBox.TextFrame.MarginTop = 0
    $bRange = $bBox.TextFrame.TextRange
    $bRange.Text = $badge
    $bRange.Font.Name = "Segoe UI"
    $bRange.Font.Size = 10
    $bRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $bRange.Font.Color.RGB = $cAccentCyan

    # Title text
    $tBox = $slide.Shapes.AddTextbox(1, 40, 42, 700, 36)
    $tBox.TextFrame.MarginLeft = 0; $tBox.TextFrame.MarginTop = 0
    $tRange = $tBox.TextFrame.TextRange
    $tRange.Text = $title
    $tRange.Font.Name = "Malgun Gothic"
    $tRange.Font.Size = 20
    $tRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $tRange.Font.Color.RGB = $cTextWhite

    # Right Project Tag
    $tag = $slide.Shapes.AddShape(5, 760, 32, 160, 26)
    $tag.Fill.Solid(); $tag.Fill.ForeColor.RGB = $cSubCardDark
    $tag.Line.ForeColor.RGB = $cBorder; $tag.Line.Weight = 1
    $tagRange = $tag.TextFrame.TextRange
    $tagRange.Text = "MesMonitor (C# WPF)"
    $tagRange.Font.Name = "Segoe UI"
    $tagRange.Font.Size = 10
    $tagRange.Font.Color.RGB = $cTextSlate
}

# ==========================================
# SLIDE 1: Cover Slide
# ==========================================
Write-Host "Creating Slide 1: Cover..."
$s1 = $pres.Slides.Add(1, 12)
$bg1 = $s1.Shapes.AddShape(1, 0, 0, 960, 540)
$bg1.Fill.Solid(); $bg1.Fill.ForeColor.RGB = $cBgDark
$bg1.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse

# Top Brand Line
$topLine = $s1.Shapes.AddShape(1, 0, 0, 960, 6)
$topLine.Fill.Solid(); $topLine.Fill.ForeColor.RGB = $cAccentCyan
$topLine.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse

# Main Card Box
$card1 = $s1.Shapes.AddShape(5, 60, 60, 840, 420)
$card1.Fill.Solid(); $card1.Fill.ForeColor.RGB = $cCardDark
$card1.Line.ForeColor.RGB = $cBorder; $card1.Line.Weight = 1.5

# Brand Badge
$badge1 = $s1.Shapes.AddShape(5, 100, 95, 290, 28)
$badge1.Fill.Solid(); $badge1.Fill.ForeColor.RGB = $cAccentBlue
$badge1.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
$bText1 = $badge1.TextFrame.TextRange
$bText1.Text = "SMART FACTORY OPERATIONS CENTER"
$bText1.Font.Name = "Segoe UI"
$bText1.Font.Size = 11
$bText1.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$bText1.Font.Color.RGB = $cTextWhite

# Title Text
$tBox1 = $s1.Shapes.AddTextbox(1, 100, 135, 760, 110)
$tBox1.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
$tRange1 = $tBox1.TextFrame.TextRange
$tRange1.Text = "스마트 제조 실행 시스템 (MES)`r`n실시간 공정 관제 & 진단 솔루션"
$tRange1.Font.Name = "Malgun Gothic"
$tRange1.Font.Size = 28
$tRange1.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$tRange1.Font.Color.RGB = $cTextWhite

# Subtitle
$subBox1 = $s1.Shapes.AddTextbox(1, 100, 255, 760, 40)
$subRange1 = $subBox1.TextFrame.TextRange
$subRange1.Text = "C# (.NET 8 WPF) & LiveCharts2 기반 안돈(Andon) 모니터링, 통계적 공정 분석(SPC), 설비 진단 포트폴리오"
$subRange1.Font.Name = "Malgun Gothic"
$subRange1.Font.Size = 13
$subRange1.Font.Color.RGB = $cAccentCyan

# Divider inside card
$div1 = $s1.Shapes.AddShape(1, 100, 305, 760, 1)
$div1.Fill.Solid(); $div1.Fill.ForeColor.RGB = $cBorder
$div1.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse

# 4 Key Stats Pill Cards at Bottom of Cover
$stats = @(
    @{ Title = "구현 아키텍처"; Val = "MVVM Pattern" },
    @{ Title = "차트 그래픽 엔진"; Val = "LiveCharts2 (GPU)" },
    @{ Title = "실시간 스트리밍"; Val = "1.5s Data Engine" },
    @{ Title = "테마 시스템"; Val = "다크/라이트 동적 전환" }
)
for ($i = 0; $i -lt 4; $i++) {
    $px = 100 + ($i * 195)
    $sc = $s1.Shapes.AddShape(5, $px, 330, 180, 80)
    $sc.Fill.Solid(); $sc.Fill.ForeColor.RGB = $cSubCardDark
    $sc.Line.ForeColor.RGB = $cBorder; $sc.Line.Weight = 1

    $sBox = $s1.Shapes.AddTextbox(1, $px + 10, 340, 160, 60)
    $sBox.TextFrame.MarginLeft = 0; $sBox.TextFrame.MarginTop = 0
    $sRange = $sBox.TextFrame.TextRange
    $sRange.Text = "$($stats[$i].Title)`r`n$($stats[$i].Val)"
    $sRange.Paragraphs(1).Font.Name = "Malgun Gothic"
    $sRange.Paragraphs(1).Font.Size = 10
    $sRange.Paragraphs(1).Font.Color.RGB = $cTextMuted
    $sRange.Paragraphs(2).Font.Name = "Segoe UI"
    $sRange.Paragraphs(2).Font.Size = 12
    $sRange.Paragraphs(2).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $sRange.Paragraphs(2).Font.Color.RGB = $cTextWhite
}

# ==========================================
# SLIDE 2: Project Overview
# ==========================================
Write-Host "Creating Slide 2: Project Overview..."
$s2 = $pres.Slides.Add(2, 12)
$bg2 = $s2.Shapes.AddShape(1, 0, 0, 960, 540)
$bg2.Fill.Solid(); $bg2.Fill.ForeColor.RGB = $cBgDark
$bg2.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s2 "01. PROJECT OVERVIEW" "프로젝트 추진 배경 및 핵심 개발 목표"

$overviewCards = @(
    @{
        Title = "01. 추진 배경 (Background)"
        Icon = "🏭"
        Items = @(
            "스마트 팩토리 제조 현장의 데이터 단절 및 사후 대응 한계 극복",
            "설비 센서, 라인 가동률, 품질(SPC), 작업지시의 단일 화면 실시간 관제 필요",
            "현장 운영자(Operator)와 관리자(Manager) 모두를 만족하는 직관적 UI 요구"
        )
        Color = $cAccentBlue
    },
    @{
        Title = "02. 개발 목표 (Core Goals)"
        Icon = "🎯"
        Items = @(
            ".NET 8 WPF 최신 기술을 활용한 고성능 데스크톱 관제 시스템 구현",
            "초당 1.5회 실시간 시계열 센서 및 생산 데이터의 GPU 가속 차트 시각화",
            "CommunityToolkit.Mvvm 기반의 견고한 관심사 분리(SoC) 아키텍처 정립",
            "운영 환경에 따라 선택 가능한 다크/라이트 모드 실시간 동적 테마 탑재"
        )
        Color = $cSuccess
    },
    @{
        Title = "03. 프로젝트 성과 (Key Highlights)"
        Icon = "🏆"
        Items = @(
            "실시간 관제 5대 핵심 화면(대시보드/센서/SPC/작업지시/알람) 완성",
            "메모리 누수 없는 30포인트 슬라이딩 윈도우 시계열 데이터 엔진 구축",
            "불량 80/20 파레토 차트 및 UCL/LCL 한계선 관리도(Xbar) 연동 성공",
            "WPF와 SkiaSharp Paint 간 동적 테마 연동으로 완성도 높은 UX 제공"
        )
        Color = $cWarning
    }
)

for ($i = 0; $i -lt 3; $i++) {
    $cx = 40 + ($i * 300)
    $item = $overviewCards[$i]

    $cBox = $s2.Shapes.AddShape(5, $cx, 95, 280, 405)
    $cBox.Fill.Solid(); $cBox.Fill.ForeColor.RGB = $cCardDark
    $cBox.Line.ForeColor.RGB = $cBorder; $cBox.Line.Weight = 1.2

    # Card Top Accent
    $cAcc = $s2.Shapes.AddShape(1, $cx + 1, 96, 278, 4)
    $cAcc.Fill.Solid(); $cAcc.Fill.ForeColor.RGB = $item.Color
    $cAcc.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse

    # Card Title
    $hBox = $s2.Shapes.AddTextbox(1, $cx + 16, 115, 248, 30)
    $hRange = $hBox.TextFrame.TextRange
    $hRange.Text = $item.Title
    $hRange.Font.Name = "Malgun Gothic"
    $hRange.Font.Size = 13
    $hRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $hRange.Font.Color.RGB = $cTextWhite

    # Card Content
    $bodyBox = $s2.Shapes.AddTextbox(1, $cx + 16, 155, 248, 330)
    $bodyBox.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $bRange = $bodyBox.TextFrame.TextRange
    $bulletText = ""
    foreach ($txt in $item.Items) {
        $bulletText += "• $txt`r`n`r`n"
    }
    $bRange.Text = $bulletText.TrimEnd()
    $bRange.Font.Name = "Malgun Gothic"
    $bRange.Font.Size = 11
    $bRange.Font.Color.RGB = $cTextSlate
}

# ==========================================
# SLIDE 3: Tech Stack & Architecture
# ==========================================
Write-Host "Creating Slide 3: Tech Stack & Architecture..."
$s3 = $pres.Slides.Add(3, 12)
$bg3 = $s3.Shapes.AddShape(1, 0, 0, 960, 540)
$bg3.Fill.Solid(); $bg3.Fill.ForeColor.RGB = $cBgDark
$bg3.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s3 "02. TECH STACK & ARCHITECTURE" "기술 스택 및 계층형 MVVM 시스템 구조"

# Left Card: Tech Stack Details
$lCard3 = $s3.Shapes.AddShape(5, 40, 95, 420, 405)
$lCard3.Fill.Solid(); $lCard3.Fill.ForeColor.RGB = $cCardDark
$lCard3.Line.ForeColor.RGB = $cBorder; $lCard3.Line.Weight = 1.2

$lTitle3 = $s3.Shapes.AddTextbox(1, 60, 110, 380, 26)
$lTRange3 = $lTitle3.TextFrame.TextRange
$lTRange3.Text = "🛠️ 핵심 기술 스택 명세 (Tech Stack Specification)"
$lTRange3.Font.Name = "Malgun Gothic"; $lTRange3.Font.Size = 12; $lTRange3.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$lTRange3.Font.Color.RGB = $cAccentCyan

$stackRows = @(
    @{ Layer = "Platform / Framework"; Tech = ".NET 8.0 (C# 12) / WPF"; Desc = "최신 64-bit 런타임, 고성능 Windows 데스크톱 클라이언트" },
    @{ Layer = "Chart Engine"; Tech = "LiveChartsCore 2.0.5"; Desc = "SkiaSharp 기반 GPU 가속, 실시간 스트리밍 시각화" },
    @{ Layer = "Architecture"; Tech = "CommunityToolkit.Mvvm 8.4"; Desc = "ObservableObject, RelayCommand, 클린 아키텍처" },
    @{ Layer = "Theme System"; Tech = "WPF DynamicResource Engine"; Desc = "런타임 리소스 딕셔너리 스왑, 다크/라이트 실시간 전환" },
    @{ Layer = "Data Simulator"; Tech = "MesDataSimulator (Local)"; Desc = "1.5초 주기 실시간 센서/수량/알람 발생 엔진" }
)

for ($i = 0; $i -lt $stackRows.Count; $i++) {
    $sy = 145 + ($i * 68)
    $sPill = $s3.Shapes.AddShape(5, 55, $sy, 390, 60)
    $sPill.Fill.Solid(); $sPill.Fill.ForeColor.RGB = $cSubCardDark
    $sPill.Line.ForeColor.RGB = $cBorder; $sPill.Line.Weight = 1

    $rBox = $s3.Shapes.AddTextbox(1, 65, $sy + 6, 370, 50)
    $rBox.TextFrame.MarginLeft = 0; $rBox.TextFrame.MarginTop = 0
    $rRange = $rBox.TextFrame.TextRange
    $rRange.Text = "[$($stackRows[$i].Layer)] $($stackRows[$i].Tech)`r`n$($stackRows[$i].Desc)"
    $rRange.Paragraphs(1).Font.Name = "Segoe UI"
    $rRange.Paragraphs(1).Font.Size = 10
    $rRange.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $rRange.Paragraphs(1).Font.Color.RGB = $cTextWhite
    $rRange.Paragraphs(2).Font.Name = "Malgun Gothic"
    $rRange.Paragraphs(2).Font.Size = 9
    $rRange.Paragraphs(2).Font.Color.RGB = $cTextSlate
}

# Right Card: Architecture Diagram
$rCard3 = $s3.Shapes.AddShape(5, 480, 95, 440, 405)
$rCard3.Fill.Solid(); $rCard3.Fill.ForeColor.RGB = $cCardDark
$rCard3.Line.ForeColor.RGB = $cBorder; $rCard3.Line.Weight = 1.2

$rTitle3 = $s3.Shapes.AddTextbox(1, 500, 110, 400, 26)
$rTRange3 = $rTitle3.TextFrame.TextRange
$rTRange3.Text = "📐 4-Tier MVVM 패턴 아키텍처 설계"
$rTRange3.Font.Name = "Malgun Gothic"; $rTRange3.Font.Size = 12; $rTRange3.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$rTRange3.Font.Color.RGB = $cAccentCyan

$archBlocks = @(
    @{ Name = "View Layer (WPF XAML)"; Color = $cAccentBlue; Items = "DashboardView, EquipmentSensorView, QualitySpcView, WorkOrderView, HistoryAlarmView`r`n(DataTemplate 라우팅, DynamicResource 테마 바인딩, UserControl)" },
    @{ Name = "ViewModel Layer (MVVM Toolkit)"; Color = $cSuccess; Items = "MainViewModel (Navigation & Clock), DashboardViewModel (KPI & Charts),`r`nEquipmentSensorViewModel, QualitySpcViewModel, WorkOrderViewModel" },
    @{ Name = "Service Layer (Domain Engines)"; Color = $cWarning; Items = "ThemeService (다크/라이트 딕셔너리 런타임 스와퍼), ChartThemeHelper,`r`nMesDataSimulator (1.5초 주기 실시간 데이터 스트리밍 스레드)" },
    @{ Name = "Model Layer (Core POCOs)"; Color = $cDanger; Items = "LineStatusItem, SensorDataPoint, WorkOrderItem, DefectItem, SpcDataPoint, AlarmItem" }
)

for ($j = 0; $j -lt 4; $j++) {
    $by = 145 + ($j * 86)
    $blk = $s3.Shapes.AddShape(5, 495, $by, 410, 72)
    $blk.Fill.Solid(); $blk.Fill.ForeColor.RGB = $cSubCardDark
    $blk.Line.ForeColor.RGB = $archBlocks[$j].Color; $blk.Line.Weight = 1.5

    $tPill = $s3.Shapes.AddShape(5, 505, $by - 8, 180, 18)
    $tPill.Fill.Solid(); $tPill.Fill.ForeColor.RGB = $archBlocks[$j].Color
    $tPill.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
    $tPillRange = $tPill.TextFrame.TextRange
    $tPillRange.Text = $archBlocks[$j].Name
    $tPillRange.Font.Name = "Segoe UI"; $tPillRange.Font.Size = 8; $tPillRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue; $tPillRange.Font.Color.RGB = $cTextWhite

    $bText = $s3.Shapes.AddTextbox(1, 505, $by + 14, 390, 50)
    $bText.TextFrame.MarginLeft = 0; $bText.TextFrame.MarginTop = 0
    $bRange = $bText.TextFrame.TextRange
    $bRange.Text = $archBlocks[$j].Items
    $bRange.Font.Name = "Malgun Gothic"
    $bRange.Font.Size = 9.5
    $bRange.Font.Color.RGB = $cTextSlate
}

# ==========================================
# SLIDE 4: Module 1 - DashboardView
# ==========================================
Write-Host "Creating Slide 4: Dashboard & Andon..."
$s4 = $pres.Slides.Add(4, 12)
$bg4 = $s4.Shapes.AddShape(1, 0, 0, 960, 540)
$bg4.Fill.Solid(); $bg4.Fill.ForeColor.RGB = $cBgDark
$bg4.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s4 "03. CORE MODULE 1" "실시간 생산 관제 대시보드 (Dashboard & Andon Monitor)"

# Screenshot on Left
if (Test-Path $imgDashboard) {
    $pic4 = $s4.Shapes.AddPicture($imgDashboard, [Microsoft.Office.Core.MsoTriState]::msoFalse, [Microsoft.Office.Core.MsoTriState]::msoTrue, 40, 100, 530, 390)
    $pic4.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $pic4.Line.ForeColor.RGB = $cBorder; $pic4.Line.Weight = 1.5
}

# Feature Card on Right
$fCard4 = $s4.Shapes.AddShape(5, 590, 100, 330, 390)
$fCard4.Fill.Solid(); $fCard4.Fill.ForeColor.RGB = $cCardDark
$fCard4.Line.ForeColor.RGB = $cBorder; $fCard4.Line.Weight = 1.2

$fBox4 = $s4.Shapes.AddTextbox(1, 605, 115, 300, 360)
$fBox4.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange4 = $fBox4.TextFrame.TextRange
$fRange4.Text = "📊 모듈 주요 기능 및 설계 포인트`r`n`r`n" +
"1. 4대 핵심 생산 KPI 실시간 산출`r`n" +
"• 당일 생산 목표, 실시간 실적, 불량 수량, 종합설비효율(OEE)`r`n" +
"• 실시간 수량 기반 자동 합산 및 효율 연산`r`n`r`n" +
"2. OEE 3대 지표 도넛 차트 (PieChart)`r`n" +
"• 시간 가동률(88.4%) × 성능 가동률(92.8%) × 양품률(98.6%)`r`n" +
"• 실시간 지표 분해 및 테마 연동 범례`r`n`r`n" +
"3. 시간대별 계획 대비 생산 추이 (CartesianChart)`r`n" +
"• 시간대별 목표치 vs 실제 실적 비교 막대 차트`r`n" +
"• 다크/라이트 테마에 맞춘 목표 바 고대비 색상 적용`r`n`r`n" +
"4. 안돈(Andon) 라인 모니터 & 프로그레스 바`r`n" +
"• 4개 라인 가동/대기/알람 상태 배지 및 진행률 Bar`r`n" +
"• 수량과 진행률을 상하 분리 배치하여 가독성 극대화`r`n`r`n" +
"5. 실시간 알람 티커 피드`r`n" +
"• 미확인 알람 카운트 실시간 알림 및 최근 이상 감지 피드"

$fRange4.Font.Name = "Malgun Gothic"
$fRange4.Font.Size = 9.5
$fRange4.Font.Color.RGB = $cTextSlate
$fRange4.Paragraphs(1).Font.Size = 11
$fRange4.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange4.Paragraphs(1).Font.Color.RGB = $cAccentCyan

# ==========================================
# SLIDE 5: Module 2 - EquipmentSensorView
# ==========================================
Write-Host "Creating Slide 5: Equipment & IoT Sensors..."
$s5 = $pres.Slides.Add(5, 12)
$bg5 = $s5.Shapes.AddShape(1, 0, 0, 960, 540)
$bg5.Fill.Solid(); $bg5.Fill.ForeColor.RGB = $cBgDark
$bg5.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s5 "04. CORE MODULE 2" "설비 센서 모니터링 & 상태 분석 (IoT Sensor & Diagnostics)"

# Screenshot on Left
if (Test-Path $imgEquipment) {
    $pic5 = $s5.Shapes.AddPicture($imgEquipment, [Microsoft.Office.Core.MsoTriState]::msoFalse, [Microsoft.Office.Core.MsoTriState]::msoTrue, 40, 100, 530, 390)
    $pic5.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $pic5.Line.ForeColor.RGB = $cBorder; $pic5.Line.Weight = 1.5
}

# Feature Card on Right
$fCard5 = $s5.Shapes.AddShape(5, 590, 100, 330, 390)
$fCard5.Fill.Solid(); $fCard5.Fill.ForeColor.RGB = $cCardDark
$fCard5.Line.ForeColor.RGB = $cBorder; $fCard5.Line.Weight = 1.2

$fBox5 = $s5.Shapes.AddTextbox(1, 605, 115, 300, 360)
$fBox5.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange5 = $fBox5.TextFrame.TextRange
$fRange5.Text = "⚙️ 모듈 주요 기능 및 설계 포인트`r`n`r`n" +
"1. 설비별 실시간 스트리밍 셀렉터`r`n" +
"• Line 01 (SMT), Line 02 (CNC), Line 03 (사출), Line 04 (조립)`r`n" +
"• 콤보박스 선택 즉시 대상 설비의 시계열 데이터 자동 전환`r`n`r`n" +
"2. 다중 Y축 (Multi-Y Axis) 실시간 센서 트렌드`r`n" +
"• 좌측 Y축: 챔버/노즐 온도 (°C, 20~110°C 스케일)`r`n" +
"• 우측 Y축: 유압/공압 압력(bar) 및 진동(G) 통합 스케일`r`n" +
"• 1.5초 주기 슬라이딩 윈도우 큐(30 샘플) 기반 부드러운 애니메이션`r`n`r`n" +
"3. 실시간 5대 센서 메트릭 카드`r`n" +
"• 온도(°C), 압력(bar), 진동(G), RPM, 소비전력(kW) 실시간 표시`r`n" +
"• 정상/경고/위험 범위에 따른 직관적 컬러 맵핑`r`n`r`n" +
"4. 설비 상태 제어 시뮬레이터 인터랙션`r`n" +
"• [정상 가동], [일시 대기], [경보 발생] 제어 버튼`r`n" +
"• 상태 변경 시 실시간 센서 수치 급변 및 알람 연계 테스트 가능`r`n`r`n" +
"5. 예지보전(PdM) 임계치 가이드 패널`r`n" +
"• 안전 / 주의 / 위험 임계치 기준 정보 및 자동 인터록 규정 제시"

$fRange5.Font.Name = "Malgun Gothic"
$fRange5.Font.Size = 9.5
$fRange5.Font.Color.RGB = $cTextSlate
$fRange5.Paragraphs(1).Font.Size = 11
$fRange5.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange5.Paragraphs(1).Font.Color.RGB = $cAccentCyan

# ==========================================
# SLIDE 6: Module 3 - QualitySpcView
# ==========================================
Write-Host "Creating Slide 6: Quality & SPC..."
$s6 = $pres.Slides.Add(6, 12)
$bg6 = $s6.Shapes.AddShape(1, 0, 0, 960, 540)
$bg6.Fill.Solid(); $bg6.Fill.ForeColor.RGB = $cBgDark
$bg6.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s6 "05. CORE MODULE 3" "품질 관리 및 통계적 공정 분석 (Pareto & SPC Control Chart)"

# Screenshot on Left
if (Test-Path $imgQuality) {
    $pic6 = $s6.Shapes.AddPicture($imgQuality, [Microsoft.Office.Core.MsoTriState]::msoFalse, [Microsoft.Office.Core.MsoTriState]::msoTrue, 40, 100, 530, 390)
    $pic6.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $pic6.Line.ForeColor.RGB = $cBorder; $pic6.Line.Weight = 1.5
}

# Feature Card on Right
$fCard6 = $s6.Shapes.AddShape(5, 590, 100, 330, 390)
$fCard6.Fill.Solid(); $fCard6.Fill.ForeColor.RGB = $cCardDark
$fCard6.Line.ForeColor.RGB = $cBorder; $fCard6.Line.Weight = 1.2

$fBox6 = $s6.Shapes.AddTextbox(1, 605, 115, 300, 360)
$fBox6.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange6 = $fBox6.TextFrame.TextRange
$fRange6.Text = "📈 모듈 주요 기능 및 설계 포인트`r`n`r`n" +
"1. 불량 파레토 분석 차트 (Pareto 80/20 Rule)`r`n" +
"• ColumnSeries: 불량 유형별 발생 건수 내림차순 정렬`r`n" +
"• LineSeries: 누적 점유율 백분율 (우측 Y축 0~100%)`r`n" +
"• 주요 불량 상위 20%가 전체 불량 80%를 차지하는 현상 시각화`r`n`r`n" +
"2. 통계적 공정 관리 Xbar 관리도 (Control Chart)`r`n" +
"• 기준치: 25.0 ± 0.45 mm`r`n" +
"• UCL (상한선: 25.45 mm, 붉은색 점선)`r`n" +
"• CL (중심선: 25.00 mm, 녹색 점선)`r`n" +
"• LCL (하한선: 24.55 mm, 붉은색 점선)`r`n" +
"• 실시간 측정 실측치의 관리 한계선 이탈(Out of Control) 검출`r`n`r`n" +
"3. 품질 KPI 및 공정능력지수 (Cp / Cpk)`r`n" +
"• 총 검사 수량(3,200 EA), 불량률(2.16%), 21,562 PPM 실시간 산출`r`n" +
"• Cpk = 1.33 (우수 공정 판정), Cp = 1.48 (안정 공정 산포)`r`n`r`n" +
"4. 불량 유형 상세 현황 그리드`r`n" +
"• 표면 스크래치, 치수 공차 미달, 기포 발생 등 점유율 및 누적비율 데이터그리드"

$fRange6.Font.Name = "Malgun Gothic"
$fRange6.Font.Size = 9.5
$fRange6.Font.Color.RGB = $cTextSlate
$fRange6.Paragraphs(1).Font.Size = 11
$fRange6.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange6.Paragraphs(1).Font.Color.RGB = $cAccentCyan

# ==========================================
# SLIDE 7: Module 4 & 5 - WorkOrder & HistoryAlarm
# ==========================================
Write-Host "Creating Slide 7: WorkOrder & HistoryAlarm..."
$s7 = $pres.Slides.Add(7, 12)
$bg7 = $s7.Shapes.AddShape(1, 0, 0, 960, 540)
$bg7.Fill.Solid(); $bg7.Fill.ForeColor.RGB = $cBgDark
$bg7.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s7 "06. CORE MODULE 4 & 5" "생산 계획 및 작업지시 관리 & 알람 이력 / 기준정보 설정"

# Two Smaller Screenshots on Left
if (Test-Path $imgWorkOrder) {
    $pic7_1 = $s7.Shapes.AddPicture($imgWorkOrder, [Microsoft.Office.Core.MsoTriState]::msoFalse, [Microsoft.Office.Core.MsoTriState]::msoTrue, 40, 100, 480, 190)
    $pic7_1.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $pic7_1.Line.ForeColor.RGB = $cBorder; $pic7_1.Line.Weight = 1.2
}
if (Test-Path $imgAlarm) {
    $pic7_2 = $s7.Shapes.AddPicture($imgAlarm, [Microsoft.Office.Core.MsoTriState]::msoFalse, [Microsoft.Office.Core.MsoTriState]::msoTrue, 40, 300, 480, 190)
    $pic7_2.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $pic7_2.Line.ForeColor.RGB = $cBorder; $pic7_2.Line.Weight = 1.2
}

# Right Feature Card
$fCard7 = $s7.Shapes.AddShape(5, 540, 100, 380, 390)
$fCard7.Fill.Solid(); $fCard7.Fill.ForeColor.RGB = $cCardDark
$fCard7.Line.ForeColor.RGB = $cBorder; $fCard7.Line.Weight = 1.2

$fBox7 = $s7.Shapes.AddTextbox(1, 555, 115, 350, 360)
$fBox7.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange7 = $fBox7.TextFrame.TextRange
$fRange7.Text = "📋 생산 계획 및 알람 이력 핵심 명세`r`n`r`n" +
"【모듈 4: 생산 계획 및 작업지시 관리】`r`n" +
"• 작업지시 그리드: 지시번호, 라인, 품목코드/명, 수량, 진행률 Bar`r`n" +
"• 오더 상태 필터: 전체 / 진행중 / 대기 / 완료 조건별 실시간 필터링`r`n" +
"• 신규 작업지시 발행 폼: 라인, 품목, 계획수량 입력 후 원클릭 발행`r`n" +
"• 오더 상태 도넛 차트: 진행중/대기/완료 비율 시각화`r`n" +
"• 현장 조치 제어: [완료 처리] 버튼을 통한 오더 상태 마감 연동`r`n`r`n" +
"【모듈 5: 알람 이력 및 기준정보 설정】`r`n" +
"• 설비 경보 히스토리: 발생 시각, 설비명, 심각도(Critical/Warning/Info)`r`n" +
"• 알람 조치 인터랙션: 미확인 알람 [확인] 클릭 시 확인됨 상태 반영`r`n" +
"• 이력 CSV 파일 내보내기: 전체 알람 이력을 바탕화면 CSV로 즉시 추출`r`n" +
"• 산업 통신 프로토콜 연동 현황 확인:`r`n" +
"   - Line 01 (OPC-UA), Line 02 (FANUC FOCAS), Line 03 (Euromap 63), Line 04 (Mitsubishi MC)"

$fRange7.Font.Name = "Malgun Gothic"
$fRange7.Font.Size = 9
$fRange7.Font.Color.RGB = $cTextSlate
$fRange7.Paragraphs(1).Font.Size = 11
$fRange7.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$fRange7.Paragraphs(1).Font.Color.RGB = $cAccentCyan

# ==========================================
# SLIDE 8: Technical Challenges & Troubleshooting
# ==========================================
Write-Host "Creating Slide 8: Technical Challenges..."
$s8 = $pres.Slides.Add(8, 12)
$bg8 = $s8.Shapes.AddShape(1, 0, 0, 960, 540)
$bg8.Fill.Solid(); $bg8.Fill.ForeColor.RGB = $cBgDark
$bg8.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s8 "07. TROUBLESHOOTING" "기술적 도전 과제 및 트러블슈팅 (Problem Solving)"

$troubles = @(
    @{
        Title = "도전 1. LiveCharts2 & 동적 테마 동기화"
        Problem = "문제: WPF의 DynamicResource(XAML 브러시)는 SkiaSharp의 GPU 캔버스 페인트(SolidColorPaint)와 연동되지 않아 테마 변경 시 차트 배경 및 텍스트 미반영/고착 현상 발생."
        Solution = "해결: ThemeService와 ChartThemeHelper를 설계하여 테마 변경 시 X/Y축 라벨, 격자선, 범례 텍스트, 툴팁 배경의 SolidColorPaint를 런타임에 동적으로 일괄 교체 및 재바인딩 구현."
        Result = "성과: 프로그램 재시작 없이 다크 ↔ 라이트 모드 전환 시 차트와 카드가 100% 실시간 동기화."
        Color = $cAccentBlue
    },
    @{
        Title = "도전 2. GPU 가속 렌더링 및 UI 최적화"
        Problem = "문제: 하드웨어 가속 설정에 따라 차트 컨트롤 배경이 투명할 경우 GPU 캔버스가 검은색으로 클리어되거나, 프로그레스 바와 수량 텍스트가 겹쳐 가독성이 저하되는 현상 발생."
        Solution = "해결: App.xaml.cs에서 RenderOptions.ProcessRenderMode를 Default로 명시하고 차트 컨트롤에 CardBgBrush를 직접 바인딩. 수량 텍스트와 프로그레스 바를 수직 StackPanel로 레이아웃 재배치."
        Result = "성과: GPU 가속 하에서도 깔끔한 카드 일체형 렌더링 유지 및 프로그레스 수치 판독성 대폭 향상."
        Color = $cSuccess
    },
    @{
        Title = "도전 3. 고빈도 시계열 스트리밍 메모리 관리"
        Problem = "문제: 1.5초마다 발생하는 설비 센서 시계열 데이터를 무한정 누적할 경우 메모리 누수 및 WPF 컬렉션 렌더링 부하 발생."
        Solution = "해결: 30개 데이터 포인트 슬라이딩 윈도우 큐 알고리즘(Count > 30 RemoveAt(0))을 적용하고 백그라운드 타이머 이벤트와 UI 스레드 디스패처를 최적 분리."
        Result = "성과: 장시간 연속 구동에도 프로세스 메모리 사용량이 일정하게 유지되며 프레임 드랍 없는 부드러운 차트 갱신 달성."
        Color = $cWarning
    }
)

for ($i = 0; $i -lt 3; $i++) {
    $ty = 95 + ($i * 135)
    $tItem = $troubles[$i]

    $tCard = $s8.Shapes.AddShape(5, 40, $ty, 880, 122)
    $tCard.Fill.Solid(); $tCard.Fill.ForeColor.RGB = $cCardDark
    $tCard.Line.ForeColor.RGB = $tItem.Color; $tCard.Line.Weight = 1.2

    $tBox = $s8.Shapes.AddTextbox(1, 55, $ty + 8, 850, 106)
    $tBox.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $tBox.TextFrame.MarginLeft = 0; $tBox.TextFrame.MarginTop = 0
    $tr = $tBox.TextFrame.TextRange
    $tr.Text = "$($tItem.Title)`r`n$($tItem.Problem)`r`n$($tItem.Solution)`r`n$($tItem.Result)"

    $tr.Paragraphs(1).Font.Name = "Malgun Gothic"
    $tr.Paragraphs(1).Font.Size = 12
    $tr.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $tr.Paragraphs(1).Font.Color.RGB = $cTextWhite

    $tr.Paragraphs(2).Font.Name = "Malgun Gothic"
    $tr.Paragraphs(2).Font.Size = 9.5
    $tr.Paragraphs(2).Font.Color.RGB = $cDanger

    $tr.Paragraphs(3).Font.Name = "Malgun Gothic"
    $tr.Paragraphs(3).Font.Size = 9.5
    $tr.Paragraphs(3).Font.Color.RGB = $cAccentCyan

    $tr.Paragraphs(4).Font.Name = "Malgun Gothic"
    $tr.Paragraphs(4).Font.Size = 9.5
    $tr.Paragraphs(4).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $tr.Paragraphs(4).Font.Color.RGB = $cSuccess
}

# ==========================================
# SLIDE 9: Business Value & Future Roadmap
# ==========================================
Write-Host "Creating Slide 9: Value & Roadmap..."
$s9 = $pres.Slides.Add(9, 12)
$bg9 = $s9.Shapes.AddShape(1, 0, 0, 960, 540)
$bg9.Fill.Solid(); $bg9.Fill.ForeColor.RGB = $cBgDark
$bg9.Line.Visible = [Microsoft.Office.Core.MsoTriState]::msoFalse
Add-SlideHeader $s9 "08. VALUE & ROADMAP" "도입 기대 효과 및 향후 고도화 로드맵 (Conclusion)"

# Left: Business Value Card
$vCard = $s9.Shapes.AddShape(5, 40, 95, 420, 405)
$vCard.Fill.Solid(); $vCard.Fill.ForeColor.RGB = $cCardDark
$vCard.Line.ForeColor.RGB = $cBorder; $vCard.Line.Weight = 1.2

$vTitle = $s9.Shapes.AddTextbox(1, 60, 110, 380, 26)
$vTRange = $vTitle.TextFrame.TextRange
$vTRange.Text = "💎 스마트 팩토리 도입 기대 효과 (Business Value)"
$vTRange.Font.Name = "Malgun Gothic"; $vTRange.Font.Size = 12; $vTRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$vTRange.Font.Color.RGB = $cAccentCyan

$values = @(
    @{ H = "생산 가동률 극대화 & 비가동 최소화"; D = "안돈(Andon) 모니터링을 통해 라인 이상 발생 즉시 현장 작업자 및 관리자에게 시각적 알람을 전파하여 다운타임(비가동 시간)을 최소화" },
    @{ H = "통계적 품질 관리(SPC)로 불량 조기 차단"; D = "Xbar 관리도와 파레토 차트를 통해 공정 이상 징후를 사전에 포착하고 상위 80% 핵심 불량 요인에 집중 대응 가능" },
    @{ H = "실무 수준의 엔터프라이즈 아키텍처 검증"; D = "느슨한 결합의 MVVM 패턴, 런타임 동적 테마 엔진, GPU 가속 렌더링을 적용하여 실무 현장에 즉시 투입 가능한 소프트웨어 완성도 입증" }
)

for ($k = 0; $k -lt 3; $k++) {
    $vy = 150 + ($k * 105)
    $vPill = $s9.Shapes.AddShape(5, 55, $vy, 390, 95)
    $vPill.Fill.Solid(); $vPill.Fill.ForeColor.RGB = $cSubCardDark
    $vPill.Line.ForeColor.RGB = $cBorder; $vPill.Line.Weight = 1

    $vText = $s9.Shapes.AddTextbox(1, 65, $vy + 8, 370, 80)
    $vText.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $vText.TextFrame.MarginLeft = 0; $vText.TextFrame.MarginTop = 0
    $vRange = $vText.TextFrame.TextRange
    $vRange.Text = "• $($values[$k].H)`r`n$($values[$k].D)"
    $vRange.Paragraphs(1).Font.Name = "Malgun Gothic"
    $vRange.Paragraphs(1).Font.Size = 11
    $vRange.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $vRange.Paragraphs(1).Font.Color.RGB = $cTextWhite
    $vRange.Paragraphs(2).Font.Name = "Malgun Gothic"
    $vRange.Paragraphs(2).Font.Size = 9.5
    $vRange.Paragraphs(2).Font.Color.RGB = $cTextSlate
}

# Right: Future Roadmap Card
$mCard = $s9.Shapes.AddShape(5, 480, 95, 440, 405)
$mCard.Fill.Solid(); $mCard.Fill.ForeColor.RGB = $cCardDark
$mCard.Line.ForeColor.RGB = $cBorder; $mCard.Line.Weight = 1.2

$mTitle = $s9.Shapes.AddTextbox(1, 500, 110, 400, 26)
$mTRange = $mTitle.TextFrame.TextRange
$mTRange.Text = "🚀 단계별 시스템 고도화 로드맵 (Roadmap)"
$mTRange.Font.Name = "Malgun Gothic"; $mTRange.Font.Size = 12; $mTRange.Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
$mTRange.Font.Color.RGB = $cAccentCyan

$phases = @(
    @{ Phase = "Phase 1: 물리 설비 및 통신 프로토콜 연동"; Desc = "시뮬레이터를 넘어 실제 현장 산업용 PLC (Siemens S7, Mitsubishi, LS산전)와 OPC-UA / MQTT 브로커 실연동" },
    @{ Phase = "Phase 2: 시계열 데이터베이스 및 클라우드 연계"; Desc = "고속 센서 스트림 데이터를 InfluxDB / TimescaleDB 시계열 데이터베이스에 적재하고 Azure/AWS IoT 클라우드 통합" },
    @{ Phase = "Phase 3: 머신러닝 기반 예지보전 (PdM) AI 탑재"; Desc = "온도/진동 시계열 데이터를 학습한 ONNX / ML.NET 모델을 탑재하여 부품 고장 사전 예측 및 잔여 수명(RUL) 산출" }
)

for ($m = 0; $m -lt 3; $m++) {
    $my = 150 + ($m * 105)
    $mPill = $s9.Shapes.AddShape(5, 495, $my, 410, 95)
    $mPill.Fill.Solid(); $mPill.Fill.ForeColor.RGB = $cSubCardDark
    $mPill.Line.ForeColor.RGB = $cAccentCyan; $mPill.Line.Weight = 1.2

    $mText = $s9.Shapes.AddTextbox(1, 505, $my + 8, 390, 80)
    $mText.TextFrame.WordWrap = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $mText.TextFrame.MarginLeft = 0; $mText.TextFrame.MarginTop = 0
    $mRange = $mText.TextFrame.TextRange
    $mRange.Text = "STEP 0$($m + 1). $($phases[$m].Phase)`r`n$($phases[$m].Desc)"
    $mRange.Paragraphs(1).Font.Name = "Segoe UI"
    $mRange.Paragraphs(1).Font.Size = 11
    $mRange.Paragraphs(1).Font.Bold = [Microsoft.Office.Core.MsoTriState]::msoTrue
    $mRange.Paragraphs(1).Font.Color.RGB = $cAccentCyan
    $mRange.Paragraphs(2).Font.Name = "Malgun Gothic"
    $mRange.Paragraphs(2).Font.Size = 9.5
    $mRange.Paragraphs(2).Font.Color.RGB = $cTextSlate
}

# Save Presentation
Write-Host "Saving presentation to $outputPath..."
if (Test-Path $outputPath) {
    Remove-Item $outputPath -Force
}
$pres.SaveAs($outputPath)
$pres.Close()
$ppt.Quit()

[System.Runtime.Interopservices.Marshal]::ReleaseComObject($pres) | Out-Null
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($ppt) | Out-Null
[System.GC]::Collect()
[System.GC]::WaitForPendingFinalizers()

Write-Host "SUCCESS: Portfolio PPT created at $outputPath"
