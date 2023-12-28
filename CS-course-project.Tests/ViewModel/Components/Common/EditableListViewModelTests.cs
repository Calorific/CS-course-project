using System.Collections.ObjectModel;
using CS_course_project.ViewModel.Components.Common;

namespace CS_course_project.Tests.ViewModel.Components.Common; 

public class EditableListViewModelTests {
    [Fact]
    public void ShouldAddErrorWhenInputIsEmpty() {
        // Arrange
        var viewModel = new EditableListViewModel {
            NewItem = string.Empty,
            Items = new ObservableCollection<ListItem>()
        };


        // Act
        viewModel.AddItemCommand.Execute(null);
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
    
    [Fact]
    public void ShouldAddErrorWhenInputIsNotUnique() {
        // Arrange
        var viewModel = new EditableListViewModel {
            Items = new ObservableCollection<ListItem> { new("item") },
            IsUnique = true,
            NewItem = "item"
        };

        
        // Act
        viewModel.AddItemCommand.Execute(null);
        
        // Assert
        Assert.Single(viewModel.Errors);
    }
}