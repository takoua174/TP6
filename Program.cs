using TP6.Data;
using Microsoft.EntityFrameworkCore;
using TP6.Services;
using TP6.Services.ServiceContracts;
using TP6.Controllers;
using TP6.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TP6.JWTBearerConfig;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
//add jwt bearer
builder.Services.Configure<JWTBearerTokenSettings>(builder.Configuration.GetSection("JWTBearerTokenSettings"));

builder.Services.AddEndpointsApiExplorer();

//add category service
builder.Services.AddScoped<ICategoryService, CategoryService>();
// Add services to the container.

//add controller service
builder.Services.AddControllers();

//add usr services
builder.Services.AddScoped<IUserService, UserService>();

//add dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//add identity

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()//ApplicationUser ->user type and IdentityRole ->role type
    .AddEntityFrameworkStores<ApplicationDbContext>()//the dbcontext used for storing identity tables
    .AddDefaultTokenProviders();//token generators exp : for two factor authentication
//add authorization
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication(); // Activer l'authentification
app.UseAuthorization(); // Activer l'autorisation

//u need to add this so ur controller will be recognized by the application
app.MapControllers();
app.Logger.LogInformation(string.Join("\n", app.Urls));
app.Run();
