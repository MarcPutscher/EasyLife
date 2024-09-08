using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }


        private void Reminder_Clicked(object sender, EventArgs e)
        {
            
            if(date == new DateTime())
            { 
                datepicker.Date = DateTime.Today;
            }
            else
            {
                datepicker.Date = date;
            }
            datepicker.IsEnabled = true;
            datepicker.MinimumDate = DateTime.Now;
            datepicker.Focus();
            datepicker.DateSelected += DateSelected;
        }
        private void DateSelected(object sender, EventArgs e)
        {

            timepicker.IsEnabled = true;
            timepicker.Focus();
            timepicker.Unfocused += TimeSelected;        
        }
        private void TimeSelected(object sender, EventArgs e)
        {
            date = datepicker.Date + timepicker.Time;
            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 160);
            Selected_Date.IsVisible = true;
            Select_Date_Label.Text = date.ToString("dddd, d.M.yyyy, HH:mm", new CultureInfo("de-DE")) + "Uhr";
        }
        private void Important_Clicked(object sender, EventArgs e)
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
        private void Delet_Reminder_Clicked(object sender, EventArgs e)
        {
            date = new DateTime();
            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 120);
            Selected_Date.IsVisible = false;
        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(to_do_entry.Text) == false)
            {
                Category category = await CategoryService.Get_specific_Category("Sonstige");
                To_Do_Item to_Do_Item;
                if (date != new DateTime())
                {
                    to_Do_Item = new To_Do_Item() { To_Do = to_do_entry.Text, CategoryID = category.Id, Is_Important = is_important, Reminder = date.Date};
                }
                else
                {
                    to_Do_Item = new To_Do_Item() { To_Do = to_do_entry.Text, CategoryID = category.Id, Is_Important = is_important };
                }

                int result = await To_DoService.Add_To_Do(to_Do_Item);

                if((int)result == 2)
                {
                    return;
                }

                Dismiss(result);
            }
        }
        private void to_do_entry_Changed(object sender, EventArgs e)
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


        public DateTime date = new DateTime();
        public bool is_important = false;
    }
}