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
    /// Ist eine Serviceklasse für das Styling.
    /// </summary>
    public class StylingService
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

            await db.CreateTableAsync<Stylingprofile>();
        }

        /// <summary>
        /// Fügt einen Stylingprofil hinzu.
        /// </summary>
        /// <param name="input">Das Stylingprofil das in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_Stylingprofile(Stylingprofile input)
        {
            if (input == null)
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
        /// Entfernt einen spezifischen Stylingprofil.
        /// </summary>
        /// <param name="item">Das Stylingprofil das entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Stylingprofile(Stylingprofile item)
        {
            await Init();

            await db.DeleteAsync<Stylingprofile>(item.Id);
        }

        /// <summary>
        /// Bearbeitet ein Stylingprofil.
        /// </summary>
        /// <param name="item">Das Stylingprofil das in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        internal static async Task Edit_Stylingprofile(Stylingprofile item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle Stylingprofile in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Stylingprofile>> Get_all_Stylingprofile()
        {
            await Init();

            try
            {
                var stylingprofile = await db.Table<Stylingprofile>().ToListAsync();

                return stylingprofile.Reverse<Stylingprofile>();
            }
            catch
            {
                return new List<Stylingprofile>();
            }
        }

        /// <summary>
        /// Gibt ein spezifischen Stylingprofil zurück.
        /// </summary>
        /// <param name="result">Die Stylingprofilid die zum finden des spezifischen Stylingprofiles notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Stylingprofile> Get_specific_Stylingprofile(int result)
        {
            await Init();

            return await db.FindAsync<Stylingprofile>(result);
        }
    }

    /// <summary>
    /// Ist eine Konvertierungsklasse um Bilanzprofile de-/serealisieren.
    /// </summary>
    public class Stylingprofile_Konverter
    {
        public static string Serilize(Dictionary<string, string> input)
        {
            Stylingprofile output = new Stylingprofile();

            if (input.Count() != 0)
            {
                foreach (var s in input)
                {
                    output.Colors += "" + s.Key + "=" + s.Value + "+";
                }

                output.Colors = output.Colors.Substring(0, output.Colors.LastIndexOf("+"));
            }

            return output.Colors;
        }

        public static Dictionary<string, string> Deserilize(Stylingprofile input)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            string colors = input.Colors;

            if (colors != null)
            {
                while (colors.Length > 0)
                {
                    if (colors.Contains("+") == true)
                    {
                        string key = colors.Substring(0, colors.IndexOf("="));

                        colors = colors.Substring(colors.IndexOf("=") + 1);

                        string value = colors.Substring(0, colors.IndexOf("+"));

                        output.Add(key, value);

                        colors = colors.Substring(colors.IndexOf("+") + 1);
                    }
                    else
                    {
                        string key = colors.Substring(0, colors.IndexOf("="));

                        colors = colors.Substring(colors.IndexOf("=") + 1);

                        string value = colors;

                        output.Add(key, value);

                        colors = "";
                    }
                }
            }

            return output;
        }
    }
}