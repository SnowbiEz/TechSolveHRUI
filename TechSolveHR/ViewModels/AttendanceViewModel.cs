﻿using System;
using System.Collections.Generic;
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

    public void OnAttendanceSelected() { }

    protected override void OnViewLoaded()
    {
        base.OnViewLoaded();

        _main.LoggedInUser!.Attendances = new List<Attendance>
        {
            new()
            {
                TimeIn  = DateTimeOffset.UtcNow,
                TimeOut = DateTimeOffset.UtcNow.AddHours(8)
            },
            new()
            {
                TimeIn  = DateTimeOffset.UtcNow.AddMinutes(Random.Shared.Next(120)),
                TimeOut = DateTimeOffset.UtcNow.AddHours(8).AddMinutes(Random.Shared.Next(120))
            },
            new()
            {
                TimeIn  = DateTimeOffset.UtcNow.AddMinutes(Random.Shared.Next(120)),
                TimeOut = DateTimeOffset.UtcNow.AddHours(8).AddMinutes(Random.Shared.Next(120))
            },
            new()
            {
                TimeIn  = DateTimeOffset.UtcNow.AddMinutes(Random.Shared.Next(120)),
                TimeOut = DateTimeOffset.UtcNow.AddHours(8).AddMinutes(Random.Shared.Next(120))
            },
            new()
            {
                TimeIn  = DateTimeOffset.UtcNow.AddMinutes(Random.Shared.Next(120)),
                TimeOut = DateTimeOffset.UtcNow.AddHours(8).AddMinutes(Random.Shared.Next(120))
            }
        };

        Attendances = new BindableCollection<Attendance>(_main.LoggedInUser!.Attendances);
    }
}