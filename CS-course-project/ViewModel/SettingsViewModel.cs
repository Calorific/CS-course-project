using CS_course_project.model.Storage;

namespace CS_course_project.ViewModel; 

public class SettingsViewModel : NotifyErrorsViewModel {
    private string _breakDuration = string.Empty;

    public string BreakDuration {
        get => _breakDuration;
        set {
            _breakDuration = value;
            if (_breakDuration.Length == 0)
                AddError(nameof(BreakDuration), "Необходимо указать значение");
            else 
                ClearErrors(nameof(BreakDuration));
            Notify();
        }
    }

    private async void Init() {
        var settings = await DataManager.LoadSettings();
        BreakDuration = settings.BreakDuration.ToString();
    }

    public SettingsViewModel() {
        Init();
    }
}