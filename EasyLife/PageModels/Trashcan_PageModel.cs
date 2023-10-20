using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;
using static iText.Svg.SvgConstants;

namespace EasyLife.PageModels
{
    /// <summary>
    /// Das ist eine Funktionsklasse für die Papierkorbseite.
    /// </summary>
    public class Trashcan_PageModel : FreshBasePageModel
    {
        public ObservableRangeCollection<Transaktion> transaktions;

        public ObservableRangeCollection<Transaktion> Transaktion1
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

        public AsyncCommand RefreshCommand { get; }

        public AsyncCommand<Transaktion> ReviveCommand { get; }

        public AsyncCommand<Transaktion> RemoveCommand { get; }

        public MvvmHelpers.Commands.Command ViewIsDisappearing_Command { get; }

        public AsyncCommand View_Appering_Command { get; }
        public AsyncCommand Empty_Command { get; }

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

        public bool list_of_transaktion_Status = true;
        public bool List_of_Transaktion_Status
        {
            get { return list_of_transaktion_Status; }
            set
            {
                if (List_of_Transaktion_Status == value)
                    return;
                list_of_transaktion_Status = value; RaisePropertyChanged();
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

        public Trashcan_PageModel()
        {
            Transaktion1 = new ObservableRangeCollection<Transaktion>();

            transaktionGroups = new ObservableRangeCollection<Grouping<string, Transaktion>>();

            RefreshCommand = new AsyncCommand(Refresh);
            RemoveCommand = new AsyncCommand<Transaktion>(Remove);
            ReviveCommand = new AsyncCommand<Transaktion>(Revive);
            Empty_Command = new AsyncCommand(Empty_Methode);
            ViewIsDisappearing_Command = new MvvmHelpers.Commands.Command(ViewIsDisappearing_Methode);
            View_Appering_Command = new AsyncCommand(View_Appering_Methode);
        }

        /// <summary>
        /// Löscht alle Transaktionen díe sich im Papoerkorb befinden.
        /// </summary>
        /// <returns></returns>
        public async Task Empty_Methode()
        {
            try
            {
                /// Wenn es keine Transaktionen gibt 
                if(Transaktion1.Count == 0)
                {
                    await ToastHelper.ShowToast("Es gibt keine Transaktionen die entgültig gelöscht werden können.");
                    return;
                }

                /// Wenn es Transaktionen gibt
                bool value = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Warnung!", "Wollen Sie wirklich alle Transaktionen im Papierkorb entfernen?", "Ja", "Nein");

                /// Wenn die Antwort Ja ist, dann sollen alle Transaktionen im Papierkorb gelöscht werden
                /// Es zeigt das Ladezeichen an solange die Transaktionen gelöscht werden
                /// Es geht durch die Liste an Transaktionen die im Papierkorb sind durch und löscht jede einzelne
                if (value == true)
                {
                    IsBusy = true;

                    foreach (Transaktion trans in Transaktion1)
                    {
                        if (trans != null)
                        {
                            if (trans.Auftrags_id != null)
                            {
                                List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_disabeled_Transaktion());

                                foreach (Transaktion transs in all)
                                {
                                    if(String.IsNullOrEmpty(transs.Auftrags_id) == false)
                                    {
                                        if (transs.Auftrags_id.Substring(0, transs.Auftrags_id.IndexOf(".")) == trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")))
                                        {
                                            await ContentService.Remove_Transaktion(transs);
                                            continue;
                                        }
                                    }
                                }

                                all.Clear();

                                all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                                var result1 = await ContentService.Get_all_disabeled_Transaktion();

                                if (result1.Count() != 0)
                                {
                                    all.AddRange(result1);
                                }

                                int count = 0;

                                try
                                {
                                    foreach (Transaktion transa in all)
                                    {
                                        if (transa.Auftrags_id.Substring(0, transa.Auftrags_id.IndexOf(".")) == trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")))
                                        {
                                            count++;
                                        }
                                    }
                                }
                                catch
                                {
                                    count = 1;
                                }

                                if (count == 0)
                                {
                                    Notification notification = await NotificationService.Get_specific_Notification_with_Order_ID(int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))));

                                    LocalNotificationCenter.Current.Cancel(notification.Notification_ID);

                                    await NotificationService.Remove_Notification(notification);

                                    await OrderService.Remove_Order(await OrderService.Get_specific_Order(notification.Auftrags_ID));
                                }
                            }
                            else
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }
                    }

                    await Refresh();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        /// <summary>
        /// Gruppiert Transaktionen mit gleichen Aufträgen und erstellt eine Collektion an Transaktionen.
        /// </summary>
        /// <returns></returns>
        private async Task Add_to_GroupsAsync()
        {
            try
            {
                /// Es löscht die bestehende Collektion an Transaktionen und erstellt sie neu
                /// Falls eine Transaktion mit einem Auftrag verbunnden ist, dann wird sie zu "Transaktion mit Auftrag" sotiert
                /// Falls eine Transkation mit keinem Auftrag verbunden ist, dann wird sie zu "Einzelne Transaktion" sotiert
                transaktionGroups.Clear();

                if (Transaktion1.Where(ts => ts.Auftrags_id == null).Count() != 0)
                {
                    transaktionGroups.Add(new Grouping<string, Transaktion>("Einzelne Transaktion", Transaktion1.Where(ts => ts.Auftrags_id == null)));
                    if (Transaktion1.Where(ts => ts.Auftrags_id != null).Count() != 0)
                    {
                        transaktionGroups.Add(new Grouping<string, Transaktion>("Transaktionen mit Auftrag", Transaktion1.Where(ts => ts.Auftrags_id != null)));
                    }
                }
                else
                {
                    if (Transaktion1.Where(ts => ts.Auftrags_id != null).Count() != 0)
                    {
                        transaktionGroups.Add(new Grouping<string, Transaktion>("Transaktionen mit Auftrag", Transaktion1.Where(ts => ts.Auftrags_id != null)));
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        /// <summary>
        /// Löscht die Transaktion oder die Transaktionen die zum gleichen Auftrag gehören und sich im Papierkorb befinden.
        /// </summary>
        /// <param name="item">Die Transaktion oder das Transaktionsbündel was ausgewählt wurde.</param>
        /// <returns></returns>
        async Task Remove(Transaktion item)
        {
            try
            {
                /// Wenn die Transkation mit einem Auftrag verbunden ist, dann werden alle Transaktionen die den gleichen Auftrage besitzen und sich im Papierkorb befinden auch gelöscht
                /// Sonst wird die einzelne Transaktion gelöscht
                if (item.Auftrags_id != null)
                {
                    string message = null;

                    if (item.Auftrags_Option == 1)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                    }
                    if (item.Auftrags_Option == 2)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                    }
                    if (item.Auftrags_Option == 3)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                    }

                    bool value = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Warnung!", message , "Ja", "Nein");

                    if (value == true)
                    {
                        IsBusy = true;

                        List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_disabeled_Transaktion());

                        foreach (Transaktion trans in all)
                        {
                            try
                            {
                                bool validate = true;

                                if (int.Parse(item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))) != int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))))
                                {
                                    validate = false;
                                }
                                if (item.Anzahl_an_Wiederholungen != trans.Anzahl_an_Wiederholungen)
                                {
                                    validate = false;
                                }
                                if (item.Art_an_Wiederholungen != trans.Art_an_Wiederholungen)
                                {
                                    validate = false;
                                }
                                if (item.Auftrags_Option != trans.Auftrags_Option)
                                {
                                    validate = false;
                                }
                                if (item.Speziell != trans.Speziell)
                                {
                                    validate = false;
                                }

                                if (validate == true)
                                {
                                    await ContentService.Remove_Transaktion(trans);
                                    continue;
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        all.Clear();

                        all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        var result1 = await ContentService.Get_all_disabeled_Transaktion();

                        if (result1.Count() != 0)
                        {
                            all.AddRange(result1);
                        }

                        int count = 0;

                        try
                        {
                            foreach(Transaktion trans in all)
                            {
                                if(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf(".")))
                                {
                                    count++;
                                }
                            }
                        }
                        catch
                        {
                            count = 1;
                        }

                        if ( count == 0)
                        {
                            Notification notification = await NotificationService.Get_specific_Notification_with_Order_ID(int.Parse(item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))));

                            LocalNotificationCenter.Current.Cancel(notification.Notification_ID);

                            await OrderService.Remove_Order(await OrderService.Get_specific_Order(notification.Auftrags_ID));

                            await NotificationService.Remove_Notification(notification);
                        }

                        await Refresh();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    bool value = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Warnung!", "Wollen Sie wirklich diese Transaktionen entfernen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "", "Ja", "Nein");
                    if (value == true)
                    {
                        IsBusy = true;

                        await ContentService.Remove_Transaktion(item);
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

        /// <summary>
        /// Stellt die Transaktion oder die Transaktionen die zum gleichen Auftrag gehören und sich im Papierkorb befinden wiederher.
        /// </summary>
        /// <param name="item">Die Transaktion oder das Transaktionsbündel was ausgewählt wurde.</param>
        /// <returns></returns>
        async Task Revive(Transaktion item)
        {
            try
            {
                /// Wenn die Transkation mit einem Auftrag verbunden ist, dann werden alle Transaktionen die den gleichen Auftrage besitzen und sich im Papierkorb befinden auch wiederhergestellt
                /// Sonst wird die einzelne Transaktion wiederhergestellt
                if (item.Auftrags_id != null)
                {
                    string message = null;

                    if (item.Auftrags_Option == 1)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag wiederherstellen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                    }
                    if (item.Auftrags_Option == 2)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag wiederherstellen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nAnzahl an Wiederholungen: " + item.Anzahl_an_Wiederholungen + " Mal\nSpeziell: " + item.Speziell + "";
                    }
                    if (item.Auftrags_Option == 3)
                    {
                        message = "Wollen Sie wirklich alle Transaktionen mit diesem Auftrag wiederherstellen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt:  " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "\nAuftrags ID: " + item.Auftrags_id + "\nArt der Wiederholung: " + item.Art_an_Wiederholungen + "\nEnddatum: " + item.Anzahl_an_Wiederholungen + "\nSpeziell: " + item.Speziell + "";
                    }

                    bool value = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Wiederherstellen", message, "Ja", "Nein");

                    if (value == true)
                    {
                        IsBusy = true;

                        List<Transaktion> all = new List<Transaktion>(await ContentService.Get_all_disabeled_Transaktion());

                        foreach (Transaktion trans in all)
                        {
                            if(trans.Auftrags_id != null)
                            {
                                bool validate = true;

                                if (int.Parse(item.Auftrags_id.Substring(0, item.Auftrags_id.IndexOf("."))) != int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))))
                                {
                                    validate = false;
                                }
                                if (item.Anzahl_an_Wiederholungen != trans.Anzahl_an_Wiederholungen)
                                {
                                    validate = false;
                                }
                                if (item.Art_an_Wiederholungen != trans.Art_an_Wiederholungen)
                                {
                                    validate = false;
                                }
                                if (item.Auftrags_Option != trans.Auftrags_Option)
                                {
                                    validate = false;
                                }
                                if (item.Speziell != trans.Speziell)
                                {
                                    validate = false;
                                }

                                if (validate == true)
                                {
                                    trans.Content_Visibility = true;

                                    await ContentService.Edit_Transaktion(trans);
                                }
                            }
                        }

                        all = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

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

                        transaktionlist = transaktionlist.OrderBy(d => d.Datum).ToList();

                        Transaktion newlasttransaktion = transaktionlist.Last();

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
                    bool value = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Wiederherstellen", "Wollen Sie wirklich diese Transaktionen wiederherstellen?\n\nZweck: " + item.Zweck + "\nBetrag: " + item.Betrag + " €\nDatum: " + item.Datumanzeige + "\nNotiz: " + item.Notiz + "\nWird in Bilanz angezeigt: " + item.Balance_Visibility_String + "\nWird im Stand berechnet: "+item.Saldo_Visibility_String+"\nID: " + item.Id + "", "Ja", "Nein");

                    if (value == true)
                    {
                        IsBusy = true;

                        item.Content_Visibility = true;
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

        /// <summary>
        /// Aktualisiert den Papierkorb.
        /// </summary>
        /// <returns></returns>
        public async Task Refresh()
        {
            try
            {
                if(Is_Aktiv == true)
                {
                    IsBusy = true;
                }
                else
                {
                    Is_Aktiv = true;
                }

                Transaktion1.Clear();

                /// Erstellt eine Liste an Transaktionen die im Papierkorb gehören
                var transaktionscontent = await ContentService.Get_all_disabeled_Transaktion();

                List<Transaktion> sorted_transaktionscontent = new List<Transaktion>();

                List<Auftrag> auftrag_liste = new List<Auftrag>() { new Auftrag { Art_an_Wiederholungen = null, Anzahl_an_Wiederholungen = null, Id = 0, Speziell = null, Option = 0 } };

                /// Geht die Liste durch und erstellt die Pseudotexte der Transaktionen, 
                /// sowie fügt jede einzelne Transaktion zu einer anderen Liste hinzu und 
                /// gruppiert Tranaktionen mit dem selbem Auftrag und gibt die erste Transaktion von diesem Auftrag in die andere Liste hinzu
                
                if(transaktionscontent.Count() != 0)
                {
                    foreach (var trans in transaktionscontent)
                    {
                        if (trans.Auftrags_id == null)
                        {
                            trans.Pseudotext = trans.Zweck;
                            sorted_transaktionscontent.Add(trans);
                        }
                        else
                        {
                            List<bool> indikator_list = new List<bool>();

                            foreach (Auftrag order in auftrag_liste)
                            {
                                bool indikator = true;

                                if (order.Id != int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))))
                                {
                                    indikator = false;
                                }
                                if (order.Anzahl_an_Wiederholungen != trans.Anzahl_an_Wiederholungen)
                                {
                                    indikator = false;
                                }
                                if (order.Art_an_Wiederholungen != trans.Art_an_Wiederholungen)
                                {
                                    indikator = false;
                                }
                                if (order.Option != trans.Auftrags_Option)
                                {
                                    indikator = false;
                                }
                                if (order.Speziell != trans.Speziell)
                                {
                                    indikator = false;
                                }

                                indikator_list.Add(indikator);
                            }

                            if (indikator_list.Contains(true) == false)
                            {
                                auftrag_liste.Add(new Auftrag { Id = int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))), Anzahl_an_Wiederholungen = trans.Anzahl_an_Wiederholungen, Art_an_Wiederholungen = trans.Art_an_Wiederholungen, Speziell = trans.Speziell, Option = trans.Auftrags_Option });
                                trans.Pseudotext = trans.Zweck;
                                sorted_transaktionscontent.Add(trans);
                            }
                        }
                    }
                }

                /// Fügt die neue Liste zu einer Collektion mit Transaktionen hinzu
                Transaktion1.AddRange(sorted_transaktionscontent);

                /// Wenn diese Collektion keine Transaktion beinhaltet wird angezeigt dass keine Transaktion um Papierkorb sich befindet,
                if (Transaktion1.Count() == 0)
                {
                    List_of_Transaktion_Status = false;
                    Kein_Ergebnis_Transaktion_Status = true;
                }

                /// Sonst wird die Collektion sotiert und angezigt
                else
                {
                    await Add_to_GroupsAsync();

                    Kein_Ergebnis_Transaktion_Status = false;
                    List_of_Transaktion_Status = true;
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                IsBusy = false;
            }
            finally
            { 
                IsBusy = false; 
            }
        }

        /// <summary>
        /// Generiert die Liste an Transaktionen oder Transaktionsbündel wenn der Papierkorb erscheint.
        /// </summary>
        /// <returns></returns>
        public async Task View_Appering_Methode()
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

                /// Erstellt eine Liste an Transaktionen die nicht in den Papierkorb gehören
                var transaktioncontent2 = await ContentService.Get_all_enabeled_Transaktion();

                List<string> transaktion_auftragid_list = new List<string>();

                /// Füllt eine Liste an Transaktionen mit AuftragsIds von den Transaktionen die nicht in den Papierkorb gehören
                foreach(Transaktion trans in transaktioncontent2)
                {
                    if(trans.Auftrags_id != null)
                    {
                        transaktion_auftragid_list.Add(trans.Auftrags_id);
                    }
                }

                Transaktion1.Clear();

                var transaktionscontent = await ContentService.Get_all_disabeled_Transaktion();

                List<Transaktion> sorted_transaktionscontent = new List<Transaktion>();

                List<Auftrag> auftrag_liste = new List<Auftrag>() { new Auftrag { Art_an_Wiederholungen = null, Anzahl_an_Wiederholungen = null, Id = 0, Speziell = null, Option = 0 } };

                /// Geht die Liste durch und erstellt die Pseudotexte der Transaktionen, 
                /// sowie überprüft es die Transaktionen nach deren Auftragsid und schait ob sie auch in der Liste mit den Transaktionsauftragsids die nicht in den Papierkorb gehören und wenn es eine übereinstimmung gibt wird sie gelöscht,
                /// sowie fügt jede einzelne Transaktion zu einer anderen Liste hinzu und 
                /// gruppiert Tranaktionen mit dem selbem Auftrag und gibt die erste Transaktion von diesem Auftrag in die andere Liste hinzu
                
                if(transaktionscontent.Count() != 0)
                {
                    foreach (var trans in transaktionscontent)
                    {
                        if (trans.Auftrags_id == null)
                        {
                            trans.Pseudotext = trans.Zweck;
                            sorted_transaktionscontent.Add(trans);
                        }
                        else
                        {
                            if(transaktion_auftragid_list.Contains(trans.Auftrags_id) == true)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                            else
                            {
                                List<bool> indikator_list = new List<bool>();

                                foreach (Auftrag order in auftrag_liste)
                                {
                                    bool indikator = true;

                                    if (order.Id != int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))))
                                    {
                                        indikator = false;
                                    }
                                    if (order.Anzahl_an_Wiederholungen != trans.Anzahl_an_Wiederholungen)
                                    {
                                        indikator = false;
                                    }
                                    if (order.Art_an_Wiederholungen != trans.Art_an_Wiederholungen)
                                    {
                                        indikator = false;
                                    }
                                    if (order.Option != trans.Auftrags_Option)
                                    {
                                        indikator = false;
                                    }
                                    if (order.Speziell != trans.Speziell)
                                    {
                                        indikator = false;
                                    }

                                    indikator_list.Add(indikator);
                                }

                                if (indikator_list.Contains(true) == false)
                                {
                                    auftrag_liste.Add(new Auftrag { Id = int.Parse(trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf("."))), Anzahl_an_Wiederholungen = trans.Anzahl_an_Wiederholungen, Art_an_Wiederholungen = trans.Art_an_Wiederholungen, Speziell = trans.Speziell, Option = trans.Auftrags_Option });
                                    trans.Pseudotext = trans.Zweck;
                                    sorted_transaktionscontent.Add(trans);
                                }
                            }
                        }
                    }
                }

                Transaktion1.AddRange(sorted_transaktionscontent);

                /// Wenn diese Collektion keine Transaktion beinhaltet wird angezeigt dass keine Transaktion um Papierkorb sich befindet,
                if (Transaktion1.Count() == 0)
                {
                    List_of_Transaktion_Status = false;
                    Kein_Ergebnis_Transaktion_Status = true;
                }

                /// Sonst wird die Collektion sotiert und angezigt
                else
                {
                    await Add_to_GroupsAsync();

                    Kein_Ergebnis_Transaktion_Status = false;
                    List_of_Transaktion_Status = true;
                }

                IsBusy = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                IsBusy = false;
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Setzt den Parameter für den Indekator, der angibt ob die Seite geöffnet wurde, auf false, wenn man die Seite verlässt.
        /// </summary>
        private void ViewIsDisappearing_Methode()
        {
            Is_Aktiv = false; 
        }
    }
}
