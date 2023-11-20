using System;
using System.Collections.Generic;
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

    public BindableCollection<Performance> FilteredPerformance =>
        string.IsNullOrWhiteSpace(FilterText)
            ? Performance
            : new BindableCollection<Performance>(Performance
                .Where(x
                    => (x.Title != null && x.Title.Contains(FilterText))
                    || (x.Feedback != null && x.Feedback.Contains(FilterText))));

    public BindableCollection<Performance> Performance { get; set; } = new();

    public Performance? SelectedPerformance { get; set; }

    public string FilterText { get; set; } = string.Empty;

    public void CreateNewPerformance() => _main.ActivateItem(_ioc.Get<NewPerformanceViewModel>());

    public void OnPerformanceSelected() { }

    protected override void OnActivate()
    {
        var db = _ioc.Get<DatabaseContext>();

        _main.LoggedInUser!.Performances = new List<Performance>
        {
            new()
            {
                Category  = "Category 1",
                Title     = "Title 1",
                Feedback  = "Feedback 1",
                Rating    = 1,
                Evaluator = db.Employees.First(),
                DateTime  = DateTimeOffset.UtcNow
            },
            new()
            {
                Category  = "Category 2",
                Title     = "Title 2",
                Feedback  = "Feedback 2",
                Rating    = 2,
                Evaluator = db.Employees.First(),
                DateTime  = DateTimeOffset.UtcNow
            },
            new()
            {
                Category  = "Category 3",
                Title     = "Title 3",
                Feedback  = "Feedback 3",
                Rating    = 3,
                Evaluator = db.Employees.First(),
                DateTime  = DateTimeOffset.UtcNow
            }
        };

        Performance = new BindableCollection<Performance>(_main.LoggedInUser!.Performances);
    }
}