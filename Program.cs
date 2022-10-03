
 using ApiBlog.Data;
using ApiBlogAspNet;
using ApiBlogAspNet.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiBlog
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationMVC(builder);
            ConfigureServices(builder);
            ConfigureAuthentication(builder);

            //builder.Services.AddTransient();   // aways create a new instance (**TokenService = new TokenService**)
            //builder.Services.AddScoped(); // transiction (aways kill the processe before anding tasks 
            //builder.Services.AddSingleton(); // one for app ( started on load of app 
           
            var app = builder.Build();
            LoadConfiguration(app);

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();

            void LoadConfiguration(WebApplication app)
            {
                Configuration.JwtKey = app.Configuration.GetValue<string>(key: "JwtKey");
                Configuration.ApiKeyName = app.Configuration.GetValue<string>(key: "ApiKeyName");
                Configuration.ApiKey = app.Configuration.GetValue<string>(key: "ApiKey");

                var smtp = new Configuration.SmtpConfiguration();
                app.Configuration.GetSection("smtp").Bind(smtp);
                Configuration.Smtp = smtp;
            }
            void ConfigureAuthentication(WebApplicationBuilder builder)
            {
                var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
                builder.Services.AddAuthentication(configureOptions: x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });
                }
            void ConfigurationMVC(WebApplicationBuilder builder)
                {
                    builder.Services.AddControllers()
                          .ConfigureApiBehaviorOptions(options =>
                         { options.SuppressModelStateInvalidFilter =true;});
                }
            void ConfigureServices(WebApplicationBuilder builder)
            {
                builder.Services.AddDbContext<ApiDataContext>();
                builder.Services.AddTransient<TokenService>();
            }
        
        }
    }
}










