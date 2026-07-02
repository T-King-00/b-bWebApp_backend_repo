

// These exceptions describe business errors.
namespace BookingProject.Exceptions.DomainExceptions
    {
    public sealed class RoomNotFoundException: Exception
    {
        public RoomNotFoundException(int id):base($"Room with id {id} not found !")
        { }
    }

   

    }
