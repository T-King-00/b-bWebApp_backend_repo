using System.Text.Json.Serialization;
using BookingProject;
using BookingProject.Database;
using BookingProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        
    });

builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<HotelService>();

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

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1"));

}

await using ( var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
};


app.Run();
