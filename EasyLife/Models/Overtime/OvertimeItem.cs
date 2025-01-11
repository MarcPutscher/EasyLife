using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using static Xamarin.Forms.Internals.Profile;

namespace EasyLife.Models
{
    public class OvertimeItem : INotifyPropertyChanged
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

        int time;
        public int Time
        {
            get { return time; }
            set
            {
                if (Time == value)
                    return;
                time = value; OnPropertyChanged(nameof(Time));
            }
        }

        public bool is_select = false;
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

        public bool is_removed = false;
        public bool Is_Removed
        {
            get { return is_removed; }
            set
            {
                if (Is_Removed == value)
                    return;
                is_removed = value; OnPropertyChanged(nameof(Is_Removed));
            }
        }

        public DateTime date = new DateTime();
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (Date == value)
                    return;
                date = value; OnPropertyChanged(nameof(Date));
                Datumanzeige = date.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
            }
        }

        public string datumanzeige = null;
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

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
