using System.Linq;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class EmployeeListViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public EmployeeListViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }

    public BindableCollection<Employee> Employees { get; set; } = new();

    public BindableCollection<Employee> FilteredEmployees =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Employees
            : new BindableCollection<Employee>(Employees
                .Where(x => $"{x.Data.FirstName} {x.Data.MiddleName} {x.Data.LastName}".Contains(FilterText)));

    public Employee? SelectedEmployee { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public void OnEmployeeSelected() { }

    protected override void OnActivate()
    {
        var db = _ioc.Get<DatabaseContext>();
        Employees = new BindableCollection<Employee>(db.Employees);
    }
}