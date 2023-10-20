using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyLife.Models
{
    public class Balanceprofile : INotifyPropertyChanging
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

        List<string> outcome_account = new List<string>();
        public List<string> Outcome_Account
        {
            get { return outcome_account; }
            set
            {
                if (Outcome_Account == value)
                    return;
                outcome_account = value; OnPropertyChanged(nameof(Outcome_Account));
            }
        }

        List<string> income_account = new List<string>();
        public List<string> Income_Account
        {
            get { return income_account; }
            set
            {
                if (Income_Account == value)
                    return;
                income_account = value; OnPropertyChanged(nameof(Income_Account));
            }
        }

        List<string> outcome_cash = new List<string>();
        public List<string> Outcome_Cash
        {
            get { return outcome_cash; }
            set
            {
                if (Outcome_Cash == value)
                    return;
                outcome_cash = value; OnPropertyChanged(nameof(Outcome_Cash));
            }
        }

        List<string> income_cash = new List<string>();
        public List<string> Income_Cash
        {
            get { return income_cash; }
            set
            {
                if (Income_Cash == value)
                    return;
                income_cash = value; OnPropertyChanged(nameof(Income_Cash));
            }
        }

        List<string> letter_income = new List<string>();
        public List<string> Letter_Income
        {
            get { return letter_income; }
            set
            {
                if (Letter_Income == value)
                    return;
                letter_income = value; OnPropertyChanged(nameof(Letter_Income));
            }
        }

        List<string> letter_outcome = new List<string>();
        public List<string> Letter_Outcome
        {
            get { return letter_outcome; }
            set
            {
                if (Letter_Outcome == value)
                    return;
                letter_outcome = value; OnPropertyChanged(nameof(Letter_Outcome));
            }
        }

        List<string> ignore = new List<string>();
        public List<string> Ignore
        {
            get { return ignore; }
            set
            {
                if (Ignore == value)
                    return;
                ignore = value; OnPropertyChanged(nameof(Ignore));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
    }
}
