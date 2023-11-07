 using EasyLife.PageModels;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Master_Page();

            //Definiert die Farben in der App.

            //Popup
            dictionary["Vordergrund_Cancel_Popup"] = Preferences.Get("Vordergrund_Cancel_Popup", "#710117");
            dictionary["Hintergrund_Cancel_Popup"] = Preferences.Get("Hintergrund_Cancel_Popup", Color.Black.ToHex());
            dictionary["Text_Titel_Popup"] = Preferences.Get("Text_Titel_Popup", "#cbcdcb");
            dictionary["Hintergrund_Content_Popup"] = Preferences.Get("Hintergrund_Content_Popup", "#0b1c48");
            dictionary["Rand_Content_Popup"] = Preferences.Get("Rand_Content_Popup", "#2b6ad0");
            dictionary["Text_Content_Popup"] = Preferences.Get("Text_Content_Popup", "#ecd5bb");
            dictionary["Subtext_Content_Popup"] = Preferences.Get("Subtext_Content_Popup", "#ecd5bb");
            dictionary["Hintertgrund_Button_Popup"] = Preferences.Get("Hintertgrund_Button_Popup", "#0b1c48");
            dictionary["Rand_Button_Popup"] = Preferences.Get("Rand_Button_Popup", "#2b6ad0");
            dictionary["Text_Button_Popup"] = Preferences.Get("Text_Button_Popup", "#138b83");

            //Home
            dictionary["Hintergrund_Seite_Home"] = Preferences.Get("Hintergrund_Seite_Home", Color.DarkSlateGray.ToHex());
            dictionary["Hintergrund_Bearbeiten_Home"] = Preferences.Get("Hintergrund_Bearbeiten_Home", Color.Green.ToHex());
            dictionary["Rand_Bearbeiten_Home"] = Preferences.Get("Rand_Bearbeiten_Home", Color.DarkGreen.ToHex());
            dictionary["Text_Bearbeiten_Home"] = Preferences.Get("Text_Bearbeiten_Home", Color.White.ToHex());
            dictionary["Hintergrund_Löschen_Home"] = Preferences.Get("Hintergrund_Löschen_Home", Color.Red.ToHex());
            dictionary["Rand_Löschen_Home"] = Preferences.Get("Rand_Löschen_Home", Color.DarkRed.ToHex());
            dictionary["Text_Löschen_Home"] = Preferences.Get("Text_Löschen_Home", Color.White.ToHex());
            dictionary["Hintergrund_Transaktion_Home"] = Preferences.Get("Hintergrund_Transaktion_Home", Color.Gray.ToHex());
            dictionary["Rand_Transaktion_Home"] = Preferences.Get("Rand_Transaktion_Home", Color.DarkGray.ToHex());
            dictionary["Text_Transaktion_Home"] = Preferences.Get("Text_Transaktion_Home", Color.Black.ToHex());
            dictionary["Hintergrund_Detail_Transaktion_Home"] = Preferences.Get("Hintergrund_Detail_Transaktion_Home", Color.LightGray.ToHex());
            dictionary["Text_Detail_Transaktion_Home"] = Preferences.Get("Text_Detail_Transaktion_Home", Color.Black.ToHex());
            dictionary["Hintergrund_Positiver_Betrag_Transaktion_Home"] = Preferences.Get("Hintergrund_Positiver_Betrag_Transaktion_Home", Color.MediumSeaGreen.ToHex());
            dictionary["Rand_Positiver_Betrag_Transaktion_Home"] = Preferences.Get("Rand_Positiver_Betrag_Transaktion_Home", Color.ForestGreen.ToHex());
            dictionary["Text_Positiver_Betrag_Transaktion_Home"] = Preferences.Get("Text_Positiver_Betrag_Transaktion_Home", Color.Black.ToHex());
            dictionary["Hintergrund_Negativer_Betrag_Transaktion_Home"] = Preferences.Get("Hintergrund_Negativer_Betrag_Transaktion_Home", Color.Salmon.ToHex());
            dictionary["Rand_Negativer_Betrag_Transaktion_Home"] = Preferences.Get("Rand_Negativer_Betrag_Transaktion_Home", Color.Red.ToHex());
            dictionary["Text_Negativer_Betrag_Transaktion_Home"] = Preferences.Get("Text_Negativer_Betrag_Transaktion_Home", Color.Black.ToHex());
            dictionary["Text_MehrLaden_Home"] = Preferences.Get("Text_MehrLaden_Home", Color.Black.ToHex());
            dictionary["Vordergrund_Budget_Home"] = Preferences.Get("Vordergrund_Budget_Home", Color.SaddleBrown.ToHex());
            dictionary["Hintergrund_Saldo_Home"] = Preferences.Get("Hintergrund_Saldo_Home", Color.Black.ToHex());
            dictionary["Text_Saldo_Home"] = Preferences.Get("Text_Saldo_Home", Color.White.ToHex());
            dictionary["Vordergrund_Hinzufügen_Home"] = Preferences.Get("Vordergrund_Hinzufügen_Home", Color.Orange.ToHex());









































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
