using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;


[ApiController]
[Route("[controller]")]
public class PropertyController(PropertyService propertyService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<BaseProperty>> GetAll()
    {
        var properties = propertyService.GetAllProperties();
        return Ok(properties);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BaseProperty> GetById(int id)
    {
        var property = propertyService.GetPropertyById(id);

        if (property is null)
            return NotFound();

        return Ok(property);
    }

    [HttpPost]
    public ActionResult<BaseProperty> Create([FromBody] BaseProperty? property)
    {
        if (property is null)
            return BadRequest();

        propertyService.AddProperty(property);

        return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
    }

    [HttpPut("{id:int}")]
    public ActionResult<BaseProperty> Update(int id, [FromBody] BaseProperty? property)
    {
        if (property is null)
            return BadRequest();

        var existingProperty = propertyService.GetPropertyById(id);

        if (existingProperty is null)
            return NotFound();

        property.Id = id;
        propertyService.UpdateProperty(property);

        return Ok(property);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var property = propertyService.GetPropertyById(id);

        if (property is null)
            return NotFound();

        propertyService.DeletePropertyById(id);

        return NoContent();
    }
}
