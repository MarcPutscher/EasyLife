using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Zwischenspeicherhilsklasse während der Bearbeitung von Aufträgen.
    /// </summary>
    public static class PassingOrderService
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

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "HelperOrder.db");

            dba = new SQLiteAsyncConnection(databasePath);

            await dba.CreateTableAsync<Auftrag>();
        }

        /// <summary>
        /// Fügt einen Hilfs-Auftraghinzu.  
        /// </summary>
        /// <param name="input">Der Auftrag der in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Order(Auftrag input)
        {
            await Init();

            await dba.InsertAsync(input);
        }

        /// <summary>
        /// Bearbeitet einen Hilfs-Auftrag.
        /// </summary>
        /// <param name="item">Der Auftrag der in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Order(Auftrag item)
        {
            await Init();

            await dba.UpdateAsync(item);
        }

        /// <summary>
        /// Entfernt alle Hilfs-Aufträge in der Datenbank.
        /// </summary>
        /// <returns></returns>
        public static async Task Remove_All_Order()
        {
            await Init();

            await dba.DeleteAllAsync<Auftrag>();
        }

        /// <summary>
        /// Gibt den letzten Hilfs-Auftrag zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<Auftrag> Get_last_Order()
        {
            await Init();

            var auftrag = await dba.Table<Auftrag>().ToListAsync();

            if (auftrag.Count() == 0)
            {
                return null;
            }

            List<Auftrag> list = new List<Auftrag>(auftrag);

            return list.First();
        }

        /// <summary>
        /// Gibt einen spezifischen Hilfs-Auftrag zurück.
        /// </summary>
        /// <param name="result">Die Hilfs-AuftragId die zum finden des spezifischen Hilfs-Auftrag notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Auftrag> Get_specific_Order(int result)
        {
            await Init();

            return await dba.FindAsync<Auftrag>(result);
        }
    }
}
