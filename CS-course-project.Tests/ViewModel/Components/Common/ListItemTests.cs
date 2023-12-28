using System.Windows;
using CS_course_project.ViewModel.Components.Common;

namespace CS_course_project.Tests.ViewModel.Components.Common; 

public class ListItemTests {
    [Fact]
    public void ShouldShowErrorWhenItIsNotEmpty() {
        // Arrange
        var listItem = new ListItem("test");
        const Visibility expected = Visibility.Visible;


        // Arrange
        listItem.Error = "error";
        
        
        // Assert
        Assert.Equal(expected, listItem.ErrorVisibility);
    }
    
    [Fact]
    public void ShouldHideErrorWhenItIsEmpty() {
        // Arrange
        var listItem = new ListItem("test");
        const Visibility expected = Visibility.Collapsed;


        // Arrange
        listItem.Error = "error";
        listItem.Error = "";
        
        
        // Assert
        Assert.Equal(expected, listItem.ErrorVisibility);
    }
}