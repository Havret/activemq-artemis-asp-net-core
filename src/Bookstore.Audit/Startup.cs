using ActiveMQ.Artemis.Client;
using ActiveMQ.Artemis.Client.Extensions.DependencyInjection;
using ActiveMQ.Artemis.Client.Extensions.Hosting;
using Bookstore.Audit.Consumers;
using Bookstore.Contracts;
using Bookstore.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bookstore.Audit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddDbContext<BookstoreAuditContext>(options =>
                options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=BookstoreAudit;Integrated Security=True"));

            services.AddActiveMq("bookstore-cluster", new[] { Endpoint.Create(host: "localhost", port: 5672, "guest", "guest") })
                    .AddTypedConsumer<BookCreated, BookCreatedConsumer>(RoutingType.Multicast, nameof(Audit))
                    .AddTypedConsumer<BookUpdated, BookUpdatedConsumer>(RoutingType.Multicast, nameof(Audit))
                    .EnableAddressDeclaration()
                    .EnableQueueDeclaration();

            services.AddHostedService<InitDatabaseHostedService>();
            services.AddActiveMqHostedService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}