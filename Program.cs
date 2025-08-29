using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CarAuctionApi.Data;
using CarAuctionApi.Repositories;
using CarAuctionApi.Repositories.Interfaces;
using CarAuctionApi.Services;
using CarAuctionApi.Services.Interfaces;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using CarAuctionApi.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequiredLength = 6;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = true;
  options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

var tokenSettings = builder.Configuration.GetSection("TokenSettings");
var key = Encoding.ASCII.GetBytes(tokenSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = tokenSettings["Issuer"],
    ValidAudience = tokenSettings["Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(key)
  };
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarAdRepository, CarAdRepository>();
builder.Services.AddScoped<ICarAdService, CarAdService>();
builder.Services.AddScoped<IBidRepository, BidRepository>();
builder.Services.AddScoped<IBidService, BidService>();
builder.Services.AddScoped<IDownPaymentRepository, DownPaymentRepository>();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
  {
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
  });
});

builder.Services.Configure<FormOptions>(options =>
{
  options.MultipartBodyLengthLimit = 104857600; 
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseStaticFiles(); 
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  CarAuctionApi.Data.SeedData.Initialize(services);
}

app.Run();