using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CS_course_project.ViewModel; 

public class NotifyErrorsViewModel : BaseViewModel, INotifyDataErrorInfo {
    public Dictionary<string, List<string>> Errors { get; } = new();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    protected void OnErrorsChanged(string propertyName) {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    
    public bool HasErrors => Errors.Count > 0;
    
    public IEnumerable GetErrors(string? propertyName) {
        return Errors!.GetValueOrDefault(propertyName, new List<string>());
    }
}