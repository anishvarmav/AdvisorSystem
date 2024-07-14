using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options => { options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader()); });
builder.Services.AddDbContext<AdvisorDbContext>(options => options.UseInMemoryDatabase("AdvisorDb"));
builder.Services.AddSingleton(typeof(AdvisorCache<int, Advisor>));
builder.Services.AddScoped<IAdvisorRepository, AdvisorRepository>();
builder.Services.AddControllers();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
