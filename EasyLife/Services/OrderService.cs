using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Serviceklasse für die Aufträge.
    /// </summary>
    public class OrderService
    {
        public static SQLiteAsyncConnection dba;

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        /// <returns></returns>
        public static async Task Init()
        {
            if (dba != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

            dba = new SQLiteAsyncConnection(databasePath);

            await dba.CreateTableAsync<Auftrag>();
        }

        /// <summary>
        /// Fügt ein Auftrag hinzu.
        /// </summary>
        /// <param name="input">Der Auftrag der in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Order(Auftrag input)
        {
            await Init();

            await dba.InsertAsync(input);
        }

        /// <summary>
        /// Entfernt ein spezifischen Auftrag.
        /// </summary>
        /// <param name="item">Der Auftrag der entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Order(Auftrag item)
        {
            await Init();

            await dba.DeleteAsync<Auftrag>(item.Id);
        }

        /// <summary>
        /// Bearbeitet ein Auftrag.
        /// </summary>
        /// <param name="item">Der Auftrag der in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        internal static async Task Edit_Order(Auftrag item)
        {
            await Init();

            await dba.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt ein spezifischen Auftrag zurück.
        /// </summary>
        /// <param name="result">Die Auftragsid die zum finden des spezifischen Auftrages notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Auftrag> Get_specific_Order(int result)
        {
            await Init();

            return await dba.FindAsync<Auftrag>(result);
        }
    }
}
