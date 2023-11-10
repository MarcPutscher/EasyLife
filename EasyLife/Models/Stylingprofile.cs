using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Stylingprofile
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

        string colors;
        public string Colors
        {
            get { return colors; }
            set
            {
                if (Colors == value)
                    return;
                colors = value; OnPropertyChanged(nameof(Colors));
            }
        }
    }
}