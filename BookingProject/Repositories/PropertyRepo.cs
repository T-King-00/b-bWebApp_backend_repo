using BookingProject.Database;
using Microsoft.EntityFrameworkCore;

namespace BookingProject;

public class PropertyRepo : IPropertyRepo
{
    AppDbContext _context;
    
    public PropertyRepo(AppDbContext context)
    {
        _context = context;
    }
    
    public List<BaseProperty> GetAllProperties()
    {
        return _context.BaseProperties
            .ToList();
    }
    
    public List<Hotel> GetAllHotels()
    {
        return _context.BaseProperties
            .OfType<Hotel>().Include(h=>h.Rooms)
            .ThenInclude(h=>h.Beds)
            .ToList();
    }

    public BaseProperty GetPropertyById(int id)
    {
        return _context.BaseProperties
            .FirstOrDefault(x => x.Id == id);
    }

    public void AddProperty(BaseProperty property)
    {
        try
        {
            _context.Add(property);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       
    }

    public void UpdateProperty(BaseProperty property)
    {
        throw new NotImplementedException();
    }

    public BaseProperty FetchPropertyByIdToRemove(int id)
    {
        List<BaseProperty> properties = GetAllProperties();
        BaseProperty propertytoRemove =properties.Where(x=>x.Id==id)
            .FirstOrDefault();
        try
        {
            if (propertytoRemove!=null)
            {
                return propertytoRemove;
            }

            throw new Exception("Error : Property not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public void DeleteProperty(int id)
    {
        try
        {
            BaseProperty propertytoRemove = FetchPropertyByIdToRemove(id);
           
            _context.Remove(propertytoRemove);
            _context.SaveChanges();
           
        }
        catch (Exception e)
        {
            Console.WriteLine("There is a problem in deleting the property : " + e);
            throw;
        }
       
    }
}