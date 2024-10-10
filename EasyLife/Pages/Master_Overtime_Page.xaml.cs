using EasyLife.Pages.Overtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master_Overtime_Page : Shell
    {
        public Master_Overtime_Page()
        {
            Routing.RegisterRoute(nameof(Overtime_Home_Page), typeof(Overtime_Home_Page));
            Routing.RegisterRoute(nameof(Overtime_Trash_Page), typeof(Overtime_Trash_Page));

            InitializeComponent();
        }
        private void Header_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Start_Option", 0);

            App.Current.MainPage = new Start_Page();
        }
    }
}