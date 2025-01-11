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
    public class BudgetService
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

            await db.CreateTableAsync<Budget>();
        }

        /// <summary>
        /// Fügt einen Budget hinzu.
        /// </summary>
        /// <param name="input">Der Budget der in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_Budget(Budget input)
        {
            if (input == null)
            {
                return 1;
            }

            await Init();

            var budgets = await Get_all_Budget();

            bool is_init = false;

            foreach (var budget in budgets)
            {
                if (budget.Name == input.Name)
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
        /// Entfernt einen spezifischen Budget.
        /// </summary>
        /// <param name="item">Der Budget der entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Budget(Budget item)
        {
            await Init();

            await db.DeleteAsync<Budget>(item.Id);
        }

        /// <summary>
        /// Bearbeitet ein Budget.
        /// </summary>
        /// <param name="item">Der Budget der in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        internal static async Task Edit_Budget(Budget item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle Budgets in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Budget>> Get_all_Budget()
        {
            await Init();

            try
            {
                var budget = await db.Table<Budget>().ToListAsync();

                return budget.Reverse<Budget>();
            }
            catch
            {
                return new List<Budget>();
            }
        }
    }

    public class Budget_Konverter
    {
        public static string Serilize(List<string> input)
        {
            string output = string.Empty;

            if (input.Count() != 0)
            {
                foreach (var s in input)
                {
                    output += "" + s + "+";
                }

                output = output.Substring(0, output.LastIndexOf("+"));
            }

            return output;
        }

        public static List<string> Deserilize(Budget input)
        {
            List<string> output = new List<string>();

            string preferences = input.Name_Of_Enabled_Reasons;

            if (preferences != null)
            {
                while (preferences.Length > 0)
                {
                    if (preferences.Contains("+") == true)
                    {
                        string value = preferences.Substring(0, preferences.IndexOf("+"));

                        output.Add(value);

                        preferences = preferences.Substring(preferences.IndexOf("+") + 1);
                    }
                    else
                    {
                        string value = preferences;

                        output.Add(value);

                        preferences = "";
                    }
                }
            }

            return output;
        }
    }
}
