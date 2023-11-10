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
            dictionary["Aktiv_Schalter_Popup"] = Preferences.Get("Aktiv_Schalter_Popup", Color.ForestGreen.ToHex());
            dictionary["Deaktiv_Schalter_Popup"] = Preferences.Get("Deaktiv_Schalter_Popup", Color.DarkRed.ToHex());


            //Home
            dictionary["Hintergrund_Home"] = Preferences.Get("Hintergrund_Home", Color.DarkSlateGray.ToHex());
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


            //Hinzufügen/Bearbeiten
            dictionary["Hintergrund_Hinzufügen"] = Preferences.Get("Hintergrund_Hinzufügen", Color.DarkSlateGray.ToHex());
            dictionary["Hintergrund_Eingabefeld_Hinzufügen"] = Preferences.Get("Hintergrund_Eingabefeld_Hinzufügen", Color.Orange.ToHex());
            dictionary["Rand_Eingabefeld_Hinzufügen"] = Preferences.Get("Rand_Eingabefeld_Hinzufügen", Color.Coral.ToHex());
            dictionary["Title_Eingabefeld_Hinzufügen"] = Preferences.Get("Title_Eingabefeld_Hinzufügen", Color.White.ToHex());
            dictionary["Text_Eingabefeld_Hinzufügen"] = Preferences.Get("Text_Eingabefeld_Hinzufügen", Color.Black.ToHex());
            dictionary["Platzhater_Eingabefeld_Hinzufügen"] = Preferences.Get("Platzhater_Eingabefeld_Hinzufügen", "#525252");
            dictionary["Aktiv_Schalter_Eingabefeld_Hinzufügen"] = Preferences.Get("Aktiv_Schalter_Eingabefeld_Hinzufügen", Color.ForestGreen.ToHex());
            dictionary["Deaktiv_Schalter_Eingabefeld_Hinzufügen"] = Preferences.Get("Deaktiv_Schalter_Eingabefeld_Hinzufügen", Color.DarkRed.ToHex());
            dictionary["Hintergrund_Wahlfeld_Hinzufügen"] = Preferences.Get("Hintergrund_Wahlfeld_Hinzufügen", Color.Orange.ToHex());
            dictionary["Haubttext_Wahlfeld_Hinzufügen"] = Preferences.Get("Haubttext_Wahlfeld_Hinzufügen", Color.Black.ToHex());
            dictionary["Nebentext_Wahlfeld_Hinzufügen"] = Preferences.Get("Nebentext_Wahlfeld_Hinzufügen", Color.Black.ToHex());
            dictionary["Hintergrund_Option_Wahlfeld_Hinzufügen"] = Preferences.Get("Hintergrund_Option_Wahlfeld_Hinzufügen", Color.SkyBlue.ToHex());
            dictionary["Rand_Option_Wahlfeld_Hinzufügen"] = Preferences.Get("Rand_Option_Wahlfeld_Hinzufügen", Color.Blue.ToHex());
            dictionary["Text_Option_Wahlfeld_Hinzufügen"] = Preferences.Get("Text_Option_Wahlfeld_Hinzufügen", Color.OrangeRed.ToHex());
            dictionary["Hintergrund_Schalter_Hinzufügen"] = Preferences.Get("Hintergrund_Schalter_Hinzufügen", Color.SkyBlue.ToHex());
            dictionary["Rand_Schalter_Hinzufügen"] = Preferences.Get("Rand_Schalter_Hinzufügen", Color.Blue.ToHex());
            dictionary["Text_Schalter_Hinzufügen"] = Preferences.Get("Text_Schalter_Hinzufügen", Color.OrangeRed.ToHex());


            //Bilanz
            dictionary["Hintergrund_Bilanz"] = Preferences.Get("Hintergrund_Bilanz", Color.DarkSlateGray.ToHex());
            dictionary["Hintergrund_Kopfzeile_Bilanz"] = Preferences.Get("Hintergrund_Kopfzeile_Bilanz", Color.DarkGray.ToHex());
            dictionary["Text_Kopfzeile_Bilanz"] = Preferences.Get("Text_Kopfzeile_Bilanz", Color.Black.ToHex());
            dictionary["Hintergrund_Fußzeile_Bilanz"] = Preferences.Get("Hintergrund_Fußzeile_Bilanz", Color.DarkGray.ToHex());
            dictionary["Text_Fußzeile_Bilanz"] = Preferences.Get("Text_Fußzeile_Bilanz", Color.Black.ToHex());
            dictionary["Text_Zusammenfassung_Bilanz"] = Preferences.Get("Text_Zusammenfassung_Bilanz", Color.Black.ToHex());
            dictionary["Text_Zweck_Stack_Bilanz"] = Preferences.Get("Text_Zweck_Stack_Bilanz", Color.White.ToHex());
            dictionary["Text_Anzahl_Stack_Bilanz"] = Preferences.Get("Text_Anzahl_Stack_Bilanz", Color.White.ToHex());

            dictionary["App_Backgroundcolor"] = Preferences.Get("App_Backgroundcolor", "#1d0e21");

            dictionary["Grouping_Textcolor"] = Preferences.Get("Grouping_Textcolor", Color.White.ToHex());

            dictionary["Refresh_Color"] = Preferences.Get("Refresh_Color", Color.Blue.ToHex());

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
