using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EasyLife.Models
{
    public class Haushaltsbücher
    {
        public readonly List<string> existing_months = new List<string>() { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };

        public Dictionary<string, List<string>> Time = new Dictionary<string, List<string>>();

        public Haushaltsbücher(List<Transaktion> transaktion)
        {
            Create_Time_Dictionary(transaktion);
        }

        public void Create_Time_Dictionary(List<Transaktion> transaktion)
        {
            Get_Years(transaktion);
        }

        public void Get_Years(List<Transaktion> transaktions)
        {
            List<string> years_list = new List<string>();

            foreach (Transaktion tr in transaktions)
            {
                if (years_list.Contains(tr.Datum.Year.ToString()) == false)
                {
                    years_list.Add(tr.Datum.Year.ToString());
                }
            }

            years_list.Sort();

            foreach (string year in years_list)
            {
                Time.Add(year, new List<string>());
            }

            Add_Month(transaktions);
        }

        public void Add_Month(List<Transaktion> transaktions)
        {
            List<string> months = new List<string>();

            foreach (Transaktion transaktion in transaktions)
            {
                if (Time.Keys.ToList().Contains(transaktion.Datum.Year.ToString()) == true)
                {
                    if (Time[transaktion.Datum.Year.ToString()].Contains(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE"))) == false)
                    {
                        Time[transaktion.Datum.Year.ToString()].Add(transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE")));
                    }
                }

            }

            Sort_Months();
        }

        public void Sort_Months()
        {
            List<List<string>> months_list = Time.Values.ToList();

            List<string> years = Time.Keys.ToList();

            int h = 0;

            foreach (List<string> months in months_list)
            {
                List<string> sortet_list_of_months = new List<string>();

                sortet_list_of_months.Clear();

                List<int> months_in_int = new List<int>();

                months_in_int.Clear();

                foreach (string month in months)
                {
                    int k = 0;

                    while (month != existing_months[k])
                    {
                        k++;
                    }

                    months_in_int.Add(k);
                }

                months_in_int.Sort();

                foreach (int k in months_in_int)
                {
                    sortet_list_of_months.Add(existing_months[k]);
                }

                Time[years[h]] = sortet_list_of_months;

                h++;
            }
        }
    }
}
