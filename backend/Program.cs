using Clerk.BackendAPI;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Clerk backend secret key (appsettings.json içinden)
builder.Services.AddSingleton(_ =>
    new ClerkBackendApi(bearerAuth: builder.Configuration["Clerk:SecretKey"])
);

// JWT Auth - gelen Clerk token'larýný doðrulamak için
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
            NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", // <-- userId olarak kullanýlacak
            RoleClaimType = "role"
        };



        // DEBUG için event ekledik
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine("JWT DOÐRULANAMADI:");
                Console.WriteLine(ctx.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = ctx =>
            {
                Console.WriteLine("TOKEN DOÐRULANDI");
                Console.WriteLine(ctx.Principal.Identity.Name);
                return Task.CompletedTask;
            }
        };


    });

builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();

// CORS: Frontend'in eriþebilmesi için
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") //Ayaða kaldýrdýðýmýz front uygulamasý için
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
