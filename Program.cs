
 using ApiBlog.Data;


namespace ApiBlog
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter =true;
            });
            builder.Services.AddDbContext<ApiDataContext>();


            var app = builder.Build();
            app.MapControllers();

            app.Run();
        }
    }
}










