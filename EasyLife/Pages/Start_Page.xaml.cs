using EasyLife.Models;
using EasyLife.Services;
using MvvmHelpers.Commands;
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
	public partial class Start_Page : ContentPage
	{
		public Start_Page ()
		{
			InitializeComponent ();
        }
        private void Financial_Book_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new Master_Financial_Book_Page();

            Preferences.Set("Start_Option",1);
        }

        private void To_Do_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Start_Option", 2);

            App.Current.MainPage = App.master_To_Do_Page;
        }
    }
}