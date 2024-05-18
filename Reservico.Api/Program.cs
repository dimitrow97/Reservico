using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Reservico.Common.EmailSender;
using Reservico.Data.Interfaces;
using Reservico.Data;
using Reservico.Identity.Code;
using Reservico.Identity.IdentityConfig;
using Reservico.Identity.Otp;
using Reservico.Identity.Token;
using Reservico.Identity.UserClients;
using Reservico.Identity.UserManager;
using Reservico.Identity.UserPasswordManager;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Reservico.Mapping;
using Reservico.Api.Serialization;
using Reservico.Services.Clients;
using Reservico.Services.Reservations;
using Reservico.Services.Locations;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("reservicoDbConnection")
                       ?? throw new InvalidOperationException("Connection string 'reservicoDbConnection' not found.");

builder.Services.AddDbContext<ReservicoDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfig"))
    .Configure<IdentityAuthorizationConfig>(builder.Configuration.GetSection("IdentityAuthorizationConfig"));

// Add services to the container.

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["IdentityAuthorizationConfig:TokenIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Convert.FromBase64String(builder.Configuration["IdentityAuthorizationConfig:TokenSalt"])),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(IdentityRoles.ReadOnlyUserRole,
        options => { options.RequireRole(IdentityRoles.ReadOnlyUserRole); });

    options.AddPolicy(IdentityRoles.ReadWriteUserRole,
        options => { options.RequireRole(IdentityRoles.ReadWriteUserRole); });

    options.AddPolicy(IdentityRoles.AdminRole, options => { options.RequireRole(IdentityRoles.AdminRole); });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services
    .AddScoped<IEmailSender, EmailSender>()
    .AddScoped<IPasswordHasher, PasswordHasher>()
    .AddScoped<IPasswordGenerator, PassswordGenerator>()
    .AddScoped<IUserPasswordManager, UserPasswordManager>()
    .AddScoped<IUserManager, UserManager>()
    .AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services
    .AddTransient<IUserClientManager, UserClientManager>()
    .AddTransient<ITokenProvider, TokenProvider>()
    .AddTransient<IOtpProvider, OtpProvider>()
    .AddTransient<IClientService, ClientService>()
    .AddTransient<ILocationService, LocationService>()
    .AddTransient<IReservationService, ReservationService>()
    .AddTransient<IIdentityAuthorizationConfigProvider, IdentityAuthorizationConfigProvider>()
    .AddTransient(typeof(ICodeProvider<>), typeof(CodeProvider<>));

builder.Services
    .AddSingleton(ReservicoAutoMapperConfig.RegisterMappings(Assembly.Load("Reservico.Identity")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new UtcConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Reservico Core API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
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
            new string[] { }
        }
    });

    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
        {
            return new[] { controllerActionDescriptor.ControllerName };
        }

        return new[] { "Common Endpoints" };
    });

    c.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

app.MapGet("/", () => Results.Ok());

// Configure the HTTP request pipeline.
// if (builder.Configuration.GetValue<bool>("IsLocal"))
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();