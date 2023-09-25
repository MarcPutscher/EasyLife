using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Zwischenspeicherhilsklasse während der Bearbeitung von Transaktionen.
    /// </summary>
    public static class PassingTransaktionService
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

            // Get an absolute path to the database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "HelperTransaktion.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Transaktion>();
        }

        /// <summary>
        /// Fügt eine Hilfs-Transaktion hinzu. 
        /// </summary>
        /// <param name="input">Die Transaktionn die in der Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Transaktion(Transaktion input)
        {
            await Init();

            await db.InsertAsync(input);
        }

        /// <summary>
        /// Bearbeitet eine Hilfs-Transaktion.
        /// </summary>
        /// <param name="item">Die Transaktion der in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Transaktion(Transaktion item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Entfernt alle Hilfs-Transaktionen in der Datenbank.
        /// </summary>
        /// <returns></returns>
        public static async Task Remove_All_Transaktion()
        {
            await Init();

            await db.DeleteAllAsync<Transaktion>();
        }

        /// <summary>
        /// Gibt eine spezifische Hilfs-Transaktion zurück.
        /// </summary>
        /// <param name="result">Die Hilfs-TransaktionsId die zum finden der spezifischen Hilfs-Transaktion notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Transaktion> Get_specific_Transaktion(int id)
        {
            await Init();

            return await db.FindAsync<Transaktion>(id);
        }
    }
}
