using DauStore.Api.Authentication;
using DauStore.Core.Entities;
using DauStore.Core.Interfaces.IRepositories;
using DauStore.Core.Interfaces.IServices;
using DauStore.Core.Services;
using DauStore.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DauStore.Api
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

            var key = "This is my test key";

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        string token = context.Request.Headers["Authorization"];
                        if (!ValidateTokenClass.ValidateToken(token))
                        {
                            context.Fail("Fail okok");
                        }
                        return Task.CompletedTask;
                    }
                };
            });


            services.AddScoped(typeof(IJwtAuthenticationManager), typeof(JwtAuthenticationManager));

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .SetIsOriginAllowed((host) => true)
                               .AllowCredentials();
                    }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DauStore", Version = "v1" });
            });

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));

            services.AddScoped(typeof(INotificationRepository), typeof(NotificationRepository));
            services.AddScoped(typeof(INotificationService), typeof(NotificationService));

            services.AddScoped(typeof(ICategoryService), typeof(CategoryService));

            services.AddScoped(typeof(IItemRepository), typeof(ItemRepository));
            services.AddScoped(typeof(IItemService), typeof(ItemService));

            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddScoped(typeof(IOrderService), typeof(OrderService));

            services.AddScoped(typeof(IVoucherRepository), typeof(VoucherRepository));
            services.AddScoped(typeof(IVoucherService), typeof(VoucherService));

            services.AddOptions(); // Kích hoạt Options
            var mailsettings = Configuration.GetSection("MailSettings");  // đọc config
            services.Configure<MailSettings>(mailsettings);                // đăng ký để Inject

            // Đăng ký SendMailService với kiểu Transient, mỗi lần gọi dịch
            // vụ ISendMailService một đới tượng SendMailService tạo ra (đã inject config)
            services.AddTransient<IMailService, MailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
