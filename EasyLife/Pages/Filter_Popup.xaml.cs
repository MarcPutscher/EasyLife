using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Filter_Popup : Popup
    {
        public List<Filter> Filters { get; set; }

        public List<Filter> Origin_Filter { get; set; }
        public Filter_Popup()
        {
            InitializeComponent();

            TransaktionsIDSwitch.IsToggled = Preferences.Get("Search_For_Transaktion_ID", false);

            AuftragsIDSwitch.IsToggled = Preferences.Get("Search_For_Auftrags_ID", false);

            DatumSwitch.IsToggled = Preferences.Get("Search_For_Datum", false);

            ZweckSwitch.IsToggled = Preferences.Get("Search_For_Zweck", false);

            NotizSwitch.IsToggled = Preferences.Get("Search_For_Notiz", false);

            BetragSwitch.IsToggled = Preferences.Get("Search_For_Betrag", false);

            QuersucheSwitch.IsToggled = Preferences.Get("Quersuche", false);
        }
        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Preferences.Set("Search_For_Transaktion_ID" , TransaktionsIDSwitch.IsToggled);
                Preferences.Set("Search_For_Auftrags_ID", AuftragsIDSwitch.IsToggled);
                Preferences.Set("Search_For_Datum", DatumSwitch.IsToggled);
                Preferences.Set("Search_For_Zweck", ZweckSwitch.IsToggled);
                Preferences.Set("Search_For_Notiz", NotizSwitch.IsToggled);
                Preferences.Set("Search_For_Betrag", BetragSwitch.IsToggled);
                Preferences.Set("Quersuche", QuersucheSwitch.IsToggled);


                List<bool> toggled = new List<bool>()
                {
                    TransaktionsIDSwitch.IsToggled,
                    AuftragsIDSwitch.IsToggled,
                    DatumSwitch.IsToggled,
                    ZweckSwitch.IsToggled,
                    NotizSwitch.IsToggled,
                    BetragSwitch.IsToggled,
                    QuersucheSwitch.IsToggled
                };

                if(toggled.Contains(true) == true)
                {
                    Preferences.Set("Filter_Activity", true);
                }
                else
                {
                    Preferences.Set("Filter_Activity", false);
                }

                Dismiss(null);
            }
            catch
            {
                Dismiss(null);
            }
        }
    }
}