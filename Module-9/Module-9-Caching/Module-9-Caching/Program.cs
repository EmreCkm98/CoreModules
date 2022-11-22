using Module_9_Caching.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ICarService, CarService>();//servisimizi uygulamada kullan�labilir hale getirdik.
builder.Services.AddMemoryCache();
//In-memory cache �zelli�i asp.net core i�erisinde bir service olarak bulunmaktad�r. Bu servis default kapal� gelir.
//.net core uygulamalr�nda inmemory-cache servisini kullanmak i�in servisi ekledik.

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
