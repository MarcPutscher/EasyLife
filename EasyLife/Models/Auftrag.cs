using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Auftrag : INotifyPropertyChanging
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

        int option = 0;
        public int Option
        {
            get { return option; }
            set
            {
                if (Option == value)
                    return;
                option = value; OnPropertyChanged(nameof(Option));
            }
        }

        string anzahl_an_wiederholungen;
        public string Anzahl_an_Wiederholungen
        {
            get { return anzahl_an_wiederholungen; }
            set
            {
                if (Anzahl_an_Wiederholungen == value)
                    return;
                anzahl_an_wiederholungen = value; OnPropertyChanged(nameof(Anzahl_an_Wiederholungen));
            }
        }

        string art_an_wiederholungen = null;
        public string Art_an_Wiederholungen
        {
            get { return art_an_wiederholungen; }
            set
            {
                if (Art_an_Wiederholungen == value)
                    return;
                art_an_wiederholungen = value; OnPropertyChanged(nameof(Art_an_Wiederholungen));
            }
        }

        string speziell = null;
        public string Speziell
        {
            get { return speziell; }
            set
            {
                if (Speziell == value)
                    return;
                speziell = value; OnPropertyChanged(nameof(Speziell));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));

    }
}
