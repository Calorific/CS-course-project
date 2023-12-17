﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CS_course_project.ViewModel.UserControls;

namespace CS_course_project.UserControls.SettingsPage; 

public class Item {
    public string Data { get; }
    public string Id { get; }

    public Item(string data) {
        Data = data;
        Id = data;
    }
    
    public Item(string data, string id) {
        Data = data;
        Id = id;
    }
}

public partial class EditableList {
    private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var myControl = (EditableList)d;
        if (myControl.Form.DataContext is not EditableListViewModel viewModel) return;
        viewModel.Items = myControl.Items;
    }
    
    public static readonly DependencyProperty ItemsProperty =
        DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<Item>), 
            typeof(EditableList), new FrameworkPropertyMetadata(null,
            OnItemsChanged));

    public ObservableCollection<Item> Items {
        get => (ObservableCollection<Item>)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public static readonly DependencyProperty IsUniqueProperty =
        DependencyProperty.Register(nameof(IsUnique), typeof(bool), typeof(EditableList), new UIPropertyMetadata(true));

    public bool IsUnique {
        get => (bool)GetValue(IsUniqueProperty);
        set => SetValue(IsUniqueProperty, value);
    }
    
    public static readonly DependencyProperty AddCommandProperty =
        DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(EditableList));
    
    public ICommand AddCommand {
        get => (ICommand)GetValue(AddCommandProperty);
        set => SetValue(AddCommandProperty, value);
    }
    
    public static readonly DependencyProperty RemoveCommandProperty =
        DependencyProperty.Register(nameof(RemoveCommand), typeof(ICommand), typeof(EditableList));

    public ICommand RemoveCommand {
        get => (ICommand)GetValue(RemoveCommandProperty);
        set => SetValue(RemoveCommandProperty, value);
    }
    
    private void OnLoad(object sender, RoutedEventArgs e) {
        if (Form.DataContext is not EditableListViewModel viewModel) return;
        viewModel.BaseCommand = AddCommand;
        viewModel.Items = Items;
        viewModel.IsUnique = IsUnique;
    }
    
    public EditableList() {
        InitializeComponent();
        ItemListBox.DataContext = this;
        Loaded += OnLoad;
    }
}