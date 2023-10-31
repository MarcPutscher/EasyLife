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
using EasyLife.Models;

namespace EasyLife.PageModels
{
    public class Settings_PageModel : FreshBasePageModel
    {
        public Settings_PageModel() 
        {
            Notification_Command = new AsyncCommand(Notification_Methode);

            Styling_Color_Command = new AsyncCommand(Styling_Color_Methode);

            View_Appering_Command = new AsyncCommand(View_Appering_Methode);

            Datanmanagment_Command = new AsyncCommand(Datanmanagment_Methode);

            Last_Backup_Date = Preferences.Get("Last_Backup_Date","");

            Next_Backup_Date = Preferences.Get("Next_Backup_Date", "");

            Restored_Backup_Date = Preferences.Get("Restored_Backup_Date", "");
        }

        public async Task Datanmanagment_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Datamanagment_Popup());

                if (result == null)
                {
                    return;
                }

                if((int)result == 1)
                {
                    await Create_Backup_Methode();
                }
                if((int)result == 2)
                {
                    await Share_Backup_Methode();
                }
                if((int)result == 3)
                {
                    await Restore_Backup_Methode();
                }
                if ((int)result == 4)
                {
                    await Show_Content_of_Backup_Methode();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist beim Senden des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
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

        public async Task Share_Backup_Methode()
        {
            try
            {
                string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFile(null), "EasyLife-Backup-*");

                if (files.Count() != 0)
                {
                    List<DateTime> Backup_dates = new List<DateTime>();

                    List<string> Backup_name = new List<string>();


                    string[] Button_name = null;

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

                    Button_name = Backup_name.ToArray();

                    string result = await Shell.Current.DisplayActionSheet("Vorhandene Backups", "Zurück", null, Button_name);

                    if (result == "Zurück")
                    {
                        return;
                    }
                    else
                    {
                        if (result == null)
                        {
                            await Shell.Current.DisplayToastAsync("Es wurde kein Backup ausgewählt.", 5000);
                        }
                        else
                        {
                            await Share.RequestAsync(new ShareFileRequest { Title = result , File = new ShareFile(dict[result]) });
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayToastAsync("Es wurde kein Backup gefunden.", 5000);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist beim Senden des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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

                            await Load_Metadata();
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

        private async Task Show_Content_of_Backup_Methode()
        {
            try
            {
                string[] files = Directory.GetFiles(DependencyService.Get<IAccessFile>().CreateFile(null), "EasyLife-Backup-*");

                if (files.Count() != 0)
                {
                    List<DateTime> Backup_dates = new List<DateTime>();

                    List<string> Backup_name = new List<string>();


                    string[] Button_name = null;

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

                    Button_name = Backup_name.ToArray();

                    bool indikator = false;

                    while(indikator == false)
                    {
                        string result = await Shell.Current.DisplayActionSheet("Vorhandene Backups", "Zurück", null, Button_name);

                        if (result == "Zurück")
                        {
                            indikator = true;
                        }
                        else
                        {
                            if (result == null)
                            {
                                await Shell.Current.DisplayToastAsync("Es wurde kein Backup ausgewählt.", 5000);
                            }
                            else
                            {
                                List<List<object>> result1 = await BackupService.Show_Content_of_Backup(dict[result]);

                                if (result1 == null)
                                {
                                    await Shell.Current.DisplayToastAsync("Das Backup ist leer.", 5000);

                                    return;
                                }
                                else
                                {
                                    string content = string.Empty;

                                    foreach (List<object> list in result1)
                                    {
                                        content += list[0].ToString() + "\nerstellt: " + list[1] + "| gelöscht: " + list[2] + "\n";
                                    }

                                    await Shell.Current.DisplayAlert("Stand: " + result + "", content, "Zurück");
                                }
                            }
                        }
                    }
                }
                else
                {
                    await Shell.Current.DisplayToastAsync("Es wurde kein Backup gefunden.", 5000);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist beim Senden des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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

                await Load_Metadata();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist bei der Wiederherstellung der Daten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Load_Metadata()
        {
            try
            {
                var transaktionscontent = await ContentService.Get_all_Transaktion();

                int amount_of_generated_transaktion = 0;

                int amount_of_delete_transaktion = 0;

                int amount_of_use_transaktion = 0;

                int amount_of_unuse_transaktion = 0;

                int amount_of_transaktion_in_orders = 0;

                if (transaktionscontent.Count() != 0)
                {
                    amount_of_generated_transaktion = transaktionscontent.Last().Id;

                    amount_of_delete_transaktion = amount_of_generated_transaktion - transaktionscontent.Count();

                    foreach (var trans in transaktionscontent)
                    {
                        if (trans.Auftrags_id != null)
                        {
                            amount_of_transaktion_in_orders++;
                        }

                        if (trans.Content_Visibility == true)
                        {
                            amount_of_use_transaktion++;
                        }
                        else
                        {
                            amount_of_unuse_transaktion++;
                        }
                    }
                }

                Amount_of_generated_Transaktion = amount_of_generated_transaktion;

                Amount_of_deleted_Transaktion = amount_of_delete_transaktion;

                Amount_of_use_Transaktion = amount_of_use_transaktion;

                Amount_of_unuse_Transaktion = amount_of_unuse_transaktion;

                Amount_of_Transaktion_in_Orders = amount_of_transaktion_in_orders;


                var ordercontent = await OrderService.Get_all_Order();

                int amount_of_generated_order = 0;

                int amount_of_delete_order = 0;

                int amount_of_use_order = 0;

                int amount_of_unuse_order = 0;

                if (ordercontent.Count() != 0)
                {
                    amount_of_generated_order = ordercontent.Last().Id;

                    amount_of_delete_order = amount_of_generated_order - ordercontent.Count();

                    foreach (var order in ordercontent)
                    {
                        bool inuse = false;

                        foreach (var trans in transaktionscontent)
                        {
                            if (trans.Auftrags_id != null)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == order.Id.ToString())
                                {
                                    inuse = true;
                                }
                            }
                        }

                        if (inuse == true)
                        {
                            amount_of_use_order++;
                        }
                        else
                        {
                            amount_of_unuse_order++;
                        }
                    }
                }

                Amount_of_generated_Order = amount_of_generated_order;

                Amount_of_deleted_Order = amount_of_delete_order;

                Amount_of_use_Order = amount_of_use_order;

                Amount_of_unuse_Order = amount_of_unuse_order;


                var reasoncontent = await ReasonService.Get_all_Reason();

                int amount_of_generated_reason = 0;

                int amount_of_use_reason = 0;

                int amount_of_unuse_reason = 0;

                if (reasoncontent.Count() != 0)
                {
                    amount_of_generated_reason = reasoncontent.First().Id;

                    foreach (var reason in reasoncontent)
                    {
                        if (reason.Reason_Visibility == true)
                        {
                            amount_of_use_reason++;
                        }
                        else
                        {
                            amount_of_unuse_reason++;
                        }
                    }
                }

                Amount_of_generated_Reason = amount_of_generated_reason;

                Amount_of_use_Reason = amount_of_use_reason;

                Amount_of_unuse_Reason = amount_of_unuse_reason;


                var notificationcontent = await NotificationService.Get_all_Notification();

                List<NotificationRequest> list_of_delivered_notification = (List<NotificationRequest>)await LocalNotificationCenter.Current.GetDeliveredNotificationList();

                List<NotificationRequest> list_of_pending_notification = (List<NotificationRequest>)await LocalNotificationCenter.Current.GetPendingNotificationList();

                int amount_of_generated_notification = 0;

                int amount_of_delete_notification = 0;

                int amount_of_use_notification = 0;

                int amount_of_unuse_notification = 0;

                if (notificationcontent.Count() != 0)
                {
                    amount_of_generated_notification = notificationcontent.First().Id;

                    foreach (var notification in notificationcontent)
                    {
                        bool isused = false;

                        foreach (Auftrag order in ordercontent)
                        {
                            if (order.Id == notification.Auftrags_ID)
                            {
                                isused = true;
                            }
                        }

                        if (isused == false)
                        {
                            amount_of_delete_notification++;
                        }
                    }

                    amount_of_unuse_notification = amount_of_generated_notification - list_of_pending_notification.Count() - amount_of_delete_notification;

                    amount_of_use_notification = list_of_pending_notification.Count();
                }

                Amount_of_generated_Notification = amount_of_generated_notification;

                Amount_of_deleted_Notification = amount_of_delete_notification;

                Amount_of_use_Notification = amount_of_use_notification;

                Amount_of_unuse_Notification = amount_of_unuse_notification;


                var budgetscontent = await BudgetService.Get_all_Budget();

                int amount_of_generated_budgets = 0;

                int amount_of_deleted_budgets = 0;

                int amount_of_use_budgets = 0;

                if (budgetscontent.Count() != 0)
                {
                    amount_of_generated_budgets = budgetscontent.First().Id;

                    foreach (var budget in budgetscontent)
                    {
                        amount_of_use_budgets++;
                    }

                    amount_of_deleted_budgets = amount_of_generated_budgets - amount_of_use_budgets;
                }

                Amount_of_generated_Budgets = amount_of_generated_budgets;

                Amount_of_deleted_Budgets = amount_of_deleted_budgets;

                Amount_of_use_Budgets = amount_of_use_budgets;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist beim Erstellen der Metadaten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
        public AsyncCommand Datanmanagment_Command { get; }

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


        public int amount_of_generated_transaktion = 0;
        public int Amount_of_generated_Transaktion
        {
            get { return amount_of_generated_transaktion; }
            set
            {
                if (Amount_of_generated_Transaktion == value)
                {
                    return;
                }

                amount_of_generated_transaktion = value; RaisePropertyChanged();
            }
        }

        public int amount_of_use_transaktion = 0;
        public int Amount_of_use_Transaktion
        {
            get { return amount_of_use_transaktion; }
            set
            {
                if (Amount_of_use_Transaktion == value)
                {
                    return;
                }

                amount_of_use_transaktion = value; RaisePropertyChanged();
            }
        }

        public int amount_of_unuse_transaktion = 0;
        public int Amount_of_unuse_Transaktion
        {
            get { return amount_of_unuse_transaktion; }
            set
            {
                if (Amount_of_unuse_Transaktion == value)
                {
                    return;
                }

                amount_of_unuse_transaktion = value; RaisePropertyChanged();
            }
        }

        public int amount_of_transaktion_in_orders = 0;
        public int Amount_of_Transaktion_in_Orders
        {
            get { return amount_of_transaktion_in_orders; }
            set
            {
                if (Amount_of_Transaktion_in_Orders == value)
                {
                    return;
                }

                amount_of_transaktion_in_orders = value; RaisePropertyChanged();
            }
        }

        public int amount_of_deleted_transaktion = 0;
        public int Amount_of_deleted_Transaktion
        {
            get { return amount_of_deleted_transaktion; }
            set
            {
                if (Amount_of_deleted_Transaktion == value)
                {
                    return;
                }

                amount_of_deleted_transaktion = value; RaisePropertyChanged();
            }
        }

        public int amount_of_generated_order = 0;
        public int Amount_of_generated_Order
        {
            get { return amount_of_generated_order; }
            set
            {
                if (Amount_of_generated_Order == value)
                {
                    return;
                }

                amount_of_generated_order = value; RaisePropertyChanged();
            }
        }

        public int amount_of_use_order = 0;
        public int Amount_of_use_Order
        {
            get { return amount_of_use_order; }
            set
            {
                if (Amount_of_use_Order == value)
                {
                    return;
                }

                amount_of_use_order = value; RaisePropertyChanged();
            }
        }

        public int amount_of_unuse_order = 0;
        public int Amount_of_unuse_Order
        {
            get { return amount_of_unuse_order; }
            set
            {
                if (Amount_of_unuse_Order == value)
                {
                    return;
                }

                amount_of_unuse_order = value; RaisePropertyChanged();
            }
        }

        public int amount_of_deleted_order = 0;
        public int Amount_of_deleted_Order
        {
            get { return amount_of_deleted_order; }
            set
            {
                if (Amount_of_deleted_Order == value)
                {
                    return;
                }

                amount_of_deleted_order = value; RaisePropertyChanged();
            }
        }

        public int amount_of_generated_reason = 0;
        public int Amount_of_generated_Reason
        {
            get { return amount_of_generated_reason; }
            set
            {
                if (Amount_of_generated_Reason == value)
                {
                    return;
                }

                amount_of_generated_reason = value; RaisePropertyChanged();
            }
        }

        public int amount_of_use_reason = 0;
        public int Amount_of_use_Reason
        {
            get { return amount_of_use_reason; }
            set
            {
                if (Amount_of_use_Reason == value)
                {
                    return;
                }

                amount_of_use_reason = value; RaisePropertyChanged();
            }
        }

        public int amount_of_unuse_reason = 0;
        public int Amount_of_unuse_Reason
        {
            get { return amount_of_unuse_reason; }
            set
            {
                if (Amount_of_unuse_Reason == value)
                {
                    return;
                }

                amount_of_unuse_reason = value; RaisePropertyChanged();
            }
        }

        public int amount_of_generated_notification = 0;
        public int Amount_of_generated_Notification
        {
            get { return amount_of_generated_notification; }
            set
            {
                if (Amount_of_generated_Notification == value)
                {
                    return;
                }

                amount_of_generated_notification = value; RaisePropertyChanged();
            }
        }

        public int amount_of_deleted_notification = 0;
        public int Amount_of_deleted_Notification
        {
            get { return amount_of_deleted_notification; }
            set
            {
                if (Amount_of_deleted_Notification == value)
                {
                    return;
                }

                amount_of_deleted_notification = value; RaisePropertyChanged();
            }
        }

        public int amount_of_use_notification = 0;
        public int Amount_of_use_Notification
        {
            get { return amount_of_use_notification; }
            set
            {
                if (Amount_of_use_Notification == value)
                {
                    return;
                }

                amount_of_use_notification = value; RaisePropertyChanged();
            }
        }

        public int amount_of_unuse_notification = 0;
        public int Amount_of_unuse_Notification
        {
            get { return amount_of_unuse_notification; }
            set
            {
                if (Amount_of_unuse_Notification == value)
                {
                    return;
                }

                amount_of_unuse_notification = value; RaisePropertyChanged();
            }
        }

        public int amount_of_generated_budgets = 0;
        public int Amount_of_generated_Budgets
        {
            get { return amount_of_generated_budgets; }
            set
            {
                if (Amount_of_generated_Budgets == value)
                {
                    return;
                }

                amount_of_generated_budgets = value; RaisePropertyChanged();
            }
        }

        public int amount_of_deleted_budgets = 0;
        public int Amount_of_deleted_Budgets
        {
            get { return amount_of_deleted_budgets; }
            set
            {
                if (Amount_of_deleted_Budgets == value)
                {
                    return;
                }

                amount_of_deleted_budgets = value; RaisePropertyChanged();
            }
        }

        public int amount_of_use_budgets = 0;
        public int Amount_of_use_Budgets
        {
            get { return amount_of_use_budgets; }
            set
            {
                if (Amount_of_use_Budgets == value)
                {
                    return;
                }

                amount_of_use_budgets = value; RaisePropertyChanged();
            }
        }
    }
}
