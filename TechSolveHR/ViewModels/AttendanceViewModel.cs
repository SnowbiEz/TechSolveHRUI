using System;
using System.Collections.Generic;
using System.Linq;
using Stylet;
using StyletIoC;
using TechSolveHR.Models;

namespace TechSolveHR.ViewModels;

public class AttendanceViewModel : Screen
{
    private readonly IContainer _ioc;
    private readonly MainWindowViewModel _main;

    public AttendanceViewModel(IContainer ioc, MainWindowViewModel main)
    {
        _ioc  = ioc;
        _main = main;
    }

    public Attendance? SelectedAttendance { get; set; }

    public BindableCollection<Attendance> Attendances { get; set; } = new();

    public bool IsEmpty => !Attendances.Any();

    public void OnAttendanceSelected() { }

    protected override void OnActivate()
    {
        Attendances = new BindableCollection<Attendance>(_main.LoggedInUser!.Attendances);
    }
}