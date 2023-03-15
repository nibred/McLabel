using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McLabel.ViewModels.Base
{
    internal class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.Services.GetRequiredService<MainViewModel>();
    }
}
