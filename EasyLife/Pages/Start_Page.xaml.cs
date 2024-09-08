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
        public static Master_To_Do_Page master_To_Do_Page;
		public Start_Page ()
		{
			InitializeComponent ();
        }

        public async void Appearing_Methode(object sender, EventArgs e)
        {
            Page MainPage = new Page ();

            if (Preferences.Get("Start_Option", 0) == 1)
            {
                MainPage = new Master_Financial_Book_Page();

            }
            if (Preferences.Get("Start_Option", 0) == 2)
            {
                List<Category> result = await CategoryService.Get_all_Categorys();
                List<To_Do_Item> result2 = await To_DoService.Get_all_To_DOs();


                master_To_Do_Page = new Master_To_Do_Page(await CategoryService.Get_all_Categorys(), result2);

                MainPage = master_To_Do_Page;

            }
            if (Preferences.Get("Start_Option", 0) == 0)
            {
                return;
            }
            App.Current.MainPage = MainPage;
        }
        private void Financial_Book_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new Master_Financial_Book_Page();

            Preferences.Set("Start_Option",1);
        }

        private async void To_Do_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Start_Option", 2);

            List<Category> result = await CategoryService.Get_all_Categorys();
            List<To_Do_Item> result2 = await To_DoService.Get_all_To_DOs();

            master_To_Do_Page = new Master_To_Do_Page(await CategoryService.Get_all_Categorys(), result2);

            App.Current.MainPage = master_To_Do_Page;
        }
    }
}