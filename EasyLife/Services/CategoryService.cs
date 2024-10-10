using EasyLife.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using static SQLite.SQLite3;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Serviceklasse für die Categories.
    /// </summary>
    public class CategoryService
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

            await db.CreateTableAsync<Category>();
        }

        /// <summary>
        /// Fügt eine Category hinzu.
        ///  0 = Erfolgreich | 1 = Input war null | 2 = Input ist schon vorhanden
        /// </summary>
        /// <param name="input">Die Category die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<int> Add_Category(Category input)
        {
            if (input == null)
            {
                return 1;
            }
            
            List<Category> categorys = await Get_all_Categorys();

            bool is_init = false;

            foreach (var category in categorys)
            {
                if (category.Title == input.Title)
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
        /// Entfernt eine spezifische Category.
        /// </summary>
        /// <param name="item">Die Category die entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Category(Category item)
        {
            await Init();

            if(item.Title == "Sonstige")
            {
                return;
            }

            await db.DeleteAsync<Category>(item.Id);
        }

        /// <summary>
        /// Bearbeitet eine Category.
        /// </summary>
        /// <param name="item">Die Category die in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Category(Category item)
        {
            await Init();

            await db.UpdateAsync(item);
        }

        /// <summary>
        /// Gibt alle Categorys in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Category>> Get_all_Categorys()
        {
            await Init();

            try
            {
                List<Category> cat = await db.Table<Category>().ToListAsync();

                if (cat.Count == 0)
                {
                    await db.InsertAsync(new Category() { Title = "Sonstige", Color = Xamarin.Forms.Color.Gray.ToHex(), Is_Select = false });
                    return new List<Category>(){ await db.Table<Category>().FirstAsync() };
                }
                return cat.ToList();
            }
            catch
            {
                return new List<Category>();
            }
        }

        /// <summary>
        /// Gibt ein spezifische Category zurück.
        /// </summary>
        /// <param name="result">Der Titel der zum finden der spezifischen Category notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Category> Get_specific_Category(string result)
        {
            var table = await Get_all_Categorys();

            foreach (var re in table)
            {
                if (re.Title.ToUpper() == result.ToUpper())
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
        /// Gibt ein spezifische Category zurück.
        /// </summary>
        /// <param name="result">Die ID die zum finden der spezifischen Category notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Category> Get_specific_Category_from_ID(int result)
        {
            var table = await Get_all_Categorys();

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
