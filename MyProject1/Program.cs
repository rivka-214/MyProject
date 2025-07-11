using AiFirstAidApi.Services;
using AutoMapper;
using Common.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mock;

using Repository.Interfacese;
using Repository.Repositories;
using Service.Interfaces;
using Service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load configuration
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

// ✅ Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();
// ✅ Swagger + JWT support
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token",
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
            Array.Empty<string>()
        }
    });
});

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",
            "http://localhost:3001",
            "http://localhost:3002")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// ✅ Authentication + JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
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

// ✅ AutoMapper
builder.Services.AddAutoMapper(typeof(MyMapper));

// ✅ Register Context, Repositories, Services
builder.Services.AddDbContext<IContext, Database>();
builder.Services.AddScoped<IUserServicecs, UserService>();
builder.Services.AddScoped<ICallsRepository, CallsRepository>();
builder.Services.AddScoped<IVolunteer, VolunteersRepository>();
builder.Services.AddScoped<ICallService, CallService>();
builder.Services.AddScoped<IService<CallsDto>>(sp => sp.GetRequiredService<ICallService>());
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
// ✅ VolunteersCallService with multi-interface registration
builder.Services.AddScoped<VolunteersCallService>();
builder.Services.AddScoped<IVolunteersCallLogic>(sp => sp.GetRequiredService<VolunteersCallService>());
builder.Services.AddScoped<IService<VolunteerCallsDto>>(sp => sp.GetRequiredService<VolunteersCallService>());
builder.Services.AddHttpClient<OpenAiService>();
// ✅ Resolve circular dependency
builder.Services.AddScoped<Func<IVolunteersCallLogic>>(sp => () => sp.GetRequiredService<IVolunteersCallLogic>());

// ✅ Add SignalR (optional)
builder.Services.AddSignalR();
builder.Services.AddSingleton<FirstAidService>();
// ✅ General services extension
builder.Services.AddServices(); // ודאי שהשיטה קיימת

// ✅ HttpContextAccessor
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ✅ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// app.MapHub<VolunteersHub>("/volunteersHub"); // אם תשתמשי ב-SignalR

app.Run();
