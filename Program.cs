using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentACareGRPC.Authorization;
using RentACareGRPC.Services;
using RentACareGRPC.SQL;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Ceo", policy =>
    {
        policy.Requirements.Add(new CEORequirement("ceo"));
    });
    options.AddPolicy("Admin", policy =>
    {
        policy.Requirements.Add(new AdminRequirement("admin"));
    }); 
    options.AddPolicy("PermissionAdmin", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("Permission", "MemberAdmin");
    });
    options.AddPolicy("PermissionMemeberCEO", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("Permission", "MemberCEO");
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<CustomerServicesPROTO>();
app.MapGrpcService<CarsService>();
app.MapGrpcService<reservationService>();
app.MapGrpcService<AuthLoginService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
    