using Microsoft.EntityFrameworkCore;
using DSW_3P_PR01.Data;
using DSW_3P_PR01.Repositories;
using DSW_3P_PR01.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores MVC.
builder.Services.AddControllers();


// Configurar Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el repositorio SQL.
builder.Services.AddScoped<IMisionRepository, MisionSqlRepository>();

// Registrar el servicio de misiones.
builder.Services.AddScoped<IMisionService, MisionService>();

//Configuracion del SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    )
);
// Configuración de JWT 
var jwt = builder.Configuration.GetSection("Jwt");
var key = Convert.FromBase64String(
   jwt["Key"] ?? throw new InvalidOperationException(
       "Falta configurar Jwt:Key"));
builder.Services
   .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.TokenValidationParameters =
           new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = jwt["Issuer"],
               ValidateAudience = true,
               ValidAudience = jwt["Audience"],
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero,
               ValidateIssuerSigningKey = true,
               IssuerSigningKey =
                   new SymmetricSecurityKey(key)
           };
   });

//Configurar política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("QuestLogFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5500",
                "http://127.0.0.1:5500"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar Swagger y Swagger UI.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("QuestLogFrontend");
app.UseAuthentication();
app.UseAuthorization();
// Mapear los controladores.
app.MapControllers();

app.Run();

//ORM
//dotnet add package Microsoft.EntityFrameworkCore.SqlServer
//dotnet add package Microsoft.EntityFrameworkCore.Design
//dotnet tool install --global dotnet-ef --version 10.0.*

//dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version "10.0.*"

