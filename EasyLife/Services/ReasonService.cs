using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static SQLite.SQLite3;


namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Serviceklasse für die Zwecke.
    /// </summary>
    public static class ReasonService
    {
        public static SQLiteAsyncConnection db;

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        /// <returns></returns>
        public static async Task Init()
        {
            if (db != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Zweck>();

            var reson = await db.Table<Zweck>().ToListAsync();

            if(reson.Count() == 0)
            {
                Zweck zweck0 = new Zweck
                {
                    Benutzerdefinierter_Zweck = "Sonstiges:Ausgaben",
                    Reason_Visibility = true,
                    Benutzerdefinierter_Prevalence = 0
                };

                await db.InsertAsync(zweck0);
            }
        }

        /// <summary>
        /// Fügt einen Zweck mit seiner Bedeutung hinzu.
        /// </summary>
        /// <param name="reason">Der Name des Zweckes der in die Datenbank hinzugefügt werden soll.</param>
        /// <param name="value">Die Bedeutung des Zweckes die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<bool> Add_Reason(string reason, string value)
        {
            if (String.IsNullOrEmpty(reason) || String.IsNullOrEmpty(value))
            {
                return false;
            }

            await Init();

            Zweck zweck = new Zweck() { Benutzerdefinierter_Zweck = ""+reason.Trim()+":"+value+"" , Reason_Visibility = true , Benutzerdefinierter_Prevalence = 0};

            var result = await Get_specific_Reason(zweck.Benutzerdefinierter_Zweck.ToUpper());



            if (result != null)
            {
                string neu = result.Benutzerdefinierter_Zweck.Substring(0,result.Benutzerdefinierter_Zweck.IndexOf(":"));

                string neu2 = reason.Trim();

                bool indicator = false;

                int k = 0;

                foreach (Char ch in neu)
                {
                    if (ch != neu2[k])
                    {
                        indicator = true;
                        break;
                    }

                    k++;
                }

                if(indicator == true)
                {
                    await db.InsertAsync(zweck);
                }

                return indicator;
            }
            else
            {
                await db.InsertAsync(zweck);

                return true;
            }
        }

        /// <summary>
        /// Bearbeitet einen Zweck.
        /// </summary>
        /// <param name="item">Der Zweck der in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Reason(Zweck item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Löscht einen Zweck.
        /// </summary>
        /// <param name="item">Der Zweck der in der Datenbank gelöscht werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Reason(Zweck item)
        {
            await Init();

            await db.DeleteAsync<Zweck>(item.Id);
        }

        /// <summary>
        /// Gibt alle Zwecke in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Zweck>> Get_all_Reason()
        {
            await Init();

            try
            {
                var reson = await db.Table<Zweck>().ToListAsync();

                return reson.Reverse<Zweck>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt alle Zwecke sortiert nach Häufigkeit in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Zweck>> Get_all_Reason_sorted()
        {
            await Init();

            try
            {
                List<Zweck> zwecke = new List<Zweck>(await Get_all_Reason());
                List<Zweck> result = zwecke.OrderByDescending(zweck => zweck.Benutzerdefinierter_Prevalence).ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt alle aktiven Zwecke in der Datenbank zurück. 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Zweck>> Get_Enable_ReasonList()
        {
            List<Zweck> zweckList1 = new List<Zweck>(await Get_all_Reason());

            List<Zweck> zweckList = new List<Zweck>();

            foreach (var re in zweckList1)
            {
                if (re.Reason_Visibility == true)
                {
                    zweckList.Add(re);
                }
            }

            return zweckList;
        }

        /// <summary>
        /// Gibt alle deaktiven Zwecke in der Datenbank zurück. 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Zweck>> Get_Disable_ReasonList()
        {
            List<Zweck> zweckList1 = new List<Zweck>(await Get_all_Reason());

            List<Zweck> zweckList = new List<Zweck>();

            foreach (var re in zweckList1)
            {
                if (re.Reason_Visibility == false)
                {
                    zweckList.Add(re);
                }
            }

            return zweckList;
        }

        /// <summary>
        /// Gibt alle aktiven Zwecke in der Datenbank zurück. 
        /// </summary>
        /// <returns></returns>
        public static async Task<IDictionary<string, string>> Get_Enable_ReasonDictionary()
        {
            List<Zweck> zweckList1 = new List<Zweck>(await Get_all_Reason());

            List<Zweck> zweckList = new List<Zweck>();

            foreach (var re in zweckList1)
            {
                if (re.Reason_Visibility == true)
                {
                    zweckList.Add(re);
                }
            }

            IDictionary<string, string> result = new Dictionary<string, string>();

            Zweck sonstiges = new Zweck() { Benutzerdefinierter_Zweck = "Sonstiges:Ausgaben", Reason_Visibility = true };

            if (zweckList.Contains(sonstiges) == false)
            {
                foreach (Zweck zw in zweckList)
                {
                    result.Add(zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
                }
            }
            else
            {
                zweckList.Remove(sonstiges);

                foreach (Zweck zw in zweckList)
                {
                    if (zw.Benutzerdefinierter_Zweck != "Sonstiges:Ausgaben")
                    {
                        result.Add(zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
                    }
                }

                result.Add(sonstiges.Benutzerdefinierter_Zweck.Substring(0, sonstiges.Benutzerdefinierter_Zweck.IndexOf(":")), sonstiges.Benutzerdefinierter_Zweck.Substring(sonstiges.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
            }



            return result;
        }

        /// <summary>
        /// Gibt alle deaktiven Zwecke in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IDictionary<string, string>> Get_Disable_ReasonDictionary()
        {
            List<Zweck> zweckList1 = new List<Zweck>(await Get_all_Reason());

            List<Zweck> zweckList = new List<Zweck>();

            foreach (var re in zweckList1)
            {
                if (re.Reason_Visibility == false)
                {
                    zweckList.Add(re);
                }
            }

            IDictionary<string, string> result = new Dictionary<string, string>();

            Zweck sonstiges = new Zweck() { Benutzerdefinierter_Zweck = "Sonstiges:Ausgaben", Reason_Visibility = true , Benutzerdefinierter_Prevalence = 0};

            if (zweckList.Contains(sonstiges) == false)
            {
                foreach (Zweck zw in zweckList)
                {
                    result.Add(zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
                }
            }
            else
            {
                zweckList.Remove(sonstiges);

                foreach (Zweck zw in zweckList)
                {
                    if (zw.Benutzerdefinierter_Zweck != "Sonstiges:Ausgaben")
                    {
                        result.Add(zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
                    }
                }

                result.Add(sonstiges.Benutzerdefinierter_Zweck.Substring(0, sonstiges.Benutzerdefinierter_Zweck.IndexOf(":")), sonstiges.Benutzerdefinierter_Zweck.Substring(sonstiges.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
            }

            return result;
        }

        /// <summary>
        /// Gibt alle aktiven Zwecke, sotiert nach Häufigkeit, in der Datenbank zurück.
        /// </summary>
        public static async Task<IDictionary<string, string>> Get_Enable_ReasonDictionary_sorted()
        {
            List<Zweck> zwecke = (List<Zweck>)await ReasonService.Get_Enable_ReasonList();
            if (zwecke.Count() != 0)
            {
                List<Zweck> result = zwecke.OrderByDescending(zweck => zweck.Benutzerdefinierter_Prevalence).ToList();

                return result.ToDictionary(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw => zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt alle deaktiven Zwecke, sotiert nach Häufigkeit, in der Datenbank zurück.
        /// </summary>
        public static async Task<IDictionary<string, string>> Get_Disable_ReasonDictionary_sorted()
        {
            List<Zweck> zwecke = (List<Zweck>)await ReasonService.Get_Disable_ReasonList();
            if (zwecke.Count() != 0)
            {
                List<Zweck> result = zwecke.OrderByDescending(zweck => zweck.Benutzerdefinierter_Prevalence).ToList();

                return result.ToDictionary(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")), zw => zw.Benutzerdefinierter_Zweck.Substring(zw.Benutzerdefinierter_Zweck.IndexOf(":") + 1));
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt einen spezifischen Zweck zurück.
        /// </summary>
        /// <param name="result">Der Name des Zweckes der zum finden des spezifischen Zweckes notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Zweck> Get_specific_Reason(string result)
        {
            await Init();

            var reasontabel = await Get_all_Reason();

            foreach (var re in reasontabel)
            {
                if (re.Benutzerdefinierter_Zweck.Substring(0, re.Benutzerdefinierter_Zweck.IndexOf(":")).ToUpper() == result.Substring(0, result.IndexOf(":")).ToUpper())
                {
                    return re;
                }
                else
                {
                    continue;
                }
            }
            return null;
        }

        /// <summary>
        /// Gibt die Definition eines spezifischen Zweckes zurück.
        /// </summary>
        /// <param name="input">Der Name des Zweckes der zum finden der Difinition des spezifischen Zweckes notwendig ist.</param>
        /// <returns></returns>
        public static async Task<string> Get_definition_of_specific_Reason(string input)
        {
            await Init();

            var reasontabel = await Get_all_Reason();

            foreach (var re in reasontabel)
            {
                if (re.Benutzerdefinierter_Zweck.Substring(0, re.Benutzerdefinierter_Zweck.IndexOf(":")).ToUpper() == input.ToUpper())
                {
                    return re.Benutzerdefinierter_Zweck.Substring(re.Benutzerdefinierter_Zweck.IndexOf(":")+1);
                }
                else
                {
                    continue;
                }
            }
            return null;
        }
    }
}
