using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Zweck : INotifyPropertyChanging
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

        public string zweck;
        public string Benutzerdefinierter_Zweck
        {
            get { return zweck; }
            set
            {
                if (Benutzerdefinierter_Zweck == value)
                    return;
                zweck = value; OnPropertyChanged(nameof(Benutzerdefinierter_Zweck));
            }
        }

        bool reason_visibility;
        public bool Reason_Visibility
        {
            get { return reason_visibility; }
            set
            {
                if (Reason_Visibility == value)
                    return;
                reason_visibility = value; OnPropertyChanged(nameof(reason_visibility));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;
        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }
}
