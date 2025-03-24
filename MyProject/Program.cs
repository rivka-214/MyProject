using Microsoft.EntityFrameworkCore.Storage;
using Reposetory.Entities;
using Repository.Interfacese;
using Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//הגדרת התלויות

builder.Services.AddScoped<IService<CallsDto>, CallService>();
builder.Services.AddScoped<IRepository<Calls>, CallsRepository>();
builder.Services.AddAutoMapper(typeof(MyMapper));
builder.Services.AddDbContext<IContext, Database>();

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
