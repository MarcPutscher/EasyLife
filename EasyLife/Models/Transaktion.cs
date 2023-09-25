using SQLite;
using System;
using System.ComponentModel;
using System.Globalization;

namespace EasyLife.Models
{
    public class Transaktion : INotifyPropertyChanging
    {
        int id;
        [PrimaryKey, AutoIncrement]
        public int Id 
        {
            get { return id; }
            set 
            {
                if(Id == value)
                    return;
                id = value; OnPropertyChanged(nameof(Id)); } 
        }

        string zweck = null;
        public string Zweck 
        { 
            get { return zweck; }
            set 
            {
                if(Zweck == value)
                    return;
                zweck = value; OnPropertyChanged(nameof(Zweck)); } 
        }

        string betrag = null;
        public string Betrag 
        {
            get { return betrag; }
            set 
            {
                if (Betrag == value)
                    return;

                if (double.TryParse(value, out double result) == true)
                {
                    betrag = value.Replace(".", ","); OnPropertyChanged(nameof(Betrag));
                }
                else
                {
                    betrag = ""; OnPropertyChanged(nameof(Betrag));
                }
            } 
        }

        DateTime datum;
        public DateTime Datum 
        {
            get { return datum; }
            set
            {
                if (Datum == value)
                    return;
                datum = value; OnPropertyChanged(nameof(Datum));
                Datumanzeige = datum.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
            }
        }

        string datumanzeige = null;
        public string Datumanzeige
        {
            get { return datumanzeige; }
            set
            {
                if (Datumanzeige == value)
                    return;
                datumanzeige = value; OnPropertyChanged(nameof(Datum));
            }
        }

        string notiz = null;
        public string Notiz 
        {
            get { return notiz; }
            set 
            {
                if (Notiz == value)
                    return;
                notiz = value; OnPropertyChanged(nameof(Notiz)); } 
        }

        string auftrags_id = null;
        public string Auftrags_id
        {
            get { return auftrags_id; }
            set
            {
                if (Auftrags_id == value)
                    return;
                auftrags_id = value; OnPropertyChanged(nameof(Auftrags_id));
            }
        }

        int auftrags_option = 1;
        public int Auftrags_Option
        {
            get { return auftrags_option; }
            set
            {
                if (Auftrags_Option == value)
                    return;
                auftrags_option = value; OnPropertyChanged(nameof(Auftrags_Option));
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

        string speziell;
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

        bool order_visibility;
        public bool Order_Visibility
        {
            get { return order_visibility; }
            set
            {
                if (order_visibility == value)
                    return;
                order_visibility = value; OnPropertyChanged(nameof(Order_Visibility));
            }
        }

        bool content_visibility;
        public bool Content_Visibility
        {
            get { return content_visibility; }
            set
            {
                if (Content_Visibility == value)
                    return;
                content_visibility = value; OnPropertyChanged(nameof(Content_Visibility));
            }
        }

        bool balance_visibility = true;
        public bool Balance_Visibility
        {
            get { return balance_visibility; }
            set
            {
                if (balance_visibility == value)
                    return;
                balance_visibility = value; OnPropertyChanged(nameof(Balance_Visibility));

                if (balance_visibility == true)
                {
                    Balance_Visibility_String = "Ja";
                }
                if (balance_visibility == false)
                {
                    Balance_Visibility_String = "Nein";
                }
            }
        }

        public string balance_visibility_string = "Ja";
        public string Balance_Visibility_String
        {
            get
            {
                return balance_visibility_string;
            }

            set
            {
                if (balance_visibility_string == value)
                    return;

                balance_visibility_string = value; OnPropertyChanged(nameof(Balance_Visibility_String));
            }
        }

        string pseudotext = null;
        public string Pseudotext
        {
            get { return pseudotext; }
            set
            {
                if (Pseudotext == value)
                    return;
                pseudotext = value; OnPropertyChanged(nameof(Pseudotext));
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        void OnPropertyChanged(string name) => PropertyChanging?.Invoke(this,new PropertyChangingEventArgs(name));

        public string Search_Indicator()
        {

            return ""+Zweck+""+Notiz+""+Betrag+""+Id+ ""+Auftrags_id+""+Datumanzeige+""+Anzahl_an_Wiederholungen+""+Art_an_Wiederholungen+""+Speziell+"";
        }
    }
}
