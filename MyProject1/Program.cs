using AutoMapper;
using Common.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mock;
using Repository.Interfacese;
using Service.Interfaces;
using Service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + אבטחה
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// הגדרת CORS ל-React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3001",
            "http://localhost:3000",
            "http://localhost:3004"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

// שירותים - הוספת DbContext
builder.Services.AddDbContext<IContext, Database>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MyMapper));

// רישום VolunteersCallService כ- IVolunteersCallLogic ו- IService<VolunteerCallsDto>
builder.Services.AddScoped<VolunteersCallService>();
builder.Services.AddScoped<IVolunteersCallLogic>(sp => sp.GetRequiredService<VolunteersCallService>());
builder.Services.AddScoped<IService<VolunteerCallsDto>>(sp => sp.GetRequiredService<VolunteersCallService>());

// שבירת תלות מעגלית ב-CallService עם Func<IVolunteersCallLogic>
builder.Services.AddScoped<Func<IVolunteersCallLogic>>(sp => () => sp.GetRequiredService<IVolunteersCallLogic>());

// רישום CallService
builder.Services.AddScoped<ICallService, CallService>();
builder.Services.AddScoped<IService<CallsDto>>(sp => (IService<CallsDto>)sp.GetRequiredService<ICallService>());

// הוספת שירותים כלליים
builder.Services.AddServices();
builder.Services.AddSignalR();

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// הפעלת CORS
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.MapHub<VolunteersHub>("/volunteersHub");

app.Run();
