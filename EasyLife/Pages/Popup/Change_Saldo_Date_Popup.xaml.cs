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
    public partial class Change_Saldo_Date_Popup : Popup
    {
        public Change_Saldo_Date_Popup(DateTime firstdate, DateTime lastdate, DateTime currentdate)
        {
            InitializeComponent();

            Last_Date = lastdate;

            First_Date = firstdate;

            Current_Date = currentdate;

            My_DatePicker.MinimumDate = First_Date;

            My_DatePicker.MaximumDate = Last_Date;

            My_DatePicker.Date = Current_Date;
        }

        public void Apply(object sender, EventArgs e)
        {
            try
            {
                Dismiss(My_DatePicker.Date);
            }
            catch
            {
                Dismiss(null);
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        DateTime Last_Date { get; set; }

        DateTime First_Date { get; set; }

        DateTime Current_Date { get; set; }

    }
}