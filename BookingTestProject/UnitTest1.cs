using BookingProject;

namespace BookingTestProject;


// Test driven development:

public class UnitTest1
{
    // Feature description: Login as admin
    // Test case 1: given admin email and password, when admin tries to login, then return true
        
    [Theory]
    [InlineData("admin@outlook.com","1234567")]
    public void Login_GivenEmailPassword_ReturnsTrue(string userEmail,string userPassword)
    {
        //arrange
        var result = false;
        //act
       // result=User.Login(userEmail,userPassword);
        //assert
        Assert.True(result);
    }
    
    
}