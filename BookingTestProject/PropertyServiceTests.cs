using BookingProject;
using BookingProject.Database;
using BookingProject.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BookingTestProject;

public class PropertyServiceTests
{
    // Test Description: checks if the property with rooms and beds is added to the database
    // on success: returns true
   /* [ClassData(typeof(HotelObjectForTest))]
    [Theory]
    public void AddAPropertyWithRooms(Hotel property, Room room1, Room room2)
    {
        using var testDatabase = CreateTestDatabase();

        testDatabase.PropertyService.AddProperty(property);

        var savedProperty = testDatabase.PropertyService.GetPropertyById(property.Id);

        Assert.NotNull(savedProperty);
        Assert.Equal("Danish Hotel", savedProperty.Name, ignoreCase: true);
    }

    [Theory]
    [InlineData(11)]
    public void DeleteAProperty_OnSuccess_ReturnTrue(int propertyId)
    {
        using var testDatabase = CreateTestDatabase();
        var property = new Hotel("Danish Hotel", DateTime.Now)
        {
            Id = propertyId,
            Rooms = new List<Room>()
        };

        testDatabase.PropertyService.AddProperty(property);

        int countBefore = testDatabase.PropertyService.GetAllProperties().Count;
        testDatabase.PropertyService.DeletePropertyById(propertyId);
        int countAfter = testDatabase.PropertyService.GetAllProperties().Count;

        Assert.Equal(countBefore - 1, countAfter);
    }

    [Theory]
    [InlineData(1)]
    public void DeleteAProperty_OnFailure_ReturnException(int propertyId)
    {
        using var testDatabase = CreateTestDatabase();

        var action = () => testDatabase.PropertyService.DeletePropertyById(propertyId);

        Assert.Throws<Exception>(action);
    }

*/
}
