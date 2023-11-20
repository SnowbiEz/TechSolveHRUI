using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class AdminViewModel : Screen
{
    private readonly DatabaseContext _db;
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;
    private bool _initialized;

    public AdminViewModel(IContainer ioc, DatabaseContext db, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _db   = db;
        _main = main;

        UserInfoView = _ioc.Get<PersonalInfoViewModel>();
        UserListView = _ioc.Get<EmployeeListViewModel>();

        UserListView.EmployeeSelected += (_, _) => OnEmployeeSelected();
    }

    public Employee SelectedEmployee
    {
        get => UserListView.SelectedEmployee!;
        set => UserListView.SelectedEmployee = value;
    }

    public PersonalInfoViewModel UserInfoView { get; }
    public EmployeeListViewModel UserListView { get; }
    public int SelectedIndex { get; set; }

    private void OnEmployeeSelected()
    {
        UserInfoView.Employee = SelectedEmployee;
        SelectedIndex         = 1;

        UserInfoView.Activate();
    }

    protected override void OnActivate()
    {
        UserListView.Activate();
        UserInfoView.Activate();
    }
}