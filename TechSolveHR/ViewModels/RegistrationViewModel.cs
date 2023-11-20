using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class RegistrationViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly ISnackbarService _snackbar;
    private readonly MainWindowViewModel _main;

    public RegistrationViewModel(IContainer ioc, ISnackbarService snackbar, DatabaseContext db,
        MainWindowViewModel main)
    {
        _ioc      = ioc;
        _snackbar = snackbar;
        _main     = main;

        Employees = new BindableCollection<Employee>(db.Employees);
    }

    public BindableCollection<Employee> Employees { get; set; }

    public Employee? SelectedManager { get; set; }

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public async Task Register()
    {

        if (string.IsNullOrWhiteSpace(Username))
        {
            await _snackbar.ShowAsync("Error!", "Please provide a username.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
        {
            await _snackbar.ShowAsync("Error!", "Password is too weak.");
            return;
        }

        var db = _ioc.Get<DatabaseContext>();
        if (db.Employees.Any(x => x.Username == Username))
        {
            await _snackbar.ShowAsync("Error!", "Username already exists");
            return;
        }

        var employee = new Employee
        {
            Username = Username,
            Password = Crypto.HashPassword(Password),
            Manager  = SelectedManager
        };

        db.Employees.Add(employee);
        await db.SaveChangesAsync();
    }

    public string AccessType { get; set; } = string.Empty;
}