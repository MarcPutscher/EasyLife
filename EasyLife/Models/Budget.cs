using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Models
{
    public class Budget
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

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new System.ComponentModel.PropertyChangingEventArgs(name));

        string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (Name == value)
                    return;
                name = value; OnPropertyChanged(nameof(Name));
            }
        }

        double goal;
        public double Goal
        {
            get { return goal; }
            set
            {
                if (Goal == value)
                    return;
                goal = value; OnPropertyChanged(nameof(Goal));
            }
        }

        public double Current { get; set; }
    }

    public class Budget_Progessbar
    {
        public Budget Budget { get; set; }

        public Rectangle Rect { get; set; }
    }
}
