using EasyLife.Models;
using EasyLife.PageModels.To_Do;
using EasyLife.Pages;
using EasyLife.Pages.To_Do;
using EasyLife.Services;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.BehaviorsPack;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Command = MvvmHelpers.Commands.Command;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(To_Do_ID), nameof(To_Do_ID))]
    public class To_Do_Detail_PageModel : FreshBasePageModel
    {
        public To_Do_Detail_PageModel()
        {
            ViewAppearing_Command = new AsyncCommand(ViewAppearing_Methode);
            Save_Command = new AsyncCommand(Save_Methode);
            Picker_Command = new AsyncCommand(Picker_Methode);


            Reminder_Command = new Command(Reminder_Methode);
            Remove_Command = new Command(Remove_Methode);
        }

        public async Task ViewAppearing_Methode()
        {
            try
            {
                To_do_Item = await To_DoService.Get_specific_To_Do_from_ID(To_Do_ID);

                if (To_do_Item == null)
                {
                    await Shell.Current.GoToAsync("..");
                }

                date = To_do_Item.Reminder;

                if(To_do_Item.PseudoDate == null)
                {
                    To_do_Item.PseudoDate = "Erinnerung hinzufügen";
                }

                categories.Clear();

                categories.AddRange(await CategoryService.Get_all_Categorys());

                if(Selectet_Category == null)
                {
                    Selectet_Category = await CategoryService.Get_specific_Category_from_ID(to_Do_Item.CategoryID);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        public async Task Save_Methode()
        {
            try
            {
                if (To_do_Item != null)
                {
                    if(To_do_Item.PseudoDate == "Erinnerung hinzufügen")
                    {
                        To_do_Item.Reminder = new DateTime();
                    }
                    if (To_do_Item.PseudoDate != "Erinnerung hinzufügen" && date != new DateTime())
                    {
                        To_do_Item.Reminder = date;
                    }
                    if (To_do_Item.CategoryID != Selectet_Category.Id && Selectet_Category != null)
                    {
                        To_do_Item.CategoryID = Selectet_Category.Id;
                    }

                    await To_DoService.Edit_To_Do(To_do_Item);
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
                await Shell.Current.GoToAsync("..");
            }
        }
        public async Task Picker_Methode()
        {
            try
            {
                    var result = await Shell.Current.ShowPopupAsync(new Category_Select_Popup(categories,Selectet_Category.Id));

                    if ((Category)result != null)
                    {
                        Selectet_Category = (Category)result;
                    }
                    else
                    {
                        return;
                    }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
                await Shell.Current.GoToAsync("..");
            }
        }


        public async void Reminder_Methode()
        {
            try
            {
                DatePicker_IsEnabled = true;
                DatePicker_Minimumdate = DateTime.Today;
                Xamarin.Forms.DatePicker datePicker = Shell.Current.CurrentPage.FindByName<Xamarin.Forms.DatePicker>("datepicker");
                if (datePicker != null)
                {
                    datePicker.Focus();
                    datePicker.Unfocused += DateSelected;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Remove_Methode()
        {
            try
            {
                date = new DateTime();
                DatePicker_Date = DateTime.Today;
                TimePicker_Time = new TimeSpan(0, 0, 0, 0, 1);
                Reminder_Deletet_IsVisible= false;
                To_do_Item.PseudoDate = "Erinnerung hinzufügen";
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
                timepicker_isenabled = true;
                Xamarin.Forms.TimePicker timePicker = Shell.Current.CurrentPage.FindByName<Xamarin.Forms.TimePicker>("timepicker");
                if (timePicker != null)
                {
                    timePicker.Focus();
                    timePicker.Unfocused += TimeSelected;
                }
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
                if (TimePicker_Time == new TimeSpan(0,0,0,0,1))
                {
                    To_do_Item.Time_Select = false;
                }
                else
                {
                    To_do_Item.Time_Select= true;
                }
                date = datepicker_date + timepicker_time;
                To_do_Item.Reminder = date;
                Reminder_Deletet_IsVisible = true;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }


        public Category selectet_category = null;
        public Category Selectet_Category
        {
            get { return selectet_category; }
            set
            {
                if (selectet_category == value)
                {
                    return;
                }

                selectet_category = value; RaisePropertyChanged();
            }
        }



        public AsyncCommand ViewAppearing_Command { get; }
        public AsyncCommand Save_Command { get; }
        public AsyncCommand Picker_Command { get; }


        public Command Reminder_Command { get; }
        public Command Remove_Command { get; }
        public MvvmHelpers.Commands.Command<Object> TimePicker_Time_Changed_Command {  get; }


        public int To_Do_ID { get; set; }


        public bool timepicker_timeselect = false;
        public bool timepicker_isenabled = false;
        public bool TimePicker_IsEnabled
        {
            get { return timepicker_isenabled; }
            set
            {
                if (timepicker_isenabled == value)
                {
                    return;
                }

                timepicker_isenabled = value; RaisePropertyChanged();
            }
        }
        public bool datepicker_isenabled = false;
        public bool DatePicker_IsEnabled
        {
            get { return datepicker_isenabled; }
            set
            {
                if (datepicker_isenabled == value)
                {
                    return;
                }

                datepicker_isenabled = value; RaisePropertyChanged();
            }
        }
        public bool reminder_deletet_isvisible = false;
        public bool Reminder_Deletet_IsVisible
        {
            get { return reminder_deletet_isvisible; }
            set
            {
                if (reminder_deletet_isvisible == value)
                {
                    return;
                }

                reminder_deletet_isvisible = value; RaisePropertyChanged();
            }
        }


        DateTime date = new DateTime();

        public DateTime datepicker_date = DateTime.Today;
        public DateTime DatePicker_Date
        {
            get { return datepicker_date; }
            set
            {
                if (datepicker_date == value)
                {
                    return;
                }

                datepicker_date = value; RaisePropertyChanged();
            }
        }

        public DateTime datepicker_minimumdate = DateTime.Today;
        public DateTime DatePicker_Minimumdate
        {
            get { return datepicker_minimumdate; }
            set
            {
                if (datepicker_minimumdate == value)
                {
                    return;
                }

                datepicker_minimumdate = value; RaisePropertyChanged();
            }
        }


        public TimeSpan timepicker_time = new TimeSpan(0,0,0,0,1);
        public TimeSpan TimePicker_Time
        {
            get { return timepicker_time; }
            set
            {
                if (timepicker_time == value)
                {
                    return;
                }

                timepicker_time = value; RaisePropertyChanged();
            }
        }


        public To_Do_Item to_Do_Item;
        public To_Do_Item To_do_Item
        {
            get { return to_Do_Item; }
            set
            {
                if (to_Do_Item == value)
                {
                    return;
                }

                to_Do_Item = value; RaisePropertyChanged();
            }
        }



        public List<Category> categories = new List<Category>();
    }
}
