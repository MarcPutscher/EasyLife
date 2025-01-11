using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static Xamarin.Forms.Internals.Profile;

namespace EasyLife.Models
{
    class StackholderBundle : INotifyPropertyChanging
    {
        public ObservableRangeCollection<Stackholder> stackholdersource = new ObservableRangeCollection<Stackholder>();
        public ObservableRangeCollection<Stackholder> StackholderSource
        {
            get { return stackholdersource; }
            set
            {
                if (StackholderSource == value)
                    return;
                stackholdersource = value; OnPropertyChanged(nameof(StackholderSource));
            }
        }

        public bool visibility = false;
        public bool Visibility
        {
            get { return visibility; }
            set
            {
                if (Visibility == value)
                    return;
                visibility = value; OnPropertyChanged(nameof(Visibility));
            }
        }

        public Xamarin.Forms.Color evaluation = Xamarin.Forms.Color.White;
        public Xamarin.Forms.Color Evaluation
        {
            get { return evaluation; }
            set
            {
                if (Evaluation == value)
                    return;
                evaluation = value; OnPropertyChanged(nameof(Evaluation));
            }
        }

        public string sum;
        public string Sum
        {
            get { return sum; }
            set
            {
                if (Sum == value)
                    return;
                sum = value; OnPropertyChanged(nameof(Sum));
            }
        }

        public string total_sum;
        public string Total_Sum
        {
            get { return total_sum; }
            set
            {
                if (Total_Sum == value)
                    return;
                total_sum = value; OnPropertyChanged(nameof(Total_Sum));
            }
        }

        public string total_text;
        public string Total_Text
        {
            get { return total_text; }
            set
            {
                if (Total_Text == value)
                    return;
                total_text = value; OnPropertyChanged(nameof(Total_Text));
            }
        }

        public int definition;
        public int Definition
        {
            get { return definition; }
            set
            {
                if (Definition == value)
                    return;
                definition = value; OnPropertyChanged(nameof(Definition));
            }
        }

        public int height;
        public int Height
        {
            get { return height; }
            set
            {
                if (Height == value)
                    return;
                height = value; OnPropertyChanged(nameof(Height));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));

    }
}
