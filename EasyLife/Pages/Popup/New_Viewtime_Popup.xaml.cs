using EasyLife.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class New_Viewtime_Popup : Popup
    {
        public New_Viewtime_Popup(Viewtime viewtime, Haushaltsbücher haushaltsbucher)
        {
            Current_Viewtime = viewtime;

            Haushaltsbucher = haushaltsbucher;

            InitializeComponent();

            Create_Picker_Year_Items();
        }

        private void Year_Changed_Methode(object sender, CustomeEventArgs.ItemChangedEventArgs e)
        {
            if (Month_Control_Visibility == true)
            {
                if (String.IsNullOrWhiteSpace(Year) == false)
                {
                    Year = YearPicker.ItemText;

                    All_Months = Haushaltsbucher.Time[Year];

                    Month = null;
                }
                else
                {
                    Year = Current_Viewtime.Year.ToString();

                    Month = null;
                }
            }
            else
            {
                Year = YearPicker.ItemText;
            }
        }

        private void Month_Changed_Methode(object sender, CustomeEventArgs.ItemChangedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(MonthPicker.ItemText) == false)
                {
                    Month = MonthPicker.ItemText;
                }
                else
                {
                    if (Year == Current_Viewtime.Year.ToString())
                    {
                        Month = Current_Viewtime.Month;
                    }
                    else
                    {
                        Month = null;
                    }
                }
            }
            catch
            {
                Month = Current_Viewtime.Month;
            }
        }

        public void Create_Picker_Year_Items()
        {
            if (String.IsNullOrEmpty(Current_Viewtime.Month) == true)
            {
                Month_Control_Visibility = false;
                Switch_IsToggled = true;
                Month = null;
                New_ViewTime_Popup.Size = new Size(300, 280);
            }
            else
            {
                Month_Control_Visibility = true;
                Switch_IsToggled = false;
                Month = Current_Viewtime.Month;
                New_ViewTime_Popup.Size = new Size(300, 320);
            }

            All_Years = Haushaltsbucher.Time.Keys.ToList();

            Year = Current_Viewtime.Year.ToString();

            if (Month_Control_Visibility == true)
            {
                if (String.IsNullOrWhiteSpace(Year) == false)
                {
                    All_Months = Haushaltsbucher.Time[Year];
                }
            }
            else
            {
                Current_Viewtime.Month = null;
            }

            Month = Current_Viewtime.Month;
        }

        public async void Return_Methode(object sender, EventArgs e)
        {
            if (Month_Control_Visibility == true)
            {
                if (String.IsNullOrWhiteSpace(Year) == true || Month == null)
                {
                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler!", 300, 300, null, null, "Es wurde kein Monat ausgewählt."));

                    return;
                }

                Current_Viewtime.Month = Month;

                Current_Viewtime.Year = int.Parse(Year);

                Dismiss(Current_Viewtime);

                return;
            }
            else
            {
                if (String.IsNullOrWhiteSpace(Year) == true)
                {
                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler!", 300, 300, null, null, "Es wurde kein Jahr ausgewählt."));

                    return;
                }

                Current_Viewtime.Year = int.Parse(Year);

                Current_Viewtime.Month = "";

                Month = Current_Viewtime.Month;

                Dismiss(Current_Viewtime);

                return;
            }
        }

        private void OptionSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Switch_IsToggled = OptionSwitch.IsToggled;

            if (Switch_IsToggled == false)
            {
                Month_Control_Visibility = true;

                if (String.IsNullOrWhiteSpace(Year) == false)
                {
                    All_Months = Haushaltsbucher.Time[Year];
                }

                Month = null;
            }
            else
            {
                Month_Control_Visibility = false;

                Month = null;
            }
        }

        public void MonthCollection_Methode (List<string> months, bool visibility)
        {

        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        public ObservableRangeCollection<Transaktion> List_of_all_transaktion = new ObservableRangeCollection<Transaktion>();

        public Viewtime Current_Viewtime { get; set; }

        public Haushaltsbücher Haushaltsbucher { get; set; }

        string year;
        public string Year
        {
            get { return year; }
            set
            {
                if (year == value)
                    return;
                year = value;

                if (year == "0")
                {
                    YearPicker.ItemText = null;
                }
                else
                {
                    YearPicker.ItemText = year;

                }
            }
        }

        string month;
        public string Month
        {
            get { return month; }
            set
            {
                if (month == value)
                    return;
                month = value;
                MonthPicker.ItemText = month;
            }
        }

        List<string> all_years;
        public List<string> All_Years
        {
            get { return all_years; }
            set
            {
                if (all_years == value)
                    return;
                all_years = value;
                YearPicker.ItemSource = all_years;
            }
        }

        List<string> all_months;
        public List<string> All_Months
        {
            get { return all_months; }
            set
            {
                if (all_months == value)
                    return;
                all_months = value;
                MonthPicker.ItemSource = all_months;
            }
        }

        public bool month_control_visibility;
        public bool Month_Control_Visibility
        {
            get { return month_control_visibility; }
            set
            {
                if (month_control_visibility == value)
                    return;
                month_control_visibility = value;
                MonthLabel.IsVisible = month_control_visibility;
                MonthPicker.IsVisible = month_control_visibility;
            }
        }

        public bool switch_istoggled;
        public bool Switch_IsToggled
        {
            get { return switch_istoggled; }
            set
            {
                if (Switch_IsToggled == value)
                    return;
                switch_istoggled = value;
                OptionSwitch.IsToggled = switch_istoggled;
                if (Switch_IsToggled == false)
                {
                    New_ViewTime_Popup.Size = new Size(300, 320);
                }
                else
                {
                    New_ViewTime_Popup.Size = new Size(300, 280);
                }
            }
        }
    }
}