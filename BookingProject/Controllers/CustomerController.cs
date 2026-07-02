using BookingProject.Exceptions.DomainExceptions;
using BookingProject.Models.DTO;
using BookingProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingProject.Controllers;


[ApiController]
[Route("/api/customer/")]
public class CustomerController(CustomerService customerService, ILogger<CustomerController> logger):ControllerBase
{

    [HttpPost("id")]
    public IActionResult Get([FromBody]CustomerRequestDto customerRequest)
    {
        logger.LogInformation("Controller Action: Fetching  customer id  ....");
        var customerId="";
        try
        {
            customerId = customerService.GetId(customerRequest).ToString();
            // if no id is found then user is new. add to db
            if (string.IsNullOrEmpty(customerId))
            {
                customerService.Add(customerRequest);
                customerId = customerService.GetId(customerRequest).ToString();
                
            }
        }
        catch (CustomExceptions.InvalidCustomerData e)
        {
            logger.LogError("Controller Action: Failed to fetch  customer id  ....");
            return StatusCode(statusCode: 500, new
            {
                message = "Invalid request, please try again later",
         
            });
        }
    
        return StatusCode(statusCode: 200, new
        {
            message = "Customer is found in db",
            id=customerId
        });

    }
    
}
