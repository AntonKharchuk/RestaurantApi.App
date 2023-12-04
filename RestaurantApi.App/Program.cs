using RestaurantApi.App.RestaurantApi.App;
using RestaurantApi.Dal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var dbContesxt = new RestaurantDbContext();

builder.Services.AddSingleton(dbContesxt);

var restaurantDI = new RestaurantDI();
restaurantDI.ConfigureServices(builder.Services);
restaurantDI.AddDataToDB(dbContesxt);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
