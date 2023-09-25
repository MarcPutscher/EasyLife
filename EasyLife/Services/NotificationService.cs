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
    /// Ist eine Serviceklasse für die Benachrichtigungen.
    /// </summary>
    public class NotificationService
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

            await db.CreateTableAsync<Notification>();
        }

        /// <summary>
        /// Fügt ein Benachrichtigung hinzu.
        /// </summary>
        /// <param name="input">Die Benachrichtigung die in die Datenbank hinzugefügt werden soll.</param>
        /// <returns></returns>
        public static async Task<bool> Add_Notification(Notification input)
        {
            if (input == null)
            {
                return false;
            }

            await Init();

            await db.InsertAsync(input);
            return true;
        }

        /// <summary>
        /// Entfernt eine spezifische Benachrichtigung.
        /// </summary>
        /// <param name="input">Die Benachrichtigung die entfernt werden soll.</param>
        /// <returns></returns>
        public static async Task Remove_Notification(Notification input)
        {
            await Init();

            if(input == null)
            {
                return;
            }

            await db.DeleteAsync<Notification>(input.Id);
        }

        /// <summary>
        /// Bearbeitet eine Benachrichtigung.
        /// </summary>
        /// <param name="input">Die Benachrichtigung die in der Datenbank verändert werden soll.</param>
        /// <returns></returns>
        public static async Task Edit_Notification(Notification input)
        {
            await Init();

            await db.UpdateAsync(input);
        }

        /// <summary>
        /// Gibt alle Benachrichtigungen in der Datenbank zurück.
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<Notification>> Get_all_Notification()
        {
            await Init();

            try
            {
                var reson = await db.Table<Notification>().ToListAsync();

                return reson.Reverse<Notification>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gibt eine spezifische Benachrichtigung zurück.
        /// </summary>
        /// <param name="input">Die Auftragsid die zum finden der spezifischen Benachrichtigung notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Notification> Get_specific_Notification_with_Order_ID(int input)
        {
            await Init();

            var notificationtabel = await Get_all_Notification();

            foreach (var no in notificationtabel)
            {
                if (no.Auftrags_ID == input)
                {
                    return no;
                }
            }
            return null;
        }

        /// <summary>
        /// Gibt eine spezifische Benachrichtigung zurück.
        /// </summary>
        /// <param name="input">Die Benachrichtigungsid die zum finden der spezifischen Benachrichtigung notwendig ist.</param>
        /// <returns></returns>
        public static async Task<Notification> Get_specific_Notification_with_Notification_ID(int input)
        {
            await Init();

            var notificationtabel = await Get_all_Notification();

            foreach (var no in notificationtabel)
            {
                if (no.Notification_ID == input)
                {
                    return no;
                }
            }
            return null;
        }
    }
}
