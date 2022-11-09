using ChatApplicationWithSQLServer.DataModels;
using ChatApplicationWithSQLServer.Interfaces;
using ChatApplicationWithSQLServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace ChatApplicationWithSQLServer
{
    public class Startup
    {
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
            //configuration.GetConnectionString("database_chatapplication");
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ChatApplicationDBContext>(options =>
        options.UseSqlServer(configRoot.GetConnectionString("database_chatapplication")));

            //services.AddControllers();
            services.AddControllers(options => options.EnableEndpointRouting = false);
            services.AddRazorPages();
            

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();

            services.AddMvcCore().AddApiExplorer();
            services.AddSignalR();

            services.AddSwaggerGen();
        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

			app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
            });

            //app.UseCors();
        }
    }
}
