using EasyLife.CustomeEventArgs;
using EasyLife.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Compare_Page : ContentPage
    {
        public Compare_Page()
        {
            InitializeComponent();

            Ausgaben_Konto.CommandParameter = (int)0;

            Einnahmen_Konto.CommandParameter = (int)1;


            Barausgaben.CommandParameter = (int)2;

            Bareinnahmen.CommandParameter = (int)3;

            Ausgaben_Briefumschlag.CommandParameter = (int)7;

            Einnahmen_Briefumschlag.CommandParameter = (int)8;
        }
    }
}