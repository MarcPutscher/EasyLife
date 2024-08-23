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
using System.Text;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

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

            Load_Metadata_Command = new AsyncCommand(Load_Metadata);

            Load_KronosOverlay_Command = new AsyncCommand(Load_KronosOverlay);

            Last_Backup_Date = Preferences.Get("Last_Backup_Date", "");

            Next_Backup_Date = Preferences.Get("Next_Backup_Date", "");

            Restored_Backup_Date = Preferences.Get("Restored_Backup_Date", "");
        }

        public async Task Datanmanagment_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Datenmanagment", 340, new List<string>() { "Backup erstellen", "Backup senden", "Backup einsehen", "Daten wiederherstellen" }));

                if (result == null)
                {
                    return;
                }

                if ((string)result == "Backup erstellen")
                {
                    await Create_Backup_Methode();
                }
                if ((string)result == "Backup senden")
                {
                    await Share_Backup_Methode();
                }
                if ((string)result == "Daten wiederherstellen")
                {
                    await Restore_Backup_Methode();
                }
                if ((string)result == "Backup einsehen")
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
                var result0 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Backup erstellen", 340, 250, "Ja", "Nein", "Wollen Sie wirklich ein Backup manuell erstellen?"));

                if (result0 == null)
                {
                    return;
                }

                if ((bool)result0 == true)
                {
                    bool result = await BackupService.Create_Backup(DateTime.Now.ToString("dd.MM.yyyy"));

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
                if (BackupService.indicator == 1)
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
                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist bei der erstellung des Backups ein Fehler aufgetretten.\nFehler:" + e.ToString() + ""));
                    }
                }

                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist bei der erstellung des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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

                    var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Vorhandene Backups", 350, Backup_name));

                    if (result == null)
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
                            await Share.RequestAsync(new ShareFileRequest { Title = (string)result, File = new ShareFile(dict[(string)result]) });
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist beim Senden des Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Restore_Backup_Methode()
        {
            try
            {
                var result0 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Warnung", 350, 300, "Verstanden", "Zurück", "Wenn Sie die Daten vom Backup nehmen gehen alle vorhandenen Daten die jetzt in der App sind verloren."));

                if (result0 == null)
                {
                    return;
                }

                if ((bool)result0 == true)
                {
                    var result1 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Warnung", 350, 300, "Ja", "Nein", "Sind Sie sich WIRKLICH SICHER, dass Sie die Appdaten mit dem letztem vorhandenem Backup überschreiben wollen."));

                    if (result1 == null)
                    {
                        return;
                    }

                    if ((bool)result1 == true)
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist bei der Wiederherstellung der Daten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

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

                    while (indikator == false)
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Vorhandene Backups", 350, Backup_name));

                        if (result == null)
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
                                string result0string = (string)result;

                                List<List<object>> result1 = await BackupService.Show_Content_of_Backup(dict[result0string], DateTime.ParseExact(result0string.Substring(result0string.LastIndexOf("m") + 2), "dd.MM.yyyy", new CultureInfo("de-DE")));

                                if (result1 == null)
                                {
                                    await Shell.Current.DisplayToastAsync("Das Backup ist leer.", 5000);

                                    return;
                                }
                                else
                                {
                                    string[] content = { };

                                    List<string[]> contentlist = new List<string[]>() { };

                                    Dictionary<string, string> contentdic = new Dictionary<string, string> { };

                                    Dictionary<string, object> content1dic = new Dictionary<string, object> { };

                                    foreach (List<object> list in result1)
                                    {
                                        if (list[0].ToString() == "Transaktionen")
                                        {
                                            contentlist.Add(new string[] { list[0].ToString(), "erstellt: " + list[1] + " | gelöscht: " + list[2] + "\nStand : " + list[3] + "€\nStand des Briefumschlages: " + list[5] + "€" });

                                            contentdic.Add(list[0].ToString(), list[0].ToString());

                                            content1dic.Add(list[0].ToString(), list[4]);
                                        }
                                        else
                                        {
                                            if (list[0].ToString() == "Stylingprofile")
                                            {
                                                contentlist.Add(new string[] { list[0].ToString(), "erstellt: " + list[1] + "" });

                                                contentdic.Add(list[0].ToString(), list[0].ToString());

                                                content1dic.Add(list[0].ToString(), list[2]);
                                            }
                                            else
                                            {
                                                contentlist.Add(new string[] { list[0].ToString(), "erstellt: " + list[1] + " | gelöscht: " + list[2] + "" });

                                                contentdic.Add(list[0].ToString(), list[0].ToString());

                                                content1dic.Add(list[0].ToString(), list[3]);
                                            }
                                        }
                                    }

                                    bool indikator2 = false;

                                    while (indikator2 == false)
                                    {
                                        var result2 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Stand:" + result.ToString().Substring(result.ToString().IndexOf("m") + 1) + "", 350, contentlist));

                                        if (result2 == null)
                                        {
                                            indikator2 = true;
                                        }
                                        else
                                        {
                                            string result2string = (string)result2;

                                            if (result2string == "Zurück")
                                            {
                                                indikator2 = true;
                                            }
                                            else
                                            {
                                                if (contentdic[result2string] == "Transaktionen")
                                                {
                                                    string[] resultarray = { };

                                                    List<string[]> resultlist = new List<string[]>();

                                                    List<string> containstlist = new List<string>();


                                                    List<Transaktion> transaktionslist = (List<Transaktion>)content1dic[contentdic[result2string]];

                                                    if (transaktionslist.Count() != 0)
                                                    {
                                                        foreach (Transaktion trans in transaktionslist)
                                                        {
                                                            resultlist.Add(new string[] { "Transaktion " + trans.Id + "", "" + trans.Datumanzeige + "\n" + trans.Zweck + " | " + trans.Betrag + "€" });

                                                            containstlist.Add("Transaktion " + trans.Id + "");
                                                        }

                                                        bool indikator3 = false;

                                                        while (indikator3 == false)
                                                        {
                                                            var result3 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Transaktionen", 300, resultlist));

                                                            if (containstlist.Contains(result3) == true)
                                                            {
                                                                Transaktion item = transaktionslist[resultlist.IndexOf(resultlist[containstlist.IndexOf(result3)])];

                                                                string message = null;

                                                                if (String.IsNullOrEmpty(item.Auftrags_id) == false)
                                                                {
                                                                    if (item.Auftrags_Option == 1)
                                                                    {
                                                                        message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                                                                    }
                                                                    if (item.Auftrags_Option == 2)
                                                                    {
                                                                        message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                                                                    }
                                                                    if (item.Auftrags_Option == 3)
                                                                    {
                                                                        message = "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                                                                    }

                                                                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Transaktion " + item.Id + "", 350, 500, null, null, message));
                                                                }
                                                                else
                                                                {
                                                                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Transaktion " + item.Id + "", 350, 500, null, null, "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + ""));
                                                                }
                                                            }
                                                            else
                                                            {
                                                                indikator3 = true;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (contentdic[result2string] == "Aufträge")
                                                {
                                                    List<string[]> resultstring = new List<string[]>();

                                                    List<Auftrag> objektlist = (List<Auftrag>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Auftrag objekt in objektlist)
                                                        {
                                                            resultstring.Add(new string[] { "Auftrag " + objekt.Id + "", "Art: " + objekt.Art_an_Wiederholungen + "\nAnzahl: " + objekt.Anzahl_an_Wiederholungen + "\nSpeziell: " + objekt.Speziell + "" });
                                                        }

                                                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Aufträge", 350, 0, null, null, resultstring));
                                                    }
                                                }

                                                if (contentdic[result2string] == "Bilanzprofile")
                                                {
                                                    List<string> resultlist = new List<string>();

                                                    List<Balanceprofile> objektlist = (List<Balanceprofile>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Balanceprofile objekt in objektlist)
                                                        {
                                                            resultlist.Add("Bilanzprofil " + objekt.Id + "");
                                                        }

                                                        bool indikator3 = false;

                                                        while (indikator3 == false)
                                                        {
                                                            var result3 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Bilanzprofile", 260, resultlist));

                                                            if (result3 == null)
                                                            {
                                                                indikator3 = true;
                                                            }
                                                            else
                                                            {
                                                                string result3string = (string)result3;

                                                                if (result3string == "Zurück")
                                                                {
                                                                    indikator3 = true;
                                                                }
                                                                else
                                                                {
                                                                    int id = int.Parse(result3string.ToString().Substring(12));

                                                                    Balanceprofile ChoosenBilanceprofil = objektlist.Where(bp => bp.Id == id).First();

                                                                    List<string[]> resultstring = new List<string[]>();

                                                                    List<string> sortetinitreasons_substring = new List<string>();

                                                                    List<Stackholderhelper> sortetinitreasons = new List<Stackholderhelper>();

                                                                    foreach (string re in ChoosenBilanceprofil.Outcome_Account)
                                                                    {
                                                                        sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Konto" });
                                                                    }

                                                                    foreach (string re in ChoosenBilanceprofil.Income_Account)
                                                                    {
                                                                        sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Konto" });
                                                                    }

                                                                    foreach (string re in ChoosenBilanceprofil.Outcome_Cash)
                                                                    {
                                                                        sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Barausgaben" });
                                                                    }

                                                                    foreach (string re in ChoosenBilanceprofil.Income_Cash)
                                                                    {
                                                                        sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Bareinnahmen" });
                                                                    }

                                                                    foreach (string re in ChoosenBilanceprofil.Ignore)
                                                                    {
                                                                        sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                                                                    }

                                                                    foreach (Stackholderhelper sh in sortetinitreasons)
                                                                    {
                                                                        resultstring.Add(new string[] { sh.Reason.Substring(0, sh.Reason.LastIndexOf(":")) + " als " + sh.Reason.Substring(sh.Reason.LastIndexOf(":") + 1), "Gehört zu " + sh.Option + "" });
                                                                    }

                                                                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Bilanzprofil " + ChoosenBilanceprofil.Id + "", 390, 0, null, null, resultstring));
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (contentdic[result2string] == "Budgets")
                                                {
                                                    List<string> resultstring = new List<string>();

                                                    List<Budget> objektlist = (List<Budget>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Budget objekt in objektlist)
                                                        {
                                                            resultstring.Add("Budget " + objekt.Id + "\nName: " + objekt.Name + "\nZiel: " + objekt.Goal + "€");
                                                        }

                                                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Budget", 300, 0, null, null, resultstring));
                                                    }
                                                }

                                                if (contentdic[result2string] == "Zwecke")
                                                {
                                                    List<string[]> resultstring = new List<string[]>();

                                                    List<Zweck> objektlist = (List<Zweck>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Zweck objekt in objektlist)
                                                        {
                                                            resultstring.Add(new string[] { "Zweck " + objekt.Id + "", "" + objekt.Benutzerdefinierter_Zweck.Substring(0, objekt.Benutzerdefinierter_Zweck.IndexOf(":")) + " als " + objekt.Benutzerdefinierter_Zweck.Substring(objekt.Benutzerdefinierter_Zweck.IndexOf(":") + 1) + "\nHäufigkeit: " + objekt.Benutzerdefinierter_Prevalence });
                                                        }

                                                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Zwecke", 320, 0, null, null, resultstring));
                                                    }
                                                }

                                                if (contentdic[result2string] == "Suchbegriffe")
                                                {
                                                    List<string[]> resultstring = new List<string[]>();

                                                    List<Suggestion> objektlist = (List<Suggestion>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Suggestion objekt in objektlist)
                                                        {
                                                            resultstring.Add(new string[] { "Suchbegriff " + objekt.Id + "", "" + objekt.Suggestion_value + "" });
                                                        }

                                                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Suchbegriffe", 350, 0, null, null, resultstring));
                                                    }
                                                }

                                                if (contentdic[result2string] == "Benachrichtigungen")
                                                {
                                                    List<string[]> resultstring = new List<string[]>();

                                                    List<Notification> objektlist = (List<Notification>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Notification objekt in objektlist)
                                                        {
                                                            resultstring.Add(new string[] { "Benachrichtigung " + objekt.Id + "", "Zugehöriger Auftrag: " + objekt.Auftrags_ID + "" });
                                                        }

                                                        await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Benachrichtigungen", 350, 0, null, null, resultstring));
                                                    }
                                                }

                                                if (contentdic[result2string] == "Stylingprofile")
                                                {
                                                    List<string> resultlist = new List<string>();

                                                    List<Stylingprofile> objektlist = (List<Stylingprofile>)content1dic[contentdic[result2string]];

                                                    if (objektlist.Count() != 0)
                                                    {
                                                        foreach (Stylingprofile objekt in objektlist)
                                                        {
                                                            resultlist.Add("Stylingprofil " + objekt.Id + "");
                                                        }

                                                        bool indikator3 = false;

                                                        while (indikator3 == false)
                                                        {
                                                            var result3 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Stylingprofile", 280, resultlist));

                                                            if (result3 == null)
                                                            {
                                                                indikator3 = true;
                                                            }
                                                            else
                                                            {
                                                                string result3string = (string)result3;

                                                                if (result3string == "Zurück")
                                                                {
                                                                    indikator3 = true;
                                                                }
                                                                else
                                                                {
                                                                    int id = int.Parse(result3string.ToString().Substring(13));

                                                                    Stylingprofile ChoosenStylingprofil = objektlist.Where(bp => bp.Id == id).First();

                                                                    if (ChoosenStylingprofil != null)
                                                                    {
                                                                        await Shell.Current.ShowPopupAsync(new CustomeColorSheet_Popup("Stylingprofil " + ChoosenStylingprofil.Id + "", 390, ChoosenStylingprofil));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist bei der Vorschau der Backups ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task View_Appering_Methode()
        {
            try
            {
                var result = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if (result == false)
                {
                    Is_Notification_Enable = "Nein";
                }
                else
                {
                    Is_Notification_Enable = "Ja";
                }

                var status1 = PermissionStatus.Denied;

                var status2 = PermissionStatus.Denied;

                if (Xamarin.Essentials.DeviceInfo.Version.Major >= 11)
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

                if (status1 == PermissionStatus.Denied)
                {
                    Is_Read_Storage_Enable = "Nein";
                }

                if (status1 == PermissionStatus.Unknown)
                {
                    Is_Read_Storage_Enable = "Unbekannt";
                }


                if (status2 == PermissionStatus.Granted)
                {
                    if (Is_Write_Storage_Enable == "Nein")
                    {
                        Is_Write_Storage_Enable = "Ja";

                        await Shell.Current.DisplayToastAsync("Aufgrund von internen Äderungen bei der Berechtigung von EasyLife muss das Programm neugestartet werden.", 5000);

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

        private async Task Load_Metadata()
        {
            try
            {
                List<int> amount_list = new List<int>();

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

                amount_list.Add(amount_of_generated_transaktion);

                amount_list.Add(amount_of_delete_transaktion);

                amount_list.Add(amount_of_use_transaktion);

                amount_list.Add(amount_of_unuse_transaktion);

                amount_list.Add(amount_of_transaktion_in_orders);


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

                amount_list.Add(amount_of_generated_order);

                amount_list.Add(amount_of_delete_order);

                amount_list.Add(amount_of_use_order);

                amount_list.Add(amount_of_unuse_order);


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

                amount_list.Add(amount_of_generated_reason);

                amount_list.Add(amount_of_use_reason);

                amount_list.Add(amount_of_unuse_reason);


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

                amount_list.Add(amount_of_generated_notification);

                amount_list.Add(amount_of_delete_notification);

                amount_list.Add(amount_of_use_notification);

                amount_list.Add(amount_of_unuse_notification);


                var suggestioncontent = await SearchSuggestionService.Get_all_Suggestion();

                int amount_of_generated_suggestion = 0;

                int amount_of_deleted_suggestion = 0;

                int amount_of_use_suggestionn = 0;

                if (suggestioncontent.Count() != 0)
                {
                    amount_of_generated_suggestion = suggestioncontent.First().Id;

                    amount_of_use_suggestionn = suggestioncontent.Count();

                    amount_of_deleted_suggestion = suggestioncontent.First().Id - suggestioncontent.Count();
                }

                amount_list.Add(amount_of_generated_suggestion);

                amount_list.Add(amount_of_deleted_suggestion);

                amount_list.Add(amount_of_use_suggestionn);


                var balanceprofilecontent = await BalanceService.Get_all_Balanceprofile();

                int amount_of_generated_balanceprofile = 0;

                int amount_of_deleted_balanceprofile = 0;

                int amount_of_use_balanceprofile = 0;

                if (balanceprofilecontent.Count() != 0)
                {
                    amount_of_generated_balanceprofile = balanceprofilecontent.First().Id;

                    amount_of_use_balanceprofile = balanceprofilecontent.Count();

                    amount_of_deleted_balanceprofile = balanceprofilecontent.First().Id - balanceprofilecontent.Count();
                }

                amount_list.Add(amount_of_generated_balanceprofile);

                amount_list.Add(amount_of_deleted_balanceprofile);

                amount_list.Add(amount_of_use_balanceprofile);


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

                amount_list.Add(amount_of_generated_budgets);

                amount_list.Add(amount_of_deleted_budgets);

                amount_list.Add(amount_of_use_budgets);

                var stylingprofilecontent = await StylingService.Get_all_Stylingprofile();

                int amount_of_generated_stylingprofile = 0;

                if (stylingprofilecontent.Count() != 0)
                {
                    amount_of_generated_stylingprofile = stylingprofilecontent.First().Id;
                }

                amount_list.Add(amount_of_generated_stylingprofile);

                await Shell.Current.ShowPopupAsync(new MetaData_Popup(amount_list));

            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist beim Erstellen der Metadaten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Load_KronosOverlay()
        {
            try
            {
                await Shell.Current.ShowPopupAsync(new KronosOverlay_Popup());
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist beim Erstellen der Metadaten ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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
                    list_of_strings_of_pending_notification.Add("ID:" + pn.NotificationId + "| " + time.ToString("dddd, d.M.yyyy, H:mm", new CultureInfo("de-DE")) + "Uhr");
                }

                if (list_of_strings_of_pending_notification.Count() == 0)
                {
                    await Shell.Current.DisplayToastAsync("Es stehen keine Benachrichtigungen an.", 5000);
                }
                else
                {
                    try
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Benachrichtigungen", 400, list_of_strings_of_pending_notification));

                        if (result != null)
                        {
                            string resultstring = (string)result;

                            resultstring = resultstring.Substring(3, -3 + resultstring.IndexOf("|"));

                            Models.Notification notification = await NotificationService.Get_specific_Notification_with_Notification_ID(int.Parse(resultstring));

                            DateTime time = (DateTime)list_of_pending_notification.Where(t => t.NotificationId == notification.Notification_ID).First().Schedule.NotifyTime;

                            await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Benachrichtigung " + resultstring + "", 400, 400, null, null, list_of_pending_notification.Where(t => t.NotificationId == notification.Notification_ID).First().Description + "\nDatum:" + time.ToString("dddd, d.M.yyyy,  H:mm", new CultureInfo("de-DE")) + "Uhr"));
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Styling_Color_Methode()
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Styling_Color_Page));
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public AsyncCommand Styling_Color_Command { get; }
        public AsyncCommand Notification_Command { get; }
        public AsyncCommand View_Appering_Command { get; }
        public AsyncCommand Datanmanagment_Command { get; }
        public AsyncCommand Load_Metadata_Command { get; }
        public AsyncCommand Load_KronosOverlay_Command { get; }


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