using RestaurantApi.App.RestaurantApi.App;
using RestaurantApi.Dal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<RestaurantDbContext>();

var restaurantDI = new RestaurantDI();
restaurantDI.ConfigureServices(builder.Services);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//var restaurantDbContext = app.Services.GetService<RestaurantDbContext>();
//restaurantDI.AddDataToDB(restaurantDbContext!);


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
