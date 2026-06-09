using System.Text.Json.Serialization;
using BookingProject;
using BookingProject.Database;
using BookingProject.Services;
using Microsoft.EntityFrameworkCore;

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
    });

builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<HotelService>();

builder.Services.AddLogging();

//db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("connection string not valid");

builder.Services.AddDbContext<AppDbContext>(
    options=> options.UseSqlite(connectionString));

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


app.Run();