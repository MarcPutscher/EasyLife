using EasyLife.Helpers;
using EasyLife.Interfaces;
using EasyLife.Models;
using EasyLife.Pages;
using Microsoft.Extensions.DependencyModel;
using Plugin.LocalNotification;
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
using Xamarin.CommunityToolkit.Extensions;
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

        public static string source_path = Path.Combine(FileSystem.AppDataDirectory, "EasyLife.db");

        public static string placeholder_path = Path.Combine(FileSystem.AppDataDirectory, "Placeholder.db");

        public static string helper_path = Path.Combine(DependencyService.Get<IAccessFile>().CreateFileDocuments("EasyLife-HelperBackup.db"));

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        public static void Init_Source()
        {
            if (db_create != null)
                return;

            db_create = new SQLiteAsyncConnection(source_path);
        }

        /// <summary>
        /// Erstellt eine Verbindung zur Datenbank her.
        /// </summary>
        public static void Init_Placeholder()
        {
            if (db_create != null)
                return;

            db_create = new SQLiteAsyncConnection(placeholder_path);
        }

        /// <summary>
        /// Löscht das älteste Backup wenn schon zwölf unterschiedliche Backups vorhanden sind, falls welche vorhanden sind. 
        /// </summary>
        /// <param name="current_backup_path">Das aktuellste Backup was nicht gelöscht werden soll.</param>
        public static void Delet_oldest_Backup(string current_backup_path)
        {
            if (indicator == 1)
            {
                //  Fast alle Dateiname in dem Ordner wo sich die Backups befinden her, die mit EasyLife-Backup- beginnen.

                string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFileDocuments(""), "EasyLife-Backup-*");

                //  Löscht das älteste Backup wenn schon zwölf Backups vorhanden sind. 

                if (files.Count() >= 12)
                {
                    List<DateTime> Backupdates = new List<DateTime>();

                    Dictionary<DateTime, string> dict = new Dictionary<DateTime, string>();

                    foreach (string file in files)
                    {
                        Backupdates.Add(DateTime.ParseExact(file.Substring(file.LastIndexOf("-") + 1).Substring(0, file.Substring(file.LastIndexOf("-")).LastIndexOf(".") - 1), "dd.MM.yyyy", new CultureInfo("de-DE")));

                        dict.Add(Backupdates.Last(), file);
                    }

                    Backupdates = Backupdates.OrderByDescending(d => d.Date).ToList();

                    Backupdates.Reverse();

                    while (Backupdates.Count() > 12)
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
            if (indicator == 1)
            {
                if (File.Exists(source_path))
                {
                    File.Delete(source_path);
                }
            }
        }

        /// <summary>
        /// Erstellt ein Backup von der Datenbank in der App.
        /// </summary>
        /// <param name="backup_date">Das Datum an dem das Backup erstellt wurde.</param>
        /// <returns>Wenn true, wurde erfolgreich ein Backup erstellt, falls false dann nicht. </returns>
        public static async Task<bool> Create_Backup(string backup_date)
        {
            //Falls eine Datenbankverbindung noch geöffnet ist wird sie hier geschlossen.

            await Create_AppPreferences();

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

            Preferences.Set("Create_Backup_Path", DependencyService.Get<IAccessFile>().CreateFileDocuments("EasyLife-Backup-" + backup_date + ".db"));

            Init_Source();


            indicator = 0;

            //  Falls dieser Pfad nicht existiert, dann soll ein Filestream von der Datenbank geöffnet werden und ein Filestream vom Backup erstellt werden.
            //  Danach soll die Datenbank zum Backup kopiert werden und alle Filestreams geschlossen werden.
            //  Am Ende werden alle älteren Backups in dem Ordner gelöschen.(um auszuschließen, dass bei der Wiederherstellung ein altes Backup genommen werden kann.)

            if (!File.Exists(Preferences.Get("Create_Backup_Path", "")))
            {
                sourceStream = new FileStream(source_path, FileMode.Open, FileAccess.ReadWrite);

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

            sourceStream = new FileStream(source_path, FileMode.Open, FileAccess.ReadWrite);

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

            string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFileDocuments(null), "EasyLife-Backup-*");

            List<DateTime> Backup_dates = new List<DateTime>();

            if (files.Count() != 0)
            {
                List<string> Backup_name = new List<string>();

                Dictionary<DateTime, string> dict_0 = new Dictionary<DateTime, string>();

                Dictionary<string, string> dict = new Dictionary<string, string>();

                foreach (string file in files)
                {
                    try
                    {
                        Backup_dates.Add(DateTime.ParseExact(file.Substring(file.LastIndexOf("-") + 1).Substring(0, file.Substring(file.LastIndexOf("-")).LastIndexOf(".") - 1), "dd.MM.yyyy", new CultureInfo("de-DE")));

                        dict_0.Add(Backup_dates.Last(), file);
                    }
                    catch { }
                }

                Backup_dates = Backup_dates.OrderByDescending(d => d.Date).ToList();

                Backup_dates.Reverse();

                foreach (DateTime time in Backup_dates)
                {
                    Backup_name.Add("Backup vom " + time.ToString("dd.MM.yyyy") + "");

                    dict.Add(Backup_name.Last(), dict_0[time]);
                }

                Backup_name.Reverse();

                var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Vorhandene Backups", 380, Backup_name));

                if (result == null)
                {
                    { return 2; }
                }
                else
                {
                    if (result == null)
                    {
                        { return 0; }
                    }
                    else
                    {
                        Preferences.Set("Restored_Backup_Path", dict[(string)result]);
                    }
                }

            }
            else
            { return 0; }

            //  Falls der Pfad der Datenbank nicht existiert, dann soll ein Filestream von der Datenbank erstellt werden und ein Filestream vom Backup geöffnet werden.
            //  Danach soll das Backup zur Datenbank kopiert werden und alle Filestreams geschlossen werden.
            //  Danach wird die Datenbankverbinndung auf null gesetzt und der Indikator auf 1.
            //  Am Ende werden alle Benachrichtigungen die noch anstehen können wiederhergestellt und das Datum des letzten Backups, falls dieses leer seien sollte, erstellt.

            if (!File.Exists(source_path))
            {
                sourceStream = new FileStream(Preferences.Get("Restored_Backup_Path", ""), FileMode.Open, FileAccess.ReadWrite);

                destinationStream = new FileStream(source_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                sourceStream.CopyTo(destinationStream);

                destinationStream.Close();

                sourceStream.Close();

                indicator = 1;

                db_create = null;

                await Restore_AppPreferences();

                await Reset_Notification();

                await Reset_Notification();

                await Patch_Reason(DateTime.ParseExact(Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-") + 1).Substring(0, Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-")).LastIndexOf(".") - 1), "dd.MM.yyyy", new CultureInfo("de-DE")));

                return 1;
            }

            sourceStream = new FileStream(Preferences.Get("Restored_Backup_Path", ""), FileMode.Open, FileAccess.ReadWrite);

            File.Delete(source_path);

            destinationStream = new FileStream(source_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            sourceStream.CopyTo(destinationStream);

            destinationStream.Close();

            sourceStream.Close();

            indicator = 1;

            db_create = null;

            await Restore_AppPreferences();

            await Reset_Notification();

            await Reset_Notification();

            await Patch_Reason(DateTime.ParseExact(Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-") + 1).Substring(0, Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-")).LastIndexOf(".") - 1), "dd.MM.yyyy", new CultureInfo("de-DE")));

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

            LocalNotificationCenter.Current.CancelAll();

            List<Notification> notifications_list = await db_create.Table<Notification>().ToListAsync();

            List<Transaktion> transaktions_with_order_list = new List<Transaktion>();

            List<Transaktion> transaktions_of_an_spezific_order = new List<Transaktion>();

            List<Transaktion> last_transaktion_of_an_order_list = new List<Transaktion>();

            foreach (Transaktion trans in transaktion_list)
            {
                if (String.IsNullOrEmpty(trans.Auftrags_id) == false)
                {
                    if (trans.Order_Visibility == true)
                    {
                        transaktions_with_order_list.Add(trans);
                    }
                }
            }

            if (transaktions_with_order_list.Count() != 0)
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

        /// <summary>
        /// Erstellt, aus den wiederhergestellt Daten, alle App-Preferences wieder her. 
        /// </summary>
        /// <returns></returns>
        public static async Task Restore_AppPreferences()
        {
            Init_Source();

            try
            {
                await db_create.CreateTableAsync<AppPreferences>();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist beim Senden des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }

            List<AppPreferences> AppPreferences_list = new List<AppPreferences>(await db_create.Table<AppPreferences>().ToListAsync());

            List<string> Preferences_List_Bool = new List<string>()
            {
                "Is_Saldo_Date_Changed",
                "Search_For_Transaktion_ID",
                "Search_For_Auftrags_ID",
                "Search_For_Datum",
                "Search_For_Zweck",
                "Search_For_Notiz",
                "Search_For_Betrag",
                "Quersuche",
                "Filter_Activity",
            };

            List<string> Preferences_List_Other = new List<string>()
            {
                "Saldo_Date",
                "Blanceprofile",
                "Stylingprofil",
                "Transaktion_per_load"
            };

            Dictionary<string, string> Colordic = new Dictionary<string, string>()
            {
                //Popup
                { "Vordergrund_Cancel_Popup", "#710117" },
                { "Hintergrund_Cancel_Popup", Color.Black.ToHex()},
                { "Text_Titel_Popup", "#cbcdcb"},
                { "Hintergrund_Content_Popup", "#0b1c48"},
                { "Rand_Content_Popup", "#2b6ad0"},
                { "Text_Content_Popup", "#ecd5bb"},
                { "Subtext_Content_Popup", "#ecd5bb"},
                { "Hintertgrund_Button_Popup", "#0b1c48"},
                { "Rand_Button_Popup", "#2b6ad0"},
                { "Text_Button_Popup", "#138b83"},
                { "Aktiv_Schalter_Popup", Color.ForestGreen.ToHex()},
                { "Deaktiv_Schalter_Popup", Color.DarkRed.ToHex()},

                //Home
                { "Hintergrund_Home", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Bearbeiten_Home", Color.Green.ToHex()},
                { "Rand_Bearbeiten_Home", Color.DarkGreen.ToHex()},
                { "Text_Bearbeiten_Home", Color.White.ToHex()},
                { "Hintergrund_Löschen_Home", Color.Red.ToHex()},
                { "Rand_Löschen_Home", Color.DarkRed.ToHex()},
                { "Text_Löschen_Home", Color.White.ToHex()},
                { "Hintergrund_Transaktion_Home", Color.Gray.ToHex()},
                { "Rand_Transaktion_Home", Color.DarkGray.ToHex()},
                { "Text_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Detail_Transaktion_Home", Color.LightGray.ToHex()},
                { "Text_Detail_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Positiver_Betrag_Transaktion_Home", Color.MediumSeaGreen.ToHex()},
                { "Rand_Positiver_Betrag_Transaktion_Home", Color.ForestGreen.ToHex()},
                { "Text_Positiver_Betrag_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Negativer_Betrag_Transaktion_Home", Color.Salmon.ToHex()},
                { "Rand_Negativer_Betrag_Transaktion_Home", Color.Red.ToHex()},
                { "Text_Negativer_Betrag_Transaktion_Home", Color.Black.ToHex()},
                { "Text_MehrLaden_Home", Color.Black.ToHex()},
                { "Vordergrund_Budget_Home", Color.SaddleBrown.ToHex()},
                { "Hintergrund_Saldo_Home", Color.Black.ToHex()},
                { "Text_Saldo_Home", Color.White.ToHex()},
                { "Vordergrund_Hinzufügen_Home", Color.Orange.ToHex()},

                //Hinzufügen/Bearbeiten
                { "Hintergrund_Hinzufügen", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Eingabefeld_Hinzufügen", Color.Orange.ToHex()},
                { "Rand_Eingabefeld_Hinzufügen", Color.Coral.ToHex()},
                { "Title_Eingabefeld_Hinzufügen", Color.White.ToHex()},
                { "Text_Eingabefeld_Hinzufügen", Color.Black.ToHex()},
                { "Platzhater_Eingabefeld_Hinzufügen", "#525252"},
                { "Aktiv_Schalter_Eingabefeld_Hinzufügen", Color.ForestGreen.ToHex()},
                { "Deaktiv_Schalter_Eingabefeld_Hinzufügen", Color.DarkRed.ToHex()},
                { "Hintergrund_Wahlfeld_Hinzufügen", Color.Orange.ToHex()},
                { "Haubttext_Wahlfeld_Hinzufügen", Color.Black.ToHex()},
                { "Nebentext_Wahlfeld_Hinzufügen", Color.Black.ToHex()},
                { "Hintergrund_Option_Wahlfeld_Hinzufügen", Color.SkyBlue.ToHex()},
                { "Rand_Option_Wahlfeld_Hinzufügen", Color.Blue.ToHex()},
                { "Text_Option_Wahlfeld_Hinzufügen", Color.OrangeRed.ToHex()},
                { "Hintergrund_Schalter_Hinzufügen", Color.SkyBlue.ToHex()},
                { "Rand_Schalter_Hinzufügen", Color.Blue.ToHex()},
                { "Text_Schalter_Hinzufügen", Color.OrangeRed.ToHex()},

                //Bilanz
                { "Hintergrund_Bilanz", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Kopfzeile_Bilanz", Color.DarkGray.ToHex()},
                { "Text_Kopfzeile_Bilanz", Color.Black.ToHex()},
                { "Hintergrund_Fußzeile_Bilanz", Color.DarkGray.ToHex()},
                { "Text_Fußzeile_Bilanz", Color.Black.ToHex()},
                { "Text_Zusammenfassung_Bilanz", Color.Black.ToHex()},
                { "Text_Zweck_Stack_Bilanz", Color.White.ToHex()},
                { "Text_Anzahl_Stack_Bilanz", Color.White.ToHex()}
            };

            if (AppPreferences_list.Count() != 0)
            {
                Dictionary<string, string> preferences = AppPreferences_Konverter.Deserilize(AppPreferences_list.First());

                foreach (string ap in preferences.Keys)
                {
                    if (Preferences.ContainsKey(ap) == true)
                    {
                        if (Preferences_List_Other.Contains(ap) == true)
                        {
                            if (ap == "Saldo_Date")
                            {
                                Preferences.Set(ap, preferences[ap]);
                            }
                            if (ap == "Blanceprofile" || ap == "Stylingprofil")
                            {
                                Preferences.Set(ap, int.Parse(preferences[ap]));
                            }
                            if (ap == "Transaktion_per_load")
                            {
                                Preferences.Set(ap, double.Parse(preferences[ap]));
                            }
                        }
                        if (Preferences_List_Bool.Contains(ap) == true)
                        {
                            Preferences.Set(ap, bool.Parse(preferences[ap]));
                        }
                        if (Colordic.ContainsKey(ap) == true)
                        {
                            App.Current.Resources[ap] = preferences[ap];

                            Preferences.Set(ap, preferences[ap]);
                        }
                    }
                }
            }

            await db_create.CloseAsync();
        }

        /// <summary>
        /// Erstellt ein App-Preferences für das Backup. 
        /// </summary>
        /// <returns></returns>
        public static async Task Create_AppPreferences()
        {
            Dictionary<string, string> Current_Preferences = new Dictionary<string, string>();

            try
            {
                Current_Preferences.Add("Is_Saldo_Date_Changed", Preferences.Get("Is_Saldo_Date_Changed", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Transaktion_ID", Preferences.Get("Search_For_Transaktion_ID", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Auftrags_ID", Preferences.Get("Search_For_Auftrags_ID", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Datum", Preferences.Get("Search_For_Datum", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Zweck", Preferences.Get("Search_For_Zweck", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Notiz", Preferences.Get("Search_For_Notiz", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Search_For_Betrag", Preferences.Get("Search_For_Betrag", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Quersuche", Preferences.Get("Quersuche", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Filter_Activity", Preferences.Get("Filter_Activity", false).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Saldo_Date", Preferences.Get("Saldo_Date", DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))));
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Blanceprofile", Preferences.Get("Blanceprofile", 0).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Stylingprofil", Preferences.Get("Stylingprofil", -1).ToString());
            }
            catch
            { }

            try
            {
                Current_Preferences.Add("Transaktion_per_load", Preferences.Get("Transaktion_per_load", 20.0).ToString());
            }
            catch
            { }

            Dictionary<string, string> Colordic = new Dictionary<string, string>()
            {
                //Popup
                { "Vordergrund_Cancel_Popup", "#710117" },
                { "Hintergrund_Cancel_Popup", Color.Black.ToHex()},
                { "Text_Titel_Popup", "#cbcdcb"},
                { "Hintergrund_Content_Popup", "#0b1c48"},
                { "Rand_Content_Popup", "#2b6ad0"},
                { "Text_Content_Popup", "#ecd5bb"},
                { "Subtext_Content_Popup", "#ecd5bb"},
                { "Hintertgrund_Button_Popup", "#0b1c48"},
                { "Rand_Button_Popup", "#2b6ad0"},
                { "Text_Button_Popup", "#138b83"},
                { "Aktiv_Schalter_Popup", Color.ForestGreen.ToHex()},
                { "Deaktiv_Schalter_Popup", Color.DarkRed.ToHex()},

                //Home
                { "Hintergrund_Home", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Bearbeiten_Home", Color.Green.ToHex()},
                { "Rand_Bearbeiten_Home", Color.DarkGreen.ToHex()},
                { "Text_Bearbeiten_Home", Color.White.ToHex()},
                { "Hintergrund_Löschen_Home", Color.Red.ToHex()},
                { "Rand_Löschen_Home", Color.DarkRed.ToHex()},
                { "Text_Löschen_Home", Color.White.ToHex()},
                { "Hintergrund_Transaktion_Home", Color.Gray.ToHex()},
                { "Rand_Transaktion_Home", Color.DarkGray.ToHex()},
                { "Text_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Detail_Transaktion_Home", Color.LightGray.ToHex()},
                { "Text_Detail_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Positiver_Betrag_Transaktion_Home", Color.MediumSeaGreen.ToHex()},
                { "Rand_Positiver_Betrag_Transaktion_Home", Color.ForestGreen.ToHex()},
                { "Text_Positiver_Betrag_Transaktion_Home", Color.Black.ToHex()},
                { "Hintergrund_Negativer_Betrag_Transaktion_Home", Color.Salmon.ToHex()},
                { "Rand_Negativer_Betrag_Transaktion_Home", Color.Red.ToHex()},
                { "Text_Negativer_Betrag_Transaktion_Home", Color.Black.ToHex()},
                { "Text_MehrLaden_Home", Color.Black.ToHex()},
                { "Vordergrund_Budget_Home", Color.SaddleBrown.ToHex()},
                { "Hintergrund_Saldo_Home", Color.Black.ToHex()},
                { "Text_Saldo_Home", Color.White.ToHex()},
                { "Vordergrund_Hinzufügen_Home", Color.Orange.ToHex()},

                //Hinzufügen/Bearbeiten
                { "Hintergrund_Hinzufügen", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Eingabefeld_Hinzufügen", Color.Orange.ToHex()},
                { "Rand_Eingabefeld_Hinzufügen", Color.Coral.ToHex()},
                { "Title_Eingabefeld_Hinzufügen", Color.White.ToHex()},
                { "Text_Eingabefeld_Hinzufügen", Color.Black.ToHex()},
                { "Platzhater_Eingabefeld_Hinzufügen", "#525252"},
                { "Aktiv_Schalter_Eingabefeld_Hinzufügen", Color.ForestGreen.ToHex()},
                { "Deaktiv_Schalter_Eingabefeld_Hinzufügen", Color.DarkRed.ToHex()},
                { "Hintergrund_Wahlfeld_Hinzufügen", Color.Orange.ToHex()},
                { "Haubttext_Wahlfeld_Hinzufügen", Color.Black.ToHex()},
                { "Nebentext_Wahlfeld_Hinzufügen", Color.Black.ToHex()},
                { "Hintergrund_Option_Wahlfeld_Hinzufügen", Color.SkyBlue.ToHex()},
                { "Rand_Option_Wahlfeld_Hinzufügen", Color.Blue.ToHex()},
                { "Text_Option_Wahlfeld_Hinzufügen", Color.OrangeRed.ToHex()},
                { "Hintergrund_Schalter_Hinzufügen", Color.SkyBlue.ToHex()},
                { "Rand_Schalter_Hinzufügen", Color.Blue.ToHex()},
                { "Text_Schalter_Hinzufügen", Color.OrangeRed.ToHex()},

                //Bilanz
                { "Hintergrund_Bilanz", Color.DarkSlateGray.ToHex()},
                { "Hintergrund_Kopfzeile_Bilanz", Color.DarkGray.ToHex()},
                { "Text_Kopfzeile_Bilanz", Color.Black.ToHex()},
                { "Hintergrund_Fußzeile_Bilanz", Color.DarkGray.ToHex()},
                { "Text_Fußzeile_Bilanz", Color.Black.ToHex()},
                { "Text_Zusammenfassung_Bilanz", Color.Black.ToHex()},
                { "Text_Zweck_Stack_Bilanz", Color.White.ToHex()},
                { "Text_Anzahl_Stack_Bilanz", Color.White.ToHex()}
            };

            try
            {
                foreach (string colorname in Colordic.Keys)
                {
                    Current_Preferences.Add(colorname, Preferences.Get(colorname, Colordic[colorname]));
                }
            }
            catch
            { }

            if (Current_Preferences.Count() == 0)
            {
                return;
            }

            AppPreferences New_AppPreferences = new AppPreferences() { Preferences = AppPreferences_Konverter.Serilize(Current_Preferences) };

            List<AppPreferences> AppPreferences_list = await AppPreferencesService.Get_all_AppPreferences();

            if (AppPreferences_list.Count() != 0)
            {
                New_AppPreferences.Id = AppPreferences_list.First().Id;

                await AppPreferencesService.Edit_AppPreferences(New_AppPreferences);
            }
            else
            {
                await AppPreferencesService.Add_AppPreferences(New_AppPreferences);
            }
        }

        /// <summary>
        /// Erstellt eine Vorschau zu einem Backup. 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<List<object>>> Show_Content_of_Backup(string path, DateTime saldo_date)
        {
            List<List<object>> Content = new List<List<object>>();

            if (!File.Exists(placeholder_path))
            {
                sourceStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

                destinationStream = new FileStream(placeholder_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                sourceStream.CopyTo(destinationStream);

                destinationStream.Close();

                sourceStream.Close();

                indicator = 1;

                db_create = null;

            }
            else
            {
                sourceStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);

                File.Delete(placeholder_path);

                destinationStream = new FileStream(placeholder_path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                sourceStream.CopyTo(destinationStream);

                destinationStream.Close();

                sourceStream.Close();

                indicator = 1;

                db_create = null;
            }

            Init_Placeholder();

            await db_create.CreateTableAsync<Transaktion>();

            List<Transaktion> transaktion_list = new List<Transaktion>(await db_create.Table<Transaktion>().ToListAsync());

            if (transaktion_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Transaktionen",
                    0,
                    0,
                    0,
                    transaktion_list,
                    0
                });
            }
            else
            {
                double saldo = 0;

                double letter = 0;

                foreach (var trans in transaktion_list)
                {
                    if (trans.Content_Visibility == true)
                    {
                        if (DateTime.Compare(trans.Datum, saldo_date.AddDays(1).AddSeconds(-1)) <= 0)
                        {
                            if (trans.Saldo_Visibility == true)
                            {
                                saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                            }
                            else
                            {
                                letter += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                            }
                        }
                    }
                }

                Content.Add(new List<object>()
                {
                    "Transaktionen",
                    transaktion_list.Last().Id,
                    transaktion_list.Last().Id - transaktion_list.Count(),
                    Math.Round(saldo,2),
                    transaktion_list,
                    Math.Round(letter,2)
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Auftrag>();

            List<Auftrag> order_list = new List<Auftrag>(await db_create.Table<Auftrag>().ToListAsync());

            if (order_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Aufträge",
                    0,
                    0,
                    order_list
                });
            }
            else
            {
                Content.Add(new List<object>()
                {
                    "Aufträge",
                    order_list.Last().Id,
                    order_list.Last().Id - order_list.Count(),
                    order_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Notification>();

            List<Notification> notification_list = new List<Notification>(await db_create.Table<Notification>().ToListAsync());

            if (notification_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Benachrichtigungen",
                    0,
                    0,
                    notification_list
                });
            }
            else
            {
                Content.Add(new List<object>()
                {
                    "Benachrichtigungen",
                    notification_list.Last().Id,
                    notification_list.Last().Id - notification_list.Count(),
                    notification_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<HelperBalanceprofile>();

            List<HelperBalanceprofile> helperbalanceprofile_list = new List<HelperBalanceprofile>(await db_create.Table<HelperBalanceprofile>().ToListAsync());

            List<Balanceprofile> balanceprofiles_list = new List<Balanceprofile>();

            if (helperbalanceprofile_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Bilanzprofile",
                    0,
                    0,
                    balanceprofiles_list
                });
            }
            else
            {
                foreach (HelperBalanceprofile helperBalanceprofile in helperbalanceprofile_list)
                {
                    balanceprofiles_list.Add(Balanceprofile_Konverter.Deserilize(helperBalanceprofile));
                }

                Content.Add(new List<object>()
                {
                    "Bilanzprofile",
                    helperbalanceprofile_list.Last().Id,
                    helperbalanceprofile_list.Last().Id - helperbalanceprofile_list.Count(),
                    balanceprofiles_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Budget>();

            List<Budget> budget_list = new List<Budget>(await db_create.Table<Budget>().ToListAsync());

            if (budget_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Budgets",
                    0,
                    0,
                    budget_list
                });
            }
            else
            {
                Content.Add(new List<object>()
                {
                    "Budgets",
                    budget_list.Last().Id,
                    budget_list.Last().Id - budget_list.Count(),
                    budget_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Zweck>();

            List<Zweck> reason_list = new List<Zweck>(await db_create.Table<Zweck>().ToListAsync());

            if (reason_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Zwecke",
                    0,
                    0,
                    reason_list
                });
            }
            else
            {
                List<object> objekt = Content[0].ToList();

                List<Transaktion> transaktion = objekt[4] as List<Transaktion>;

                List<Transaktion> transaktions_with_order_list = new List<Transaktion>();

                List<Transaktion> transaktions_of_an_spezific_order = new List<Transaktion>();

                List<Transaktion> last_transaktion_of_an_order_list = new List<Transaktion>();

                foreach (Transaktion trans in transaktion_list)
                {
                    if (String.IsNullOrEmpty(trans.Auftrags_id) == false)
                    {
                        if (trans.Order_Visibility == true)
                        {
                            transaktions_with_order_list.Add(trans);
                        }
                    }
                    else
                    {
                        reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence = reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence + 1;
                    }
                }

                if (transaktions_with_order_list.Count() != 0)
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
                        reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence = reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence + 1;
                    }
                }

                Content.Add(new List<object>()
                {
                    "Zwecke",
                    reason_list.Last().Id,
                    reason_list.Last().Id - reason_list.Count(),
                    reason_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Suggestion>();

            List<Suggestion> suggestion_list = new List<Suggestion>(await db_create.Table<Suggestion>().ToListAsync());

            if (suggestion_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Suchbegriffe",
                    0,
                    0,
                    suggestion_list
                });
            }
            else
            {
                Content.Add(new List<object>()
                {
                    "Suchbegriffe",
                    suggestion_list.Last().Id,
                    suggestion_list.Last().Id - suggestion_list.Count(),
                    suggestion_list
                });
            }

            await db_create.CloseAsync();

            Init_Placeholder();

            await db_create.CreateTableAsync<Stylingprofile>();

            List<Stylingprofile> stylingprofile_list = new List<Stylingprofile>(await db_create.Table<Stylingprofile>().ToListAsync());

            if (stylingprofile_list.Count() == 0)
            {
                Content.Add(new List<object>()
                {
                    "Stylingprofile",
                    0,
                    stylingprofile_list
                });
            }
            else
            {
                Content.Add(new List<object>()
                {
                    "Stylingprofile",
                    stylingprofile_list.Last().Id,
                    stylingprofile_list
                });
            }

            await db_create.CloseAsync();

            return Content;
        }

        /// <summary>
        /// Patch die Häufigkeit zu den Zwecken. 
        /// </summary>
        public static async Task Patch_Reason(DateTime date)
        {
            if (DateTime.Compare(date, DateTime.Now) > 0)
            {
                return;
            }

            Init_Source();

            await db_create.CreateTableAsync<Transaktion>();

            List<Transaktion> transaktion_list = new List<Transaktion>(await db_create.Table<Transaktion>().ToListAsync());

            await db_create.CloseAsync();

            await db_create.CreateTableAsync<Zweck>();

            List<Zweck> Reason_list = new List<Zweck>(await db_create.Table<Zweck>().ToListAsync());

            List<Transaktion> transaktions_with_order_list = new List<Transaktion>();

            List<Transaktion> transaktions_of_an_spezific_order = new List<Transaktion>();

            List<Transaktion> last_transaktion_of_an_order_list = new List<Transaktion>();

            foreach (Transaktion trans in transaktion_list)
            {
                if (String.IsNullOrEmpty(trans.Auftrags_id) == false)
                {
                    if (trans.Order_Visibility == true)
                    {
                        transaktions_with_order_list.Add(trans);
                    }
                }
                else
                {
                    Reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence = Reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence + 1;
                }
            }

            if (transaktions_with_order_list.Count() != 0)
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
                    Reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence = Reason_list.Where(zw => zw.Benutzerdefinierter_Zweck.Substring(0, zw.Benutzerdefinierter_Zweck.IndexOf(":")) == trans.Zweck).First().Benutzerdefinierter_Prevalence + 1;
                }
            }

            foreach (Zweck reason in Reason_list)
            {
                await db_create.UpdateAsync(reason);
            }
            await db_create.CloseAsync();
        }
    }
}