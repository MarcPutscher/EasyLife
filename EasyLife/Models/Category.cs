using EasyLife.Pages;
using EasyLife.Services;
using MvvmHelpers.Commands;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace EasyLife.Models
{
    public class Category : INotifyPropertyChanged
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

        string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (Title == value)
                    return;
                title = value; OnPropertyChanged(nameof(Title));
            }
        }

        string color;
        public string Color
        {
            get { return color; }
            set
            {
                if (Color == value)
                    return;
                color = value; OnPropertyChanged(nameof(Color));
            }
        }

        int count;
        public int Count
        {
            get { return count; }
            set
            {
                if (Count == value)
                    return;
                count = value; OnPropertyChanged(nameof(Count));
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
                is_select = value; OnPropertyChanged(nameof(Count));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    }
}
