
using LibraryManagementApp.Configuration;
using LibraryManagementApp.Data;
using LibraryManagementApp.Helpers;
using LibraryManagementApp.Repositories;
using LibraryManagementApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Text;

namespace LibraryManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            connString = connString!.Replace("{DB_PASS}", Environment.GetEnvironmentVariable("DB_PASS") ?? "");

            builder.Services.AddDbContext<LibraryAppDbContext>(options => options.UseSqlServer(connString));
            builder.Services.AddRepositories();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IReaderService, ReaderService>();
            builder.Services.AddScoped<ILibrarianService, LibrarianService>();

            // ToDo Add Services

            builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MapperConfig>());
            builder.Host.UseSerilog((ctx, lc) =>
                lc.ReadFrom.Configuration(ctx.Configuration));

           

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularApp",
                   policy =>
                   {
                       policy.WithOrigins(
                               "http://localhost:4200",    // HTTP
                               "https://localhost:4200",   // HTTPS
                               "http://localhost:4201",    // Alternative port
                               "https://localhost:4201"
                           )
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials()
                           .SetPreflightMaxAge(TimeSpan.FromMinutes(10)) // Cache preflight
                           .WithExposedHeaders("Authorization"); // Εξαγωγή Authorization header
                   });
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("Authentication");
                options.IncludeErrorDetails = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://localhost:5001",

                    ValidateAudience = true,
                    ValidAudience = "https://localhost:5001",

                    ValidateLifetime = true,    // Validate the token's expiration

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
                };
            });

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("LocalClient",
            //        b => b.WithOrigins("https://localhost:5001")
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //    );
            //});

            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        b => b.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //    );
            //});


            //builder.Services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

            //});

            //builder.Services.AddControllers();

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Management App", Version = "v1" });
            // options.SupportNonNullableReferenceTypes(); // default true > .NET 6
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
               new OpenApiSecurityScheme 
            {
                   Description = "JWT Authorization header using the Bearer scheme.",
                   Name = "Authorization",
                   In = ParameterLocation.Header,
                   Type = SecuritySchemeType.Http,
                   Scheme = JwtBearerDefaults.AuthenticationScheme,
                   BearerFormat = "JWT"
               });
             options.OperationFilter<AuthorizeOperationFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Management App v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors("AngularApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.MapControllers();

            // Προσθήκη για OPTIONS requests
            app.MapMethods("/api/{path}", new[] { "OPTIONS" },
                async (HttpContext context) =>
                {
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("OK");
                });

            app.Run();
        }
    }
}
