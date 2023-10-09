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

namespace EasyLife.PageModels
{
    public class Home_PageModel : FreshBasePageModel
    {
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

        public AsyncCommand Period_Command { get; }

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

        public Suggestion selected_suggestion;
        public Suggestion Selected_Suggestion
        {
            get { return selected_suggestion; }
            set
            {
                if (selected_suggestion == value)
                {
                    return;
                }

                selected_suggestion = value;

                if (selected_suggestion != null)
                {
                    Search_Text = selected_suggestion.Suggestion_value;
                    Selected_Suggestion = null;
                    Suggestion_SelectedMode = ListViewSelectionMode.None;
                }
            }
        }

        public ListViewSelectionMode suggestion_selectedMode = ListViewSelectionMode.Single;
        public ListViewSelectionMode Suggestion_SelectedMode
        {
            get { return suggestion_selectedMode; }
            set
            {

                if (suggestion_selectedMode == value)
                {
                    return;
                }

                suggestion_selectedMode = value; RaisePropertyChanged();
                Suggestion_SelectedMode = ListViewSelectionMode.Single;
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

        public string saldo_date = DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
        public string Saldo_Date
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

        public bool activityindicator_isrunning = false;
        public bool ActivityIndicator_IsRunning
        {
            get { return activityindicator_isrunning; }
            set
            {
                if (ActivityIndicator_IsRunning == value)
                    return;
                activityindicator_isrunning = value; RaisePropertyChanged();
            }
        }

        public bool activityindicator_isvisible = false;
        public bool ActivityIndicator_IsVisible
        {
            get { return activityindicator_isvisible; }
            set
            {
                if (ActivityIndicator_IsVisible == value)
                    return;
                activityindicator_isvisible = value; RaisePropertyChanged();
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
            Set_Searchbar_Visibility_Command = new AsyncCommand(Set_Searchbar_Visibility_MethodeAsync);
            The_Searchbar_is_Tapped = new AsyncCommand(The_Searchbar_is_Tapped_Methode);
            Add_Command = new AsyncCommand(Add_Methode);

            Period_Command = new AsyncCommand(Period_Popup);

            title = "Haushaltsbuch " + Current_Viewtime.Year + " " + Current_Viewtime.Month + "";
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
                    var groups1 = await ReasonService.Get_all_Reason();

                    List<string> groups = new List<string>();

                    foreach (var group in groups1)
                    {
                        groups.Add((group.Benutzerdefinierter_Zweck.Substring(0, group.Benutzerdefinierter_Zweck.IndexOf(":"))));
                    }

                    groups.Sort();

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
                        if (tr.Datumanzeige != tr1)
                        {
                            groups.Add(tr.Datumanzeige);
                            tr1 = tr.Datumanzeige;
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
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        async Task GroupingOption_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Grouping_Popup(GroupingOption));

                if(result == null)
                {
                    return;
                }

                GroupingOption = (int)result;

                await Refresh();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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

                    string message = null;

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
                        var result = await Shell.Current.DisplayActionSheet("Entfernen", "Zurück", null, new string[] { "Diese Transaktion entfernen", "Alle mit dieser Auftrag-ID entfernen"});

                        if (result == "Diese Transaktion entfernen")
                        {
                            bool value = await Shell.Current.DisplayAlert("Entfernen", "Wollen Sie diese Transaktion wirklich entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "", "Ja", "Nein");
                            if (value == true)
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
                            else
                            {
                                return;
                            }
                        }
                        if (result == "Alle mit dieser Auftrag-ID entfernen")
                        {
                            if(item.Auftrags_Option == 1)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }

                            bool value = await Shell.Current.DisplayAlert("Entfernen", message , "Ja", "Nein");
                            if (value == true)
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
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        var result = await Shell.Current.DisplayActionSheet("Entfernen", "Zurück", null, new string[] { "Diese Transaktion entfernen","Alle mit dieser Auftrag-ID entfernen", "Alle mit dieser Auftrag-ID ab dieser Transaktion entfernen" });

                        if (result == "Diese Transaktion entfernen")
                        {
                            bool value = await Shell.Current.DisplayAlert("Entfernen", "Wollen Sie diese Transaktion wirklich entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "", "Ja", "Nein");
                            if (value == true)
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
                            else
                            {
                                return;
                            }
                        }
                        if (result == "Alle mit dieser Auftrag-ID entfernen")
                        {
                            if (item.Auftrags_Option == 1)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = "Wollen Sie wirklich alle Transaktionen mit diese Auftrag-ID entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }

                            bool value = await Shell.Current.DisplayAlert("Entfernen", message, "Ja", "Nein");
                            if (value == true)
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
                            else
                            {
                                return;
                            }
                        }
                        if (result == "Alle mit dieser Auftrag-ID ab dieser Transaktion entfernen")
                        {
                            if (item.Auftrags_Option == 1)
                            {
                                message = "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 2)
                            {
                                message = "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                            }
                            if (item.Auftrags_Option == 3)
                            {
                                message = "Wollen Sie wirklich alle folgenden Transaktionen mit dieser Auftrag-Id entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "\n\nAuftragsdetails\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                            }

                            bool value = await Shell.Current.DisplayAlert("Entfernen", message , "Ja", "Nein");
                            if (value == true)
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
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    bool value = await Shell.Current.DisplayAlert("Entfernen", "Wollen Sie diese Transaktion wirklich entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "", "Ja", "Nein");
                    if (value == true)
                    {
                        item.Content_Visibility = false;
                        await ContentService.Edit_Transaktion(item);
                        await Refresh();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
                        var result = await Shell.Current.DisplayActionSheet("Bearbeiten", "Zurück", null, new string[] { "Diese Transaktion bearbeiten", "Alle mit dieser Auftrags-ID bearbeiten" , "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten"});

                        if (result == "Diese Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=1");

                        }

                        if (result == "Alle mit dieser Auftrags-ID bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=2");
                        }

                        if(result == "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten")
                        {
                            var result1 = await Shell.Current.ShowPopupAsync(new Timespane_Popup(item,transaktion_list3));

                            if((bool)result1 == true)
                            {
                                await Refresh();
                            }
                            else
                            {
                                await Notificater("Bei der Versetzung der Transaktionen um ein bestimmte Zeit ist ein Fehler aufgetretten.");
                            }
                        }

                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        var result = await Shell.Current.DisplayActionSheet("Bearbeiten", "Zurück", null, new string[] { "Diese Transaktion bearbeiten", "Alle mit dieser Auftrags-ID bearbeiten", "Alle mit dieser Auftrags-ID ab dieser Transaktion bearbeiten", "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten" });

                        if (result == "Diese Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=1");
                        }

                        if (result == "Alle mit dieser Auftrags-ID bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=2");
                        }

                        if (result == "Alle mit dieser Auftrags-ID ab dieser Transaktion bearbeiten" && item != null)
                        {
                            await PassingTransaktionService.Remove_All_Transaktion();

                            await PassingOrderService.Remove_All_Order();

                            await Shell.Current.GoToAsync($"{nameof(Edit_Item_With_Order_Page)}?TransaktionID={item.Id}&OrderID={item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))}&EditID=3");
                        }

                        if (result == "Alle mit dieser Auftrags-ID um eine bestimmte Zeit versetzten")
                        {
                            var result1 = await Shell.Current.ShowPopupAsync(new Timespane_Popup(item, transaktion_list3));

                            if(result == null)
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

                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    bool value = await Application.Current.MainPage.DisplayAlert("Bearbeiten", "Wollen Sie wirklich diese Transaktionen bearbeiten?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:" + item.Balance_Visibility_String + "\nID: " + item.Id + "", "Ja", "Nein");

                    if (value == true && item != null)
                    {
                        await PassingTransaktionService.Remove_All_Transaktion();

                        await PassingOrderService.Remove_All_Order();

                        await Shell.Current.GoToAsync($"{nameof(Edit_Item_Page)}?TransaktionID={item.Id}");
                    }

                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                await PassingOrderService.Remove_All_Order();

                await PassingTransaktionService.Remove_All_Transaktion();
            }
        }

        async Task Search_Methode()
        {
            try
            {
                ActivityIndicator_IsVisible = true;

                ActivityIndicator_IsRunning = true;

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

                await Search_Methode2();

                List<Transaktion> search_transaktionscontent = new List<Transaktion>();

                search_transaktionscontent.Clear();

                if (Search_Text.Contains("-") == true)
                {
                    string New_Search_Text = Search_Text;

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
                            Tag = New_Search_Text.Substring(0, New_Search_Text.IndexOf("-"));

                            New_Search_Text = New_Search_Text.Substring(New_Search_Text.IndexOf("-") + 1);
                        }

                        if (String.IsNullOrEmpty(Tag) == false)
                        {
                            new_search_transaktioncontent = search_transaktionscontent.Where(s => s.Search_Indicator().ToUpper().Contains(Tag.ToUpper())).ToList();

                            search_transaktionscontent = new_search_transaktioncontent;
                        }
                    }
                }
                else
                {
                    search_transaktionscontent = transaktionscontent.Where(s => s.Search_Indicator().ToUpper().Contains(Search_Text.ToUpper())).ToList();
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

                    ActivityIndicator_IsRunning = false;

                    ActivityIndicator_IsVisible = false;

                    return;
                }
                else
                {
                    Kein_Ergebnis_Transaktion_Status = false;

                    Kein_Ergebnis_Suggestion_Status = false;

                    List_of_Suggestion_Status = false;

                    List_of_Transaktion_Status = true;

                    Transaktion.AddRange(search_transaktionscontent);
                }

                await Add_to_Groups();

                ActivityIndicator_IsRunning = false;

                ActivityIndicator_IsVisible = false;

                await SearchSuggestionService.Add_Suggestion(Search_Text);

                SuggestionCollection.Clear();

                SuggestionCollection.AddRange(await SearchSuggestionService.Get_all_Suggestion());
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
                        Transaktion.Clear();

                        var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                        List<Transaktion> sorted_after_month_transaktionscontent = new List<Transaktion>();

                        sorted_after_month_transaktionscontent.Clear();

                        foreach (var trans in transaktionscontent)
                        {
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

                        Transaktion.AddRange(sorted_after_month_transaktionscontent);

                        await Add_to_Groups();


                        if (Transaktion.Count() == 0)
                        {
                            Kein_Ergebnis_Transaktion_Status = true;

                            Kein_Ergebnis_Suggestion_Status = false;

                            List_of_Transaktion_Status = false;
                        }
                        else
                        {
                            Kein_Ergebnis_Suggestion_Status = false;
                            Kein_Ergebnis_Transaktion_Status = false;
                            List_of_Transaktion_Status = true;
                        }
                        ActivityIndicator_IsRunning = false;

                        ActivityIndicator_IsVisible = false;

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

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
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

                if(transaktionscontent.Count() != 0)
                {
                    foreach (var trans in transaktionscontent)
                    {
                        if (DateTime.Compare(trans.Datum, DateTime.Today.AddDays(1).AddSeconds(-1)) <= 0)
                        {
                            saldo += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
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

                Saldo_Date = DateTime.Now.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));

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
                    IsSaldoVisibility = false;
                }

                sorted_after_month_transaktionscontent = (from p in sorted_after_month_transaktionscontent orderby DateTime.ParseExact(p.Datumanzeige, "dddd, d.M.yyyy", new CultureInfo("de-DE")) descending select p).ToList();

                Transaktion.AddRange(sorted_after_month_transaktionscontent);

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
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                IsBusy = false;
            }
        }

        private void ViewIsDisappearing_Methode()
        {
            Is_Aktiv = false;
        }

        public async Task Period_Popup()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Viewtime_Popup(Current_Viewtime));

                if(result == null)
                {
                    return;
                }

                Current_Viewtime = (Viewtime)result;

                await Refresh();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Current_Viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };

                await Refresh();
            }
        }

        private async Task Notificater(string v)
        {
            await ToastHelper.ShowToast(v);
        }
    }
}
