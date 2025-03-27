using Hotels.Repository.Data;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            builder.AddControllers();
            builder.AddOpenApi();
            builder.AddDatabase();
            builder.AddMapping();
            builder.AddRepository();
            builder.AddService();

            builder.ConfigureJwtOptions();
            builder.AddIdentity();
            builder.AddJwtGenerator();
            builder.AddAuthentication();
            builder.AddAuthService();

            var app = builder.Build();

           
             app.MapOpenApi();
           

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
