﻿using Autofac;
using Jimu.Client.ApiGateway;
using Jimu.Client.ApiGateway.Swagger;
using Jimu.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Jimu.Client.ApiGateway.Swagger
{
    internal class SwaggerClientModule : ClientWebModuleBase
    {
        SwaggerOptions _options;

        public SwaggerClientModule(IConfigurationRoot jimuAppSettings) : base(jimuAppSettings)
        {
            _options = jimuAppSettings.GetSection(typeof(SwaggerOptions).Name).Get<SwaggerOptions>();
            if (_options == null)
                _options = new SwaggerOptions();
        }
        public override void DoWebConfigureServices(IServiceCollection services, IContainer container)
        {
            if (_options.Enable)
            {
                services.AddSwaggerGen(c =>
                {
                    c.DocumentFilter<SwaggerDocumentFilter>();
                    c.SwaggerDoc(_options.Version, new OpenApiInfo { Title = _options.Title, Version = _options.Version });
                });

            }
                base.DoWebConfigureServices(services, container);
        }
        public override void DoWebConfigure(IApplicationBuilder app, IContainer container)
        {
            if (_options.Enable)
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/{_options.Version}/swagger.json", $"{_options.Title} {_options.Version}");
                });
                app.UseSwagger();
            }
            base.DoWebConfigure(app, container);
        }
    }
}
