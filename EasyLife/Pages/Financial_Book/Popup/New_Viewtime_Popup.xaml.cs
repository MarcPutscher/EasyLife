using EasyLife.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.VisualStateManager;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class New_Viewtime_Popup : Popup
    {
        public New_Viewtime_Popup(Viewtime viewtime, Haushaltsbücher haushaltsbucher)
        {
            Current_Viewtime = new Viewtime() { Year = viewtime.Year, Month = viewtime.Month };
            Select_Viewtime = new Viewtime() { Year = viewtime.Year, Month = viewtime.Month };
            InitializeComponent();

            MonthCollectionView.IsVisible = false;

            foreach(var item in haushaltsbucher.Time)
            {
                foreach (Months months in item.Value)
                {
                    months.new_viewtime_popup = this;
                }
                calendarItems.Add(new CalendarItem() { Year = item.Key, Months = item.Value , new_viewtime_popup = this});
            }

            YearCollectionView.ItemsSource = calendarItems;

            if(calendarItems.SingleOrDefault<CalendarItem>(x=>x.Year ==Select_Viewtime.Year.ToString()) != null)
            {
                calendarItems.SingleOrDefault<CalendarItem>(x => x.Year == Select_Viewtime.Year.ToString()).PerformYearButton_Command();
                Select_Viewtime.Month = Current_Viewtime.Month;

                calendarItems.SingleOrDefault<CalendarItem>(x => x.Year == Select_Viewtime.Year.ToString())?.Months.SingleOrDefault(y => y.Month == Select_Viewtime.Month)?.PerformMonthButton_Command();
            }

            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width/DeviceDisplay.MainDisplayInfo.Density, 600);
        }

        public async void Return_Methode(object sender, EventArgs e)
        {
            if (Select_Viewtime.Year == 0)
            {
                await Shell.Current.DisplayAlert("Fehler!", "Es wurde kein Jahr ausgewählt.", "OK");
                return;
            }

            if (Select_Viewtime.Month == null)
            {
                Select_Viewtime.Month = "";
            }

            Current_Viewtime.Month = Select_Viewtime.Month;
            Current_Viewtime.Year = Select_Viewtime.Year;

            Dismiss(Current_Viewtime);
        }

        public void YearCollection_Methode (CalendarItem item)
        {
            MonthCollectionView.IsVisible = !item.Months_Collection_is_Visibile;

            if (item.Months_Collection_is_Visibile == true)
            {
                Select_Viewtime.Year = 0;
            }
            else
            {
                Select_Viewtime.Year = int.Parse(item.Year);
            }

            Select_Viewtime.Month = null;

            MonthCollectionView.ItemsSource = item.Months;

            YearCollectionView.ItemsSource = null;

            foreach (CalendarItem ye in calendarItems.Where(x => x.Months_Collection_is_Visibile == true).ToList())
            {
                if (ye != item)
                {
                    ye.Months_Collection_is_Visibile = false;
                    ye.Backgroundcolor = ye.ChangeBrightness(ye.Backgroundcolor, -0.2);
                }
            }

            foreach (var item1 in calendarItems)
            {
                foreach (Months mo in item1.Months.Where(x => x.Months_Selected == true).ToList())
                {
                    mo.Months_Selected = false;
                    mo.Backgroundcolor = mo.ChangeBrightness(mo.Backgroundcolor, -0.2);
                }
            }

            YearCollectionView.ItemsSource = calendarItems;
            MonthCollectionView.ItemsSource = null;
            MonthCollectionView.ItemsSource = item.Months;
        }

        public void MonthCollection_Methode(Months item)
        {
            if(item.Months_Selected == false)
            {
                Select_Viewtime.Month = null;
            }
            else
            {
                Select_Viewtime.Month = item.Month;
            }

            MonthCollectionView.ItemsSource = null;

            foreach(Months mo in calendarItems.SingleOrDefault(x => x.Year == Select_Viewtime.Year.ToString()).Months.Where(x=>x.Months_Selected == true).ToList())
            {
                if(mo != item)
                {
                    mo.Months_Selected = false;
                    mo.Backgroundcolor = mo.ChangeBrightness(mo.Backgroundcolor, -0.2);
                }
            }

            MonthCollectionView.ItemsSource = calendarItems.SingleOrDefault(x => x.Year == Select_Viewtime.Year.ToString()).Months;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        public ObservableRangeCollection<CalendarItem> calendarItems = new ObservableRangeCollection<CalendarItem>();

        private Viewtime Current_Viewtime = new Viewtime();

        public Viewtime Select_Viewtime = new Viewtime();

    }
}