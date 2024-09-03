using ChatRoom.ChatServerApi.SerivceExtention;
using ChatRoom.Core.Hubs;
using ChatRoom.Core.Middleware;   

namespace ChatRoom.ChatServerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            ENV = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment ENV { get;  }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.ChatServiceApiConfigure(ENV);
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5168")
                            .AllowAnyHeader()
                            .WithMethods("GET", "POST")
                            .AllowCredentials();
                    });
            });
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(); 
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionMiddleware>(); 
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapControllers();
            });
        }
    }
}
