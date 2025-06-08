using Clerk.BackendAPI;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Clerk backend secret key (appsettings.json i�inden)
builder.Services.AddSingleton(_ =>
    new ClerkBackendApi(bearerAuth: builder.Configuration["Clerk:SecretKey"])
);

// JWT Auth - gelen Clerk token'lar�n� do�rulamak i�in
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://simple-sponge-60.clerk.accounts.dev";
        options.Audience = "pk_test_key";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", // <-- userId olarak kullan�lacak
            RoleClaimType = "role"
        };



        // DEBUG i�in event ekledik
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT DO�RULANAMADI:");
                Console.WriteLine(ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("TOKEN DO�RULANDI");
                Console.WriteLine(ctx.Principal.Identity.Name);
                return Task.CompletedTask;
            }
        };


    });

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();

// CORS: Frontend'in eri�ebilmesi i�in
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") //Aya�a kald�rd���m�z front uygulamas� i�in
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();
