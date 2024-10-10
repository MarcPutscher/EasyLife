using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Overtime_DataItem : INotifyPropertyChanged
    {
        public int position = 0;
        public int Position
        {
            get { return position; }
            set
            {
                if (Position == value)
                    return;
                position = value; OnPropertyChanged(nameof(Position));
            }
        }

        public int data = 0;
        public int Data
        {
            get { return data; }
            set
            {
                if (Data == value)
                    return;
                data = value; OnPropertyChanged(nameof(Data));
            }
        }

        public string data_string;
        public string Data_String
        {
            get { return data_string; }
            set
            {
                if (Data_String == value)
                    return;
                data_string = value; OnPropertyChanged(nameof(Data_String));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
