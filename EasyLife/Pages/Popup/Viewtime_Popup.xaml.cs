using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Viewtime_Popup : Popup
    {
        public Viewtime_Popup(Viewtime viewtime)
        {
            Current_Viewtime = viewtime;

            InitializeComponent();

            Create_Picker_Year_Items();
        }



        public ObservableRangeCollection<Transaktion> List_of_all_transaktion = new ObservableRangeCollection<Transaktion>();

        public Command Set_Month_Control_Visibility_to_true_Command { get; set; }

        public Command Set_Month_Control_Visibility_to_false_Command { get; set; }

        public AsyncCommand Return_Command { get; }

        public AsyncCommand Year_Changed { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public Command ViewIsDisappearing_Command { get; }

        public Viewtime Current_Viewtime { get; set; }

        List<int> existing_years = new List<int>();

        List<int> years_list = new List<int>();

        int year;
        public int Year
        {
            get { return year; }
            set
            {
                if (year == value)
                    return;
                year = value;
                YearPicker.SelectedItem = year;
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
                MonthPicker.SelectedItem = month;
            }
        }

        List<int> all_years;
        public List<int> All_Years
        {
            get { return all_years; }
            set
            {
                if (all_years == value)
                    return;
                all_years = value;
                YearPicker.ItemsSource = all_years;
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
                MonthPicker.ItemsSource=all_months;
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

        public bool month_control_status;
        public bool Month_Control_Status
        {
            get { return month_control_status; }
            set
            {
                if (month_control_status == value)
                    return;
                month_control_status = value;
                MonthPicker.IsEnabled = month_control_status;
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
                if(Switch_IsToggled == false)
                {
                    ViewTime_Popup.Size = new Size(250, 320);
                }
                else
                {
                    ViewTime_Popup.Size = new Size(250, 280);
                }
            }
        }


        private async void Year_Changed_Methode(object sender, EventArgs e)
        {
            if (Month_Control_Visibility == true)
            {
                Year = (int)YearPicker.SelectedItem;
                Month_Control_Status = false;
                await Show_Months_Methode(Year);
                Month_Control_Status = true;
            }
            else
            {
                Year = (int)YearPicker.SelectedItem;
            }
        }

        private void Month_Changed_Methode(object sender, EventArgs e)
        {
            try
            {
                Month = MonthPicker.SelectedItem.ToString();
            }
            catch 
            {
                Month = null;
            }
        }

        public async void Create_Picker_Year_Items()
        {
            YearPicker.IsEnabled = false;

            MonthPicker.IsEnabled = false;

            if (String.IsNullOrEmpty(Current_Viewtime.Month) == true)
            {
                Month_Control_Visibility = false;
                Month_Control_Status = false;
                Switch_IsToggled = true;
                Month = null;
                ViewTime_Popup.Size = new Size(250, 280);
            }
            else
            {
                Month_Control_Visibility = true;
                Month_Control_Status = true;
                Switch_IsToggled = false;
                Month = Current_Viewtime.Month;
                ViewTime_Popup.Size = new Size(250, 320);
            }

            years_list.Clear();

            List_of_all_transaktion.Clear();

            var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

            List_of_all_transaktion.AddRange(transaktionscontent);

            foreach (Transaktion tr in List_of_all_transaktion)
            {
                if (existing_years.Contains(tr.Datum.Year) == false)
                {
                    years_list.Add(tr.Datum.Year);
                    existing_years.Add(tr.Datum.Year);
                }
            }

            years_list.Sort();

            All_Years = years_list;

            Year = Current_Viewtime.Year;

            if (Month_Control_Visibility == true)
            {
                await Show_Months_Methode(Year);
            }
            else
            {
                Current_Viewtime.Month = null;
            }

            YearPicker.IsEnabled = true;

            MonthPicker.IsEnabled = true;

            Month = Current_Viewtime.Month;
        }

        public async Task<List<Transaktion>> Get_All_Transaktion_of_one_Year(int input)
        {
            List<Transaktion> all_transaktion_of_one_year = new List<Transaktion>();

            all_transaktion_of_one_year.Clear();

            await Get_All_Transaktion();

            foreach (Transaktion tr in List_of_all_transaktion)
            {
                if (tr.Datum.Year == input)
                {
                    all_transaktion_of_one_year.Add(tr);
                }
            }

            return all_transaktion_of_one_year;
        }

        public async Task Get_All_Transaktion()
        {
            List_of_all_transaktion.Clear();

            var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

            List_of_all_transaktion.AddRange(transaktionscontent);
        }

        public async Task Show_Months_Methode(int input)
        {

            Haushaltsbücher haushaltsbuch = new Haushaltsbücher(await Get_All_Transaktion_of_one_Year(input));

            All_Months = haushaltsbuch.Select_Month();
        }

        public void Set_Month_Control_to_true()
        {
            Month_Control_Visibility = true;
        }

        public void Set_Month_Control_to_false()
        {
            Month_Control_Visibility = false;
        }

        public async void Return_Methode(object sender, EventArgs e)
        {
            if (Month_Control_Visibility == true)
            {
                if (Year == 0 || Month == null)
                {
                    await Shell.Current.DisplayAlert("Fehler!", "Es wurde kein Monat ausgewählt.", "OK");
                    return;
                }

                Current_Viewtime.Month = Month;

                Current_Viewtime.Year = Year;

                Dismiss(Current_Viewtime);

                return;
            }
            else
            {
                if (Year == 0)
                {
                    await Shell.Current.DisplayAlert("Fehler!", "Es wurde kein Jahr ausgewählt.", "OK");
                    return;
                }

                Current_Viewtime.Year = Year;

                Current_Viewtime.Month = "";

                Month = Current_Viewtime.Month;

                Dismiss(Current_Viewtime);

                return;
            }
        }

        private async void OptionSwitch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Switch_IsToggled = OptionSwitch.IsToggled;
            if(Switch_IsToggled == false)
            {
                Month_Control_Visibility = true;
                Month_Control_Status = true;
                await Show_Months_Methode(Current_Viewtime.Year);
                Month = null;
            }
            else
            {
                Month_Control_Visibility = false;
                Month_Control_Status = false;
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
        public List<Transaktion> Transaktion_of_one_year = new List<Transaktion>();

        List<string> existing_months = new List<string>() { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };

        public Haushaltsbücher(List<Transaktion> transaktion)
        {
            Transaktion_of_one_year = transaktion;
        }

        public List<string> Select_Month()
        {
            List<string> months = new List<string>();

            foreach (Transaktion transaktion in Transaktion_of_one_year)
            {
                if (!months.Contains(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE"))))
                {
                    months.Add(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE")));
                }
            }

            return Sort_Months(months);
        }

        public List<string> Sort_Months(List<string> list_of_months)
        {
            List<string> sortet_list_of_months = new List<string>();

            sortet_list_of_months.Clear();

            List<int> months_in_int = new List<int>();

            months_in_int.Clear();

            foreach (string month in list_of_months)
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

            return sortet_list_of_months;
        }
    }
}
