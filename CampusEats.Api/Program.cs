using CampusEats.Api.Services;
using CampusEats.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http, Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste ONLY the token - no 'Bearer ' prefix."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference
        { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
        Array.Empty<string>() }
    });
});


builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IMenuService, MenuService>();
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);
builder.Services
 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(opt => opt.TokenValidationParameters = new()
 {
     ValidateIssuer = true, ValidIssuer = jwt["Issuer"],
     ValidateAudience = true, ValidAudience = jwt["Audience"],
     ValidateIssuerSigningKey = true,
     IssuerSigningKey = new SymmetricSecurityKey(key),
     ValidateLifetime = true
 });
builder.Services.AddAuthorization();

builder.Services.AddCors(o => o.AddPolicy("Frontend", p =>
    p.WithOrigins("https://campuseats-web.vercel.app")
     .AllowAnyHeader()
     .AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();