using FreshMvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System;
using MvvmHelpers.Commands;
using Plugin.LocalNotification;
using System.Globalization;
using EasyLife.Services;
using EasyLife.Pages;
using Xamarin.Essentials;
using EasyLife.Interfaces;
using Xamarin.CommunityToolkit.Extensions;
using System.IO;

namespace EasyLife.PageModels
{
    public class Settings_PageModel : FreshBasePageModel
    {
        public Settings_PageModel() 
        {
            Notification_Command = new AsyncCommand(Notification_Methode);

            Styling_Color_Command = new AsyncCommand(Styling_Color_Methode);

            View_Appering_Command = new AsyncCommand(View_Appering_Methode);

            Create_Backup_Command = new AsyncCommand(Create_Backup_Methode);

            Restore_Backup_Command = new AsyncCommand(Restore_Backup_Methode);

            Last_Backup_Date = Preferences.Get("Last_Backup_Date","");

            Next_Backup_Date = Preferences.Get("Next_Backup_Date", "");

            Restored_Backup_Date = Preferences.Get("Restored_Backup_Date", "");
        }

        private async Task Create_Backup_Methode()
        {
            try
            {
                var result0 = await Shell.Current.DisplayAlert("Backup erstellen", "Wollen Sie wirklich ein Backup manuell erstellen?", "Ja", "Nein");
                
                if(result0 == true)
                {
                    bool result = BackupService.Create_Backup(DateTime.Now.ToString("dd.MM.yyyy"));

                    List<string> backup_list = new List<string>() { "Transaktionen", "Zwecken", "Benachrichtigungen", "Aufträgen", "Suchvorschlägen" };

                    if (result == false)
                    {
                        await Shell.Current.DisplayToastAsync("Es konnte kein Backup erstellt werden.", 5000);
                    }
                    else
                    {
                        Preferences.Set("Last_Backup_Date", DateTime.Now.ToString("dd.MM.yyyy"));

                        Preferences.Set("Next_Backup_Date", DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy"));

                        Last_Backup_Date = Preferences.Get("Last_Backup_Date", "");

                        Next_Backup_Date = Preferences.Get("Next_Backup_Date", "");


                        await Shell.Current.DisplayToastAsync("Es wurde erfolgreich ein Backup erstellt.", 5000);
                    }
                }
            }
            catch (Exception ex)
            {         
                if(BackupService.indicator == 1)
                {
                    try
                    {
                        BackupService.destinationStream = new System.IO.FileStream(Preferences.Get("Create_Backup_Path", ""), FileMode.OpenOrCreate, FileAccess.ReadWrite);

                        BackupService.helperStream.CopyTo(BackupService.destinationStream);

                        BackupService.helperStream.Close();

                        File.Delete(BackupService.helper_path);

                        BackupService.destinationStream.Close();
                    }
                    catch (Exception e)
                    {
                        await Shell.Current.DisplayAlert("Fehler", "Es ist bei der erstellung des Backups ein Fehler aufgetretten.\nFehler:" + e.ToString() + "", "Verstanden");
                    }
                }
                await Shell.Current.DisplayAlert("Fehler", "Es ist bei der erstellung des Backups ein Fehler aufgetretten.\nFehler:"+ex.ToString()+"", "Verstanden");
            }
        }

        private async Task Restore_Backup_Methode()
        {
            try
            {
                var result0 = await Shell.Current.DisplayAlert("Warnung", "Wenn Sie die Daten vom Backup nehmen gehen alle vorhandenen Daten die jetzt in der App sind verloren.", "Verstanden", "Zurück");

                if (result0 == true)
                {
                    var result1 = await Shell.Current.DisplayAlert("Warnung", "Sind Sie sich WIRKLICH SICHER, dass Sie die Appdaten mit dem letztem vorhandenem Backup überschreiben wollen.", "Ja", "Nein");

                    if (result1 == true)
                    {
                        var result2 = await BackupService.Restore_Backup();

                        if (result2 == 0)
                        {
                            await Shell.Current.DisplayToastAsync("Es wurde kein Backup gefunden.", 5000);
                        }
                        if (result2 == 1)
                        {
                            Last_Backup_Date = Preferences.Get("Last_Backup_Date", "");

                            Preferences.Set("Restored_Backup_Date", Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-") + 1).Substring(0, Preferences.Get("Restored_Backup_Path", "").Substring(Preferences.Get("Restored_Backup_Path", "").LastIndexOf("-")).LastIndexOf(".") - 1));

                            Restored_Backup_Date = Preferences.Get("Restored_Backup_Date", "");

                            await Shell.Current.DisplayToastAsync("Es wurden erfolgreich die Daten aus dem Backup vom " + Preferences.Get("Restored_Backup_Date", "") + " wiederherstellt.", 5000);
                        }
                        if (result2 == 2)
                        {
                            await Shell.Current.DisplayToastAsync("Es wurde kein Backup ausgewählt.", 5000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist bei der Wiederherstellung der Daten ein Fehler aufgetretten.\nFehler:"+ex.ToString()+"", "Verstanden");

                Preferences.Set("Restored_Backup_Path", "");

                BackupService.Delete_Restored_Source();
            }
        }

        private async Task View_Appering_Methode()
        {
            try
            {
                var result = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if(result == false)
                {
                    Is_Notification_Enable = "Nein";
                }
                else
                {
                    Is_Notification_Enable = "Ja";
                }

                var status1 = PermissionStatus.Denied;

                var status2 = PermissionStatus.Denied;

                if (DeviceInfo.Version.Major >= 11)
                {
                    status1 = await Permissions.CheckStatusAsync<Permissions.Media>();

                    status2 = await Permissions.CheckStatusAsync<Permissions.Photos>();
                }
                else
                {
                    status1 = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

                    status2 = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
                }

                if (status1 == PermissionStatus.Granted)
                {
                    Is_Read_Storage_Enable = "Ja";
                }

                if(status1 == PermissionStatus.Denied)
                {
                    Is_Read_Storage_Enable = "Nein";
                }

                if(status1 == PermissionStatus.Unknown)
                {
                    Is_Read_Storage_Enable = "Unbekannt";
                }


                if (status2 == PermissionStatus.Granted)
                {
                    if (Is_Write_Storage_Enable == "Nein")
                    {
                        Is_Write_Storage_Enable = "Ja";

                        await Shell.Current.DisplayToastAsync("Aufgrund von internen Äderungen bei der Berechtigung von EasyLife muss das Programm neugestartet werden.",5000);

                        DependencyService.Get<ICloseApplication>().closeApplication();
                    }

                    Is_Write_Storage_Enable = "Ja";
                }

                if (status2 == PermissionStatus.Denied)
                {
                    Is_Write_Storage_Enable = "Nein";
                }

                if (status2 == PermissionStatus.Unknown)
                {
                    Is_Write_Storage_Enable = "Unbekannt";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist bei der Wiederherstellung der Daten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Notification_Methode()
        {
            try
            {
                List<NotificationRequest> list_of_pending_notification = (List<NotificationRequest>)await LocalNotificationCenter.Current.GetPendingNotificationList();

                list_of_pending_notification = list_of_pending_notification.OrderBy(t => t.Schedule.NotifyTime).ToList();

                List<string> list_of_strings_of_pending_notification = new List<string>();

                foreach (NotificationRequest pn in list_of_pending_notification)
                {
                    DateTime time = (DateTime)pn.Schedule.NotifyTime;
                    list_of_strings_of_pending_notification.Add("ID:" + pn.NotificationId + "| " + time.ToString("dddd, d.M.yyyy,  H:mm", new CultureInfo("de-DE")) + "Uhr");
                }

                if (list_of_strings_of_pending_notification.Count() == 0)
                {
                    await Shell.Current.DisplayToastAsync("Es stehen keine Benachrichtigungen an.", 5000);
                }
                else
                {
                    try
                    {
                        string[] string_of_pending_notification = list_of_strings_of_pending_notification.ToArray();

                        string result = await Shell.Current.DisplayActionSheet("Anstehende Benachrichtigungen", "Zurück", null, string_of_pending_notification);

                        if (result != "Zurück")
                        {
                            result = result.Substring(3, -3 + result.IndexOf("|"));

                            Models.Notification notification = await NotificationService.Get_specific_Notification_with_Notification_ID(int.Parse(result));

                            DateTime time = (DateTime)list_of_pending_notification.Where(t => t.NotificationId == notification.Notification_ID).First().Schedule.NotifyTime;

                            await Shell.Current.DisplayAlert("Benachrichtigung " + result + "", "" + list_of_pending_notification.Where(t => t.NotificationId == notification.Notification_ID).First().Description + "\nDatum:" + time.ToString("dddd, d.M.yyyy,  H:mm", new CultureInfo("de-DE")) + "Uhr", "Zurück");
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task Styling_Color_Methode()
        {
            await Shell.Current.GoToAsync(nameof(Styling_Color_Page));
        }

        public AsyncCommand Styling_Color_Command { get;}
        public AsyncCommand Notification_Command { get;}
        public AsyncCommand View_Appering_Command { get; }
        public AsyncCommand Create_Backup_Command { get; }
        public AsyncCommand Restore_Backup_Command { get; }

        public string is_notification_enable;
        public string Is_Notification_Enable
        {
            get { return is_notification_enable; }
            set
            {
                if (Is_Notification_Enable == value)
                {
                    return;
                }

                is_notification_enable = value; RaisePropertyChanged();
            }
        }

        public string is_read_storage_enable;
        public string Is_Read_Storage_Enable
        {
            get { return is_read_storage_enable; }
            set
            {
                if (Is_Read_Storage_Enable == value)
                {
                    return;
                }

                is_read_storage_enable = value; RaisePropertyChanged();
            }
        }

        public string is_write_storage_enable;
        public string Is_Write_Storage_Enable
        {
            get { return is_write_storage_enable; }
            set
            {
                if (Is_Write_Storage_Enable == value)
                {
                    return;
                }

                is_write_storage_enable = value; RaisePropertyChanged();
            }
        }

        public string las_backup_date;
        public string Last_Backup_Date
        {
            get { return las_backup_date; }
            set
            {
                if (Last_Backup_Date == value)
                {
                    return;
                }

                las_backup_date = value; RaisePropertyChanged();
            }
        }

        public string next_backup_date;
        public string Next_Backup_Date
        {
            get { return next_backup_date; }
            set
            {
                if (Next_Backup_Date == value)
                {
                    return;
                }

                next_backup_date = value; RaisePropertyChanged();
            }
        }

        public string restored_backup_date;
        public string Restored_Backup_Date
        {
            get { return restored_backup_date; }
            set
            {
                if (Restored_Backup_Date == value)
                {
                    return;
                }

                restored_backup_date = value; RaisePropertyChanged();
            }
        }
    }
}
