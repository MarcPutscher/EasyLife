using EasyLife.Helpers;
using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EasyLife.Services
{    /// <summary>
     /// Ist eine Serviceklasse für die Überstunden.
     /// </summary>
    public class OvertimeService
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

            await db.CreateTableAsync<OvertimeItem>();
        }

        /// <summary>
        /// Fügt eine Überstunde hinzu.
        ///  0 = Erfolgreich | 1 = Input war null
        /// </summary>
        /// <param name="input">Die Überstunde die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_Overtime(OvertimeItem input)
        {
            if (input == null)
            {
                return 1;
            }

            await db.InsertAsync(input);

            return 0;
        }

        /// <summary>
        /// Entfernt eine spezifische Überstunde.
        /// </summary>
        /// <param name="item">Die Überstunde die entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Overtime(OvertimeItem item)
        {
            await Init();

            await db.DeleteAsync<OvertimeItem>(item.Id);
        }

        /// <summary>
        /// Bearbeitet eine Überstunde.
        /// </summary>
        /// <param name="item">Die Überstunde die in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Overtime(OvertimeItem item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle Überstunden in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<OvertimeItem>> Get_all_Overtime()
        {
            await Init();

            try
            {
                List<OvertimeItem> cat = await db.Table<OvertimeItem>().ToListAsync();

                return cat.ToList();
            }
            catch
            {
                return new List<OvertimeItem>();
            }
        }

        /// <summary>
        /// Gibt alle Überstunden in der Datenbank zurück die nicht im Mülleimer sind.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<OvertimeItem>> Get_all_Overtime_for_Logs()
        {
            await Init();

            try
            {
                List<OvertimeItem> cat = await db.Table<OvertimeItem>().ToListAsync();

                ObservableCollection<OvertimeItem> result = new ObservableCollection<OvertimeItem>();

                foreach (OvertimeItem item in cat)
                {
                    if(item.Is_Removed == false)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            catch
            {
                return new ObservableCollection<OvertimeItem>();
            }
        }

        /// <summary>
        /// Gibt alle Überstunden in der Datenbank zurück die im Mülleimer sind.
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<OvertimeItem>> Get_all_Overtime_for_Trash_Logs()
        {
            await Init();

            try
            {
                List<OvertimeItem> cat = await db.Table<OvertimeItem>().ToListAsync();

                ObservableCollection<OvertimeItem> result = new ObservableCollection<OvertimeItem>();

                foreach (OvertimeItem item in cat)
                {
                    if (item.Is_Removed == true)
                    {
                        result.Add(item);
                    }
                }

                return result;
            }
            catch
            {
                return new ObservableCollection<OvertimeItem>();
            }
        }

        /// <summary>
        /// Gibt ein spezifische Überstunde zurück.
        /// </summary>
        /// <param name="result">Das Datum das zum finden der spezifischen Überstunde notwendig ist.</param>
        /// <returns></returns>
        public static async Task<OvertimeItem> Get_specific_Overtime(DateTime input)
        {
            var table = await Get_all_Overtime();

            foreach (var re in table)
            {
                if (re.Date == input.Date)
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
        /// Gibt ein spezifische Überstunde zurück.
        /// </summary>
        /// <param name="result">Die ID die zum finden der spezifischen Überstunde notwendig ist.</param>
        /// <returns></returns>
        public static async Task<OvertimeItem> Get_specific_Overtime_from_ID(int result)
        {
            var table = await Get_all_Overtime();

            foreach (var re in table)
            {
                if (re.Id == result)
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
    }
}
