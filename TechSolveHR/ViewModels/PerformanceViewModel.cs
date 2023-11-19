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

    public BindableCollection<Performance> Employees { get; set; } = new();

    public BindableCollection<Performance> FilteredEmployees =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Employees
            : new BindableCollection<Performance>(Employees
                .Where(x
                    => (x.Title != null && x.Title.Contains(FilterText))
                    || (x.Feedback != null && x.Feedback.Contains(FilterText))));

    public Employee? SelectedEmployee { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public void OnEmployeeSelected() { }

    protected override void OnViewLoaded()
    {
        base.OnViewLoaded();

        var db = _ioc.Get<DatabaseContext>();
        Employees = new BindableCollection<Performance>(_main.LoggedInUser!.Performances);
    }
}