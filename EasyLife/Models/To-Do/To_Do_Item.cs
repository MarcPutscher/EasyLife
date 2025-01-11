using EasyLife.Pages;
using EasyLife.Services;
using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;

namespace EasyLife.Models
{
    public class To_Do_Item : INotifyPropertyChanged
    {
        int id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set
            {
                if (Id == value)
                    return;
                id = value; OnPropertyChanged(nameof(Id));
            }
        }

        string to_do;
        public string To_Do
        {
            get { return to_do; }
            set
            {
                if (To_Do == value)
                    return;
                to_do = value; OnPropertyChanged(nameof(To_Do));
            }
        }

        DateTime reminder;
        public DateTime Reminder
        {
            get { return reminder; }
            set
            {
                if (Reminder == value)
                    return;
                reminder = value; OnPropertyChanged(nameof(Reminder));
                if(reminder == new DateTime())
                {
                    Datumanzeige = null;
                    Timeanzeige = null;
                }
                else
                {
                    if(Time_Select == false)
                    {
                        Datumanzeige = reminder.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                        Timeanzeige = null;
                        PseudoDate = reminder.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                    }
                    else
                    {
                        Datumanzeige = reminder.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                        Timeanzeige = reminder.ToString("HH: mm", new CultureInfo("de-DE")) + "Uhr";
                        PseudoDate = reminder.ToString("dddd, d.M.yyyy, HH:mm", new CultureInfo("de-DE")) + "Uhr";
                    }
                }
            }
        }
        DateTime self_destruct_date = new DateTime();
        public DateTime Self_Destruct_Date
        {
            get { return self_destruct_date; }
            set
            {
                if (Self_Destruct_Date == value)
                    return;
                self_destruct_date = value; OnPropertyChanged(nameof(Self_Destruct_Date));
            }
        }

        string datumanzeige = null;
        public string Datumanzeige
        {
            get { return datumanzeige; }
            set
            {
                if (Datumanzeige == value)
                    return;
                datumanzeige = value; OnPropertyChanged(nameof(Datumanzeige));
            }
        }

        string timeanzeige = null;
        public string Timeanzeige
        {
            get { return timeanzeige; }
            set
            {
                if (Timeanzeige == value)
                    return;
                timeanzeige = value; OnPropertyChanged(nameof(Timeanzeige));
            }
        }

        string pseudodate = null;
        public string PseudoDate
        {
            get { return pseudodate; }
            set
            {
                if (PseudoDate == value)
                    return;
                pseudodate = value; OnPropertyChanged(nameof(PseudoDate));
            }
        }
        string viewdate = null;
        public string ViewDate
        {
            get { return viewdate; }
            set
            {
                if (ViewDate == value)
                    return;
                viewdate = value; OnPropertyChanged(nameof(ViewDate));
            }
        }

        bool is_important;
        public bool Is_Important
        {
            get { return is_important; }
            set
            {
                if (Is_Important == value)
                    return;
                is_important = value; OnPropertyChanged(nameof(Is_Important));
            }
        }

        bool is_removed = false;
        public bool Is_Removed
        {
            get { return is_removed; }
            set
            {
                if (Is_Removed == value)
                    return;
                is_removed = value; OnPropertyChanged(nameof(Is_Removed));
                if(is_removed == true)
                {
                    Self_Destruct_Date = DateTime.Today.AddDays(30);
                }
                else
                {
                    Self_Destruct_Date = new DateTime();
                }
            }
        }

        bool is_done = false;
        public bool Is_Done
        {
            get { return is_done; }
            set
            {
                if (Is_Done == value)
                    return;
                is_done = value; OnPropertyChanged(nameof(Is_Done));
                if(is_done == true)
                {
                    Item_BackgroundColor = Color.AliceBlue.ToHex();
                }
                else
                {
                    Item_BackgroundColor = "#e3f2ff";
                }
            }
        }

        bool is_select = false;
        public bool Is_Select
        {
            get { return is_select; }
            set
            {
                if (Is_Select == value)
                    return;
                is_select = value; OnPropertyChanged(nameof(Is_Select));
            }
        }

        bool time_select = false;
        public bool Time_Select
        {
            get { return time_select; }
            set
            {
                if (Time_Select == value)
                    return;
                time_select = value; OnPropertyChanged(nameof(Time_Select));
            }
        }
        string note;
        public string Note
        {
            get { return note; }
            set
            {
                if (Note == value)
                    return;
                note = value; OnPropertyChanged(nameof(Note));
            }
        }

        int categoryid;
        public int CategoryID
        {
            get { return categoryid; }
            set
            {
                if (CategoryID == value)
                    return;
                categoryid = value; OnPropertyChanged(nameof(CategoryID));
            }
        }

        string item_backgroundcolor = "#e3f2ff";
        public string Item_BackgroundColor
        {
            get { return item_backgroundcolor; }
            set
            {
                if (Item_BackgroundColor == value)
                    return;
                item_backgroundcolor = value; OnPropertyChanged(nameof(Color));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
