namespace Application.UnitTests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        int expected = 2;

        // Act
        int actual = 1 + 1;

        // Assert
        Assert.Equal(expected, actual);
    }
}