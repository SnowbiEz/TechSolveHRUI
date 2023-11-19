using System.Linq;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class PerformanceViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public PerformanceViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }

    public BindableCollection<Performance> Performance { get; set; } = new();

    public BindableCollection<Performance> FilteredPerformance =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Performance
            : new BindableCollection<Performance>(Performance
                .Where(x
                    => (x.Title != null && x.Title.Contains(FilterText))
                    || (x.Feedback != null && x.Feedback.Contains(FilterText))));

    public Performance? SelectedEmployee { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public void OnPerformanceSelected() { }

    protected override void OnViewLoaded()
    {
        base.OnViewLoaded();

        var db = _ioc.Get<DatabaseContext>();
        Performance = new BindableCollection<Performance>(_main.LoggedInUser!.Performances);
    }
}