

// These exceptions describe business errors.
namespace BookingProject.Exceptions.Exceptions
    {
    public sealed class RoomNotFoundException: Exception
    {
        public RoomNotFoundException(int id):base($"Room with id {id} not found !")
        { }
    }

   

    }
