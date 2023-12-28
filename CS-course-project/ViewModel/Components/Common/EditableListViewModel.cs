using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CS_course_project.ViewModel.Commands;
using CS_course_project.ViewModel.Common;

namespace CS_course_project.ViewModel.Components.Common; 

public class EditableListViewModel : NotifyErrorsViewModel {

    private ObservableCollection<ListItem>? _items;
    public ObservableCollection<ListItem>? Items {
        get => _items;
        set {
            _items = value;
            ErrorVisibility = _items is { Count: <= 0 } ? Visibility.Visible : Visibility.Collapsed;
            Notify();
        }
    }
    
    public ICommand? BaseRemoveCommand { get; set; }
    public ICommand RemoveItemCommand => Command.Create(RemoveItem);
    private void RemoveItem(object? sender, EventArgs e) {
        if (_items is { Count: 1 })
            ErrorVisibility = Visibility.Visible;
        
        BaseRemoveCommand?.Execute(sender);
        Notify(nameof(Items));
    }
    
    public ICommand? BaseAddCommand { get; set; }
    public ICommand AddItemCommand => Command.Create(AddItem);
    private void AddItem(object? sender, EventArgs e) {
        if (Items == null) return;
        if (NewItem.Length == 0) {
            AddError(nameof(NewItem), "Нужно указать значение");
            return;
        }
        if (IsUnique && Items.FirstOrDefault(item => item.Data == _newItem) != null) {
            AddError(nameof(NewItem), "Значение уже существует");
            return;
        }

        ErrorVisibility = Visibility.Collapsed;
        
        ClearErrors(nameof(NewItem));
        BaseAddCommand?.Execute(_newItem);
        NewItem = string.Empty;
        Notify(nameof(NewItem));
    }
    
    public bool IsUnique { get; set; }

    
    private Visibility _errorVisibility = Visibility.Visible;
    public Visibility ErrorVisibility {
        get => _errorVisibility;
        set {
            _errorVisibility = value;
            Notify();
        }
    }
    
    private string _newItem = string.Empty;
    public string NewItem {
        get => _newItem;
        set {
            _newItem = value;
            ClearErrors(nameof(NewItem));
            Notify();
        }
    }
}