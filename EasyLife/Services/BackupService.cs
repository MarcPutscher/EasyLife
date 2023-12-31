﻿using EasyLife.Helpers;
using EasyLife.Interfaces;
using EasyLife.Models;
using Microsoft.Extensions.DependencyModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static iText.Svg.SvgConstants;

namespace EasyLife.Services
{
    /// <summary>
    /// Ist eine Servicellasse zur Erstellung von Backups und Wiederherstellung aus Backups.
    /// </summary>
    public class BackupService
    {
        public static FileStream sourceStream;

        public static FileStream destinationStream;

        public static FileStream helperStream;

        public static SQLiteAsyncConnection db_create;

        public static int indicator = 0;

        public static string sourch_path = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

        public static string helper_path = Path.Combine(DependencyService.Get<IAccessFile>().CreateFile("EasyLife-HelperBackup.db"));

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        public static void Init_Source()
        {
            if (db_create != null)
                return;

            db_create = new SQLiteAsyncConnection(sourch_path);
        }

        /// <summary>
        /// Löscht das älteste Backup wenn schon zwölf unterschiedliche Backups vorhanden sind, falls welche vorhanden sind. 
        /// </summary>
        /// <param name="current_backup_path">Das aktuellste Backup was nicht gelöscht werden soll.</param>
        public static void Delet_oldest_Backup(string current_backup_path)
        {
            if(indicator == 1)
            {
                //  Fast alle Dateiname in dem Ordner wo sich die Backups befinden her, die mit EasyLife-Backup- beginnen.

                string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFile(""), "EasyLife-Backup-*");

                //  Löscht das älteste Backup wenn schon zwölf Backups vorhanden sind. 

                if (files.Count() >= 12)
                {
                    List<DateTime> Backupdates = new List<DateTime>();

                    Dictionary<DateTime, string> dict = new Dictionary<DateTime, string>();

                    foreach (string file in files)
                    {
                        Backupdates.Add(DateTime.ParseExact(file.Substring(file.LastIndexOf("-") + 1).Substring(0, file.Substring(file.LastIndexOf("-")).LastIndexOf(".") - 1) , "dd.MM.yyyy" , new CultureInfo("de-DE")));

                        dict.Add(Backupdates.Last(),file);
                    }

                    Backupdates = Backupdates.OrderByDescending(d => d.Date).ToList();

                    Backupdates.Reverse();

                    while(Backupdates.Count() > 12)
                    {
                        File.Delete(dict[Backupdates.First()]);

                        Backupdates.RemoveAt(0);
                    }
                }
            }
        }

        /// <summary>
        /// Löscht die Datenbank in der App. 
        /// </summary>
        public static void Delete_Restored_Source() 
        {
            if(indicator == 1)
            {
                if (File.Exists(sourch_path))
                {
                    File.Delete(sourch_path);
                }
            }
        }

        /// <summary>
        /// Erstellt ein Backup von der Datenbank in der App.
        /// </summary>
        /// <param name="backup_date">Das Datum an dem das Backup erstellt wurde.</param>
        /// <returns>Wenn true, wurde erfolgreich ein Backup erstellt, falls false dann nicht. </returns>
        public static bool Create_Backup(string backup_date)
        {
            //Falls eine Datenbankverbindung noch geöffnet ist wird sie hier geschlossen.

            try
            {
                destinationStream.Close();
            }
            catch { }

            try
            {
                sourceStream.Close();
            }
            catch { }

            try
            {
                helperStream.Close();
            }
            catch { }

            //  Setzt den Backuppfad für das Backup fest und speichert es in den Appsettings.

            Preferences.Set("Create_Backup_Path", DependencyService.Get<IAccessFile>().CreateFile("EasyLife-Backup-" + backup_date + ".db"));

            Init_Source();


            indicator = 0;

            //  Falls dieser Pfad nicht existiert, dann soll ein Filestream von der Datenbank geöffnet werden und ein Filestream vom Backup erstellt werden.
            //  Danach soll die Datenbank zum Backup kopiert werden und alle Filestreams geschlossen werden.
            //  Am Ende werden alle älteren Backups in dem Ordner gelöschen.(um auszuschließen, dass bei der Wiederherstellung ein altes Backup genommen werden kann.)

            if (!File.Exists(Preferences.Get("Create_Backup_Path", "")))
            {
                sourceStream = new FileStream(sourch_path, FileMode.Open, FileAccess.ReadWrite);

                destinationStream = new FileStream(Preferences.Get("Create_Backup_Path", ""), FileMode.OpenOrCreate, FileAccess.ReadWrite);

                sourceStream.CopyTo(destinationStream);

                destinationStream.Close();

                sourceStream.Close();

                Delet_oldest_Backup(Preferences.Get("Create_Backup_Path", ""));

                return true;
            }

            //  Falls dieser Pfad existiert, dann soll ein Filestream von der Datenbank geöffnet werden, ein Filestream vom Backup geöffnet werden, ein Filestream vom Helper erstellt werden, das Backup zum Helper kopiert werden,  die Backupdatei gelöscht werden und ein Filestream vom Backup erstellt werden.
            //  Danach soll die Datenbank zum Backup kopiert werden und alle Filestreams geschlossen werden und der Indikator auf 1 gesetzt.
            //  Am Ende werden alle älteren Backups in dem Ordner gelöschet und die Helperdatei gelöscht. (um auszuschließen, dass bei der Wiederherstellung ein altes Backup genommen werden kann.)

            indicator = 1;

            sourceStream = new FileStream(sourch_path, FileMode.Open, FileAccess.ReadWrite);

            destinationStream = new FileStream(Preferences.Get("Create_Backup_Path", ""), FileMode.OpenOrCreate, FileAccess.ReadWrite);

            helperStream = new FileStream(helper_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            destinationStream.CopyTo(helperStream);

            destinationStream.Close();

            File.Delete(Preferences.Get("Create_Backup_Path", ""));

            destinationStream = new FileStream(Preferences.Get("Create_Backup_Path", ""), FileMode.OpenOrCreate, FileAccess.ReadWrite);

            sourceStream.CopyTo(destinationStream);

            destinationStream.Close();

            sourceStream.Close();

            Delet_oldest_Backup(Preferences.Get("Create_Backup_Path", ""));

            File.Delete(BackupService.helper_path);

            return true;
        }

        /// <summary>
        /// Stellt aus dem aktuellstem Backup die Daten in der App wieder her.
        /// </summary>
        /// <returns>Wenn 1, wurden erfolgreich alle Daten aus dem aktuellstem Backup wiederhergestellt, falls 0 dann nicht, falls 2 dann wurde kein Backup ausgewählt und auf zurück gedrückt. </returns>
        public static async Task<int> Restore_Backup()
        {
            //Falls eine Datenbankverbindung noch geöffnet ist wird sie hier geschlossen.
            try
            {
                destinationStream.Close();
            }
            catch { }

            try
            {
                sourceStream.Close();
            }
            catch { }

            try
            {
                helperStream.Close();
            }
            catch { }

            //  Der Indikator wird benutzt um sicherzustellen, dass die Datenbank löscht wird wenn ein Fehler bei dem Copieren der Daten entsteht und davor.

            indicator = 0;

            //  Falls Backups vorhanden sind wird eine Fenster erstellt wo man sich das Backup aussuchen kann welches geladen werden soll.
            //  Falls nicht dann wird diese Funktion geschlossen.

            string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFile(null), "EasyLife-Backup-*");

            List<DateTime> Backup_dates = new List<DateTime>();

            if (files.Count()!=0)
            {
                List<string> Backup_name = new List<string>();


                string[] Button_name = null;

                Dictionary<DateTime, string> dict_0 = new Dictionary<DateTime, string>();

                Dictionary<string, string> dict = new Dictionary<string, string>();

                foreach(string file in files)
                {
                    Backup_dates.Add(DateTime.ParseExact(file.Substring(file.LastIndexOf("-") + 1).Substring(0, file.Substring(file.LastIndexOf("-")).LastIndexOf(".") - 1), "dd.MM.yyyy", new CultureInfo("de-DE")));

                    dict_0.Add(Backup_dates.Last(), file);
                }

                Backup_dates = Backup_dates.OrderByDescending(d => d.Date).ToList();

                Backup_dates.Reverse();

                foreach (DateTime time in Backup_dates)
                {
                    Backup_name.Add("Backup vom "+ time.ToString("dd.MM.yyyy") +"");

                    dict.Add(Backup_name.Last(), dict_0[time]);
                }

                Backup_name.Reverse();

                Button_name = Backup_name.ToArray();

                string result = await Shell.Current.DisplayActionSheet("Vorhande Backups", "Zurück", null, Button_name);

                if (result == "Zurück")
                {
                    { return 2; }
                }
                else
                {
                    if(result == null)
                    {
                        { return 0; }
                    }
                    else
                    {
                        Preferences.Set("Restored_Backup_Path", dict[result]);
                    }
                }

            }
            else
            { return 0; }

            //  Falls der Pfad der Datenbank nicht existiert, dann soll ein Filestream von der Datenbank erstellt werden und ein Filestream vom Backup geöffnet werden.
            //  Danach soll das Backup zur Datenbank kopiert werden und alle Filestreams geschlossen werden.
            //  Danach wird die Datenbankverbinndung auf null gesetzt und der Indikator auf 1.
            //  Am Ende werden alle Benachrichtigungen die noch anstehen können wiederhergestellt und das Datum des letzten Backups, falls dieses leer seien sollte, erstellt.

            if (!File.Exists(sourch_path))
            {
                sourceStream = new FileStream(Preferences.Get("Restored_Backup_Path", ""), FileMode.Open, FileAccess.ReadWrite);

                destinationStream = new FileStream(sourch_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                sourceStream.CopyTo(destinationStream);

                destinationStream.Close();

                sourceStream.Close();

                indicator = 1;

                db_create = null;

                await Reset_Notification();

                return 1;
            }

            sourceStream = new FileStream(Preferences.Get("Restored_Backup_Path", ""), FileMode.Open, FileAccess.ReadWrite);

            File.Delete(sourch_path);

            destinationStream = new FileStream(sourch_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            sourceStream.CopyTo(destinationStream);

            destinationStream.Close();

            sourceStream.Close();

            indicator = 1;

            db_create = null;

            await Reset_Notification();

            return 1;
        }

        /// <summary>
        /// Erstellt, aus den wiederhergestellt Daten, alle Benachrichtigungen wieder her. 
        /// </summary>
        /// <returns></returns>
        public static async Task Reset_Notification()
        {
            Init_Source();

            await db_create.CreateTableAsync<Transaktion>();

            List<Transaktion> transaktion_list = new List<Transaktion>(await db_create.Table<Transaktion>().ToListAsync());

            await db_create.CloseAsync();

            Init_Source();

            await db_create.CreateTableAsync<Notification>();

            List<Notification> notifications_list = await db_create.Table<Notification>().ToListAsync();

            List<Transaktion> transaktions_with_order_list = new List<Transaktion>();

            List<Transaktion> transaktions_of_an_spezific_order = new List<Transaktion>();

            List<Transaktion> last_transaktion_of_an_order_list = new List<Transaktion>();

            foreach(Transaktion trans in transaktion_list)
            {
                if(String.IsNullOrEmpty(trans.Auftrags_id) == false)
                {
                    if(trans.Order_Visibility == true)
                    {
                        transaktions_with_order_list.Add(trans);
                    }
                }
            }

            if(transaktions_with_order_list.Count() != 0)
            {
                if (transaktions_with_order_list.Count() == 1)
                {
                    await NotificationHelper.ModifyNotification(transaktions_with_order_list[0], 0);
                }
                else
                {
                    string order_id = transaktions_with_order_list.First().Auftrags_id.Substring(0, transaktions_with_order_list.First().Auftrags_id.IndexOf("."));

                    foreach (Transaktion trans in transaktions_with_order_list)
                    {
                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == order_id)
                        {
                            transaktions_of_an_spezific_order.Add(trans);
                        }
                        else
                        {
                            last_transaktion_of_an_order_list.Add(transaktions_of_an_spezific_order.OrderBy(d => d.Datum).ToList().Last());

                            transaktions_of_an_spezific_order.Clear();

                            order_id = trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."));

                            transaktions_of_an_spezific_order.Add(trans);
                        }
                    }

                    last_transaktion_of_an_order_list.Add(transaktions_of_an_spezific_order.OrderBy(d => d.Datum).ToList().Last());

                    foreach (Transaktion trans in last_transaktion_of_an_order_list)
                    {
                        await NotificationHelper.ModifyNotification(trans, 0);
                    }
                }
            }

            await db_create.CloseAsync();
        }
    }
}
