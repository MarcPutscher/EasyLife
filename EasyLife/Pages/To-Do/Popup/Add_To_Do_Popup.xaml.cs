using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.To_Do
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Add_To_Do_Popup : Popup
	{
		public Add_To_Do_Popup ()
		{
			InitializeComponent ();
            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 120);
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
            timepicker.Time = time;
        }


        private async void Reminder_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (date == new DateTime())
                { 
                    datepicker.Date = DateTime.Today;
                }
                else
                {
                    datepicker.Date = date;
                }
                datepicker.IsEnabled = true;
                datepicker.MinimumDate = DateTime.Today;
                datepicker.Focus();
                datepicker.DateSelected += DateSelected;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void DateSelected(object sender, EventArgs e)
        {
            try
            {
                timepicker.IsEnabled = true;
                timepicker.Focus();
                timepicker.Unfocused += TimeSelected;    
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void TimeSelected(object sender, EventArgs e)
        {
            try
            {
                date = datepicker.Date + timepicker.Time;
                this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 160);
                Selected_Date.IsVisible = true;
                if (timepicker.Time == new TimeSpan(0, 0, 0, 0, 1))
                {
                    time_selected = false;
                    Select_Date_Label.Text = date.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                }
                else
                {
                    time_selected = true;
                    Select_Date_Label.Text = date.ToString("dddd, d.M.yyyy, HH:mm", new CultureInfo("de-DE")) + "Uhr";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Important_Clicked(object sender, EventArgs e)
        {
            try
            {
                is_important = !is_important;

                if(is_important == true)
                {
                    is_important_button.TextColor = Color.Red;
                }
                else
                {
                    is_important_button.TextColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Delet_Reminder_Clicked(object sender, EventArgs e)
        {
            try
            {
                date = new DateTime();
                timepicker.Time = time;
                time_selected = false;
                this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 120);
                Selected_Date.IsVisible = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(to_do_entry.Text) == false)
                {
                    Category category = await CategoryService.Get_specific_Category("Sonstige");
                    To_Do_Item to_Do_Item;
                    if (date != new DateTime())
                    {
                        to_Do_Item = new To_Do_Item() { To_Do = to_do_entry.Text, CategoryID = category.Id, Is_Important = is_important, Reminder = date};
                    }
                    else
                    {
                        to_Do_Item = new To_Do_Item() { To_Do = to_do_entry.Text, CategoryID = category.Id, Is_Important = is_important };
                    }


                    to_Do_Item.Time_Select = time_selected;


                    int result = await To_DoService.Add_To_Do(to_Do_Item);

                    if((int)result == 2)
                    {
                        return;
                    }

                    Dismiss(result);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void to_do_entry_Changed(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(to_do_entry.Text) == false)
                {
                    save_button.IsEnabled = true;
                }
                else
                {
                    save_button.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }


        public DateTime date = new DateTime();
        public TimeSpan time = new TimeSpan(0,0,0,0,1);
        public bool is_important = false;
        public bool time_selected = false;
    }
}