using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Timespane_Popup : Popup
    {
        public Timespane_Popup(Transaktion input , List<Transaktion> input2)
        {
            Current_Transaktion = input;

            Transaktion_list = input2;

            InitializeComponent();

            Transaktion_list = Transaktion_list.OrderBy(d => d.Datum).ToList();

            My_DatePicker.Date = Transaktion_list[0].Datum.Date;

            Current_Date = Transaktion_list[0].Datum.Date;
        }

        public async void Apply(object sender, EventArgs e)
        {
            try
            {
                int timespan = (My_DatePicker.Date - Current_Date.Date).Days;

                List<Transaktion> transaktion_list = new List<Transaktion>(Transaktion_list);

                int follower = 0;

                foreach (Transaktion trans in Transaktion_list)
                {
                    if (trans.Auftrags_Option == 3)
                    {
                        transaktion_list[follower].Datum = transaktion_list[follower].Datum.Date.AddDays(timespan);

                        transaktion_list[follower].Anzahl_an_Wiederholungen = transaktion_list[follower].Anzahl_an_Wiederholungen = DateTime.ParseExact(trans.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddDays(timespan).ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                    }
                    else
                    {
                        transaktion_list[follower].Datum = transaktion_list[follower].Datum.Date.AddDays(timespan);
                    }

                    await ContentService.Edit_Transaktion(transaktion_list[follower]);

                    follower++;
                }

                transaktion_list = transaktion_list.OrderBy(d => d.Datum).ToList();

                await NotificationHelper.ModifyNotification(transaktion_list.Last(), 0);

                Dismiss(true);
            }
            catch 
            {
                Dismiss(false);
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        DateTime Current_Date { get; set; }

        Transaktion Current_Transaktion { get; set;}

        List<Transaktion> Transaktion_list { get; set; }
    }
}