using EasyLife.Models;
using EasyLife.PageModels.To_Do;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.To_Do
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class To_Do_Detail_Page : ContentPage
    {
        public To_Do_Detail_Page()
        {
            InitializeComponent();
        }
    }
}