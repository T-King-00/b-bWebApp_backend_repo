using System.Text.Json.Serialization;
using BookingProject;
using BookingProject.Database;
using BookingProject.Exceptions.ExceptionHandler;
using BookingProject.Services;
using BookingProject.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddCors();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        
    });
// services DI
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<HotelService>();

// validation rules DI
builder.Services.AddScoped<IBookingRule, BookingMustNotOverlapRule>();
builder.Services.AddScoped<IBookingRule, DateValidationRule>();
builder.Services.AddScoped<IBookingRule, BookingNoDuplicateIdRule>();
builder.Services.AddScoped<CompositeValidator>();


builder.Services.AddLogging();

//db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("connection string not valid");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString)
    .UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
        var logger = context.GetService<ILoggerFactory>().CreateLogger<DbSeeder>();
        await DbSeeder.SeedAsync((AppDbContext)context, logger, cancellationToken);
    })
    .UseSeeding((context,_)=>
    {        
        var logger = context.GetService<ILoggerFactory>().CreateLogger<DbSeeder>();
        DbSeeder.SeedSync((AppDbContext)context,logger);
    } ) );


    
var app = builder.Build();

await using ( var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
};


//custom middleware
app.Use(async (context, next) =>
    {
        Console.WriteLine("Middleware running");
        await next();
    }
);
app.UseExceptionHandler();


app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1"));

}

/*if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error");
    
}*/



app.MapControllers();
app.Run();
