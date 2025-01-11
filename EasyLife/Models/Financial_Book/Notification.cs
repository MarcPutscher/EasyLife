using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Notification : INotifyPropertyChanging
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

        public int auftrags_id;

        public int Auftrags_ID
        {
            get { return auftrags_id; }
            set
            {
                if (Auftrags_ID == value)
                    return;
                auftrags_id = value; OnPropertyChanged(nameof(Auftrags_ID));
            }
        }

        public int notification_id;

        public int Notification_ID
        {
            get { return notification_id; }
            set
            {
                if (Notification_ID == value)
                    return;
                notification_id = value; OnPropertyChanged(nameof(Notification_ID));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));

    }
}
