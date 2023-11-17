using EasyLife.Models;
using EasyLife.Services;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Viewtime_Popup : Popup
    {
        public Viewtime_Popup(Viewtime viewtime, Haushaltsbücher haushaltsbucher)
        {
            Current_Viewtime = viewtime;

            Haushaltsbucher = haushaltsbucher;

            InitializeComponent();

            Create_Picker_Year_Items();
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
                    ViewTime_Popup.Size = new Size(300, 320);
                }
                else
                {
                    ViewTime_Popup.Size = new Size(300, 280);
                }
            }
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
                ViewTime_Popup.Size = new Size(300, 280);
            }
            else
            {
                Month_Control_Visibility = true;
                Switch_IsToggled = false;
                Month = Current_Viewtime.Month;
                ViewTime_Popup.Size = new Size(300, 320);
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

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
    }

    public class Haushaltsbücher
    {
        public readonly List<string> existing_months = new List<string>() { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };

        public Dictionary<string, List<string>> Time = new Dictionary<string, List<string>>();

        public Haushaltsbücher(List<Transaktion> transaktion)
        {
            Create_Time_Dictionary(transaktion);
        }

        public void Create_Time_Dictionary(List<Transaktion> transaktion)
        {
            Get_Years(transaktion);
        }

        public void Get_Years(List<Transaktion> transaktions)
        {
            List<string> years_list = new List<string>();

            foreach (Transaktion tr in transaktions)
            {
                if (years_list.Contains(tr.Datum.Year.ToString()) == false)
                {
                    years_list.Add(tr.Datum.Year.ToString());
                }
            }

            years_list.Sort();

            foreach (string year in years_list)
            {
                Time.Add(year, new List<string>());
            }

            Add_Month(transaktions);
        }

        public void Add_Month(List<Transaktion> transaktions)
        {
            List<string> months = new List<string>();

            foreach (Transaktion transaktion in transaktions)
            {
                if (Time.Keys.ToList().Contains(transaktion.Datum.Year.ToString()) == true)
                {
                    if (Time[transaktion.Datum.Year.ToString()].Contains(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE"))) == false)
                    {
                        Time[transaktion.Datum.Year.ToString()].Add(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE")));
                    }
                }

            }

            Sort_Months();
        }

        public void Sort_Months()
        {
            List<List<string>> months_list = Time.Values.ToList();

            List<string> years = Time.Keys.ToList();

            int h = 0;

            foreach (List<string> months in months_list)
            {
                List<string> sortet_list_of_months = new List<string>();

                sortet_list_of_months.Clear();

                List<int> months_in_int = new List<int>();

                months_in_int.Clear();

                foreach (string month in months)
                {
                    int k = 0;

                    while (month != existing_months[k])
                    {
                        k++;
                    }

                    months_in_int.Add(k);
                }

                months_in_int.Sort();

                foreach (int k in months_in_int)
                {
                    sortet_list_of_months.Add(existing_months[k]);
                }

                Time[years[h]] = sortet_list_of_months;

                h++;
            }
        }
    }
}