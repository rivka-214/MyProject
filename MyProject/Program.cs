using Service.Services;
using Repository.Interfacese;
using Mock;
using AutoMapper; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IContext, Database>(); 
builder.Services.AddAutoMapper(typeof(MyMapper));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices();

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
