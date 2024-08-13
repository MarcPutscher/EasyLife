using EasyLife.Models;
using EasyLife.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using System;
using EasyLife.Pages;
using System.Globalization;
using System.Collections.Generic;
using FreshMvvm;
using Xamarin.CommunityToolkit.Extensions;
using Plugin.LocalNotification;
using System.Diagnostics;
using EasyLife.Helpers;
using Xamarin.Essentials;
using EasyLife.Interfaces;
using System.IO;
using PermissionStatus = Xamarin.Essentials.PermissionStatus;
using Xamarin.Forms.Internals;
using System.ComponentModel;
using static SQLite.SQLite3;
using System.Transactions;

namespace EasyLife.PageModels
{
    public class Home_PageModel : FreshBasePageModel
    {
        public Home_PageModel()
        {
            Transaktion = new ObservableRangeCollection<Transaktion>();

            SuggestionCollection = new ObservableRangeCollection<Suggestion>();

            transaktionGroups = new ObservableRangeCollection<Grouping<string, Transaktion>>();

            RefreshCommand = new AsyncCommand(Refresh);
            ViewIsDisappearing_Command = new Command(ViewIsDisappearing_Methode);
            RemoveCommand = new AsyncCommand<Transaktion>(Remove);
            EditCommand = new AsyncCommand<Transaktion>(Edit);
            GroupingOption_Command = new AsyncCommand(GroupingOption_Methode);
            Search_Command = new AsyncCommand(Search_Methode);
            Search_Command2 = new AsyncCommand(Search_Methode2);
            Delet_Suggestion = new AsyncCommand<Suggestion>(Delet_Suggestion_Methode);
            Select_Suggestion = new AsyncCommand<Suggestion>(Select_Suggestion_Methode);
            Clear_SearchText_Command = new AsyncCommand(Clear_SearchText_Methode);
            Set_Searchbar_Visibility_Command = new AsyncCommand(Set_Searchbar_Visibility_MethodeAsync);
            The_Searchbar_is_Tapped = new AsyncCommand(The_Searchbar_is_Tapped_Methode);
            Add_Catchphrase_To_Search_Command = new AsyncCommand(Add_Catchphrase_To_Search_Methode);
            Add_Command = new AsyncCommand(Add_Methode);
            Filter_Command = new AsyncCommand(Filter_Methode);
            Calculator_Addition_Command = new AsyncCommand<Transaktion>(Calculator_Addition_Methode);
            Calculator_Substraction_Command = new AsyncCommand<Transaktion>(Calculator_Substraction_Methode);
            ShowCalculator_Command = new Command(ShowCalculator_Methode);
            Calculator_RemoveLast_Command = new AsyncCommand(Calculator_RemoveLast_Methode);
            Calculator_RemoveAll_Command = new AsyncCommand(Calculator_RemoveAll_Methode);
            ShowCalculator_List_Command = new AsyncCommand(ShowCalculator_List_Methode);
            Show_Letter_Saldo_Command = new Command(Show_Letter_Saldo_Methode);
            Load_on_demand_Command = new AsyncCommand<string>(Load_on_demand_Methode);
            Load_Ratio_Command = new AsyncCommand(Load_Ratio_Methode);
            Budget_Command = new AsyncCommand(Budget_Methode);
            Change_Saldo_Date_Command = new AsyncCommand(Change_Saldo_Date_Methode);

            Period_Command = new AsyncCommand(Period_Popup);

            title = "Haushaltsbuch " + Current_Viewtime.Year + " " + Current_Viewtime.Month + "";

            if (Preferences.Get("Filter_Activity", false) == true)
            {
                Filter_ActivityColor = Color.Green;
            }
            else
            {
                Filter_ActivityColor = Color.White;
            }
        }

        private async Task Add_Methode()
        {
            await Shell.Current.GoToAsync(nameof(Add_Item_Page));
        }

        private async Task Add_to_Groups()
        {
            try
            {
                transaktionGroups.Clear();

                if (GroupingOption == 1)
                {
                    List<Transaktion> help = Transaktion.ToList();

                    var groups1 = await ReasonService.Get_all_Reason_sorted();

                    Transaktion.Clear();

                    Transaktion.AddRange(help);

                    List<string> groups = new List<string>();

                    foreach (var group in groups1)
                    {
                        groups.Add((group.Benutzerdefinierter_Zweck.Substring(0, group.Benutzerdefinierter_Zweck.IndexOf(":"))));
                    }

                    //groups.Sort();

                    foreach (string st in groups)
                    {
                        if (Transaktion.Where(ts => ts.Zweck == st).Count() != 0)
                        {
                            transaktionGroups.Add(new Grouping<string, Transaktion>(st, Transaktion.Where(ts => ts.Zweck == st)));
                        }
                    }
                }

                if (GroupingOption == 0)
                {
                    List<string> groups = new List<string>();

                    string tr1 = "";

                    foreach (Transaktion tr in Transaktion)
                    {
                        if (tr.Auftrags_id != "Load")
                        {
                            if (tr.Datumanzeige != tr1)
                            {
                                groups.Add(tr.Datumanzeige);
                                tr1 = tr.Datumanzeige;
                            }
                        }
                    }

                    groups = (from p in groups orderby DateTime.ParseExact(p, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();

                    List<string> double_safe_groups = new List<string>();

                    double_safe_groups.Clear();

                    string temp_date = "";

                    foreach (string date in groups)
                    {
                        if (date != temp_date)
                        {
                            double_safe_groups.Add(date);
                            temp_date = date;
                        }
                    }

                    foreach (string st in double_safe_groups)
                    {
                        if (Transaktion.Where(ts => ts.Datumanzeige == st).Count() != 0)
                        {
                            transaktionGroups.Add(new Grouping<string, Transaktion>(st, Transaktion.Where(ts => ts.Datumanzeige == st)));
                        }
                    }

                    if (Transaktion.Count() != 0)
                    {
                        if (Transaktion.Last().Auftrags_id == "Load")
                        {
                            transaktionGroups.Add(new Grouping<string, Transaktion>("", new List<Transaktion>() { Transaktion.Last() }));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        async Task GroupingOption_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Anordnung", 300, new List<string>() { "Sortieren nach Datum", "Sortieren nach Zweck" }));

                if (result == null)
                {
                    return;
                }
                if ((string)result == "Sortieren nach Datum")
                {
                    GroupingOption = 0;
                }
                else
                {
                    GroupingOption = 1;
                }


                Load_Progress.Clear();

                Load_Progress.Add(new double[] { -1, -1 });

                All_Transaktion_List_for_Load.Clear();

                Transaktion_List_Load_for_Load.Clear();

                Transaktion_List_from_Load.Clear();

                await Refresh();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        async Task Remove(Transaktion item)
        {
            try
            {
                if (item.Auftrags_id != null)
                {
                    List<Transaktion> transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                    List<Transaktion> transaktion_list2 = new List<Transaktion>();

                    int capcity = 0;

                    string[] message = null;

                    foreach (Transaktion trans in transaktion_list)
                    {
                        if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                        {
                            if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                            {
                                capcity++;
                                if (DateTime.Compare(trans.Datum, item.Datum) >= 0)
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }
                    }

                    if(capcity == transaktion_list2.Count)
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Transaktion löschen", 370, new List<string>() { "Diese Transaktion entfernen", "Alle mit dieser Auftrag-ID entfernen" }));

                        if (result == null)
                        {
                            return;
                        }
                        if ((string)result == "Diese Transaktion entfernen")
                        {
                            var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 300, 450, "Ja", "Nein", new string[] { "Wollen Sie diese Transaktion wirklich entfernen?", "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\nID: " + item.Id + "" }));

                            if (value == null)
                            {
                                return;
                            }
                            if ((bool)value == true)
                            {
                                item.Content_Visibility = false;
                                await ContentService.Edit_Transaktion(item);

                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                List<Transaktion> transaktionlist = new List<Transaktion>();

                                foreach (Transaktion trans in all)
                                {
                                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                                    {
                                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                        {
                                            transaktionlist.Add(trans);
                                        }
                                    }
                                }

                                if(transaktionlist.Count() != 0)
                                {
                                    transaktionlist = transaktionlist.OrderBy(d => d.Datum).ToList();

                                    Transaktion newlasttransaktion = transaktionlist.Last();

                                    await NotificationHelper.ModifyNotification(newlasttransaktion, 0);
                                }
                                else
                                {
                                    await NotificationHelper.ModifyNotification(item, 1);
                                }

                                await Refresh();
                            }
                        }
                        if ((string)result == "Alle mit dieser Auftrag-ID entfernen")
                        {
                            if(item.Auftrags_Option == 1)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:  " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }

                            var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 300, 420, "Ja", "Nein", message));

                            if (value == null)
                            {
                                return;
                            }
                            if ((bool)value == true)
                            {
                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                List<Transaktion> transaktionlist = new List<Transaktion>();

                                foreach (Transaktion trans in all)
                                {
                                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                                    {
                                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                        {
                                            trans.Content_Visibility = false;
                                            await ContentService.Edit_Transaktion(trans);
                                            transaktionlist.Add(trans);
                                        }
                                    }
                                }

                                transaktionlist = transaktionlist.OrderBy(d => d.Datum).ToList();

                                Transaktion newlasttransaktion = transaktionlist.Last();

                                await NotificationHelper.ModifyNotification(newlasttransaktion, 1);

                                await Refresh();
                            }
                        }
                    }
                    else
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Transaktion löschen", 370, new List<string>() { "Diese Transaktion entfernen", "Alle mit dieser Auftrag-ID entfernen", "Alle mit dieser Auftrag-ID ab dieser Transaktion entfernen" }));

                        if (result == null)
                        {
                            return;
                        }
                        if ((string)result == "Diese Transaktion entfernen")
                        {
                            var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 300, 450, "Ja", "Nein", new string[] { "Wollen Sie diese Transaktion wirklich entfernen?", "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\nID: " + item.Id + "" }));

                            if (value == null)
                            {
                                return;
                            }
                            if ((bool)value == true)
                            {
                                item.Content_Visibility = false;
                                await ContentService.Edit_Transaktion(item);

                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                List<Transaktion> transaktionlist = new List<Transaktion>();

                                foreach (Transaktion trans in all)
                                {
                                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                                    {
                                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                        {
                                            transaktionlist.Add(trans);
                                        }
                                    }
                                }

                                if (transaktionlist.Count() != 0)
                                {
                                    transaktionlist = transaktionlist.OrderBy(d => d.Datum).ToList();

                                    Transaktion newlasttransaktion = transaktionlist.Last();

                                    await NotificationHelper.ModifyNotification(newlasttransaktion, 0);
                                }

                                await Refresh();
                            }
                        }
                        if ((string)result == "Alle mit dieser Auftrag-ID entfernen")
                        {
                            if (item.Auftrags_Option == 1)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = new string[] { "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }

                            var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 350, 500, "Ja", "Nein", message));

                            if (value == null)
                            {
                                return;
                            }
                            if ((bool)value == true)
                            {
                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                List<Transaktion> transaktionlist = new List<Transaktion>();

                                foreach (Transaktion trans in all)
                                {
                                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                                    {
                                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                        {
                                            trans.Content_Visibility = false;
                                            await ContentService.Edit_Transaktion(trans);
                                            transaktionlist.Add(trans);
                                        }
                                    }
                                }

                                transaktionlist = transaktionlist.OrderBy(d => d.Datum).ToList();

                                Transaktion newlasttransaktion = transaktionlist.Last();

                                await NotificationHelper.ModifyNotification(newlasttransaktion, 1);

                                await Refresh();
                            }
                        }
                        if ((string)result == "Alle mit dieser Auftrag-ID ab dieser Transaktion entfernen")
                        {
                            if (item.Auftrags_Option == 1)
                            {
                                message = new string[] { "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = new string[] { "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "" };
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = new string[] { "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?","Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:  " + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "" };
                            }

                            var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 350, 500, "Ja", "Nein", message));

                            if (value == null)
                            {
                                return;
                            }
                            if ((bool)value == true)
                            {
                                Transaktion newlasttransaktion = new Transaktion();

                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                foreach (Transaktion trans in all)
                                {
                                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                                    {
                                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                        {
                                            if (DateTime.Compare(trans.Datum, item.Datum) >= 0)
                                            {
                                                trans.Content_Visibility = false;
                                                await ContentService.Edit_Transaktion(trans);
                                            }
                                            else
                                            {
                                                newlasttransaktion = trans;
                                            }
                                        }
                                    }
                                }

                                await NotificationHelper.ModifyNotification(newlasttransaktion, 0);

                                await Refresh();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                }
                else
                {
                    var value = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Entfernen", 300, 420, "Ja", "Nein", new string[] { "Wollen Sie diese Transaktion wirklich entfernen?", "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "" }));

                    if (value == null)
                    {
                        return;
                    }
                    if ((bool)value == true)
                    {
                        item.Content_Visibility = false;
                        await ContentService.Edit_Transaktion(item);
                        await Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        async Task Edit(Transaktion item)
        {
            try
            {
                if (item.Auftrags_id != null)
                {
                    List<Transaktion> transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                    List<Transaktion> transaktion_list2 = new List<Transaktion>();

                    List<Transaktion> transaktion_list3 = new List<Transaktion>();

                    int capcity = 0;

                    foreach (Transaktion trans in transaktion_list)
                    {
                        if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                        {
                            if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                            {
                                transaktion_list3.Add(trans);

                                capcity++;

                                if (DateTime.Compare(trans.Datum, item.Datum) >= 0)
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }
                    }

                    if(capcity == transaktion_list2.Count)
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Transaktion bearbeiten", 380, new List<string>() { "Diese Transaktion bearbeiten", "Alle mit dieser Auftrags-ID bearbeiten", "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten" }));

                        if (result == null)
                        {
                            return;
                        }

                        if ((string)result == "Diese Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=1");

                        }

                        if ((string)result == "Alle mit dieser Auftrags-ID bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=2");
                        }

                        if((string)result == "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten")
                        {
                            var result1 = await Shell.Current.ShowPopupAsync(new Timespane_Popup(item,transaktion_list3));

                            if (result1 == null)
                            {
                                return;
                            }

                            if ((bool)result1 == true)
                            {
                                await Refresh();
                            }
                            else
                            {
                                await Notificater("Bei der Versetzung der Transaktionen um ein bestimmte Zeit ist ein Fehler aufgetretten.");
                            }
                        }
                    }
                    else
                    {
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Transaktion bearbeiten", 450, new List<string>() { "Diese Transaktion bearbeiten", "Alle mit dieser Auftrags-ID bearbeiten", "Alle mit dieser Auftrags-ID ab dieser Transaktion bearbeiten", "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten" }));

                        if (result == null)
                        {
                            return;
                        }

                        if ((string)result == "Diese Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=1");
                        }

                        if ((string)result == "Alle mit dieser Auftrags-ID bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=2");
                        }

                        if ((string)result == "Alle mit dieser Auftrags-ID ab dieser Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=3");
                        }

                        if ((string)result == "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten")
                        {
                            var result1 = await Shell.Current.ShowPopupAsync(new Timespane_Popup(item, transaktion_list3));

                            if(result1 == null)
                            {
                                return;
                            }

                            if ((bool)result1 == true)
                            {
                                await Refresh();
                            }
                            else
                            {
                                await Notificater("Bei der Versetzung der Transaktionen um ein bestimmte Zeit ist ein Fehler aufgetretten.");
                            }
                        }
                    }
                }
                else
                {
                    var result = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Bearbeiten", 300, 400, "Ja", "Nein", new string[] { "Wollen Sie wirklich diese Transaktionen bearbeiten?", "Zweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nWird im Stand berechnet: " + item.Saldo_Visibility_String + "\nID: " + item.Id + "" }));

                    if (result == null)
                    {
                        return;
                    }
                    if ((bool)result == true && item != null)
                    {
                        await PassingTransaktionService.Remove_All_Transaktion();

                        await PassingOrderService.Remove_All_Order();

                        await Shell.Current.GoToAsync($"{nameof(Edit_Item_Page)}?TransaktionID={item.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                await PassingOrderService.Remove_All_Order();

                await PassingTransaktionService.Remove_All_Transaktion();
            }
        }

        async Task Search_Methode()
        {
            try
            {
                if (Search_Text == Last_Search_Text)
                {
                    return;
                }
                else
                {
                    Last_Search_Text = Search_Text;

                    if (String.IsNullOrWhiteSpace(Last_Search_Text) == true)
                    {
                        return;
                    }
                }

                string pseudosearchtext = Search_Text.Trim();

                List<Filter> filters = new List<Filter>()
                    {
                        new Filter(){Name="Transaktions_ID" , State = Preferences.Get("Search_For_Transaktion_ID", false) },
                        new Filter(){Name="Auftrags_ID" , State = Preferences.Get("Search_For_Auftrags_ID", false) },
                        new Filter(){Name="Datum" , State = Preferences.Get("Search_For_Datum", false) },
                        new Filter(){Name="Zweck" , State = Preferences.Get("Search_For_Zweck", false) },
                        new Filter(){Name="Notiz" , State = Preferences.Get("Search_For_Notiz", false) },
                        new Filter(){Name="Betrag" , State = Preferences.Get("Search_For_Betrag", false) },
                        new Filter(){Name="Quersuche" , State = Preferences.Get("Quersuche", false) }
                    };

                Transaktion.Clear();

                var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                foreach (Transaktion trans in transaktionscontent)
                {
                    if (GroupingOption == 0)
                    {
                        trans.Pseudotext = trans.Zweck;
                    }
                    else
                    {
                        trans.Pseudotext = trans.Datumanzeige;
                    }
                }

                if (Search_Text == null)
                {
                    await Search_Methode2();

                    return;
                }

                List<Transaktion> search_transaktionscontent = new List<Transaktion>();

                search_transaktionscontent.Clear();

                if (pseudosearchtext.Contains("-") == true)
                {
                    string New_Search_Text = pseudosearchtext;

                    string Tag = null;

                    List<Transaktion> new_search_transaktioncontent = new List<Transaktion>();

                    new_search_transaktioncontent.Clear();

                    search_transaktionscontent = transaktionscontent.ToList();

                    while (New_Search_Text.Count() > 0)
                    {
                        if(New_Search_Text.IndexOf("-") == -1)
                        {
                            Tag = New_Search_Text;

                            New_Search_Text = "";
                        }
                        else
                        {
                            Tag = New_Search_Text.Substring(0, New_Search_Text.IndexOf("-")).Trim();

                            New_Search_Text = New_Search_Text.Substring(New_Search_Text.IndexOf("-") + 1);
                        }

                        if (String.IsNullOrEmpty(Tag) == false)
                        {
                            if(filters.Last().State == true)
                            {
                                new_search_transaktioncontent = search_transaktionscontent.Where(s => s.CrossSearch_Indicator(filters).ToUpper().Contains(Tag.ToUpper())).ToList();

                                search_transaktionscontent = new_search_transaktioncontent;
                            }
                            else
                            {
                                foreach (Transaktion trans in search_transaktionscontent)
                                {
                                    bool contains = false;

                                    if (trans.Search_Indicator(filters).Count() != 0)
                                    {
                                        foreach (string word in trans.Search_Indicator(filters))
                                        {
                                            if (Tag.ToUpper() == word.ToUpper())
                                            {
                                                contains = true;
                                            }
                                        }
                                    }

                                    if (contains == true)
                                    {
                                        new_search_transaktioncontent.Add(trans);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(filters.Last().State == true)
                    {
                        search_transaktionscontent = transaktionscontent.Where(s => s.CrossSearch_Indicator(filters).ToUpper().Contains(pseudosearchtext.ToUpper().Trim())).ToList();
                    }
                    else
                    {
                        foreach(Transaktion trans in transaktionscontent)
                        {
                            bool contains = false;

                            if(trans.Search_Indicator(filters).Count() != 0)
                            {
                                foreach(string word in trans.Search_Indicator(filters))
                                {
                                    if(pseudosearchtext.ToUpper().Trim() == word.ToUpper())
                                    {
                                        contains = true;
                                    }
                                }
                            }


                            if(contains == true)
                            {
                                search_transaktionscontent.Add(trans);
                            }
                        }
                    }
                }

                search_transaktionscontent = (from p in search_transaktionscontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();

                if (search_transaktionscontent.Count() == 0)
                {
                    List_of_Transaktion_Status = false;

                    Kein_Ergebnis_Suggestion_Status = false;

                    List_of_Suggestion_Status = false;

                    Kein_Ergebnis_Transaktion_Status = true;

                    await SearchSuggestionService.Add_Suggestion(Search_Text);

                    SuggestionCollection.Clear();

                    SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());

                    return;
                }
                else
                {
                    Kein_Ergebnis_Transaktion_Status = false;

                    Kein_Ergebnis_Suggestion_Status = false;

                    List_of_Suggestion_Status = false;

                    List_of_Transaktion_Status = true;


                    if (GroupingOption == 0)
                    {
                        All_Transaktion_List_for_Load = search_transaktionscontent.ToList();

                        if (Load_Progress[0][0] != search_transaktionscontent.Count)
                        {

                            Load_Progress[0][0] = search_transaktionscontent.Count;

                            Load_Progress[0][1] = Preferences.Get("Transaktion_per_load", 20.0);

                            if (Load_Progress[0][1] > Load_Progress[0][0])
                            {
                                Load_Progress[0][1] = Load_Progress[0][0];
                            }

                            List<Transaktion> new_sorted_after_month_transaktionscontent = search_transaktionscontent.GetRange(0, (int)Load_Progress[0][1]);

                            if (Load_Progress[0][1] != Load_Progress[0][0])
                            {
                                new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0" });
                            }

                            search_transaktionscontent.Clear();

                            search_transaktionscontent = new_sorted_after_month_transaktionscontent;
                        }
                        else
                        {
                            if (Load_Progress[0][1] < Load_Progress[0][0])
                            {
                                if (Transaktion_List_from_Load.Count == 0)
                                {
                                    Load_Progress[0][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                    if (Load_Progress[0][1] > Load_Progress[0][0])
                                    {
                                        Load_Progress[0][1] = Load_Progress[0][0];
                                    }

                                    List<Transaktion> new_sorted_after_month_transaktionscontent = search_transaktionscontent.GetRange(0, (int)Load_Progress[0][1]);

                                    if (Load_Progress[0][1] != Load_Progress[0][0])
                                    {
                                        new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0" });
                                    }

                                    search_transaktionscontent.Clear();

                                    search_transaktionscontent = new_sorted_after_month_transaktionscontent;
                                }
                                else
                                {
                                    search_transaktionscontent = Transaktion_List_from_Load;
                                }
                            }
                        }
                    }
                    else
                    {
                        int count = 0;

                        if (Load_Progress.Count() != 0)
                        {
                            foreach (double[] db in Load_Progress)
                            {
                                count += (int)db[0];
                            }
                        }
                        if (count != search_transaktionscontent.Count)
                        {
                            used_reasons_list.Clear();

                            Load_Progress.Clear();

                            foreach (Transaktion trans in search_transaktionscontent)
                            {
                                if (!used_reasons_list.Contains(trans.Zweck))
                                {
                                    used_reasons_list.Add(trans.Zweck);
                                }
                            }
                        }

                        if (used_reasons_list.Count() != 0)
                        {
                            int follower = 0;

                            All_Transaktion_List_for_Load = search_transaktionscontent.ToList();

                            List<Transaktion> root_sorted_after_month_transaktionscontent = new List<Transaktion>();

                            foreach (string zweck in used_reasons_list)
                            {
                                if (Load_Progress.Count() < follower + 1)
                                {
                                    Load_Progress.Add(new double[] { -1, -1 });
                                }

                                if (Load_Progress[follower][0] != search_transaktionscontent.Where(ts => ts.Zweck == zweck).Count())
                                {
                                    Load_Progress[follower][0] = search_transaktionscontent.Where(ts => ts.Zweck == zweck).Count();

                                    Load_Progress[follower][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                    if (Load_Progress[follower][1] > Load_Progress[follower][0])
                                    {
                                        Load_Progress[follower][1] = Load_Progress[follower][0];
                                    }

                                    List<Transaktion> new_sorted_after_month_transaktionscontent = search_transaktionscontent.Where(ts => ts.Zweck == zweck).ToList().GetRange(0, (int)Load_Progress[follower][1]);

                                    if (Load_Progress[follower][1] != Load_Progress[follower][0])
                                    {
                                        new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = zweck, Datum = new DateTime(1999, 12, 31) });
                                    }

                                    root_sorted_after_month_transaktionscontent.AddRange(new_sorted_after_month_transaktionscontent);
                                }
                                else
                                {
                                    if (Load_Progress[follower][1] < Load_Progress[follower][0])
                                    {
                                        if (Transaktion_List_Load_for_Load.Where(ts => ts.Zweck == zweck).Count() == 0)
                                        {
                                            Load_Progress[follower][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                            if (Load_Progress[follower][1] > Load_Progress[follower][0])
                                            {
                                                Load_Progress[follower][1] = Load_Progress[follower][0];
                                            }

                                            List<Transaktion> new_sorted_after_month_transaktionscontent = search_transaktionscontent.Where(ts => ts.Zweck == zweck).ToList().GetRange(0, (int)Load_Progress[follower][1]);

                                            if (Load_Progress[follower][1] != Load_Progress[follower][0])
                                            {
                                                new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = zweck, Datum = new DateTime(1999, 12, 31) });
                                            }

                                            root_sorted_after_month_transaktionscontent = new_sorted_after_month_transaktionscontent;
                                        }
                                        else
                                        {
                                            root_sorted_after_month_transaktionscontent.AddRange(Transaktion_List_Load_for_Load.Where(ts => ts.Zweck == zweck));
                                        }
                                    }
                                    else
                                    {
                                        root_sorted_after_month_transaktionscontent.AddRange(All_Transaktion_List_for_Load.Where(ts => ts.Zweck == zweck));
                                    }
                                }

                                follower++;
                            }

                            search_transaktionscontent = root_sorted_after_month_transaktionscontent;

                            Transaktion_List_Load_for_Load = root_sorted_after_month_transaktionscontent;

                            search_transaktionscontent = (from p in search_transaktionscontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();
                        }
                    }

                    Transaktion.AddRange(search_transaktionscontent);

                    await Add_to_Groups();
                }

                await SearchSuggestionService.Add_Suggestion(Search_Text);

                SuggestionCollection.Clear();

                SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());

                Title = "Suche im Haushaltsbuch";
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        async Task Search_Methode2()
        {
            try
            {
                if (Kein_Ergebnis_Suggestion_Status == true | List_of_Suggestion_Status == true)
                {
                    List_of_Transaktion_Status = false;
                    Kein_Ergebnis_Transaktion_Status = false;
                    if (SuggestionCollection.Count() == 0)
                    {
                        List_of_Suggestion_Status = false;
                        Kein_Ergebnis_Suggestion_Status = true;
                    }
                    else
                    {
                        List_of_Suggestion_Status = true;
                        Kein_Ergebnis_Suggestion_Status = false;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(Search_Text))
                    {
                        await Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Delet_Suggestion_Methode(Suggestion input)
        {
            try
            {
                if (input == null)
                {
                    return;
                }
                else
                {
                    await SearchSuggestionService.Remove_Suggestion(input);

                    SuggestionCollection.Clear();

                    SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());

                    if (SuggestionCollection.Count() == 0)
                    {
                        Kein_Ergebnis_Suggestion_Status = true;
                        List_of_Suggestion_Status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Select_Suggestion_Methode(Suggestion input)
        {
            try
            {
                if (input != null)
                {
                    Search_Text = input.Suggestion_value;

                    await Search_Methode();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Clear_SearchText_Methode()
        {
            try
            {
                if (Search_Text != null)
                {
                    Load_Progress.Clear();

                    Load_Progress.Add(new double[] { -1, -1 });

                    All_Transaktion_List_for_Load.Clear();

                    Transaktion_List_Load_for_Load.Clear();

                    Transaktion_List_from_Load.Clear();

                    Search_Text = null;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }

        }

        public async Task Set_Searchbar_Visibility_MethodeAsync()
        {
            try
            {
                if (Serchbar_Visibility == true)
                {
                    Serchbar_Visibility = false;
                    List_of_Suggestion_Status = false;
                    Kein_Ergebnis_Suggestion_Status = false;

                    if (Transaktion.Count() == 0)
                    {
                        Kein_Ergebnis_Transaktion_Status = true;
                    }
                    else
                    {
                        List_of_Transaktion_Status = true;
                    }
                }
                else
                {
                    Serchbar_Visibility = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task The_Searchbar_is_Tapped_Methode()
        {
            try
            {
                SuggestionCollection.Clear();

                SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());

                if (Serchbar_Visibility == true)
                {
                    if (List_of_Transaktion_Status == true | Kein_Ergebnis_Transaktion_Status == true)
                    {
                        List_of_Transaktion_Status = false;
                        Kein_Ergebnis_Transaktion_Status = false;

                        if (SuggestionCollection.Count() == 0)
                        {
                            Kein_Ergebnis_Suggestion_Status = true;
                            List_of_Suggestion_Status = false;
                        }
                        else
                        {
                            Kein_Ergebnis_Suggestion_Status = false;
                            List_of_Suggestion_Status = true;
                        }
                    }
                    else
                    {
                        List_of_Suggestion_Status = false;
                        Kein_Ergebnis_Suggestion_Status = false;

                        if (Transaktion.Count() == 0)
                        {
                            List_of_Transaktion_Status = false;
                            Kein_Ergebnis_Transaktion_Status = true;
                        }
                        else
                        {
                            Kein_Ergebnis_Transaktion_Status = false;
                            List_of_Transaktion_Status = true;
                        }
                    }
                }
                else
                {
                    List_of_Suggestion_Status = false;
                    Kein_Ergebnis_Suggestion_Status = false;

                    if (Transaktion.Count() == 0)
                    {
                        List_of_Transaktion_Status = false;
                        Kein_Ergebnis_Transaktion_Status = true;
                    }
                    else
                    {
                        Kein_Ergebnis_Transaktion_Status = false;
                        List_of_Transaktion_Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Add_Catchphrase_To_Search_Methode()
        {
            try
            {
                string title = "Suchbegriff erstellen";

                string message = "Nach welchen Suchbegriff wollen Sie im Haushaltsbuch suchen?";

                if (string.IsNullOrWhiteSpace(Search_Text) == false)
                {
                    title = "Suchbegriff hinzufügen";

                    message = "Nach welchem Suchbegriff wollen Sie weiter im Haushaltsbuch suchen?\nAktueller Suchbegriff: " + Search_Text + "";
                }

                var result = await Shell.Current.ShowPopupAsync(new CustomePromt_Popup(title, 350, 300, "Hinzufügen", null, "Hier Suchbegriff eingeben"));

                if (result == null)
                {
                    return;
                }
                else
                {
                    string resultstring = (string)result;

                    if (String.IsNullOrWhiteSpace(Search_Text) == false)
                    {
                        Search_Text = Search_Text.Trim();

                        Search_Text += "-" + resultstring.Trim() + "";
                    }
                    else
                    {
                        Search_Text = resultstring;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Refresh()
        {
            try
            {
                if (Is_Aktiv == true)
                {
                    IsBusy = true;
                }
                else
                {
                    Is_Aktiv = true;
                }

                if (string.IsNullOrEmpty(Search_Text) == false)
                {
                    await Search_Methode();

                    if (Transaktion.Count() == 0)
                    {
                        Title = "Haushaltsbuch";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Current_Viewtime.Year.ToString()) == false && String.IsNullOrEmpty(Current_Viewtime.Month) == false)
                        {
                            Title = "Haushaltsbuch " + Current_Viewtime.Year + " " + Current_Viewtime.Month + "";
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(Current_Viewtime.Year.ToString()) == true)
                            {
                                Title = "Haushaltsbuch";
                            }
                            if (String.IsNullOrEmpty(Current_Viewtime.Month.ToString()) == true)
                            {
                                Title = "Haushaltsbuch " + Current_Viewtime.Year + "";
                            }
                        }
                    }

                    IsBusy = false;

                    return;
                }

                Transaktion.Clear();

                transaktionGroups.Clear();

                var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                List<Transaktion> sorted_after_month_transaktionscontent = new List<Transaktion>();

                double saldo = 0;

                double letter_saldo = 0;

                if (Preferences.Get("Is_Saldo_Date_Changed", false) == false)
                {
                    Preferences.Set("Saldo_Date", DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")));

                    Saldo_Date = Preferences.Get("Saldo_Date", DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")));
                }

                if (transaktionscontent.Count() != 0)
                {
                    if (GroupingOption == 1)
                    {
                        if (Load_Progress.Count() == 1)
                        {
                            Load_Progress.Clear();
                        }

                        used_reasons_list.Clear();

                        foreach (var trans in transaktionscontent)
                        {
                            if (DateTime.Compare(trans.Datum, DateTime.ParseExact(Saldo_Date, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddDays(1).AddSeconds(-1)) <= 0)
                            {
                                if (trans.Saldo_Visibility == true)
                                {
                                    saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                                else
                                {
                                    letter_saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                            }

                            if (trans.Datum.Year == Current_Viewtime.Year)
                            {
                                if (trans.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Current_Viewtime.Month)
                                {
                                    if (GroupingOption == 0)
                                    {
                                        trans.Pseudotext = trans.Zweck;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                    else
                                    {
                                        trans.Pseudotext = trans.Datumanzeige;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                }
                                if (Current_Viewtime.Month == "")
                                {
                                    if (GroupingOption == 0)
                                    {
                                        trans.Pseudotext = trans.Zweck;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                    else
                                    {
                                        trans.Pseudotext = trans.Datumanzeige;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                }
                            }

                            if (!used_reasons_list.Contains(trans.Zweck))
                            {
                                used_reasons_list.Add(trans.Zweck);
                            }
                        }
                    }
                    else
                    {
                        foreach (var trans in transaktionscontent)
                        {
                            if (DateTime.Compare(trans.Datum, DateTime.ParseExact(Saldo_Date, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddDays(1).AddSeconds(-1)) <= 0)
                            {
                                if (trans.Saldo_Visibility == true)
                                {
                                    saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                                else
                                {
                                    letter_saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                            }

                            if (trans.Datum.Year == Current_Viewtime.Year)
                            {
                                if (trans.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Current_Viewtime.Month)
                                {
                                    if (GroupingOption == 0)
                                    {
                                        trans.Pseudotext = trans.Zweck;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                    else
                                    {
                                        trans.Pseudotext = trans.Datumanzeige;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                }
                                if (Current_Viewtime.Month == "")
                                {
                                    if (GroupingOption == 0)
                                    {
                                        trans.Pseudotext = trans.Zweck;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                    else
                                    {
                                        trans.Pseudotext = trans.Datumanzeige;
                                        sorted_after_month_transaktionscontent.Add(trans);
                                    }
                                }
                            }
                        }
                    }
                }

                if (saldo != 0)
                {
                    saldo = Math.Round(saldo, 2);

                    Saldo_Value = saldo.ToString().Replace(".", ",");

                    IsSaldoVisibility = true;

                    if (saldo < 0)
                    {
                        Saldo_Evaluate = Color.Red;
                    }
                    if (saldo == 0)
                    {
                        Saldo_Evaluate = Color.White;
                    }
                    if (saldo > 0)
                    {
                        Saldo_Evaluate = Color.Green;
                    }
                }
                else
                {
                    Saldo_Value = null;

                    if (transaktionscontent.Count() == 0)
                    {
                        IsSaldoVisibility = false;
                    }
                }

                if (letter_saldo != 0)
                {
                    letter_saldo = Math.Round(letter_saldo, 2);

                    Letter_Saldo_Value = letter_saldo.ToString().Replace(".", ",");

                    if (letter_saldo < 0)
                    {
                        Letter_Saldo_Evaluate = Color.Red;
                    }
                    if (letter_saldo == 0)
                    {
                        Letter_Saldo_Evaluate = Color.White;
                    }
                    if (letter_saldo > 0)
                    {
                        Letter_Saldo_Evaluate = Color.Green;
                    }
                }
                else
                {
                    Letter_Saldo_Value = "0";
                }

                sorted_after_month_transaktionscontent = (from p in sorted_after_month_transaktionscontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();

                All_Transaktion_List_for_Load = sorted_after_month_transaktionscontent.ToList();

                if (GroupingOption == 0)
                {
                    if (Load_Progress[0][0] != sorted_after_month_transaktionscontent.Count)
                    {

                        Load_Progress[0][0] = sorted_after_month_transaktionscontent.Count;

                        Load_Progress[0][1] = Preferences.Get("Transaktion_per_load", 20.0);

                        if (Load_Progress[0][1] > Load_Progress[0][0])
                        {
                            Load_Progress[0][1] = Load_Progress[0][0];
                        }

                        List<Transaktion> new_sorted_after_month_transaktionscontent = sorted_after_month_transaktionscontent.GetRange(0, (int)Load_Progress[0][1]);

                        if (Load_Progress[0][1] != Load_Progress[0][0])
                        {
                            new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0" });
                        }

                        sorted_after_month_transaktionscontent.Clear();

                        sorted_after_month_transaktionscontent = new_sorted_after_month_transaktionscontent;
                    }
                    else
                    {
                        if (Load_Progress[0][1] < Load_Progress[0][0])
                        {
                            if (Transaktion_List_from_Load.Count == 0)
                            {
                                Load_Progress[0][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                if (Load_Progress[0][1] > Load_Progress[0][0])
                                {
                                    Load_Progress[0][1] = Load_Progress[0][0];
                                }

                                List<Transaktion> new_sorted_after_month_transaktionscontent = sorted_after_month_transaktionscontent.GetRange(0, (int)Load_Progress[0][1]);

                                if (Load_Progress[0][1] != Load_Progress[0][0])
                                {
                                    new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0" });
                                }

                                sorted_after_month_transaktionscontent.Clear();

                                sorted_after_month_transaktionscontent = new_sorted_after_month_transaktionscontent;
                            }
                            else
                            {
                                sorted_after_month_transaktionscontent = Transaktion_List_from_Load;
                            }
                        }
                    }
                }
                else
                {
                    if (used_reasons_list.Count() != 0)
                    {
                        int follower = 0;

                        List<Transaktion> root_sorted_after_month_transaktionscontent = new List<Transaktion>();

                        foreach (string zweck in used_reasons_list)
                        {
                            if (Load_Progress.Count() < follower + 1)
                            {
                                Load_Progress.Add(new double[] { -1, -1 });
                            }

                            if (Load_Progress[follower][0] != sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == zweck).Count())
                            {
                                Load_Progress[follower][0] = sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == zweck).Count();

                                Load_Progress[follower][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                if (Load_Progress[follower][1] > Load_Progress[follower][0])
                                {
                                    Load_Progress[follower][1] = Load_Progress[follower][0];
                                }

                                List<Transaktion> new_sorted_after_month_transaktionscontent = sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == zweck).ToList().GetRange(0, (int)Load_Progress[follower][1]);

                                if (Load_Progress[follower][1] != Load_Progress[follower][0])
                                {
                                    new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = zweck, Datum = new DateTime(1999, 12, 31) });
                                }

                                root_sorted_after_month_transaktionscontent.AddRange(new_sorted_after_month_transaktionscontent);
                            }
                            else
                            {
                                if (Load_Progress[follower][1] < Load_Progress[follower][0])
                                {
                                    if (Transaktion_List_Load_for_Load.Where(ts => ts.Zweck == zweck).Count() == 0)
                                    {
                                        Load_Progress[follower][1] = Preferences.Get("Transaktion_per_load", 20.0);

                                        if (Load_Progress[follower][1] > Load_Progress[follower][0])
                                        {
                                            Load_Progress[follower][1] = Load_Progress[follower][0];
                                        }

                                        List<Transaktion> new_sorted_after_month_transaktionscontent = sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == zweck).ToList().GetRange(0, (int)Load_Progress[follower][1]);

                                        if (Load_Progress[follower][1] != Load_Progress[follower][0])
                                        {
                                            new_sorted_after_month_transaktionscontent.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = zweck, Datum = new DateTime(1999, 12, 31) });
                                        }

                                        root_sorted_after_month_transaktionscontent = new_sorted_after_month_transaktionscontent;
                                    }
                                    else
                                    {
                                        root_sorted_after_month_transaktionscontent.AddRange(Transaktion_List_Load_for_Load.Where(ts => ts.Zweck == zweck));
                                    }
                                }
                                else
                                {
                                    root_sorted_after_month_transaktionscontent.AddRange(All_Transaktion_List_for_Load.Where(ts => ts.Zweck == zweck));
                                }
                            }

                            follower++;
                        }

                        sorted_after_month_transaktionscontent = root_sorted_after_month_transaktionscontent;

                        Transaktion_List_Load_for_Load = root_sorted_after_month_transaktionscontent;

                        sorted_after_month_transaktionscontent = (from p in sorted_after_month_transaktionscontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();
                    }
                }

                Transaktion.AddRange(sorted_after_month_transaktionscontent);


                if (Kein_Ergebnis_Suggestion_Status != true && List_of_Suggestion_Status != true)
                {
                    if (Transaktion.Count() == 0)
                    {
                        List_of_Transaktion_Status = false;
                        Kein_Ergebnis_Transaktion_Status = true;
                        if (SuggestionCollection.Count() == 0)
                        {
                            SuggestionCollection.Clear();

                            SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());
                        }
                    }
                    else
                    {
                        await Add_to_Groups();

                        Kein_Ergebnis_Transaktion_Status = false;
                        List_of_Transaktion_Status = true;
                        if (SuggestionCollection.Count() == 0)
                        {
                            SuggestionCollection.Clear();

                            SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());
                        }
                    }
                }

                if (Transaktion.Count() == 0)
                {
                    Title = "Haushaltsbuch";
                }
                else
                {
                    if (String.IsNullOrEmpty(Current_Viewtime.Year.ToString()) == false && String.IsNullOrEmpty(Current_Viewtime.Month) == false)
                    {
                        Title = "Haushaltsbuch " + Current_Viewtime.Year + " " + Current_Viewtime.Month + "";
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(Current_Viewtime.Year.ToString()) == true)
                        {
                            Title = "Haushaltsbuch";
                        }
                        if (String.IsNullOrEmpty(Current_Viewtime.Month.ToString()) == true)
                        {
                            Title = "Haushaltsbuch " + Current_Viewtime.Year + "";
                        }
                    }
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                IsBusy = false;
            }
        }

        private void ViewIsDisappearing_Methode()
        {
            Is_Aktiv = false;

            Load_Progress.Clear();

            Load_Progress.Add(new double[] { -1, -1 });

            All_Transaktion_List_for_Load.Clear();

            Transaktion_List_Load_for_Load.Clear();

            Transaktion_List_from_Load.Clear();
        }

        public async Task Period_Popup()
        {
            try
            {

                if (String.IsNullOrEmpty(Search_Text) == true)
                {
                    var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                    Haushaltsbücher Haushaltsbucher = new Haushaltsbücher(transaktionscontent.ToList());

                    var result = await Shell.Current.ShowPopupAsync(new Viewtime_Popup(Current_Viewtime, Haushaltsbucher));

                    if (result == null)
                    {
                        return;
                    }

                    Current_Viewtime = (Viewtime)result;

                    await Refresh();
                }
                else
                {
                    await Notificater("Während Sie suchen können Sie nicht die Zeit verändern.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                Current_Viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };

                await Refresh();
            }
        }

        public async Task Filter_Methode()
        {
            try
            {
                await Shell.Current.ShowPopupAsync(new Filter_Popup());

                if (Preferences.Get("Filter_Activity", true) == true)
                {
                    Filter_ActivityColor = Color.Green;
                }
                else
                {
                    Filter_ActivityColor = Color.White;
                }

                await Refresh();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public void ShowCalculator_Methode()
        {
            if (Normal_State == true)
            {
                Normal_State = false;

                Calculator_State = true;

                Calculator_Value = "0";

                Calculator_Evaluate = Color.Gray;
            }
            else
            {
                Normal_State = true;

                Calculator_State = false;
            }

            Calculator_List.Clear();
        }

        public async Task ShowCalculator_List_Methode()
        {
            try
            {
                List<Transaktion> result = (List<Transaktion>)await Shell.Current.ShowPopupAsync(new CalculateListe_Popup(Calculator_List));

                Calculator_List = result;

                if (Calculator_List.Count() != 0)
                {
                    double sum = 0;

                    foreach (Transaktion trans in Calculator_List)
                    {
                        sum += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                    }

                    sum = Math.Round(sum, 2);

                    Calculator_Value = sum.ToString().Replace(".", ",");

                    if (sum < 0)
                    {
                        Calculator_Evaluate = Color.Red;
                    }
                    if (sum == 0)
                    {
                        Calculator_Evaluate = Color.White;
                    }
                    if (sum > 0)
                    {
                        Calculator_Evaluate = Color.Green;
                    }
                }
                else
                {
                    Calculator_Value = "0";

                    Calculator_Evaluate = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Calculator_Addition_Methode(Transaktion input)
        {
            try
            {
                if(input != null)
                {
                    bool indicator = true;

                    if(Calculator_List.Count() != 0)
                    {
                        foreach(Transaktion trans in Calculator_List)
                        {
                            if(trans.Id == input.Id)
                            {
                                indicator = false;
                            }
                        }
                    }


                    if (indicator == true)
                    {
                        Calculator_List.Add(input);

                        if(Calculator_List.Count() != 0)
                        {
                            double sum = 0;

                            foreach (Transaktion trans in Calculator_List)
                            {
                                sum += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                            }

                            sum = Math.Round(sum,2);

                            Calculator_Value = sum.ToString().Replace(".", ",");

                            if (sum < 0)
                            {
                                Calculator_Evaluate = Color.Red;
                            }
                            if (sum == 0)
                            {
                                Calculator_Evaluate = Color.White;
                            }
                            if (sum > 0)
                            {
                                Calculator_Evaluate = Color.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Calculator_Substraction_Methode(Transaktion input)
        {
            try
            {
                if (input != null)
                {
                    bool indicator = false;

                    if (Calculator_List.Count() != 0)
                    {
                        foreach (Transaktion trans in Calculator_List)
                        {
                            if (trans.Id == input.Id)
                            {
                                indicator = true;
                            }
                        }
                    }

                    if (indicator == true)
                    {
                        List<Transaktion> placeholder = new List<Transaktion>();

                        placeholder.Clear();

                        foreach (Transaktion trans in Calculator_List)
                        {
                            if (trans.Id != input.Id)
                            {
                                placeholder.Add(trans);
                            }
                        }

                        Calculator_List = placeholder;

                        if (Calculator_List.Count() != 0)
                        {
                            double sum = 0;

                            foreach (Transaktion trans in Calculator_List)
                            {
                                sum += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                            }

                            sum = Math.Round(sum, 2);

                            Calculator_Value = sum.ToString().Replace(".", ",");

                            if (sum < 0)
                            {
                                Calculator_Evaluate = Color.Red;
                            }
                            if (sum == 0)
                            {
                                Calculator_Evaluate = Color.White;
                            }
                            if (sum > 0)
                            {
                                Calculator_Evaluate = Color.Green;
                            }
                        }
                        else
                        {
                            Calculator_Value = "0";

                            Calculator_Evaluate = Color.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Calculator_RemoveLast_Methode()
        {
            try
            {
                if(Calculator_List.Count() != 0)
                {
                    Calculator_List.RemoveAt(Calculator_List.Count()-1);

                    if (Calculator_List.Count() != 0)
                    {
                        double sum = 0;

                        foreach (Transaktion trans in Calculator_List)
                        {
                            sum += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        sum = Math.Round(sum, 2);

                        Calculator_Value = sum.ToString().Replace(".", ",");

                        if (sum < 0)
                        {
                            Calculator_Evaluate = Color.Red;
                        }
                        if (sum == 0)
                        {
                            Calculator_Evaluate = Color.White;
                        }
                        if (sum > 0)
                        {
                            Calculator_Evaluate = Color.Green;
                        }
                    }
                    else
                    {
                        Calculator_Value = "0";

                        Calculator_Evaluate = Color.Gray;
                    }

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Calculator_RemoveAll_Methode()
        {
            try
            {
                if (Calculator_List.Count() != 0)
                {
                    Calculator_List.Clear();

                    Calculator_Value = "0";

                    Calculator_Evaluate = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private void Show_Letter_Saldo_Methode()
        {
            IsLetterSaldoVisibility = !IsLetterSaldoVisibility;

            if(IsLetterSaldoVisibility == true)
            {
                Height = 120;
            }
            else
            {
                Height = 80;
            }
        }

        private async Task Load_on_demand_Methode(string input)
        {
            try
            {
                List<Transaktion> output = new List<Transaktion>();

                if (input == null)
                {
                    if (Load_Progress[0][0] != All_Transaktion_List_for_Load.Count())
                    {
                        Load_Progress[0][0] = All_Transaktion_List_for_Load.Count;

                        Load_Progress[0][1] = Preferences.Get("Transaktion_per_load", 20.0);
                    }
                    else
                    {
                        Load_Progress[0][1] += Preferences.Get("Transaktion_per_load", 20.0);
                    }

                    if (Load_Progress[0][1] > Load_Progress[0][0])
                    {
                        Load_Progress[0][1] = Load_Progress[0][0];
                    }

                    output = All_Transaktion_List_for_Load.GetRange(0, (int)Load_Progress[0][1]);

                    Transaktion_List_from_Load = output;

                    if (Load_Progress[0][1] != Load_Progress[0][0])
                    {
                        output.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = null });
                    }
                }
                else
                {
                    if (!used_reasons_list.Contains(input))
                    {
                        return;
                    }

                    output.AddRange(Transaktion_List_Load_for_Load);

                    foreach (Transaktion trans in Transaktion_List_Load_for_Load.Where(ts => ts.Zweck == input).ToList())
                    {
                        output.Remove(trans);
                    }

                    if (Load_Progress[used_reasons_list.IndexOf(input)][0] != All_Transaktion_List_for_Load.Where(ts => ts.Zweck == input).Count())
                    {
                        Load_Progress[used_reasons_list.IndexOf(input)][0] = All_Transaktion_List_for_Load.Where(ts => ts.Zweck == input).Count();

                        Load_Progress[used_reasons_list.IndexOf(input)][1] = Preferences.Get("Transaktion_per_load", 20.0);
                    }
                    else
                    {
                        Load_Progress[used_reasons_list.IndexOf(input)][1] += Preferences.Get("Transaktion_per_load", 20.0);
                    }

                    if (Load_Progress[used_reasons_list.IndexOf(input)][1] > Load_Progress[used_reasons_list.IndexOf(input)][0])
                    {
                        Load_Progress[used_reasons_list.IndexOf(input)][1] = Load_Progress[used_reasons_list.IndexOf(input)][0];
                    }


                    output.AddRange(All_Transaktion_List_for_Load.Where(ts => ts.Zweck == input).ToList().GetRange(0, (int)Load_Progress[used_reasons_list.IndexOf(input)][1]));

                    if (Load_Progress[used_reasons_list.IndexOf(input)][1] != Load_Progress[used_reasons_list.IndexOf(input)][0])
                    {
                        output.Add(new Transaktion { Auftrags_id = "Load", Betrag = "0", Zweck = input, Datum = new DateTime(1999, 12, 31) });
                    }

                    Transaktion_List_Load_for_Load = output.ToList();
                }

                Transaktion.Clear();

                Transaktion.AddRange(output);

                await Add_to_Groups();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Load_Ratio_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Ladeverhalten\nAktuell : " + Preferences.Get("Transaktion_per_load", 20.0) + " Transaktionen pro Laden",380,new List<string>() { "20 Transaktion pro Laden", "30 Transaktion pro Laden", "40 Transaktion pro Laden", "50 Transaktion pro Laden" }));

                if (result != null)
                {
                    if ((string)result == "20 Transaktion pro Laden")
                    {
                        Preferences.Set("Transaktion_per_load", 20.0);
                    }
                    if ((string)result == "30 Transaktion pro Laden")
                    {
                        Preferences.Set("Transaktion_per_load", 30.0);
                    }
                    if ((string)result == "40 Transaktion pro Laden")
                    {
                        Preferences.Set("Transaktion_per_load", 40.0);
                    }
                    if ((string)result == "50 Transaktion pro Laden")
                    {
                        Preferences.Set("Transaktion_per_load", 50.0);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Budget_Methode()
        {
            try
            {
                if (Current_Viewtime.Month != "")
                {
                    List<Transaktion> transaktionlist = new List<Transaktion>();

                    var allbudget = await BudgetService.Get_all_Budget();

                    var enablereason = await ReasonService.Get_Enable_ReasonDictionary_sorted();

                    List<string> enablereasonlist = new List<string>();

                    var transaktioncontent = await ContentService.Get_all_enabeled_Transaktion();

                    if (enablereason.Count() != 0)
                    {
                        foreach (var value in enablereason)
                        {
                            if (value.Value == "Ausgaben")
                            {
                                enablereasonlist.Add(value.Key);
                            }
                        }
                    }

                    if (transaktioncontent.Count() != 0)
                    {
                        if (allbudget.Count() != 0)
                        {
                            List<Budget> budgetlist = new List<Budget>();

                            foreach (Budget bg in allbudget)
                            {
                                bg.Current = 0;

                                if (enablereasonlist.Contains(bg.Name) == true || bg.Name == "Monat")
                                {
                                    budgetlist.Add(bg);
                                }
                            }

                            budgetlist.Reverse();

                            allbudget = budgetlist;
                        }

                        foreach (var transaktion in transaktioncontent)
                        {
                            if (transaktion.Datum.Year == Current_Viewtime.Year)
                            {
                                if (transaktion.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Current_Viewtime.Month)
                                {
                                    if(transaktion.Saldo_Visibility == true)
                                    {
                                        transaktionlist.Add(transaktion);

                                        if (allbudget.Count() != 0)
                                        {
                                            foreach (Budget budget in allbudget)
                                            {
                                                if (transaktion.Zweck == budget.Name)
                                                {
                                                    budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                                }

                                                if (budget.Name == "Monat")
                                                {
                                                    if (double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                                    {
                                                        budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        if (allbudget.Where(bg => bg.Name == "Monat").Count() != 0)
                        {
                            if (allbudget.Where(bg => bg.Name == "Monat").First() != null)
                            {
                                List<string> diabled_reaons = new List<string>();

                                if (allbudget.Where(bg => bg.Name == "Monat").First().Name_Of_Enabled_Reasons == null)
                                {
                                    var result0 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Monat eingrenzen", 350, 250, "Ja", "Nein", "Wollen Sie bestimmte Zwecke aus dem Monats-Budget entfernen?"));

                                    if (result0 != null)
                                    {
                                        if ((bool)result0 == true)
                                        {
                                            bool indkator = false;

                                            while (indkator == false)
                                            {
                                                var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Zweck der ausgeschlossen werden sollen", 400, enablereasonlist));

                                                if (result == null)
                                                {
                                                    indkator = true; break;
                                                }
                                                else
                                                {
                                                    if (diabled_reaons.Contains((string)result) == true)
                                                    {
                                                        var result1 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup((string)result, 300, 300, "Ja", "Nein", "Wollen Sie den Zweck " + (string)result + " wieder in das Monats-Budget einführen?"));

                                                        if (result1 != null)
                                                        {
                                                            if ((bool)result1 == true)
                                                            {
                                                                diabled_reaons.Remove((string)result);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        diabled_reaons.Add((string)result);
                                                    }
                                                }
                                            }

                                            if (diabled_reaons.Count() != 0)
                                            {
                                                allbudget.Where(bg => bg.Name == "Monat").First().Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                            }
                                            else
                                            {
                                                diabled_reaons.Add("leer");

                                                allbudget.Where(bg => bg.Name == "Monat").First().Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                            }
                                        }
                                        else
                                        {
                                            allbudget.Where(bg => bg.Name == "Monat").First().Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                        }
                                    }
                                    else
                                    {
                                        allbudget.Where(bg => bg.Name == "Monat").First().Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                    }
                                }
                                else
                                {
                                    diabled_reaons = Budget_Konverter.Deserilize(allbudget.Where(bg => bg.Name == "Monat").First());
                                }

                                if (diabled_reaons.Count() != 0)
                                {
                                    allbudget.Where(bg => bg.Name == "Monat").First().Current = 0.0;

                                    foreach (Transaktion trans in transaktionlist)
                                    {
                                        if (double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                        {
                                            if (diabled_reaons.Contains(trans.Zweck) == false)
                                            {
                                                allbudget.Where(bg => bg.Name == "Monat").First().Current += Math.Abs(double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                            }
                                        }
                                    }
                                }

                                await BudgetService.Edit_Budget(allbudget.Where(bg => bg.Name == "Monat").First());
                            }
                        }
                    }

                    await Shell.Current.ShowPopupAsync(new Budget_Popup(allbudget.ToList(), Current_Viewtime, transaktionlist));
                }
                else
                {
                    await Notificater("Um das Budget einsehen zu können, müssen Sie sich in einem Monat befinden.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Change_Saldo_Date_Methode()
        {
            try
            {
                var transaktioncontent = await ContentService.Get_all_enabeled_Transaktion();

                if (transaktioncontent.Count() != 0)
                {
                    DateTime lastdate = DateTime.Now;

                    DateTime firstdate = DateTime.Now;

                    transaktioncontent = (from p in transaktioncontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();

                    lastdate = transaktioncontent.First().Datum;

                    firstdate = transaktioncontent.Last().Datum;

                    var result = await Shell.Current.ShowPopupAsync(new Change_Saldo_Date_Popup(firstdate, lastdate, DateTime.ParseExact(Saldo_Date, "dddd, d.M.yyyy", new CultureInfo("de-DE"))));

                    if (result != null)
                    {
                        DateTime currentdate = (DateTime)result;

                        double saldo = 0;

                        double letter_saldo = 0;

                        Saldo_Date = currentdate.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));

                        Preferences.Set("Saldo_Date", Saldo_Date);

                        foreach (var trans in transaktioncontent)
                        {
                            if (DateTime.Compare(trans.Datum, currentdate.AddDays(1).AddSeconds(-1)) <= 0)
                            {
                                if (trans.Saldo_Visibility == true)
                                {
                                    saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                                else
                                {
                                    letter_saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                            }
                        }

                        if (saldo != 0)
                        {
                            saldo = Math.Round(saldo, 2);

                            Saldo_Value = saldo.ToString().Replace(".", ",");

                            IsSaldoVisibility = true;

                            if (saldo < 0)
                            {
                                Saldo_Evaluate = Color.Red;
                            }
                            if (saldo == 0)
                            {
                                Saldo_Evaluate = Color.White;
                            }
                            if (saldo > 0)
                            {
                                Saldo_Evaluate = Color.Green;
                            }
                        }
                        else
                        {
                            Saldo_Value = null;

                            if(transaktioncontent.Count() == 0)
                            {
                                IsSaldoVisibility = false;
                            }
                        }

                        if (letter_saldo != 0)
                        {
                            letter_saldo = Math.Round(letter_saldo, 2);

                            Letter_Saldo_Value = letter_saldo.ToString().Replace(".", ",");

                            if (letter_saldo < 0)
                            {
                                Letter_Saldo_Evaluate = Color.Red;
                            }
                            if (letter_saldo == 0)
                            {
                                Letter_Saldo_Evaluate = Color.White;
                            }
                            if (letter_saldo > 0)
                            {
                                Letter_Saldo_Evaluate = Color.Green;
                            }
                        }
                        else
                        {
                            Letter_Saldo_Value = "0";
                        }


                        if (Saldo_Date != DateTime.Today.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE")))
                        {
                            Preferences.Set("Is_Saldo_Date_Changed", true);
                        }
                        else
                        {
                            Preferences.Set("Is_Saldo_Date_Changed", false);
                        }
                    }
                }
                else
                {
                    await Notificater("Es gibt keine Transaktionen, wo man den Stand bestimmen kann.");
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Notificater(string v)
        {
            await ToastHelper.ShowToast(v);
        }



        public ObservableRangeCollection<Suggestion> suggestions;

        public ObservableRangeCollection<Suggestion> SuggestionCollection
        {
            get { return suggestions; }
            set
            {
                if (suggestions == value)
                {
                    return;
                }
                suggestions = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<Transaktion> transaktions;

        public ObservableRangeCollection<Transaktion> Transaktion
        {
            get { return transaktions; }
            set
            {
                if (transaktions == value)
                {
                    return;
                }
                transaktions = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<Grouping<string, Transaktion>> transaktionGroups { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public AsyncCommand RefreshCommand { get; }
        public Command ViewIsDisappearing_Command { get; }
        public AsyncCommand<Transaktion> EditCommand { get; }

        public AsyncCommand<Transaktion> RemoveCommand { get; }

        public AsyncCommand GroupingOption_Command { get; }

        public AsyncCommand Search_Command { get; }

        public AsyncCommand Search_Command2 { get; }

        public AsyncCommand Add_Command { get; }

        public AsyncCommand Set_Searchbar_Visibility_Command { get; }

        public AsyncCommand The_Searchbar_is_Tapped { get; }

        public AsyncCommand<Suggestion> Delet_Suggestion { get; }

        public AsyncCommand<Suggestion> Select_Suggestion { get; }

        public AsyncCommand Clear_SearchText_Command { get; }

        public AsyncCommand Period_Command { get; }

        public AsyncCommand Filter_Command { get; }

        public AsyncCommand<Transaktion> Calculator_Addition_Command { get; }

        public AsyncCommand<Transaktion> Calculator_Substraction_Command { get; }

        public Command ShowCalculator_Command { get; }

        public AsyncCommand Calculator_RemoveLast_Command { get; }

        public AsyncCommand Calculator_RemoveAll_Command { get; }

        public AsyncCommand ShowCalculator_List_Command { get; }

        public Command Show_Letter_Saldo_Command { get; }

        public AsyncCommand<string> Load_on_demand_Command { get; }

        public AsyncCommand Load_Ratio_Command { get; }

        public AsyncCommand Add_Catchphrase_To_Search_Command { get; }

        public AsyncCommand Budget_Command { get; }

        public AsyncCommand Change_Saldo_Date_Command { get; }

        public bool serchbar_visibility = false;
        public bool Serchbar_Visibility
        {
            get { return serchbar_visibility; }
            set
            {

                if (serchbar_visibility == value)
                {
                    return;
                }

                serchbar_visibility = value; RaisePropertyChanged();
            }
        }

        public Viewtime current_viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };
        public Viewtime Current_Viewtime
        {
            get { return current_viewtime; }
            set
            {
                if (current_viewtime == value)
                {
                    return;
                }

                current_viewtime = value; RaisePropertyChanged();
            }
        }

        public bool isbusy = false;
        public bool IsBusy
        {
            get { return isbusy; }
            set
            {
                if (isbusy == value)
                {
                    return;
                }

                isbusy = value; RaisePropertyChanged();
            }
        }

        public bool is_aktiv = false;
        public bool Is_Aktiv
        {
            get { return is_aktiv; }
            set
            {
                if (Is_Aktiv == value)
                {
                    return;
                }

                is_aktiv = value; RaisePropertyChanged();
            }
        }

        public int groupingoption = 0;
        public int GroupingOption
        {
            get { return groupingoption; }
            set
            {
                if (groupingoption == value)
                {
                    return;
                }

                groupingoption = value; RaisePropertyChanged();
            }
        }

        public string title = "Haushaltsbuch";
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                {
                    return;
                }

                title = value; RaisePropertyChanged();
            }
        }

        public string search_text;
        public string Search_Text
        {
            get { return search_text; }
            set
            {
                if (search_text == value)
                {
                    return;
                }

                search_text = value; RaisePropertyChanged();
            }
        }

        public string Last_Search_Text;

        public string saldo_value = null;
        public string Saldo_Value
        {
            get { return saldo_value; }
            set
            {
                if (Saldo_Value == value)
                {
                    return;
                }

                saldo_value = value; RaisePropertyChanged();
            }
        }

        public string letter_saldo_value = null;
        public string Letter_Saldo_Value
        {
            get { return letter_saldo_value; }
            set
            {
                if (Letter_Saldo_Value == value)
                {
                    return;
                }

                letter_saldo_value = value; RaisePropertyChanged();
            }
        }

        public string saldo_date = Preferences.Get("Saldo_Date", DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"))); public string Saldo_Date
        {
            get { return saldo_date; }
            set
            {
                if (Saldo_Date == value)
                {
                    return;
                }

                saldo_date = value; RaisePropertyChanged();
            }
        }

        public string calculator_value = null;
        public string Calculator_Value
        {
            get { return calculator_value; }
            set
            {
                if (Calculator_Value == value)
                {
                    return;
                }

                calculator_value = value; RaisePropertyChanged();
            }
        }

        public Color calculator_evaluate = Color.White;
        public Color Calculator_Evaluate
        {
            get { return calculator_evaluate; }
            set
            {
                if (Calculator_Evaluate == value)
                {
                    return;
                }

                calculator_evaluate = value; RaisePropertyChanged();
            }
        }

        public Color saldo_evaluate = Color.White;
        public Color Saldo_Evaluate
        {
            get { return saldo_evaluate; }
            set
            {
                if (Saldo_Evaluate == value)
                {
                    return;
                }

                saldo_evaluate = value; RaisePropertyChanged();
            }
        }

        public Color letter_saldo_evaluate = Color.White;
        public Color Letter_Saldo_Evaluate
        {
            get { return letter_saldo_evaluate; }
            set
            {
                if (Letter_Saldo_Evaluate == value)
                {
                    return;
                }

                letter_saldo_evaluate = value; RaisePropertyChanged();
            }
        }

        public Color filter_activitycolor = Color.White;

        public Color Filter_ActivityColor
        {
            get { return filter_activitycolor; }
            set
            {
                if (Filter_ActivityColor == value)
                {
                    return;
                }

                filter_activitycolor = value; RaisePropertyChanged();
            }
        }

        public bool list_of_suggestion_Status = false;
        public bool List_of_Suggestion_Status
        {
            get { return list_of_suggestion_Status; }
            set
            {
                if (List_of_Suggestion_Status == value)
                    return;
                list_of_suggestion_Status = value; RaisePropertyChanged();
                if (list_of_suggestion_Status == true)
                {
                    Title = "Suchverlauf";
                }
            }
        }

        public bool list_of_transaktion_Status = true;
        public bool List_of_Transaktion_Status
        {
            get { return list_of_transaktion_Status; }
            set
            {
                if (List_of_Transaktion_Status == value)
                    return;
                list_of_transaktion_Status = value; RaisePropertyChanged();
                if (list_of_transaktion_Status == true)
                {
                    Title = "Haushaltsbuch";
                }
            }
        }

        public bool kein_ergebnis_suggestion_Status = false;
        public bool Kein_Ergebnis_Suggestion_Status
        {
            get { return kein_ergebnis_suggestion_Status; }
            set
            {
                if (Kein_Ergebnis_Suggestion_Status == value)
                    return;
                kein_ergebnis_suggestion_Status = value; RaisePropertyChanged();
                if (kein_ergebnis_suggestion_Status == true)
                {
                    Title = "Suchverlauf";
                }
            }
        }

        public bool kein_ergebnis_transaktion_Status = false;
        public bool Kein_Ergebnis_Transaktion_Status
        {
            get { return kein_ergebnis_transaktion_Status; }
            set
            {
                if (Kein_Ergebnis_Transaktion_Status == value)
                    return;
                kein_ergebnis_transaktion_Status = value; RaisePropertyChanged();
                if (kein_ergebnis_transaktion_Status == true)
                {
                    Title = "Haushaltsbuch";
                }
            }
        }

        public bool issaldovisibility = false;
        public bool IsSaldoVisibility
        {
            get { return issaldovisibility; }
            set
            {
                if (IsSaldoVisibility == value)
                    return;
                issaldovisibility = value; RaisePropertyChanged();
            }
        }

        public bool islettersaldovisibility = false;
        public bool IsLetterSaldoVisibility
        {
            get { return islettersaldovisibility; }
            set
            {
                if (IsLetterSaldoVisibility == value)
                    return;
                islettersaldovisibility = value; RaisePropertyChanged();
            }
        }

        public bool calculator_state = false;
        public bool Calculator_State
        {
            get { return calculator_state; }
            set
            {
                if (Calculator_State == value)
                    return;
                calculator_state = value; RaisePropertyChanged();
            }
        }

        public bool normal_state = true;
        public bool Normal_State
        {
            get { return normal_state; }
            set
            {
                if (Normal_State == value)
                    return;
                normal_state = value; RaisePropertyChanged();
            }
        }

        public int height = 80;
        public int Height
        {
            get { return height; }
            set
            {
                if (Height == value)
                    return;
                height = value; RaisePropertyChanged();
            }
        }

        public List<Transaktion> Calculator_List = new List<Transaktion>();

        public List<double[]> Load_Progress = new List<double[]>() { new double[] { -1, -1 } };

        public List<Transaktion> All_Transaktion_List_for_Load = new List<Transaktion>();

        public List<Transaktion> Transaktion_List_Load_for_Load = new List<Transaktion>();

        public List<Transaktion> Transaktion_List_from_Load = new List<Transaktion>();

        public List<string> used_reasons_list = new List<string>();
    }
}
