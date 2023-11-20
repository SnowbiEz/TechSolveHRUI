using System;
using System.Linq;
using System.Threading.Tasks;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class PersonalInfoViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly ISnackbarService _snackBar;
    private readonly DatabaseContext _db;
    private readonly MainWindowViewModel _main;

    public PersonalInfoViewModel(
        IContainer ioc, ISnackbarService snackBar,
        DatabaseContext db, MainWindowViewModel main)
    {
        _ioc      = ioc;
        _snackBar = snackBar;
        _db       = db;
        _main     = main;
    }

    public BindableCollection<Employee> Employees => new(_db.Employees.Where(x => x.Id != Employee.Id));

    public DateTime DateOfBirth
    {
        get => Employee.Data.DateOfBirth?.DateTime ?? DateTime.Now;
        set => Employee.Data.DateOfBirth = value;
    }

    public Employee Employee => _main.LoggedInUser!;

    public Employee? SelectedManager
    {
        get => Employees.FirstOrDefault(x => x.Id == Employee.ManagerId);
        set => Employee.ManagerId = value?.Id;
    }

    public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateOfBirth.DayOfYear < DateTime.Now.DayOfYear ? 0 : 1);

    public async Task Save()
    {
        var db = _ioc.Get<DatabaseContext>();
        db.Employees.Update(Employee);
        await db.SaveChangesAsync();

        await _snackBar.ShowAsync("Success", "Successfully saved changes to the database.",
            SymbolRegular.PeopleCheckmark20, ControlAppearance.Success);
    }
}