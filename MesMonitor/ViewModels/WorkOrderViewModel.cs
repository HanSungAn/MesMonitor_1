using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using MesMonitor.Models;
using MesMonitor.Services;
using SkiaSharp;

namespace MesMonitor.ViewModels;

public class WorkOrderViewModel : ViewModelBase
{
    private readonly IMesDataSimulator _simulator;

    public ObservableCollection<WorkOrderItem> AllOrders => _simulator.WorkOrders;
    public ObservableCollection<WorkOrderItem> FilteredOrders { get; } = new();

    private string _selectedFilter = "전체";
    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            if (SetProperty(ref _selectedFilter, value))
            {
                ApplyFilter();
            }
        }
    }

    // New Order input fields
    private string _newProductCode = "PRD-NEW-01";
    public string NewProductCode
    {
        get => _newProductCode;
        set => SetProperty(ref _newProductCode, value);
    }

    private string _newProductName = "Next-Gen Sensor Controller";
    public string NewProductName
    {
        get => _newProductName;
        set => SetProperty(ref _newProductName, value);
    }

    private string _newLineId = "L01";
    public string NewLineId
    {
        get => _newLineId;
        set => SetProperty(ref _newLineId, value);
    }

    private int _newPlanQty = 500;
    public int NewPlanQty
    {
        get => _newPlanQty;
        set => SetProperty(ref _newPlanQty, value);
    }

    // Summary Counts
    public int TotalCount => AllOrders.Count;
    public int InProgressCount => AllOrders.Count(o => o.Status == OrderStatus.InProgress);
    public int WaitingCount => AllOrders.Count(o => o.Status == OrderStatus.Waiting);
    public int CompletedCount => AllOrders.Count(o => o.Status == OrderStatus.Completed);

    // Order status donut chart
    public ISeries[] StatusSeries { get; private set; }

    public IRelayCommand AddOrderCommand { get; }
    public IRelayCommand<WorkOrderItem> CompleteOrderCommand { get; }

    public WorkOrderViewModel(IMesDataSimulator simulator)
    {
        Title = "생산 계획 및 작업지시 관리 (Work Order Management)";
        _simulator = simulator;

        AddOrderCommand = new RelayCommand(ExecuteAddOrder);
        CompleteOrderCommand = new RelayCommand<WorkOrderItem>(ExecuteCompleteOrder);

        StatusSeries = new ISeries[]
        {
            new PieSeries<int> { Name = "진행중", Values = new int[] { InProgressCount }, Fill = new SolidColorPaint(new SKColor(59, 130, 246)), InnerRadius = 45 },
            new PieSeries<int> { Name = "대기", Values = new int[] { WaitingCount }, Fill = new SolidColorPaint(new SKColor(148, 163, 184)), InnerRadius = 45 },
            new PieSeries<int> { Name = "완료", Values = new int[] { CompletedCount }, Fill = new SolidColorPaint(new SKColor(16, 185, 129)), InnerRadius = 45 }
        };

        ApplyFilter();
        _simulator.DataUpdated += OnDataUpdated;
    }

    private void OnDataUpdated()
    {
        ApplyFilter();
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(InProgressCount));
        OnPropertyChanged(nameof(WaitingCount));
        OnPropertyChanged(nameof(CompletedCount));
    }

    private void ApplyFilter()
    {
        FilteredOrders.Clear();
        var query = AllOrders.AsEnumerable();

        if (SelectedFilter == "진행중")
            query = query.Where(o => o.Status == OrderStatus.InProgress);
        else if (SelectedFilter == "대기")
            query = query.Where(o => o.Status == OrderStatus.Waiting);
        else if (SelectedFilter == "완료")
            query = query.Where(o => o.Status == OrderStatus.Completed);

        foreach (var order in query)
        {
            FilteredOrders.Add(order);
        }
    }

    private void ExecuteAddOrder()
    {
        if (NewPlanQty <= 0 || string.IsNullOrWhiteSpace(NewProductName)) return;

        var newOrder = new WorkOrderItem
        {
            OrderNo = $"WO-{DateTime.Now:yyMMdd}-{AllOrders.Count + 1:D2}",
            LineId = NewLineId,
            ProductCode = NewProductCode,
            ProductName = NewProductName,
            PlanQty = NewPlanQty,
            ActualQty = 0,
            DefectQty = 0,
            Status = OrderStatus.Waiting,
            StartDate = DateTime.Now,
            DueDate = DateTime.Now.AddDays(1)
        };

        _simulator.AddWorkOrder(newOrder);
    }

    private void ExecuteCompleteOrder(WorkOrderItem? order)
    {
        if (order == null) return;
        order.Status = OrderStatus.Completed;
        order.ActualQty = order.PlanQty;
        ApplyFilter();
    }
}

