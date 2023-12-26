using System;
using System.Windows.Input;

namespace CS_course_project.Commands; 

public class Command : ICommand {
    public event EventHandler? CanExecuteChanged;

    private Command(EventHandler method) => CanExecuteChanged += method;

    public static Command Create(EventHandler method) => new(method);

    public bool CanExecute(object? parameter) => CanExecuteChanged is not null;

    public void Execute(object? parameter) => CanExecuteChanged?.Invoke(parameter, EventArgs.Empty);
}