﻿using System;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.JSInterop;

namespace Toolbelt.Blazor.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding and using LoadingBar.
    /// </summary>
    public static class LoadingBarExtension
    {
        /// <summary>
        ///  Adds a LoadingBar service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static void AddLoadingBar(this IServiceCollection services)
        {
            services.AddLoadingBar(configure: null);
        }

        /// <summary>
        ///  Adds a LoadingBar service to the specified Microsoft.Extensions.DependencyInjection.IServiceCollection.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service to.</param>
        public static void AddLoadingBar(this IServiceCollection services, Action<LoadingBarOptions> configure)
        {
            services.AddHttpClientInterceptor();
            services.TryAddSingleton(sp =>
            {
                var loadingBar = new LoadingBar(
                    sp.GetService<HttpClientInterceptor>(),
                    sp.GetService<IJSRuntime>());
                configure?.Invoke(loadingBar.Options);
                return loadingBar;
            });

        }

        private static bool Installed;

        /// <summary>
        ///  Installs a LoadingBar service to the runtime hosting environment.
        /// </summary>
        /// <param name="host">The Microsoft.AspNetCore.Blazor.Hosting.WebAssemblyHost.</param>
        public static WebAssemblyHost UseLoadingBar(this WebAssemblyHost host)
        {
            if (Installed) return host;

            var loadinBar = host.Services.GetService<LoadingBar>();
            loadinBar.ConstructDOM();

            Installed = true;
            return host;
        }
    }
}
