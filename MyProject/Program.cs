<<<<<<< HEAD
using Microsoft.EntityFrameworkCore.Storage;
using Reposetory.Entities;
using Repository.Interfacese;
using Repository.Repositories;
=======
using Service.Services;
using Repository.Interfacese;
using Mock;
using AutoMapper; 
>>>>>>> e0db1c2e030d426d2d54f58510fdb1b41cab3ce3

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IContext, Database>(); 
builder.Services.AddAutoMapper(typeof(MyMapper));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();

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
