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
    /// Ist eine Serviceklasse für die App-Preferences.
    /// </summary>
    public class AppPreferencesService
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

            await db.CreateTableAsync<AppPreferences>();
        }

        /// <summary>
        /// Fügt App-Preferences hinzu.
        /// </summary>
        /// <param name="input">Die App-Preferences die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_AppPreferences(AppPreferences input)
        {
            if(input == null)
            {
                return 0;
            }

            await Init();

            try
            {
                await db.InsertAsync(input);

                return input.Id;
            }
            catch
            {
                return -1;
            }
        }


        /// <summary>
        /// Bearbeitet die App-Preferences.
        /// </summary>
        /// <param name="item">Die App-Preferences die in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        internal static async Task Edit_AppPreferences(AppPreferences item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle App-Preferences in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<AppPreferences>> Get_all_AppPreferences()
        {
            await Init();
            
            try
            {
                var apppreferences = await db.Table<AppPreferences>().ToListAsync();

                var apl = apppreferences.Reverse<AppPreferences>();

                List<AppPreferences> list = new List<AppPreferences>();

                if(apl.Count() != 0)
                {
                    foreach(var apppref in apl)
                    {
                        list.Add(apppref);
                    }
                }

                return list;
            }
            catch
            {
                return new List<AppPreferences>();
            }
        }
    }

    /// <summary>
    /// Ist eine Konvertierungsklasse um Bilanzprofile de-/serealisieren.
    /// </summary>
    public class AppPreferences_Konverter
    {
        public static string Serilize(Dictionary<string, string> input)
        {
            AppPreferences output = new AppPreferences();

            if(input.Count() != 0)
            {
                foreach(var s in input)
                {
                    output.Preferences += "" + s.Key + "="+ s.Value +"+";
                }

                output.Preferences = output.Preferences.Substring(0,output.Preferences.LastIndexOf("+"));
            }

            return output.Preferences;
        }

        public static Dictionary<string,string> Deserilize(AppPreferences input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            string preferences = input.Preferences;

            if (preferences != null)
            {
                while(preferences.Length > 0)
                {
                    if (preferences.Contains("+") == true)
                    {
                        string key = preferences.Substring(0, preferences.IndexOf("="));

                        preferences = preferences.Substring(preferences.IndexOf("=") + 1);

                        string value = preferences.Substring(0, preferences.IndexOf("+"));

                        output.Add(key, value);

                        preferences = preferences.Substring(preferences.IndexOf("+") + 1);
                    }
                    else
                    {
                        string key = preferences.Substring(0, preferences.IndexOf("="));

                        preferences = preferences.Substring(preferences.IndexOf("=") + 1);

                        string value = preferences;

                        output.Add(key, value);

                        preferences = "";
                    }
                }
            }

            return output;
        }
    }
}
