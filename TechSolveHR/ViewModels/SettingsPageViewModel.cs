using System;
using System.Reflection;
using ModernWpf;
using Stylet;
using StyletIoC;
using TechSolveHR.ModernWPF;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Contracts;
using Transition = TechSolveHR.ModernWPF.Animation.Transition;
using TransitionCollection = TechSolveHR.ModernWPF.Animation.Transitions.TransitionCollection;

namespace TechSolveHR.ViewModels;

public class SettingsPageViewModel : Screen
{
    private readonly IEventAggregator _events;
    private readonly IContainer _ioc;
    private readonly IThemeService _theme;
    private readonly MainWindowViewModel _main;

    public SettingsPageViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc    = ioc;
        _events = ioc.Get<IEventAggregator>();
        _theme  = ioc.Get<IThemeService>();
        _main   = main;
    }

    public static CaptionedObject<Transition>? Transition { get; set; } =
        TransitionCollection.Transitions[0];

    public static Version ProgramVersion => Assembly.GetExecutingAssembly().GetName().Version!;

    public void OnThemeChanged() => _theme.SetTheme(ThemeManager.Current.ApplicationTheme switch
    {
        ApplicationTheme.Light => ThemeType.Light,
        ApplicationTheme.Dark  => ThemeType.Dark,
        _                      => _theme.GetSystemTheme()
    });

    protected override void OnActivate() { }
}