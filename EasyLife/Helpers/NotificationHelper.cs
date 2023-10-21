using EasyLife.Models;
using EasyLife.Services;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyLife.Helpers
{
    /// <summary>
    /// Eine Hilfe um Benachrichtigungen am Handy zu erstellen, verändern oder zu löschen.
    /// </summary>
    public class NotificationHelper
    {
        public static string description;

        /// <summary>
        /// Erstellt eine Beschreibung für die Benachrichtigung.
        /// </summary>
        /// <param name="transaktion">Die Transaktion nach dem die Benachrichtigung erstellt werden soll.</param>
        public static void Description_Methode(Transaktion transaktion)
        {
            if (transaktion.Auftrags_Option == 1)
            {
                description = "Zweck: " + transaktion.Zweck + "\nBetrag: " + transaktion.Betrag + "€\nArt des Auftrages: " + transaktion.Art_an_Wiederholungen + "\nEinmaliger Auftrag\nSpeziell: " + transaktion.Speziell + "";
            }
            if (transaktion.Auftrags_Option == 2)
            {
                description = "Zweck: " + transaktion.Zweck + "\nBetrag: " + transaktion.Betrag + "€\nArt des Auftrages: " + transaktion.Art_an_Wiederholungen + "\nAnzahl der Wiederholungen: " + transaktion.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + transaktion.Speziell + "";
            }
            if (transaktion.Auftrags_Option == 3)
            {
                description = "Zweck: " + transaktion.Zweck + "\nBetrag: " + transaktion.Betrag + "€\nArt des Auftrages: " + transaktion.Art_an_Wiederholungen + "\nEnddatum: " + transaktion.Anzahl_an_Wiederholungen + "\nSpeziell: " + transaktion.Speziell + "";
            }
        }

        /// <summary>
        /// Erstellt eine Benachrichtigung im Handy nach bestimmten Anforderungen.
        /// </summary>
        /// <param name="auftrag">Der Auftrag nach dem die Benachrichtigung erstellt werden soll.</param>
        /// <param name="transaktion">Die Transaktion die für die Beschreibung der Benachrichtigung zuständig ist.</param>
        /// <returns>Wenn true, dann wurde die Benachrichtigung erfolgreich erstellt, falls false dann nicht.</returns>
        public static async Task<bool> RequestNotification(Auftrag auftrag, Transaktion transaktion)
        {
            try
            {
                Notification notification = new Notification() { Auftrags_ID = auftrag.Id };

                await NotificationService.Add_Notification(notification);

                notification.Notification_ID = notification.Id;

                await NotificationService.Edit_Notification(notification);

                Description_Methode(transaktion);

                var message = new NotificationRequest
                {
                    BadgeNumber = 1,

                    Title = "Der Auftrag mit der ID " + notification.Auftrags_ID + " endet heute",

                    Description = description,

                    NotificationId = notification.Notification_ID,

                    CategoryType = NotificationCategoryType.Reminder,

                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = transaktion.Datum.Date.AddHours(10),
                    }
                };

                await LocalNotificationCenter.Current.Show(message);

                return true;
            }
            catch { return false;}
        }

        /// <summary>
        /// Verändert einne bestehende Benachrichtigung im Handy nach bestimmten Anforderungen.
        /// </summary>
        /// <param name="transaktion">Die Transaktion die für die Beschreibung der Benachrichtigung zuständig ist.</param>
        /// <param name="option">Der Modifizierungungsprameter, Falls 0 dann wird die Benachrichtigung verändert, sonst wird sie gelöscht.</param>
        /// <returns>Wenn true, dann wurde die Benachrichtigung erfolgreich verändert, falls false dann nicht.</returns>
        public static async Task<bool> ModifyNotification(Transaktion transaktion , int option)
        {
            try
            {
                Notification notification = await NotificationService.Get_specific_Notification_with_Order_ID(int.Parse(transaktion.Auftrags_id.Substring(0,transaktion.Auftrags_id.IndexOf("."))));

                Description_Methode(transaktion);

                if(option == 0)
                {
                    var message = new NotificationRequest
                    {
                        BadgeNumber = 1,

                        Title = "Der Auftrag mit der ID " + notification.Auftrags_ID + " endet heute",

                        Description = description,

                        NotificationId = notification.Notification_ID,

                        CategoryType = NotificationCategoryType.Reminder,

                        Schedule = new NotificationRequestSchedule
                        {
                            NotifyTime = transaktion.Datum.Date.AddHours(10),
                        }
                    };

                    await LocalNotificationCenter.Current.Show(message);
                }
                else
                {
                    LocalNotificationCenter.Current.Cancel(notification.Notification_ID);
                }

                return true;
            }
            catch { return false; }
        }
    }
}
