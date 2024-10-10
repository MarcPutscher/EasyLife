using EasyLife.Models;
using EasyLife.PageModels;
using EasyLife.Pages;
using EasyLife.Pages.To_Do;
using EasyLife.Services;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife
{
    public partial class App : Application
    {
        public static Master_To_Do_Page master_To_Do_Page;
        public App()
        {
            InitializeComponent();

            MainPage = Choose_Methode();

            //Definiert die Farben in der App.

            dictionary["App_Backgroundcolor"] = Preferences.Get("App_Backgroundcolor", "#1d0e21");

            dictionary["Page_Backgroundcolor"] = Preferences.Get("Page_Backgroundcolor", Color.DarkSlateGray.ToHex());

            dictionary["Transaktion_Backgroundcolor"] = Preferences.Get("Transaktion_Backgroundcolor", Color.Gray.ToHex());
            dictionary["Transaktion_Bordercolor"] = Preferences.Get("Transaktion_Bordercolor", Color.DarkGray.ToHex());
            dictionary["Transaktion_Textcolor"] = Preferences.Get("Transaktion_Textcolor", Color.Black.ToHex());

            dictionary["EntryFrame_Backgroundcolor"] = Preferences.Get("EntryFrame_Backgroundcolor", Color.Orange.ToHex());
            dictionary["EntryFrame_Bordercolor"] = Preferences.Get("EntryFrame_Bordercolor", Color.Coral.ToHex());
            dictionary["EntryFrame_Textcolor"] = Preferences.Get("EntryFrame_Textcolor", Color.Black.ToHex());

            dictionary["Button_Backgroundcolor"] = Preferences.Get("Button_Backgroundcolor", Color.SkyBlue.ToHex());
            dictionary["Button_Bordercolor"] = Preferences.Get("Button_Bordercolor", Color.Blue.ToHex());
            dictionary["Button_Textcolor"] = Preferences.Get("Button_Textcolor", Color.OrangeRed.ToHex());

            dictionary["Label_Textcolor"] = Preferences.Get("Label_Textcolor", Color.White.ToHex());

            dictionary["Grouping_Textcolor"] = Preferences.Get("Grouping_Textcolor", Color.White.ToHex());

            dictionary["Refresh_Color"] = Preferences.Get("Refresh_Color", Color.Blue.ToHex());

            dictionary["Edit_Backgroundcolor"] = Preferences.Get("Edit_Backgroundcolor", Color.Green.ToHex());
            dictionary["Edit_Bordercolor"] = Preferences.Get("Edit_Bordercolor", Color.DarkGreen.ToHex());
            dictionary["Edit_Textcolor"] = Preferences.Get("Edit_Textcolor", Color.White.ToHex());

            dictionary["Revive_Backgroundcolor"] = Preferences.Get("Revive_Backgroundcolor", Color.Blue.ToHex());
            dictionary["Revive_Bordercolor"] = Preferences.Get("Revive_Bordercolor", Color.DarkBlue.ToHex());
            dictionary["Revive_Textcolor"] = Preferences.Get("Revive_Textcolor", Color.White.ToHex());


            dictionary["Remove_Backgroundcolor"] = Preferences.Get("Remove_Backgroundcolor", Color.Red.ToHex());
            dictionary["Remove_Bordercolor"] = Preferences.Get("Remove_Bordercolor", Color.DarkRed.ToHex());
            dictionary["Remove_Textcolor"] = Preferences.Get("Remove_Textcolor", Color.White.ToHex());

            dictionary["Delete_Backgroundcolor"] = Preferences.Get("Delete_Backgroundcolor", Color.Red.ToHex());
            dictionary["Delete_Bordercolor"] = Preferences.Get("Delete_Bordercolor", Color.DarkRed.ToHex());
            dictionary["Delete_Textcolor"] = Preferences.Get("Delete_Textcolor", Color.White.ToHex());

            dictionary["Order_Backgroundcolor"] = Preferences.Get("Order_Backgroundcolor", Color.Blue.ToHex());
            dictionary["Order_Bordercolor"] = Preferences.Get("Order_Bordercolor", Color.DarkBlue.ToHex());
            dictionary["Order_Textcolor"] = Preferences.Get("Order_Textcolor", Color.White.ToHex());

            dictionary["Saldo_Backgroundcolor"] = Preferences.Get("Saldo_Backgroundcolor", Color.Black.ToHex());
            dictionary["Saldo_Textcolor"] = Preferences.Get("Saldo_Textcolor", Color.White.ToHex());

            dictionary["Flyout_Backgroundcolor"] = Preferences.Get("Flyout_Backgroundcolor", Color.DarkGray.ToHex());
            dictionary["Flyout_Selectcolor"] = Preferences.Get("Flyout_Selectcolor", Color.LightGray.ToHex());
            dictionary["Flyout_Textcolor"] = Preferences.Get("Flyout_Textcolor", Color.Black.ToHex());
            dictionary["Flyout_Iconcolor"] = Preferences.Get("Flyout_Iconcolor", Color.Black.ToHex());
        }

        private Page Choose_Methode()
        {
            master_To_Do_Page = new Master_To_Do_Page();

            Page StartPage = new Start_Page();
            if (Preferences.Get("Start_Option", 0) == 1)
            {
                StartPage = new Master_Financial_Book_Page();
            }
            if (Preferences.Get("Start_Option", 0) == 2)
            {
                StartPage = master_To_Do_Page;
            }
            if (Preferences.Get("Start_Option", 0) == 3)
            {
                StartPage = new Master_Overtime_Page();
            }
            if (Preferences.Get("Start_Option", 0) == 0)
            {
                StartPage = new Start_Page();
            }
            return StartPage;
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
