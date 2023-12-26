using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CS_course_project.ViewModel.Common; 

public class NotifyErrorsViewModel : BaseViewModel, INotifyDataErrorInfo {
    public Dictionary<string, List<string>> Errors { get; } = new();
    
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    
    private void OnErrorsChanged(string propertyName) {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    
    public bool HasErrors => Errors.Count > 0;
    
    public IEnumerable GetErrors(string? propertyName) {
        return Errors!.GetValueOrDefault(propertyName, new List<string>());
    }
    
    protected void AddError(string propertyName, string error) {
        if (!Errors.ContainsKey(propertyName)) 
            Errors.Add(propertyName, new List<string>());
        Errors[propertyName].Add(error);
        OnErrorsChanged(propertyName);
    }

    protected void ClearErrors(string propertyName) {
        if (Errors.Remove(propertyName)) 
            OnErrorsChanged(propertyName);
    }
}