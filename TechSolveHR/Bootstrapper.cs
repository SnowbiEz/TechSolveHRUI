using System.Configuration;
using System.Diagnostics;
using System.IO;
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
        var config = ConfigurationManager
            .OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

        var path = Path.GetDirectoryName(config.FilePath);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path!);

        builder.Bind<DatabaseContext>().ToFactory(_ =>
        {
            var source = Path.Combine(path!, "data.db");

            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseLazyLoadingProxies()
                .UseSqlite($"Data Source={source}")
                
                .Options;

            var db = new DatabaseContext(options);
            db.Database.EnsureCreated();

            return db;
        });

        builder.Bind<IThemeService>().To<ThemeService>().InSingletonScope();
        builder.Bind<ISnackbarService>().To<SnackbarService>().InSingletonScope();
    }
}