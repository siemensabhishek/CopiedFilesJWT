using Microsoft.EntityFrameworkCore;

namespace WebAPI
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
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerEntities.CustomerEntities>(optionsAction: options =>
                                    options.UseSqlServer(configRoot.GetConnectionString("CustomerDB")));
            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ViewFinder Hub", Version = "v1" }
            //);
            services.AddSwaggerGen();

            services.AddRazorPages();

        }
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CustomerAPI v1");
            });
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
            app.Run();
        }
    }
}
