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
    /// Ist eine Serviceklasse für die Suchbegriffe.
    /// </summary>
    public class SearchSuggestionService
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

            await db.CreateTableAsync<Suggestion>();
        }

        /// <summary>
        /// Fügt einen Suchbegriff hinzu.
        /// </summary>
        /// <param name="input">De Suchbegriff der in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Suggestion(string input)
        {
            if(String.IsNullOrEmpty(input))
            {
                return;
            }

            await Init();


            var result = await Get_specific_Suggestion(input.ToUpper());

            if(result!=null)
            {
                return;
            }
            else
            {
                await db.InsertAsync(new Suggestion() { Suggestion_value = input });
            }
        }

        /// <summary>
        /// Entfernt einen spezifischen Suchbegriff.
        /// </summary>
        /// <param name="item">Der Suchbegriff der entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Suggestion(Suggestion item)
        {
            await Init();

            await db.DeleteAsync<Suggestion>(item.Id);
        }

        /// <summary>
        /// Gibt alle Suchbegriffe in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Suggestion>> Get_all_Suggestion()
        {
            await Init();
            
            try
            {
                var auftrag = await db.Table<Suggestion>().ToListAsync();

                return auftrag.Reverse<Suggestion>();
            }
            catch
            {
                return new List<Suggestion>();
            }
        }

        /// <summary>
        /// Gibt einen spezifischen Suchbegriff zurück.
        /// </summary>
        /// <param name="result">Der Suchbegriff der zum finden des spezifischen Suchbegriffes notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Suggestion> Get_specific_Suggestion(string result)
        {
            await Init();

            var suggestiontabel = await Get_all_Suggestion();

            foreach( var sug in suggestiontabel)
            {
                if(sug.Suggestion_value.ToUpper() == result)
                {
                    return sug;
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
