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
    private readonly ISnackbarService _snackBar;
    private readonly DatabaseContext _db;
    private readonly MainWindowViewModel _main;

    public RegistrationViewModel(
        IContainer ioc, ISnackbarService snackBar,
        DatabaseContext db, MainWindowViewModel main)
    {
        _ioc      = ioc;
        _snackBar = snackBar;
        _db       = db;
        _main     = main;
    }

    public BindableCollection<Employee> Employees => new(_db.Employees);

    public Employee? SelectedManager { get; set; }

    public string AccessType { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public async Task Register()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            await _snackBar.ShowAsync("Error!", "Please provide a username.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
        {
            await _snackBar.ShowAsync("Error!", "Password is too weak.");
            return;
        }

        if (_db.Employees.Any(x => x.Username == Username))
        {
            await _snackBar.ShowAsync("Error!", "Username already exists");
            return;
        }

        var employee = new Employee
        {
            Username = Username,
            Password = Crypto.HashPassword(Password),
            Manager  = SelectedManager
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
    }
}