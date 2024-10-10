using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Essentials;

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

                if (double.TryParse(value, NumberStyles.Any, new CultureInfo("de-DE"), out double result) == true)
                {
                    value = result.ToString("F2");

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
                datumanzeige = value; OnPropertyChanged(nameof(Datumanzeige));
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

        /// <summary>
        /// Option 1 = Einmaliger Auftrag |
        /// Option 2 = Auftrag nach einer Anzahl |
        /// Option 3 = Auftrag bis zu einem Datum
        /// </summary>
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

        bool saldo_visibility = true;
        public bool Saldo_Visibility
        {
            get { return saldo_visibility; }
            set
            {
                if (Saldo_Visibility == value)
                    return;
                saldo_visibility = value; OnPropertyChanged(nameof(Saldo_Visibility));

                if (saldo_visibility == true)
                {
                    Saldo_Visibility_String = "Ja";
                }
                if (saldo_visibility == false)
                {
                    Saldo_Visibility_String = "Nein";
                }
            }
        }

        public string saldo_visibility_string = "Ja";
        public string Saldo_Visibility_String
        {
            get
            {
                return saldo_visibility_string;
            }

            set
            {
                if (saldo_visibility_string == value)
                    return;

                saldo_visibility_string = value; OnPropertyChanged(nameof(Saldo_Visibility_String));
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

        public Dictionary<string, string> Dicfilter = new Dictionary<string, string>();

        public string CrossSearch_Indicator( List<Filter> filters)
        {
            if (filters.Where(fil => fil.Name == "Quersuche").Count() != 0)
            {
                filters.RemoveAt(6);
            }

            Dicfilter.Clear();

            Dicfilter.Add("Betrag", betrag + " €");

            Dicfilter.Add("Datum", datum.ToString("dddd, d.MMMM.yyyy", new CultureInfo("de-DE")));

            Dicfilter.Add("Zweck", zweck);

            Dicfilter.Add("Notiz", notiz);

            Dicfilter.Add("Transaktions_ID", id.ToString());

            Dicfilter.Add("Auftrags_ID", auftrags_id);

            List<bool> nofilter = new List<bool>();

            string search_indicator = "";

            foreach (Filter filter in filters)
            {
                nofilter.Add(filter.State);

                if (filter.State == true)
                {
                    search_indicator += Dicfilter[filter.Name];
                }
            }

            if (nofilter.Contains(true) == false)
            {
                search_indicator = "" + Betrag + "€" + Zweck + "" + Notiz + "" + Datumanzeige + "" + id.ToString() + "" + auftrags_id + "";
            }

            return search_indicator;
        }

        public List<string> Search_Indicator(List<Filter> filters)
        {
            if (filters.Where(fil => fil.Name == "Quersuche").Count() != 0)
            {
                filters.RemoveAt(6);
            }

            Dicfilter.Clear();

            Dicfilter.Add("Betrag", betrag + " €");

            Dicfilter.Add("Datum", datum.ToString("dddd, d.MMMM.yyyy", new CultureInfo("de-DE")));

            Dicfilter.Add("Zweck", zweck);

            Dicfilter.Add("Notiz", notiz);

            Dicfilter.Add("Transaktions_ID", id.ToString());

            Dicfilter.Add("Auftrags_ID", auftrags_id);

            List<bool> nofilter = new List<bool>();

            List<string> search_indicator = new List<string>();

            char[] delimiterChars = { ' ', '-', '.', ':', '?','!','+','=','/','&','<','>','|' };

            foreach (Filter filter in filters)
            {
                nofilter.Add(filter.State);

                if (filter.State == true)
                {
                    if(Dicfilter[filter.Name] != null)
                    {
                        string[] placeholder = Dicfilter[filter.Name].Split(delimiterChars);

                        if (placeholder.Length != 0)
                        {
                            foreach (string placeholderChar in placeholder)
                            {
                                if (placeholderChar != "")
                                {
                                    search_indicator.Add(placeholderChar);
                                }
                            }
                        }
                    }
                }
            }

            if (nofilter.Contains(true) == false)
            {
                foreach (Filter filter in filters)
                {
                    if (Dicfilter[filter.Name] != null)
                    {
                        string[] placeholder = Dicfilter[filter.Name].Split(delimiterChars);

                        if (placeholder.Length != 0)
                        {
                            foreach (string placeholderChar in placeholder)
                            {
                                if (placeholderChar != "")
                                {
                                    search_indicator.Add(placeholderChar);
                                }
                            }
                        }
                    }
                }
            }

            return search_indicator;
        }
    }
}
