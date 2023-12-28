using CS_course_project.Model.Services.AuthServices;

namespace CS_course_project.Tests.Model.Services.AuthServices; 

public class SessionTests {
    [Fact]
    public void ShouldThrowErrorWhenDataIsEmpty() {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Session(true, ""));
    }
}