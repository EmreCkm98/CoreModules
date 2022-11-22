using Module_9_Caching.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ICarService, CarService>();//servisimizi uygulamada kullanýlabilir hale getirdik.
builder.Services.AddMemoryCache();
//In-memory cache özelliði asp.net core içerisinde bir service olarak bulunmaktadýr. Bu servis default kapalý gelir.
//.net core uygulamalrýnda inmemory-cache servisini kullanmak için servisi ekledik.

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
