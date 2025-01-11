using EasyLife.Services;
using MvvmHelpers.Commands;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyLife.Models
{
    public class Suggestion : INotifyPropertyChanging
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

        string suggestion_value;
        public string Suggestion_value
        {
            get { return suggestion_value; }
            set
            {
                if (Suggestion_value == value)
                    return;
                suggestion_value = value; OnPropertyChanged(nameof(Suggestion_value));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this,new PropertyChangingEventArgs(name));
    }
}
