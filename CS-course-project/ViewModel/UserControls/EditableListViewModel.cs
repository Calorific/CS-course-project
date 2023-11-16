using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CS_course_project.Commands;

namespace CS_course_project.ViewModel.UserControls; 

public class EditableListViewModel : NotifyErrorsViewModel {
    
    public ObservableCollection<string>? Items;

    public ICommand? BaseCommand { get; set; }
    
    public ICommand AddItemCommand => Command.Create(AddItem);
    private void AddItem(object? sender, EventArgs e) {
        if (Items == null) return;
        if (NewItem.Length == 0) {
            AddError(nameof(NewItem), "Нужно указать значение");
            return;
        }
        if (Items.IndexOf(NewItem) != -1) {
            AddError(nameof(NewItem), "Значение уже существует");
            return;
        }
        ClearErrors(nameof(NewItem));
        BaseCommand?.Execute(_newItem);
        NewItem = string.Empty;
        Notify(nameof(NewItem));
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

    public EditableListViewModel() {}
    
    public EditableListViewModel(ObservableCollection<string> items, ICommand addCommand) {
        Items = items;
        BaseCommand = addCommand;
    }
}