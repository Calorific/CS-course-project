using System;
using System.Windows.Input;
using CS_course_project.Commands;

namespace CS_course_project.ViewModel.UserControls; 

public class AccountSettingsViewModel : NotifyErrorsViewModel {
    public ICommand? BaseCommand;
    public ICommand SubmitCommand => Command.Create(ChangePassword);
    private void ChangePassword(object? sender, EventArgs e) {
        Console.WriteLine(BaseCommand is null);
        BaseCommand?.Execute(_newPassword);
    }

    private bool _isFirstRender = true;
    
    private string _newPassword = string.Empty;
    public string NewPassword {
        get => _newPassword;
        set {
            _newPassword = value;
            if (_isFirstRender) {
                _isFirstRender = false;
            }
            else if (_newPassword == string.Empty) {
                AddError(nameof(NewPassword), "Необходимо указать значение");
            }
            else {
                ClearErrors(nameof(NewPassword));
            }
            Notify();
        }
    }
}