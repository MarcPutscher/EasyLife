using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Essentials;
using static SQLite.SQLite3;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Serviceklasse für die Transaktionen.
    /// </summary>
    public static class ContentService
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

            await db.CreateTableAsync<Transaktion>();
        }

        /// <summary>
        /// Fügt eine Transaktion hinzu.
        /// </summary>
        /// <param name="input">Die Transaktion die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Transaktion(Transaktion input)
        {
            await Init();

            await db.InsertAsync(input);
        }

        /// <summary>
        /// Entfernt eine spezifische Transaktion.
        /// </summary>
        /// <param name="item">Die Transaktion die entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Transaktion(Transaktion item)
        {
            await Init();

            await db.DeleteAsync<Transaktion>(item.Id);
        }

        /// <summary>
        /// Gibt alle Transaktionen in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Transaktion>> Get_all_Transaktion()
        {
            await Init();
            try
            {
                return await db.Table<Transaktion>().ToListAsync();
            }
            catch
            {
                return Enumerable.Empty<Transaktion>();
            }
        }

        /// <summary>
        /// Gibt alle aktiven Transaktionen in der Datenbank zurück. 
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Transaktion>> Get_all_enabeled_Transaktion()
        {
            await Init();

            List<Transaktion> enabled = new List<Transaktion>();

            try
            {
                var transaktion = await db.Table<Transaktion>().ToListAsync();


                foreach(Transaktion trans in transaktion)
                {
                    if(trans.Content_Visibility == true)
                    {
                        enabled.Add(trans);
                        continue;
                    }
                }

                return enabled;
            }
            catch
            {
                return enabled;
            }
        }

        /// <summary>
        /// Gibt alle deaktiven Transaktionen in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Transaktion>> Get_all_disabeled_Transaktion()
        {
            await Init();

            List<Transaktion> disabled = new List<Transaktion>();

            try
            {
                var transaktion = await db.Table<Transaktion>().ToListAsync();


                foreach (Transaktion trans in transaktion)
                {
                    if (trans.Content_Visibility == false)
                    {
                        disabled.Add(trans);
                        continue;
                    }
                }

                return disabled;
            }
            catch
            {
                return disabled;
            }
        }

        /// <summary>
        /// Gibt die letzte Transakktion zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<Transaktion>Get_last_Transaktion()
        {
            await Init();

            var transaktion = await Get_all_Transaktion();

            List<Transaktion> list = new List<Transaktion>(transaktion);

            return list.Last();
        }

        /// <summary>
        /// Gibt eine spezifische Transaktion zurück.
        /// </summary>
        /// <param name="id">Die Transaktionsid die zum finden der spezifischen Transaktion notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Transaktion> Get_specific_Transaktion(int id)
        {
            await Init();

            return await db.FindAsync<Transaktion>(id);
        }

        /// <summary>
        /// Bearbeitet eine Transaktion.
        /// </summary>
        /// <param name="item">Die Transaktion die in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        internal static async Task Edit_Transaktion(Transaktion item)
        {
            await Init();

            await db.UpdateAsync(item);
        }
    }
}
