namespace BookingProject.Services;

public class PropertyService (IPropertyRepo propertyRepo)
{
    public List<BaseProperty> GetAllProperties()
    {
        
        return propertyRepo.GetAllProperties();
    }
    public BaseProperty GetPropertyById(int id)
    {
        return propertyRepo.GetPropertyById(id);
    }
    public void AddProperty(BaseProperty property)
    {
        propertyRepo.AddProperty(property);
    }
    public void AddHotel(Hotel property)
    {
        propertyRepo.AddProperty(property);    }
    public void UpdateProperty(BaseProperty property)
    {
        propertyRepo.UpdateProperty(property);
    }
    public void DeletePropertyById(int id)
    {
        propertyRepo.DeleteProperty(id);
    }

  
}