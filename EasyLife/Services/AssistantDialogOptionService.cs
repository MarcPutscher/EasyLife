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
    public class AssistantDialogOptionService
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
        /// Fügt eine Dialogoption hinzu.
        /// </summary>
        /// <param name="input">Die Dialogoption die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task Add_Dialogoption(string question, string answer)
        {
            if(String.IsNullOrEmpty(question) || String.IsNullOrEmpty(answer))
            {
                return;
            }

            await Init();

            string pseudoinput = question;

            var result = await Get_specific_Dialogoption(pseudoinput.ToUpper());

            if(result!=null)
            {
                return;
            }
            else
            {
                await db.InsertAsync(new AssistantDialogOption(question,answer));
            }
        }

        /// <summary>
        /// Entfernt eine spezifische Dialogoption.
        /// </summary>
        /// <param name="item">Die Dialogoption die entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Dialogoption(AssistantDialogOption dialogOption)
        {
            await Init();

            await db.DeleteAsync<AssistantDialogOption>(dialogOption.Id);
        }

        /// <summary>
        /// Gibt alle Dialogoptionen in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<AssistantDialogOption>> Get_all_Dialogoption()
        {
            await Init();
            
            try
            {
                var dialogOptions = await db.Table<AssistantDialogOption>().ToListAsync();

                return dialogOptions.Reverse<AssistantDialogOption>();
            }
            catch
            {
                return new List<AssistantDialogOption>();
            }
        }

        /// <summary>
        /// Gibt eine spezifische Dialogoption zurück.
        /// </summary>
        /// <param name="result">Die Dialogoption die zum finden der spezifischen Dialogoption notwendig ist.</param>
        /// <returns></returns>
        public static async Task<AssistantDialogOption> Get_specific_Dialogoption(string question)
        {
            await Init();

            var dialogOptions = await Get_all_Dialogoption();

            foreach( var diop in dialogOptions)
            {
                if(diop.Question.ToUpper() == question)
                {
                    return diop;
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
