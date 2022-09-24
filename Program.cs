
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

            builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter =true;
            });
            builder.Services.AddDbContext<ApiDataContext>();
            //builder.Services.AddTransient();   // aways create a new instance (**TokenService = new TokenService**)
            //builder.Services.AddScoped(); // transiction (aways kill the processe before anding tasks 
            //builder.Services.AddSingleton(); // one for app ( started on load of app 
            builder.Services.AddTransient<TokenService>();

             
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}










