using Product.Microservice.Extensions;
using Product.Microservice.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
// Prepare database by adding data from json
app.PrepareDatabase(System.IO.File.ReadAllText(builder.Environment.ContentRootPath + @"\Data\DummyData\MOCK_DATA.json"))
                .GetAwaiter()
                .GetResult();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
