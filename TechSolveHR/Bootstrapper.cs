using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;
using Microsoft.EntityFrameworkCore;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;
using TechSolveHR.ViewModels;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace TechSolveHR;

public class Bootstrapper : Bootstrapper<MainWindowViewModel>
{
    private IStyletIoCBuilder _builder;

    public Bootstrapper()
    {
        // Make Hyperlinks handle themselves
        EventManager.RegisterClassHandler(
            typeof(Hyperlink), Hyperlink.RequestNavigateEvent,
            new RequestNavigateEventHandler((_, e) =>
            {
                var url = e.Uri.ToString();
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            })
        );
    }


    protected override void ConfigureIoC(IStyletIoCBuilder builder)
    {
        builder.Bind<DatabaseContext>().ToFactory(_ =>
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseLazyLoadingProxies()
                .UseSqlite("Data Source=hr_data.db")
                .Options;

            var db = new DatabaseContext(options);
            db.Database.EnsureCreated();

            if (db.Employees.Any()) return db;

            db.Employees.Add(new Employee
            {
                AccessType = "Admin",
                Username   = "admin",
                Password   = Crypto.HashPassword("password"),
            });

            db.SaveChanges();

            return db;
        });

        builder.Bind<IThemeService>().To<ThemeService>().InSingletonScope();
        builder.Bind<ISnackbarService>().To<SnackbarService>().InSingletonScope();
        builder.Bind<IDialogService>().To<DialogService>().InSingletonScope();
        builder.Bind<IStyletIoCBuilder>().ToInstance(builder);

        _builder = builder;
    }
}