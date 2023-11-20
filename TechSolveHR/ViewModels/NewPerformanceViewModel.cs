using System.Linq;
using System.Threading.Tasks;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class NewPerformanceViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public NewPerformanceViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;

        var db = _ioc.Get<DatabaseContext>();

        Employees = new BindableCollection<Employee>(db.Employees.Where(x
            => x.Manager != null && x.Manager.Id == _main.LoggedInUser!.Id));
    }

    public BindableCollection<Employee> Employees { get; }

    public Employee? SelectedEmployee { get; set; }

    public Performance Performance { get; set; } = new() { Rating = 3 };

    public async Task CreateNewPerformance()
    {
        if (SelectedEmployee is null) return;

        var db = _ioc.Get<DatabaseContext>();
        SelectedEmployee.Performances.Add(Performance);
        db.Employees.Update(SelectedEmployee);

        await db.SaveChangesAsync();

        var service = _ioc.Get<ISnackbarService>();

        await service.ShowAsync("Success", "Successfully created performance review.",
            SymbolRegular.NotepadPerson20, ControlAppearance.Success);

        _main.ActivateItem(_main.PerformancePage);
    }

    public void Cancel() => _main.ActivateItem(_main.PerformancePage);
}