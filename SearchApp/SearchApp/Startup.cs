using AutoMapper;
using BingSearchClient.Startup;
using Domain.Core.Models;
using GoogleSearchClient.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YandexSearchClient.Startup;
using Repositories.Startup;

namespace SearchApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            AddAutoMapperServices(services);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));

            services.AddGoogleClient(Configuration);
            services.AddYandexClient(Configuration);
            services.AddBingClient(Configuration);

            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseMvc();
        }

        private void AddAutoMapperServices(IServiceCollection services)
        {
            services.AddSingleton(
                sp =>
                {
                    var profiles = sp.GetServices<Profile>();
                    return new MapperConfiguration(
                            config =>
                            {
                                foreach (var profile in profiles)
                                {
                                    config.AddProfile(profile);
                                }
                            })
                        .CreateMapper(sp.GetService);
                });
        }
    }
}
