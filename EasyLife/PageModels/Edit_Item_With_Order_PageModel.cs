using EasyLife.Models;
using EasyLife.Services;
using EasyLife.Pages;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FreshMvvm;
using System.Threading.Tasks;
using Xamarin.Forms;
using MvvmHelpers;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Drawing;
using Plugin.LocalNotification;
using System.Reactive;
using EasyLife.Helpers;
using EasyLife.PageModels.Edit_Item_With_Order_Submethods;
using Xamarin.CommunityToolkit.Extensions;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(TransaktionID), nameof(TransaktionID))]
    [QueryProperty(nameof(OrderID), nameof(OrderID))]
    [QueryProperty(nameof(EditID), nameof(EditID))]
    class Edit_Item_With_Order_PageModel : FreshBasePageModel
    {
        public Edit_Item_With_Order_PageModel()
        {
            Edit_Item_Command = new AsyncCommand(Edit);

            Edit_Order_Command = new AsyncCommand(Edit_Order_Methode);

            ViewIsAppearing_Command = new AsyncCommand(ViewIsAppearing);

            Zweck_Liste = new ObservableRangeCollection<string>();

            Virtuelle_Transaktion = null;

            Virtueller_Auftrag = null;
        }

        async Task Edit()
        {
            try
            {
                if (string.IsNullOrEmpty(Zweck) == true)
                {
                    await Notificater("Es wurde kein Zweck ausgewählt.");
                    return;
                }

                if (double.TryParse(Betrag, NumberStyles.Any, new CultureInfo("de-DE"), out double result) == true)
                {
                    ActivityIndicator_IsRunning = true;

                    if (Entscheider_ob_Einnahme_oder_Ausgabe[Zweck] == "Einnahmen")
                    {
                        result = Math.Abs(result);
                    }

                    else
                    {
                        result = -Math.Abs(result);
                    }

                    if (0 > Math.Abs(result) || Math.Abs(result) > 9999999)
                    {
                        Zweck = Transaktion.Zweck;
                        Betrag = double.Parse(Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")).ToString(new CultureInfo("de-DE"));
                        Datum = Transaktion.Datum;
                        Notiz = Transaktion.Notiz;
                        Show_Hide_Balance = Transaktion.Balance_Visibility;
                        Show_Hide_Saldo = Transaktion.Saldo_Visibility;

                        Anzahl_an_Wiederholungen = Transaktion.Anzahl_an_Wiederholungen;
                        Art_an_Wiederholungen = Transaktion.Art_an_Wiederholungen;
                        Speziell = Transaktion.Speziell;

                        ActivityIndicator_IsRunning = false;

                        await Notificater("Der Betrag ist nicht zwischen 0€ und 9999999€");

                        return;
                    }

                    if (result == 0)
                    {
                        await Notificater("Es wurde kein Betrag eingegeben.");

                        ActivityIndicator_IsRunning = false;

                        return;
                    }

                    Betrag = result.ToString();

                    Virtuelle_Transaktion.Betrag = Betrag;
                    Virtuelle_Transaktion.Notiz = Notiz;
                    Virtuelle_Transaktion.Zweck = Zweck;
                    Virtuelle_Transaktion.Anzahl_an_Wiederholungen = Anzahl_an_Wiederholungen;
                    Virtuelle_Transaktion.Art_an_Wiederholungen = Art_an_Wiederholungen;
                    Virtuelle_Transaktion.Speziell = Speziell;
                    Virtuelle_Transaktion.Balance_Visibility = Show_Hide_Balance;
                    Virtuelle_Transaktion.Saldo_Visibility = Show_Hide_Saldo;

                    List<Transaktion> transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                    if (Edit_Version <= 3 && Edit_Version >= 1)
                    {
                        if (Edit_Version == 1)
                        {
                            Transaktion.Betrag = Betrag;
                            Transaktion.Zweck = Zweck;
                            Transaktion.Datum = Datum;
                            Transaktion.Notiz = Notiz;
                            Transaktion.Balance_Visibility = Show_Hide_Balance;
                            Transaktion.Saldo_Visibility= Show_Hide_Saldo;

                            await ContentService.Edit_Transaktion(Transaktion);
                        }

                        if (Edit_Version == 2)
                        {
                            var result_Edit_Version2 = await Edit_Item_With_Order_Submethods.Edit_Version2.Edit_Version2_Methode(Virtuelle_Transaktion,Virtueller_Auftrag,transaktion_list,Betrag,Zweck,Notiz,Datum,OrderId,Revive_Switch,Full_Order,Anzahl_an_Wiederholungen);

                            if (result_Edit_Version2 == Errorhandler.Errors[0])
                            {
                                await Notificater("Es gab ein Logikfehler.");

                                ActivityIndicator_IsRunning = false;

                                return;
                            }

                            if (result_Edit_Version2 == Errorhandler.Errors[1])
                            {
                                await Notificater("Es konnte die Benachrichtigung nicht geändert werden");

                                ActivityIndicator_IsRunning = false;

                                return;
                            }

                        }

                        if (Edit_Version == 3)
                        {
                            var result_Edit_Version3 = await Edit_Item_With_Order_Submethods.Edit_Version3.Edit_Version3_Methode(Virtuelle_Transaktion, Transaktion , Virtueller_Auftrag, transaktion_list, Betrag, Zweck, Notiz, Datum, OrderId, Revive_Switch, Full_Order, Anzahl_an_Wiederholungen);

                            if (result_Edit_Version3 == Errorhandler.Errors[0])
                            {
                                await Notificater("Es gab ein Logikfehler.");

                                ActivityIndicator_IsRunning = false;

                                return;
                            }

                            if (result_Edit_Version3 == Errorhandler.Errors[1])
                            {
                                await Notificater("Es konnte die Benachrichtigung nicht geändert werden");

                                ActivityIndicator_IsRunning = false;

                                return;
                            }
                        }
                    }

                    else
                    {
                        await Notificater("Ein Fehler ist aufgetretten.");

                        ActivityIndicator_IsRunning = false;

                        return;
                    }

                    TransaktionID = null;

                    if (EditID == "2")
                    {
                        Auftrag auftrag = new Auftrag();

                        auftrag.Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;
                        auftrag.Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen.ToString();
                        auftrag.Speziell = Virtuelle_Transaktion.Speziell;
                        auftrag.Id = int.Parse(OrderID);

                        await OrderService.Edit_Order(auftrag);
                    }

                    Virtuelle_Transaktion = null;

                    EditID = null;

                    OrderID = null;

                    ActivityIndicator_IsRunning = false;

                    await Notificater("Erfolgreich bearbeitet");

                    Return();
                }

                else
                {
                    Zweck = Transaktion.Zweck;
                    Betrag = double.Parse(Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")).ToString(new CultureInfo("de-DE"));
                    Datum = Transaktion.Datum;
                    Notiz = Transaktion.Notiz;
                    Show_Hide_Balance = Transaktion.Balance_Visibility;
                    Show_Hide_Saldo = Transaktion.Saldo_Visibility;

                    Anzahl_an_Wiederholungen = Transaktion.Anzahl_an_Wiederholungen;
                    Art_an_Wiederholungen = Transaktion.Art_an_Wiederholungen;
                    Speziell = Transaktion.Speziell;

                    await Notificater("Die Eingaben sind fehlerhaft.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                Virtuelle_Transaktion = Transaktion;

                Zweck = Transaktion.Zweck;
                Betrag = double.Parse(Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")).ToString(new CultureInfo("de-DE"));
                Datum = Transaktion.Datum;
                Notiz = Transaktion.Notiz;
                Show_Hide_Balance = Transaktion.Balance_Visibility;
                Show_Hide_Saldo= Transaktion.Saldo_Visibility;

                Anzahl_an_Wiederholungen = Transaktion.Anzahl_an_Wiederholungen;
                Art_an_Wiederholungen = Transaktion.Art_an_Wiederholungen;
                Speziell = Transaktion.Speziell;
            }
        }

        public async Task Edit_Order_Methode()
        {
            try
            {
                Virtuelle_Transaktion.Betrag = Betrag;
                Virtuelle_Transaktion.Datum = Datum;
                Virtuelle_Transaktion.Notiz = Notiz;
                Virtuelle_Transaktion.Zweck = Zweck;
                Virtuelle_Transaktion.Anzahl_an_Wiederholungen = Anzahl_an_Wiederholungen;
                Virtuelle_Transaktion.Art_an_Wiederholungen = Art_an_Wiederholungen;
                Virtuelle_Transaktion.Speziell = Speziell;
                Virtuelle_Transaktion.Balance_Visibility = Show_Hide_Balance;
                Virtuelle_Transaktion.Saldo_Visibility = Show_Hide_Saldo;
                Virtuelle_Transaktion.Id = 0;

                await PassingTransaktionService.Add_Transaktion(Virtuelle_Transaktion);

                TransaktionID = Virtuelle_Transaktion.Id.ToString();

                await Shell.Current.GoToAsync($"{nameof(Edit_Order_Page)}?Viertuelle_TransaktionID={TransaktionID}&OrderID={OrderID}&EditID={EditID}");
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async void Return()
        {
            try
            {
                TransaktionID = null;

                OrderID = null;

                EditID = null;

                Virtueller_Auftrag = null;

                Virtuelle_Transaktion = null;

                await PassingTransaktionService.Remove_All_Transaktion();

                await PassingOrderService.Remove_All_Order();

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        async Task Get_Zweck_Liste()
        {
            try
            {
                Zweck_IsEnable = false;

                Entscheider_ob_Einnahme_oder_Ausgabe = (Dictionary<string, string>)await ReasonService.Get_Enable_ReasonDictionary();

                Zweck_Liste.Clear();

                Zweck_Liste.AddRange(Entscheider_ob_Einnahme_oder_Ausgabe.Keys.ToArray());

                Zweck_IsEnable = true;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                Zweck_IsEnable = true;
            }
        }

        private async Task Notificater(string v)
        {
            await Shell.Current.DisplayToastAsync(v, 5000);
        }

        public async Task ViewIsAppearing()
        {
            await Get_Zweck_Liste();

            try
            {
                if (int.TryParse(TransaktionID, out var result1) == true)
                {
                    if(Virtuelle_Transaktion == null)
                    {
                        Transaktion = await ContentService.Get_specific_Transaktion(result1);

                        Virtuelle_Transaktion = Transaktion;
                    }
                    else
                    {
                        Virtuelle_Transaktion = await PassingTransaktionService.Get_specific_Transaktion(result1);
                    }

                    Betrag = double.Parse(Virtuelle_Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")).ToString(new CultureInfo("de-DE"));

                    Datum = Virtuelle_Transaktion.Datum;

                    Notiz = Virtuelle_Transaktion.Notiz;

                    Zweck = Virtuelle_Transaktion.Zweck;

                    Show_Hide_Balance = Virtuelle_Transaktion.Balance_Visibility;

                    Show_Hide_Saldo = Virtuelle_Transaktion.Saldo_Visibility;
                }

                if (int.TryParse(OrderID, out var result2) == true)
                {

                    if (Virtueller_Auftrag == null)
                    {
                        Virtueller_Auftrag = new Auftrag();

                        OrderId = result2;

                        Virtueller_Auftrag.Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;

                        Virtueller_Auftrag.Option = Virtuelle_Transaktion.Auftrags_Option;

                        Virtueller_Auftrag.Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen;

                        Virtueller_Auftrag.Speziell = Virtuelle_Transaktion.Speziell;

                        Virtueller_Auftrag.Id = 0;

                        await PassingOrderService.Add_Order(Virtueller_Auftrag);

                        OrderID = Virtueller_Auftrag.Id.ToString();
                    }
                    else
                    {
                        Virtueller_Auftrag = await PassingOrderService.Get_specific_Order(result2);
                    }
                }

                if (int.TryParse(EditID, out var result3) == true)
                {
                    Edit_Version = result3;
                    if(result3 == 3)
                    {
                        Order_Button_Text = "Neuer Auftrag";
                    }
                    else
                    {
                        Order_Button_Text = "Auftrag bearbeiten";
                    }
                }

                Auftrag origin = await OrderService.Get_specific_Order(OrderId);

                List<Transaktion> transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                List<Transaktion> transaktion_list2 = new List<Transaktion>();

                List<Transaktion> transaktion_list3 = new List<Transaktion>();

                bool validate = true;

                foreach (Transaktion trans in transaktion_list)
                {
                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                    {
                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                        {
                            transaktion_list3.Add(trans);

                            if (origin.Anzahl_an_Wiederholungen != trans.Anzahl_an_Wiederholungen)
                            {
                                validate = false;
                            }
                            if (origin.Art_an_Wiederholungen != trans.Art_an_Wiederholungen)
                            {
                                validate = false;
                            }
                            if (origin.Option != trans.Auftrags_Option)
                            {
                                validate = false;
                            }
                            if (origin.Speziell != trans.Speziell)
                            {
                                validate = false;
                            }

                            if (DateTime.Compare(trans.Datum, Transaktion.Datum) >= 0)
                            {
                                transaktion_list2.Add(trans);
                            }
                        }
                    }
                }

                Datum = Virtuelle_Transaktion.Datum;

                if (Virtueller_Auftrag != null)
                {
                    Days_Visibility = true;

                    if (Virtueller_Auftrag.Option == 1)
                    {
                        Amount_Text = "Anzahl :";
                    }
                    if (Virtueller_Auftrag.Option == 2)
                    {
                        Amount_Text = "Anzahl an Wiederholungen :";
                    }
                    if (Virtueller_Auftrag.Option == 3)
                    {
                        Amount_Text = "Enddatum :";
                    }
                }

                if (Edit_Version == 1)
                {
                    Edit_Order_Visibility = false;
                    Min_Datum = transaktion_list2[0].Datum;
                }
                if (Edit_Version == 2)
                {
                    transaktion_list3 = transaktion_list3.OrderBy(d => d.Datum).ToList();
                    Datum = transaktion_list3[0].Datum;
                    Min_Datum = new DateTime(2000, 1, 1);

                    Last_Order_Anzahl_an_Wiederholungen = origin.Anzahl_an_Wiederholungen;

                    Last_Order_Art_an_Wiederholungen = origin.Art_an_Wiederholungen;

                    Last_Order_Speziell = origin.Speziell;

                    Last_Order_Option = origin.Option; 

                    int count = 0;

                    Auftrag virtueller_auftrag = new Auftrag()
                    {
                        Anzahl_an_Wiederholungen = Virtueller_Auftrag.Anzahl_an_Wiederholungen,
                        Art_an_Wiederholungen = Virtueller_Auftrag.Art_an_Wiederholungen,
                        Speziell = Virtueller_Auftrag.Speziell,
                        Option = Virtueller_Auftrag.Option,
                        Id = OrderId
                    };

                    Auftrag origin_order = await OrderService.Get_specific_Order(OrderId);

                    int compare = 0;

                    if (virtueller_auftrag.Id != origin_order.Id)
                    {
                        compare = 1;
                    }
                    if (virtueller_auftrag.Anzahl_an_Wiederholungen != origin_order.Anzahl_an_Wiederholungen)
                    {
                        compare = 1;
                    }
                    if (virtueller_auftrag.Art_an_Wiederholungen != origin_order.Art_an_Wiederholungen)
                    {
                        compare = 1;
                    }
                    if (virtueller_auftrag.Speziell != origin_order.Speziell)
                    {
                        compare = 1;
                    }
                    if (virtueller_auftrag.Option != origin_order.Option)
                    {
                        compare = 1;
                    }

                    DateTime current_day = transaktion_list3[0].Datum;

                    DateTime enddate = DateTime.Now;

                    if (origin.Option == 2)
                    {
                        count = int.Parse(origin.Anzahl_an_Wiederholungen);
                    }
                    if (origin.Option == 3)
                    {
                       enddate = DateTime.ParseExact(origin.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(12);
                    }

                    int Count = count+1;

                    if (origin.Art_an_Wiederholungen == "Jeden Tag")
                    {
                        if (origin.Option == 3)
                        {
                            count = (enddate - current_day).Days;

                            Count = count + 1;
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Jede Woche")
                    {
                        if (origin.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Jeden Monat")
                    {
                        if (origin.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddMonths(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Jedes Jahr")
                    {
                        if (origin.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        string Days = origin.Speziell;

                        List<int> Days_List = new List<int>();

                        List<DateTime> sorted_date_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Days))
                        {
                            if (Days.Substring(Days.Length - 2) == "So")
                            {
                                Days_List.Add(0);
                            }
                            if (Days.Substring(Days.Length - 2) == "Mo")
                            {
                                Days_List.Add(1);
                            }
                            if (Days.Substring(Days.Length - 2) == "Di")
                            {
                                Days_List.Add(2);
                            }
                            if (Days.Substring(Days.Length - 2) == "Mi")
                            {
                                Days_List.Add(3);
                            }
                            if (Days.Substring(Days.Length - 2) == "Do")
                            {
                                Days_List.Add(4);
                            }
                            if (Days.Substring(Days.Length - 2) == "Fr")
                            {
                                Days_List.Add(5);
                            }
                            if (Days.Substring(Days.Length - 2) == "Sa")
                            {
                                Days_List.Add(6);
                            }
                            if (Days.Length > 2)
                            {
                                Days = Days.Remove(Days.Length - 5);
                            }
                            else
                            {
                                Days = null;
                            }
                        }

                        Count = Count * Days_List.Count();

                        DateTime trans_dateTime = transaktion_list3[0].Datum;

                        foreach (int days in Days_List)
                        {
                            if (days - (int)trans_dateTime.DayOfWeek >= 0)
                            {
                                sorted_date_List.Add(trans_dateTime.AddDays(days - (int)trans_dateTime.DayOfWeek));
                            }
                            else
                            {
                                sorted_date_List.Add(trans_dateTime.AddDays(days - (int)trans_dateTime.DayOfWeek + 7));
                            }
                        }

                        sorted_date_List = sorted_date_List.OrderBy(d => d.Date).ToList();

                        if (origin.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

                            Count = 0;

                            bool indicator = false;

                            int follower = 0;

                            int follower2 = 0;

                            int follower3 = 0;

                            while (indicator == false)
                            {
                                dates.Add(sorted_date_List[follower2].AddDays(7 * follower3));

                                if (dates[follower] > enddate)
                                {
                                    indicator = true;

                                    break;
                                }
                                else
                                {
                                    if (follower2 == sorted_date_List.Count() - 1)
                                    {
                                        follower2 = -1;
                                        follower3++;
                                    }

                                    follower2++;

                                    follower++;

                                    Count++;
                                }
                            }
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        int h = 0;

                        if (origin.Speziell == "derselbe Tag in jeder Woche")
                        {
                            h = 1;
                        }
                        if (origin.Speziell == "derselbe Tag in jeder zweiten Woche")
                        {
                            h = 2;
                        }
                        if (origin.Speziell == "derselbe Tag in jeder dritten Woche")
                        {
                            h = 3;
                        }
                        if (origin.Speziell == "derselbe Tag in jeder vierten Woche")
                        {
                            h = 4;
                        }

                        if (origin.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7 * h);
                                count++;
                            }
                            Count = count + 1;
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        string Months = origin.Speziell;

                        List<int> Month_List = new List<int>();

                        List<DateTime> sorted_Month_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Months))
                        {
                            if (Months.Substring(Months.Length - 3) == "Jan")
                            {
                                Month_List.Add(01);
                            }
                            if (Months.Substring(Months.Length - 3) == "Feb")
                            {
                                Month_List.Add(02);
                            }
                            if (Months.Substring(Months.Length - 3) == "Mrz")
                            {
                                Month_List.Add(03);
                            }
                            if (Months.Substring(Months.Length - 3) == "Apr")
                            {
                                Month_List.Add(04);
                            }
                            if (Months.Substring(Months.Length - 3) == "Mai")
                            {
                                Month_List.Add(05);
                            }
                            if (Months.Substring(Months.Length - 3) == "Jun")
                            {
                                Month_List.Add(06);
                            }
                            if (Months.Substring(Months.Length - 3) == "Jul")
                            {
                                Month_List.Add(07);
                            }
                            if (Months.Substring(Months.Length - 3) == "Aug")
                            {
                                Month_List.Add(08);
                            }
                            if (Months.Substring(Months.Length - 3) == "Sep")
                            {
                                Month_List.Add(09);
                            }
                            if (Months.Substring(Months.Length - 3) == "Okt")
                            {
                                Month_List.Add(10);
                            }
                            if (Months.Substring(Months.Length - 3) == "Nov")
                            {
                                Month_List.Add(11);
                            }
                            if (Months.Substring(Months.Length - 3) == "Dez")
                            {
                                Month_List.Add(12);
                            }
                            if (Months.Length > 3)
                            {
                                Months = Months.Remove(Months.Length - 6);
                            }
                            else
                            {
                                Months = null;
                            }
                        }

                        Count = Count * Month_List.Count;

                        DateTime trans_dateTime = transaktion_list3[0].Datum;

                        foreach (int month in Month_List)
                        {
                            if (month - trans_dateTime.Month >= 0)
                            {
                                sorted_Month_List.Add(trans_dateTime.AddMonths(month - trans_dateTime.Month));
                            }
                            else
                            {
                                sorted_Month_List.Add(trans_dateTime.AddMonths(month - trans_dateTime.Month + 12));
                            }
                        }

                        sorted_Month_List = sorted_Month_List.OrderBy(d => d.Date).ToList();

                        if (origin.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

                            Count = 0;

                            bool indicator = false;

                            int follower = 0;

                            int follower2 = 0;

                            int follower3 = 0;

                            while (indicator == false)
                            {
                                dates.Add(sorted_Month_List[follower2].AddMonths(12 * follower3));

                                if (dates[follower] > enddate)
                                {
                                    indicator = true;

                                    break;
                                }
                                else
                                {
                                    if (follower2 == sorted_Month_List.Count() - 1)
                                    {
                                        follower2 = -1;
                                        follower3++;
                                    }

                                    follower2++;

                                    follower++;

                                    Count++;
                                }
                            }
                        }
                    }

                    if (origin.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        int h = 0;

                        if (origin.Speziell == "derselbe Tag in jedem Jahr")
                        {
                            h = 1;
                        }
                        if (origin.Speziell == "derselbe Tag in jedem zweitem Jahr")
                        {
                            h = 2;
                        }
                        if (origin.Speziell == "derselbe Tag in jedem drittem Jahr")
                        {
                            h = 3;
                        }
                        if (origin.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 4;
                        }
                        if (origin.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 5;
                        }

                        if (origin.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1 * h);
                                count++;
                            }

                            Count = count + 1;
                        }
                    }

                    Full_Order = Count;

                    if (transaktion_list3.Count() != Full_Order || compare == 1 || validate == false)
                    {
                        Revive_Switch = false;
                        Revive_Visibility = true;
                    }
                    else
                    {
                        Revive_Visibility = false;
                        Revive_Switch = false;
                    }
                }
                if (Edit_Version == 3)
                {
                    Min_Datum = transaktion_list2[0].Datum;
                }

                Betrag = Math.Abs(double.Parse(Virtuelle_Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE"))).ToString(new CultureInfo("de-DE"));
                Notiz = Virtuelle_Transaktion.Notiz;
                Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen;
                Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;
                Speziell = Virtuelle_Transaktion.Speziell;
                Show_Hide_Balance = Virtuelle_Transaktion.Balance_Visibility;
                Show_Hide_Saldo = Virtuelle_Transaktion.Saldo_Visibility;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                await Shell.Current.GoToAsync("..");
            }
        }



        public AsyncCommand Edit_Item_Command { get; }
        public AsyncCommand Edit_Order_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public string order_button_text;
        public string Order_Button_Text
        {
            get { return order_button_text; }
            set
            {
                if (Order_Button_Text == value)
                    return;
                order_button_text = value; RaisePropertyChanged();
            }
        }

        public string TransaktionID { get; set; }
        public string OrderID { get; set; }
        public string EditID { get; set; }

        public int Edit_Version;

        public int orderid;
        public int OrderId
        {
            get { return orderid; }
            set
            {
                if (OrderId == value)
                    return;
                orderid = value; RaisePropertyChanged();
            }
        }

        public int full_order;
        public int Full_Order
        {
            get { return full_order; }
            set
            {
                if (full_order == value)
                {
                    return;
                }
                full_order = value; RaisePropertyChanged();
            }
        }

        public Transaktion transaktion;
        public Transaktion Transaktion
        {
            get { return transaktion; }
            set
            {
                if (transaktion == value)
                {
                    return;
                }
                transaktion = value; RaisePropertyChanged();
            }
        }

        public Transaktion Virtuelle_Transaktion { get; set; }

        public Auftrag Virtueller_Auftrag { get; set; }

        public int transaktion_Position { get; set; }

        public bool days_visibility = false;
        public bool Days_Visibility
        {
            get { return days_visibility; }
            set
            {
                if (Days_Visibility == value)
                    return;
                days_visibility = value; RaisePropertyChanged();
            }
        }

        public bool edit_order_visibility = true;
        public bool Edit_Order_Visibility
        {
            get { return edit_order_visibility; }
            set
            {
                if (Edit_Order_Visibility == value)
                    return;
                edit_order_visibility = value; RaisePropertyChanged();
            }
        }

        public IDictionary<string, string> Entscheider_ob_Einnahme_oder_Ausgabe = new Dictionary<string, string>();

        public ObservableRangeCollection<string> zweck_liste;

        public ObservableRangeCollection<string> Zweck_Liste
        {
            get { return zweck_liste; }
            set
            {
                if (zweck_liste == value)
                {
                    return;
                }
                zweck_liste = value; RaisePropertyChanged();
            }
        }

        string amount_text;
        public string Amount_Text
        {
            get { return amount_text; }
            set
            {
                if (Amount_Text == value)
                    return;
                amount_text = value; RaisePropertyChanged();
            }
        }

        string zweck;
        public string Zweck
        {
            get { return zweck; }
            set
            {
                if (Zweck == value)
                    return;
                zweck = value; RaisePropertyChanged();
            }
        }

        string betrag;
        public string Betrag
        {
            get { return betrag; }
            set
            {
                if (Betrag == value)
                    return;

                switch (value)
                {
                    case ",":
                        betrag = "0,";
                        break;

                    case ".":
                        betrag = "0,";
                        break;

                    case "":
                        betrag = "";
                        break;

                    case null:
                        betrag = "";
                        break;

                    default:
                        if (double.TryParse(value, NumberStyles.Any, new CultureInfo("de-DE"), out double result) == true)
                        {
                            betrag = value.Replace(".", ",");
                        }
                        else
                        {
                            betrag = Betrag;
                        }
                        break;
                }

                RaisePropertyChanged();
            }
        }

        DateTime datum;
        public DateTime Datum
        {
            get { return datum; }
            set
            {
                if (Datum == value)
                    return;
                datum = value; RaisePropertyChanged();
            }
        }

        DateTime min_datum;
        public DateTime Min_Datum
        {
            get { return min_datum; }
            set
            {
                if (Min_Datum == value)
                    return;
                min_datum = value; RaisePropertyChanged();
            }
        }

        string notiz;
        public string Notiz
        {
            get { return notiz; }
            set
            {
                if (Notiz == value)
                    return;
                notiz = value; RaisePropertyChanged();
            }
        }

        string anzahl_an_wiederholungen;
        public string Anzahl_an_Wiederholungen
        {
            get { return anzahl_an_wiederholungen; }
            set
            {
                if (Anzahl_an_Wiederholungen == value)
                    return;
                anzahl_an_wiederholungen = value; RaisePropertyChanged();
            }
        }

        string art_an_wiederholungen;
        public string Art_an_Wiederholungen
        {
            get { return art_an_wiederholungen; }
            set
            {
                if (Art_an_Wiederholungen == value)
                    return;
                art_an_wiederholungen = value; RaisePropertyChanged();
            }
        }

        string speziell;
        public string Speziell
        {
            get { return speziell; }
            set
            {
                if (Speziell == value)
                    return;
                speziell = value; RaisePropertyChanged();
            }
        }

        string last_order_anzahl_an_wiederholungen;
        public string Last_Order_Anzahl_an_Wiederholungen
        {
            get { return last_order_anzahl_an_wiederholungen; }
            set
            {
                if (Last_Order_Anzahl_an_Wiederholungen == value)
                    return;
                last_order_anzahl_an_wiederholungen = value; RaisePropertyChanged();
            }
        }

        string last_order_art_an_wiederholungen;
        public string Last_Order_Art_an_Wiederholungen
        {
            get { return last_order_art_an_wiederholungen; }
            set
            {
                if (Last_Order_Art_an_Wiederholungen == value)
                    return;
                last_order_art_an_wiederholungen = value; RaisePropertyChanged();
            }
        }

        string last_order_speziell;
        public string Last_Order_Speziell
        {
            get { return last_order_speziell; }
            set
            {
                if (Last_Order_Speziell == value)
                    return;
                last_order_speziell = value; RaisePropertyChanged();
            }
        }

        int last_order_option;
        public int Last_Order_Option
        {
            get { return last_order_option; }
            set
            {
                if (Last_Order_Option == value)
                    return;
                last_order_option = value; RaisePropertyChanged();
            }
        }

        public bool zweck_isEnable = true;
        public bool Zweck_IsEnable
        {
            get { return zweck_isEnable; }
            set
            {
                if (Zweck_IsEnable == value)
                    return;
                zweck_isEnable = value; RaisePropertyChanged();
            }
        }

        public bool show_hide_balance;
        public bool Show_Hide_Balance
        {
            get { return show_hide_balance; }
            set
            {
                if (show_hide_balance == value)
                    return;
                show_hide_balance = value; RaisePropertyChanged();
            }
        }

        public bool show_hide_saldo;
        public bool Show_Hide_Saldo
        {
            get { return show_hide_saldo; }
            set
            {
                if (Show_Hide_Saldo == value)
                    return;
                show_hide_saldo = value; RaisePropertyChanged();
            }
        }

        public bool revive_visibility = false;
        public bool Revive_Visibility
        {
            get { return revive_visibility; }
            set
            {
                if (Revive_Visibility == value)
                    return;
                revive_visibility = value; RaisePropertyChanged();
            }
        }

        public bool revive_switch = false;
        public bool Revive_Switch
        {
            get { return revive_switch; }
            set
            {
                if (Revive_Switch == value)
                    return;
                revive_switch = value; RaisePropertyChanged();

                if (value == true)
                {
                    if (Last_Order_Option == 1)
                    {
                        Amount_Text = "Anzahl :";
                    }
                    if (Last_Order_Option == 2)
                    {
                        Amount_Text = "Anzahl an Wiederholungen :";
                    }
                    if (Last_Order_Option == 3)
                    {
                        Amount_Text = "Enddatum :";
                    }

                    Anzahl_an_Wiederholungen = Last_Order_Anzahl_an_Wiederholungen;

                    Art_an_Wiederholungen = Last_Order_Art_an_Wiederholungen;

                    Speziell = Last_Order_Speziell;
                }
                else
                {
                    if (Virtueller_Auftrag.Option == 1)
                    {
                        Amount_Text = "Anzahl :";
                    }
                    if (Virtueller_Auftrag.Option == 2)
                    {
                        Amount_Text = "Anzahl an Wiederholungen :";
                    }
                    if (Virtueller_Auftrag.Option == 3)
                    {
                        Amount_Text = "Enddatum :";
                    }

                    Anzahl_an_Wiederholungen = Virtueller_Auftrag.Anzahl_an_Wiederholungen;

                    Art_an_Wiederholungen = Virtueller_Auftrag.Art_an_Wiederholungen;

                    Speziell = Virtueller_Auftrag.Speziell;
                }
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
    }
}
