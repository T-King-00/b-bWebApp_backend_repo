using BookingProject.Database;

namespace BookingProject.Endpoints;

public static class IdentityEndPoints
{
    public static IEndpointRouteBuilder MapCustomIdentityEndPoints(this IEndpointRouteBuilder app)
    {
        var user = app.MapGroup("/");
        
        user.MapGet("test/public", (AppDbContext dbContext) =>
        {
            return Results.Ok("Anyone can access this endpoint");

        });

        app.MapGet("test/private", (AppDbContext dbContext) =>
        {
            var users=dbContext.Users.ToList();
            
            //makes the response intention explicit.
            return Results.Ok(users);
        }).RequireAuthorization();
        


        return app;
        
    }

}