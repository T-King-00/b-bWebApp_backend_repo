namespace BookingProject;

public interface IPropertyRepo
{
    public List<BaseProperty> GetAllProperties();
    public BaseProperty GetPropertyById(int id);
    public void AddProperty(BaseProperty property);
    public void UpdateProperty(BaseProperty property);
    public void DeleteProperty(int id);
}


