using FlightBooking_CaseStudy_sameer.AppDbContext;
using FlightBooking_CaseStudy_sameer.Sevices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Sending_Emails_in_Asp.Net_Core;

var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
   
        builder.Services.AddControllers();
        builder.Services.AddSingleton<EmailSending>();
        builder.Services.AddSingleton<EmailBooking>();


        builder.Services.AddSingleton<PasswordHasher>();

        builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        });



        // Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var provider = builder.Services.BuildServiceProvider();
        var config = provider.GetService<IConfiguration>();
        builder.Services.AddDbContext<ApplicationDbContext>(item => item.UseSqlServer(config.GetConnectionString("FlightBooking")));
        ////////////////////////---------JWT Token------------//////////////////////////////
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        //Configure Authorization
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("SoftwareDeveloperOnly", policy => policy.RequireRole("Software Developer"));
            options.AddPolicy("PassengersOnly", policy => policy.RequireRole("Passenger"));
        });

        //////////////////////////////////////////////////////////////////

        // Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
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
            new string[]{}
        }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        // Apply CORS policy
        app.UseCors("AllowLocalhost");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
//program.cs
 

 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
 
