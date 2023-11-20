using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace TechSolveHR.ViewModels;

public class PersonalInfoViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;
    private readonly ISnackbarService _snackBar;
    private Employee? _employee;

    public PersonalInfoViewModel(
        IContainer ioc, ISnackbarService snackBar,
        MainWindowViewModel main)
    {
        _ioc      = ioc;
        _snackBar = snackBar;
        _main     = main;
    }

    public BindableCollection<Employee> Employees { get; set; } = new();

    public DateTime DateOfBirth
    {
        get => Employee.Data.DateOfBirth?.DateTime ?? DateTime.Now;
        set => Employee.Data.DateOfBirth = value;
    }

    public Employee Employee
    {
        get => _employee ?? _main.LoggedInUser!;
        set => _employee = value;
    }

    public string FirstName
    {
        get => Employee.Data.FirstName ?? string.Empty;
        set => Employee.Data.FirstName = value;
    }

    public string MiddleName
    {
        get => Employee.Data.MiddleName ?? string.Empty;
        set => Employee.Data.MiddleName = value;
    }

    public string LastName
    {
        get => Employee.Data.LastName ?? string.Empty;
        set => Employee.Data.LastName = value;
    }

    public string PreferredName
    {
        get => Employee.Data.PreferredName ?? string.Empty;
        set => Employee.Data.PreferredName = value;
    }

    public string Gender
    {
        get => Employee.Data.Gender ?? string.Empty;
        set => Employee.Data.Gender = value;
    }

    public string MaritalStatus
    {
        get => Employee.Data.MaritalStatus ?? string.Empty;
        set => Employee.Data.MaritalStatus = value;
    }

    public string PhoneNumber
    {
        get => Employee.Data.PhoneNumber ?? string.Empty;
        set => Employee.Data.PhoneNumber = value;
    }

    public string EmailAddress
    {
        get => Employee.EmailAddress ?? string.Empty;
        set => Employee.EmailAddress = value;
    }

    public string TelephoneNumber
    {
        get => Employee.Data.TelephoneNumber ?? string.Empty;
        set => Employee.Data.TelephoneNumber = value;
    }

    public Employee? SelectedManager
    {
        get => Employees.FirstOrDefault(x => x.Id == Employee.ManagerId);
        set => Employee.ManagerId = value?.Id;
    }

    public int Age => DateTime.Now.Year - DateOfBirth.Year - (DateOfBirth.DayOfYear < DateTime.Now.DayOfYear ? 0 : 1);

    public string PhilHealth
    {
        get => Employee.Data.PhilHealth ?? string.Empty;
        set => Employee.Data.PhilHealth = value;
    }

    public string Sss
    {
        get => Employee.Data.Sss ?? string.Empty;
        set => Employee.Data.Sss = value;
    }

    public string Tin
    {
        get => Employee.Data.Tin ?? string.Empty;
        set => Employee.Data.Tin = value;
    }

    public string Street
    {
        get => Employee.Data.Address?.Street ?? string.Empty;
        set => Employee.Data.Address!.Street = value;
    }

    public string AddressState
    {
        get => Employee.Data.Address?.State ?? string.Empty;
        set => Employee.Data.Address!.State = value;
    }

    public string City
    {
        get => Employee.Data.Address?.City ?? string.Empty;
        set => Employee.Data.Address!.City = value;
    }

    public string ZipCode
    {
        get => Employee.Data.Address?.ZipCode ?? string.Empty;
        set => Employee.Data.Address!.ZipCode = value;
    }

    public string ContactNumber
    {
        get => Employee.Contacts?.ContactNumber ?? string.Empty;
        set => Employee.Contacts!.ContactNumber = value;
    }

    public string ContactName
    {
        get => Employee.Contacts?.ContactName ?? string.Empty;
        set => Employee.Contacts!.ContactName = value;
    }

    public string Relationship
    {
        get => Employee.Contacts?.Relationship ?? string.Empty;
        set => Employee.Contacts!.Relationship = value;
    }

    public void Activate()
    {
        OnActivate();
    }

    protected override void OnActivate()
    {
        var db = _ioc.Get<DatabaseContext>();
        _employee = db.Employees
            .FirstOrDefault(x => x.Id == Employee.Id);
        Employees = new BindableCollection<Employee>(db.Employees.Where(x => x.Id != Employee.Id));
    }

    public async Task Save()
    {
        await using var db = _ioc.Get<DatabaseContext>();

        db.Employees.Update(Employee);
        await db.SaveChangesAsync();

        await _snackBar.ShowAsync("Success", "Successfully saved changes to the database.",
            SymbolRegular.PeopleCheckmark20, ControlAppearance.Success);
    }
}