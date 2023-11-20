using System.Linq;
using Microsoft.EntityFrameworkCore;
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

    public BindableCollection<Performance> FilteredPerformance =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Performances
            : new BindableCollection<Performance>(Performances
                .Where(x
                    => (x.Title != null && x.Title.Contains(FilterText))
                    || (x.Feedback != null && x.Feedback.Contains(FilterText))));

    public BindableCollection<Performance> Performances { get; set; } = new();

    public bool HasEmployees => _ioc.Get<DatabaseContext>().Employees
        .Any(x => _main.LoggedInUser != null && x.ManagerId == _main.LoggedInUser.Id);

    public bool IsEmpty => !Performances.Any();

    public Performance? SelectedPerformance { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public void CreateNewPerformance() => _main.ActivateItem(_ioc.Get<NewPerformanceViewModel>());

    public void OnPerformanceSelected() { }

    protected override async void OnActivate()
    {
        if (_main.LoggedInUser is null) return;

        await using var db = _ioc.Get<DatabaseContext>();
        var performances = db.Performances
            .Include(x => x.Evaluator).ThenInclude(x => x.Data)
            .Where(x => x.Employee.Id == _main.LoggedInUser.Id);

        Performances = new BindableCollection<Performance>(performances);
    }
}