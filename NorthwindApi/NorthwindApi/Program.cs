using Microsoft.EntityFrameworkCore;
using NorthwindApi.Models;
using NorthwindApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//register NWcontext to DI container
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration["default"]));
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("Northwind"));
builder.Services.AddScoped<IService, SupplierServiceLayer>();
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => 
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();