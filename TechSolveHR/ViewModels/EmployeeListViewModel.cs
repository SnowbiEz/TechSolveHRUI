using System;
using System.Linq;
using System.Threading.Tasks;
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

    public bool IsEmpty => !Employees.Any();

    public BindableCollection<Employee> Employees { get; set; } = new();

    public BindableCollection<Employee> FilteredEmployees =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Employees
            : new BindableCollection<Employee>(Employees.Where(x
                => x.Data.FullName.Contains(FilterText, StringComparison.OrdinalIgnoreCase)));

    public BindableCollection<string> EmployeeNames => new(Employees.Select(x => x.Data.FullName));

    public bool ShowDeleteButton { get; set; }

    public Employee? SelectedEmployee { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public event EventHandler<Employee>? EmployeeSelected;

    public void Activate() => OnActivate();

    public async Task DeleteEmployee(Employee employee)
    {
        await using var db = _ioc.Get<DatabaseContext>();
        db.Employees.Remove(employee);
        await db.SaveChangesAsync();

        OnActivate();
    }

    public void OnEmployeeSelected() => EmployeeSelected?.Invoke(this, SelectedEmployee!);

    protected override void OnActivate()
    {
        var db = _ioc.Get<DatabaseContext>();
        Employees = new BindableCollection<Employee>(db.Employees);
        NotifyOfPropertyChange(() => FilteredEmployees);
    }
}