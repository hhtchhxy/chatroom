using ChatRoom.Core.Extension;
using ChatRoom.Core.Filter;
using ChatRoom.Core.Provider;
using ChatRoom.Core.SerivceExtention;
using ChatRoom.Core.SqlSuger;
using ChatRoom.Model.Config;
using ChatRoom.Repository.Chat.IRepository;
using ChatRoom.Repository.Chat.Repository;
using ChatRoom.Repository.RepositoryBase.IRepositoryService;
using ChatRoom.Repository.RepositoryBase.RepositoryService;
using ChatRoom.Service;
using Hei.Captcha;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace ChatRoom.Api.SerivceExtention
{

    public static class ChatApiExtensions
    {
        private static readonly ILogger _logger;

        static ChatApiExtensions()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            _logger = _logger = loggerFactory.CreateLogger("ChatApiExtensions");
        }
        public static void ApiConfigure(this IServiceCollection services, IHostEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder();

            string jsonFilePath = "";
            try
            {
                if (hostingEnvironment.IsDevelopment())
                {
                    jsonFilePath = $"{AppContext.BaseDirectory}/Appsettings.Development.json";
                }
                else
                {
                    jsonFilePath = $"{AppContext.BaseDirectory}/Appsettings.Production.json";
                }
                if (File.Exists(jsonFilePath))
                {
                    builder.AddJsonFile(jsonFilePath, optional: false, reloadOnChange: true);
                }
                else
                {
                    _logger.LogInformation("Configuration file not found：" + jsonFilePath);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Description Failed to read the configuration file" + jsonFilePath);
                throw e;
            }
            IConfigurationRoot root = builder.Build();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.LoginPath = new PathString("/");
            });
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs }));
            services.AddControllers().AddJsonOptions(configure =>
            {
                configure.JsonSerializerOptions.Converters.Add(new DatetimeConvert());
                configure.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
            services.AddMvc(options =>
            {
                options.Filters.Add<ModelValidationFilter>();
                options.ModelMetadataDetailsProviders.Add(new RequiredBindingMetadataProvider());
            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; 
            });
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatRoom API", Version = "v1" });
            });
            services.AddHeiCaptcha(); 
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); 
            var connection = root.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
            services.AddAutoInject("ChatRoom"); 
            services.AddIPRate(root);
            if (connection != null)
                services.AddSqlSugarSetup(connection.DBConnection);
        }
    }
}

