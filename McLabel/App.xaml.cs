using McLabel.Utils.Registrators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace McLabel
{
    public partial class App : Application
    {
        private static IHost _host;
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);
        }
        public static IHost MainHost
        {
            get
            {
                if (_host == null)
                    _host = CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
                return _host;
            }
        }
        public static IServiceProvider Services => MainHost.Services;
        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddViewModels()
            .AddServices();
        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = MainHost;
            await host.StartAsync();
            base.OnStartup(e);
        }
        protected override async void OnExit(ExitEventArgs e)
        {
            using (var host = MainHost)
            {
                await host.StopAsync();
            }
            base.OnExit(e);
        }
    }
}
