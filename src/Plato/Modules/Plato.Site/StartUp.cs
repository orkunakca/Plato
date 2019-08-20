﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Plato.Internal.Abstractions.Routing;
using Plato.Internal.Assets.Abstractions;
using Plato.Internal.Models.Shell;
using Plato.Internal.Hosting.Abstractions;
using Plato.Site.Assets;

namespace Plato.Site
{
    public class Startup : StartupBase
    {
        private readonly IShellSettings _shellSettings;

        public Startup(IShellSettings shellSettings)
        {
            _shellSettings = shellSettings;
        }

        public override void ConfigureServices(IServiceCollection services)
        {

            // Register assets
            services.AddScoped<IAssetProvider, AssetProvider>();

            // Homepage route providers
            services.AddSingleton<IHomeRouteProvider, HomeRoutes>();

        }

        public override void Configure(
            IApplicationBuilder app,
            IRouteBuilder routes,
            IServiceProvider serviceProvider)
        {


            // Index
            routes.MapAreaRoute(
                name: "PlatoSite",
                areaName: "Plato.Site",
                template: "site/{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

        }
    }
}