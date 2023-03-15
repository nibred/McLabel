using McLabel.Services;
using McLabel.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.Utils.Registrators
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<FileDialogService>()
            .AddSingleton<IFileService, XmlFileService>()
            .AddSingleton<ViewNavigationService>();
    }
}
