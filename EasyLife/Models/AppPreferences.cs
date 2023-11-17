using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace EasyLife.Models
{
    public class AppPreferences
    {
        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));


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


        string preferences;
        public string Preferences
        {
            get { return preferences; }
            set
            {
                if (Preferences == value)
                    return;
                preferences = value; OnPropertyChanged(nameof(Preferences));
            }
        }
    }
}