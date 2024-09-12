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
    /// Ist eine Serviceklasse für die Budgets.
    /// </summary>
    public class To_DoService
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

            await db.CreateTableAsync<To_Do_Item>();
        }

        /// <summary>
        /// Fügt eine To-Do hinzu.
        ///  0 = Erfolgreich | 1 = Input war null | 2 = Input ist schon vorhanden
        /// </summary>
        /// <param name="input">Das To-Do das in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_To_Do(To_Do_Item input)
        {
            if (input == null)
            {
                return 1;
            }
            
            List<To_Do_Item> to_dos = await Get_all_To_DOs();

            bool is_init = false;

            foreach (var to_do in to_dos)
            {
                if (to_do.To_Do == input.To_Do)
                {
                    is_init = true;

                    break;
                }
            }

            if (is_init == true)
            {
                return 2;
            }
            else
            {
                await db.InsertAsync(input);

                return 0;
            }
        }

        /// <summary>
        /// Entfernt ein spezifisches To-Do.
        /// </summary>
        /// <param name="item">Das To-Do das entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_To_Do(To_Do_Item item)
        {
            await Init();

            await db.DeleteAsync<To_Do_Item>(item.Id);
        }

        /// <summary>
        /// Bearbeitet ein To-Do.
        /// </summary>
        /// <param name="item">Das To-Do das in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_To_Do(To_Do_Item item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle To-Dos in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<To_Do_Item>> Get_all_To_DOs()
        {
            await Init();

            try
            {
                var to_Do_s = await db.Table<To_Do_Item>().ToListAsync();

                return to_Do_s.Reverse<To_Do_Item>().ToList();
            }
            catch
            {
                return new List<To_Do_Item>();
            }
        }

        /// <summary>
        /// Gibt ein spezifisches To-Do zurück.
        /// </summary>
        /// <param name="result">Die ID die zum finden des spezifischen To-Do notwendig ist.</param>
        /// <returns></returns>
        public static async Task<To_Do_Item> Get_specific_To_Do_from_ID(int result)
        {
            var table = await Get_all_To_DOs();

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
