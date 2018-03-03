﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MidnightLizard.Schemes.Commander.AutofacModules;
using FluentValidation;
using FluentValidation.AspNetCore;
using MidnightLizard.Schemes.Commander.Requests;

namespace MidnightLizard.Schemes.Commander
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("version", "api-version", "x-api-version"),
                    new QueryStringApiVersionReader());
            });

            services.AddMvc(opt =>
            {
                opt.ModelBinderProviders.Insert(0, new RequestModelBinderProvider());
            }).AddFluentValidation(fv => fv
                .RegisterValidatorsFromAssemblyContaining<Startup>()
                .ImplicitlyValidateChildProperties = true);

            // Autofac - last part!
            var container = new ContainerBuilder();
            container.Populate(services);

            //container.RegisterModule<MediatorModule>();
            container.RegisterModule<SerializationModule>();
            container.RegisterModule<ModelBinderModule>();
            container.RegisterModule<VersionModule>();

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
        }
    }
}
