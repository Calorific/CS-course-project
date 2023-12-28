using System.Windows;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Components.Common; 

public class ListItem : NotifyErrorsViewModel {
    public string Data { get; }
    public string Id { get; }

    public Visibility ErrorVisibility { get; set; } = Visibility.Collapsed;
    
    private string? _error;
    public string? Error {
        get => _error;
        set {
            _error = value;
            ErrorVisibility = !string.IsNullOrEmpty(value) ? Visibility.Visible : Visibility.Collapsed;
            NotifyAll(nameof(Error), nameof(ErrorVisibility));
        }
    }

    public ListItem(string data) {
        Data = data;
        Id = data;
    }

    public ListItem(string data, string id) {
        Data = data;
        Id = id;
    }
}