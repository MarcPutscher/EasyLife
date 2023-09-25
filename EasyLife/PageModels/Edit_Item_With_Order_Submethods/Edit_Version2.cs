using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using iText.Layout.Borders;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Xamarin.Forms.Internals.Profile;

namespace EasyLife.PageModels.Edit_Item_With_Order_Submethods
{

    public class Edit_Version2
    {
        public static async Task<string> Edit_Version2_Methode(Transaktion Virtuelle_Transaktion, Auftrag Virtueller_Auftrag , List<Transaktion> transaktion_list , string Betrag , string Zweck , string Notiz , DateTime Datum , int OrderId , bool Revive_Switch , int Full_Order , string Anzahl_an_Wiederholungen)
        {
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

            List<string> list_of_kinds = new List<string>() { "Jedes Jahr", "Jeden Monat", "Jede Woche", "Jeden Tag" };

            if (Revive_Switch == true)
            {
                List<Transaktion> transaktion_list2 = new List<Transaktion>();

                foreach (Transaktion trans in transaktion_list)
                {
                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                    {
                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                        {
                            transaktion_list2.Add(trans);
                        }
                    }
                }

                transaktion_list2 = transaktion_list2.OrderBy(d => d.Datum).ToList();

                Virtuelle_Transaktion.Anzahl_an_Wiederholungen = origin_order.Anzahl_an_Wiederholungen;
                Virtuelle_Transaktion.Art_an_Wiederholungen = origin_order.Art_an_Wiederholungen;
                Virtuelle_Transaktion.Speziell = origin_order.Speziell;
                Virtuelle_Transaktion.Auftrags_Option = origin_order.Option;

                string Speziell = Virtuelle_Transaktion.Speziell;


                if (Full_Order < transaktion_list2.Count)
                {
                    List<Transaktion> transaktions = new List<Transaktion>();
                    transaktions.AddRange(transaktion_list2.GetRange(Full_Order, transaktion_list2.Count - Full_Order));
                    foreach (Transaktion trans in transaktions)
                    {
                        await ContentService.Remove_Transaktion(trans);
                    }
                }

                if (Full_Order > transaktion_list2.Count)
                {
                    List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, Full_Order - transaktion_list2.Count).ToList();

                    int l = transaktion_list2.Count + 1;

                    foreach (Transaktion trans in wiedrholungen)
                    {
                        trans.Auftrags_id = "" + OrderId + "." + l + "";

                        await ContentService.Add_Transaktion(trans);
                        l++;
                    }
                }

                transaktion_list2.Clear();

                transaktion_list.Clear();

                transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                foreach (Transaktion trans in transaktion_list)
                {
                    if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                    {
                        if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                        {
                            transaktion_list2.Add(trans);
                        }
                    }
                }

                if (list_of_kinds.Contains(Virtuelle_Transaktion.Art_an_Wiederholungen) == true)
                {
                    int w = 0;

                    DateTime trans_dateTime = transaktion_list2[0].Datum;

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jedes Jahr")
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = trans_dateTime.AddYears(w);

                            w++;
                        }
                    }
                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Monat")
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = trans_dateTime.AddMonths(w);

                            w++;
                        }
                    }
                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jede Woche")
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = trans_dateTime.AddDays(7 * w);

                            w++;
                        }
                    }
                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Tag")
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = trans_dateTime.AddDays(1 * w);

                            w++;
                        }
                    }
                }

                if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Tage")
                {
                    List<int> Days_List = new List<int>();

                    List<DateTime> sorted_date_List = new List<DateTime>();

                    while (!string.IsNullOrEmpty(Speziell))
                    {
                        if (Speziell.Substring(Speziell.Length - 2) == "So")
                        {
                            Days_List.Add(0);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Mo")
                        {
                            Days_List.Add(1);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Di")
                        {
                            Days_List.Add(2);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Mi")
                        {
                            Days_List.Add(3);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Do")
                        {
                            Days_List.Add(4);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Fr")
                        {
                            Days_List.Add(5);
                        }
                        if (Speziell.Substring(Speziell.Length - 2) == "Sa")
                        {
                            Days_List.Add(6);
                        }
                        if (Speziell.Length > 2)
                        {
                            Speziell = Speziell.Remove(Speziell.Length - 5);
                        }
                        else
                        {
                            Speziell = null;
                        }
                    }

                    int k = 0;

                    int w = 0;

                    DateTime trans_dateTime = Datum;

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

                    if (Virtueller_Auftrag.Option == 3)
                    {
                        List<DateTime> dates = new List<DateTime>();

                        bool indicator = false;

                        int follower = 0;

                        int follower2 = 0;

                        int follower3 = 0;

                        while (indicator == false)
                        {
                            dates.Add(sorted_date_List[follower2].AddDays(7 * follower3));

                            if (dates[follower] > DateTime.ParseExact(Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24))
                            {
                                indicator = true;

                                dates.RemoveAt(follower);

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
                            }
                        }

                        int follower4 = 0;

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = dates[follower4];

                            follower4++;
                        }
                    }
                    else
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_date_List[k].AddDays(7 * w);

                            if (k == sorted_date_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }
                }

                if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Wochen")
                {
                    int h = 0;

                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder Woche")
                    {
                        h = 1;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder zweiten Woche")
                    {
                        h = 2;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder dritten Woche")
                    {
                        h = 3;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder vierten Woche")
                    {
                        h = 4;
                    }

                    DateTime current_week = Datum;

                    foreach (Transaktion tran in transaktion_list2)
                    {
                        tran.Datum = current_week;

                        current_week = tran.Datum.AddDays(7 * h);
                    }
                }

                if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Monate")
                {
                    List<int> Months_List = new List<int>();

                    List<DateTime> sorted_Month_List = new List<DateTime>();

                    while (!string.IsNullOrEmpty(Speziell))
                    {
                        if (Speziell.Substring(Speziell.Length - 3) == "Jan")
                        {
                            Months_List.Add(01);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Feb")
                        {
                            Months_List.Add(02);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Mrz")
                        {
                            Months_List.Add(03);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Apr")
                        {
                            Months_List.Add(04);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Mai")
                        {
                            Months_List.Add(05);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Jun")
                        {
                            Months_List.Add(06);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Jul")
                        {
                            Months_List.Add(07);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Aug")
                        {
                            Months_List.Add(08);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Sep")
                        {
                            Months_List.Add(09);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Okt")
                        {
                            Months_List.Add(10);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Nov")
                        {
                            Months_List.Add(11);
                        }
                        if (Speziell.Substring(Speziell.Length - 3) == "Dez")
                        {
                            Months_List.Add(12);
                        }
                        if (Speziell.Length > 3)
                        {
                            Speziell = Speziell.Remove(Speziell.Length - 6);
                        }
                        else
                        {
                            Speziell = null;
                        }
                    }

                    int k = 0;

                    int w = 0;

                    DateTime trans_dateTime = Datum;

                    foreach (int month in Months_List)
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

                    if (Virtueller_Auftrag.Option == 3)
                    {
                        List<DateTime> dates = new List<DateTime>();

                        bool indicator = false;

                        int follower = 0;

                        int follower2 = 0;

                        int follower3 = 0;

                        while (indicator == false)
                        {
                            dates.Add(sorted_Month_List[follower2].AddMonths(12 * follower3));

                            if (dates[follower] > DateTime.ParseExact(Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24))
                            {
                                indicator = true;

                                dates.RemoveAt(follower);

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
                            }
                        }

                        int follower4 = 0;

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = dates[follower4];

                            follower4++;
                        }
                    }
                    else
                    {
                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_Month_List[k].AddMonths(1 * w);

                            if (k == sorted_Month_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }
                }

                if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Jahre")
                {
                    int h = 0;

                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem Jahr")
                    {
                        h = 1;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem zweitem Jahr")
                    {
                        h = 2;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem drittem Jahr")
                    {
                        h = 3;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                    {
                        h = 4;
                    }
                    if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                    {
                        h = 5;
                    }

                    DateTime current_year = Datum;

                    foreach (Transaktion tran in transaktion_list2)
                    {
                        tran.Datum = current_year;

                        current_year = tran.Datum.AddYears(1 * h);
                    }
                }

                foreach (Transaktion trans in transaktion_list2)
                {
                    trans.Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;
                    trans.Speziell = Virtuelle_Transaktion.Speziell;
                    trans.Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen;
                    trans.Auftrags_Option = Virtuelle_Transaktion.Auftrags_Option;
                }

                List<Transaktion> transaktion_list3 = new List<Transaktion>();

                foreach (Transaktion trans in transaktion_list2)
                {
                    trans.Betrag = Virtuelle_Transaktion.Betrag;
                    trans.Notiz = Virtuelle_Transaktion.Notiz;
                    trans.Zweck = Virtuelle_Transaktion.Zweck;
                    trans.Balance_Visibility = Virtuelle_Transaktion.Balance_Visibility;
                    transaktion_list3.Add(trans);
                }

                foreach (Transaktion trans in transaktion_list3)
                {
                    await ContentService.Edit_Transaktion(trans);
                }

                compare = -1;

                transaktion_list3 = transaktion_list3.OrderBy(d => d.Datum).ToList();

                if (false == await NotificationHelper.ModifyNotification(transaktion_list3.Last(), 0))
                {
                    return "NotificationError";
                }
            }

            if (compare != -1)
            {
                if (compare != 0)
                {
                    List<Transaktion> transaktion_list2 = new List<Transaktion>();
                    string Speziell = Virtuelle_Transaktion.Speziell;

                    foreach (Transaktion trans in transaktion_list)
                    {
                        if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                        {
                            if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                            {
                                transaktion_list2.Add(trans);
                            }
                        }
                    }

                    int count = 0;

                    DateTime current_day = Datum;

                    DateTime enddate = DateTime.Now;

                    if (Virtueller_Auftrag.Option == 2)
                    {
                        count = int.Parse(Virtueller_Auftrag.Anzahl_an_Wiederholungen);
                    }
                    if (Virtueller_Auftrag.Option == 3)
                    {
                        enddate = DateTime.ParseExact(Virtueller_Auftrag.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24).AddMilliseconds(-1);
                    }

                    int Count = count + 1;

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jeden Tag")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count = (enddate - current_day).Days;

                            Count = count + 1;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jede Woche")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jeden Monat")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddMonths(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jedes Jahr")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        string Days = Virtueller_Auftrag.Speziell;

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

                        DateTime trans_dateTime = Datum;

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

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

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

                                    dates.RemoveAt(follower);

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
                                    count++;
                                }
                            }

                            if (count == 0)
                            {
                                return "LogicError";
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        int h = 0;

                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder Woche")
                        {
                            h = 1;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder zweiten Woche")
                        {
                            h = 2;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder dritten Woche")
                        {
                            h = 3;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder vierten Woche")
                        {
                            h = 4;
                        }

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7 * h);

                                count++;
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }

                            Count = count + 1;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        string Months = Virtueller_Auftrag.Speziell;

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

                        Count = (count + 1) * Month_List.Count;

                        DateTime trans_dateTime = Datum;

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

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

                            bool indicator = false;

                            Count = 0;

                            int follower = 0;

                            int follower2 = 0;

                            int follower3 = 0;

                            while (indicator == false)
                            {
                                dates.Add(sorted_Month_List[follower2].AddMonths(12 * follower3));

                                if (dates[follower] > enddate)
                                {
                                    indicator = true;

                                    dates.RemoveAt(follower);

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

                            if (Count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        int h = 0;

                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem Jahr")
                        {
                            h = 1;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem zweitem Jahr")
                        {
                            h = 2;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem drittem Jahr")
                        {
                            h = 3;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 4;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 5;
                        }

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1 * h);
                                count++;
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }

                            Count = count + 1;
                        }
                    }


                    if (list_of_kinds.Contains(Virtuelle_Transaktion.Art_an_Wiederholungen) == true)
                    {
                        if (Count + 1 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(Count, transaktion_list2.Count - Count + 1));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (Count + 1 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, Count + 1 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        int w = 0;

                        DateTime trans_dateTime = Datum;

                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jedes Jahr")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddYears(w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Monat")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddMonths(w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jede Woche")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddDays(7 * w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Tag")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddDays(1 * w);

                                w++;
                            }
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        int count2 = 0;

                        while (Speziell.Length > 0)
                        {
                            if (Speziell.Length > 2)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 5);
                                count2++;
                            }
                            else
                            {
                                Speziell = null;
                                count2++;
                                break;
                            }
                        }

                        count2 *= Count;

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count2 = Count;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();


                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        Speziell = Virtuelle_Transaktion.Speziell;

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        List<int> Days_List = new List<int>();

                        List<DateTime> sorted_date_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Speziell))
                        {
                            if (Speziell.Substring(Speziell.Length - 2) == "So")
                            {
                                Days_List.Add(0);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Mo")
                            {
                                Days_List.Add(1);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Di")
                            {
                                Days_List.Add(2);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Mi")
                            {
                                Days_List.Add(3);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Do")
                            {
                                Days_List.Add(4);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Fr")
                            {
                                Days_List.Add(5);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Sa")
                            {
                                Days_List.Add(6);
                            }
                            if (Speziell.Length > 2)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 5);
                            }
                            else
                            {
                                Speziell = null;
                            }
                        }

                        int k = 0;

                        int w = 0;

                        DateTime trans_dateTime = Datum;

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

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_date_List[k].AddDays(7 * w);

                            if (k == sorted_date_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        int h = 0;

                        int count2 = Count;


                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder Woche")
                        {
                            h = 1;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder zweiten Woche")
                        {
                            h = 2;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder dritten Woche")
                        {
                            h = 3;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder vierten Woche")
                        {
                            h = 4;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        DateTime current_week = Datum;

                        foreach (Transaktion tran in transaktion_list2)
                        {
                            tran.Datum = current_week;

                            current_week = tran.Datum.AddDays(7 * h);
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        int count2 = 0;

                        while (Speziell.Length > 0)
                        {
                            if (Speziell.Length > 3)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 6);
                                count2++;
                            }
                            else
                            {
                                Speziell = null;
                                count2++;
                                break;
                            }
                        }

                        count2 *= Count;

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count2 = Count;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();


                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        Speziell = Virtuelle_Transaktion.Speziell;

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        List<int> Months_List = new List<int>();

                        List<DateTime> sorted_Month_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Speziell))
                        {
                            if (Speziell.Substring(Speziell.Length - 3) == "Jan")
                            {
                                Months_List.Add(01);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Feb")
                            {
                                Months_List.Add(02);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Mrz")
                            {
                                Months_List.Add(03);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Apr")
                            {
                                Months_List.Add(04);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Mai")
                            {
                                Months_List.Add(05);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Jun")
                            {
                                Months_List.Add(06);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Jul")
                            {
                                Months_List.Add(07);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Aug")
                            {
                                Months_List.Add(08);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Sep")
                            {
                                Months_List.Add(09);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Okt")
                            {
                                Months_List.Add(10);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Nov")
                            {
                                Months_List.Add(11);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Dez")
                            {
                                Months_List.Add(12);
                            }
                            if (Speziell.Length > 3)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 6);
                            }
                            else
                            {
                                Speziell = null;
                            }
                        }

                        int k = 0;

                        int w = 0;

                        DateTime trans_dateTime = Datum;

                        foreach (int month in Months_List)
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

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_Month_List[k].AddMonths(12 * w);

                            if (k == sorted_Month_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        int h = 0;

                        int count2 = Count;


                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem Jahr")
                        {
                            h = 1;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem zweitem Jahr")
                        {
                            h = 2;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem drittem Jahr")
                        {
                            h = 3;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 4;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 5;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        DateTime current_year = Datum;

                        foreach (Transaktion tran in transaktion_list2)
                        {
                            tran.Datum = current_year;

                            current_year = tran.Datum.AddYears(1 * h);
                        }
                    }

                    List<Transaktion> transaktion_list3 = new List<Transaktion>();

                    foreach (Transaktion trans in transaktion_list2)
                    {
                        trans.Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;
                        trans.Auftrags_Option = Virtueller_Auftrag.Option;
                        trans.Speziell = Virtuelle_Transaktion.Speziell;
                        trans.Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen;
                        trans.Betrag = Virtuelle_Transaktion.Betrag;
                        trans.Notiz = Virtuelle_Transaktion.Notiz;
                        trans.Zweck = Virtuelle_Transaktion.Zweck;
                        trans.Balance_Visibility = Virtuelle_Transaktion.Balance_Visibility;
                        transaktion_list3.Add(trans);
                    }

                    foreach (Transaktion trans in transaktion_list3)
                    {
                        await ContentService.Edit_Transaktion(trans);
                    }

                    transaktion_list3 = transaktion_list3.OrderBy(d => d.Datum).ToList();

                    if(false == await NotificationHelper.ModifyNotification(transaktion_list3.Last(),0))
                    {
                        return Errorhandler.Errors[1];
                    }
                }
                else
                {
                    List<Transaktion> transaktion_list2 = new List<Transaktion>();

                    foreach (Transaktion trans in transaktion_list)
                    {
                        if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                        {
                            if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                            {
                                transaktion_list2.Add(trans);
                            }
                        }
                    }

                    string Speziell = Virtuelle_Transaktion.Speziell;

                    int count = 0;

                    DateTime current_day = Datum;

                    DateTime enddate = DateTime.Now;

                    if (Virtueller_Auftrag.Option == 2)
                    {
                        count = int.Parse(Virtueller_Auftrag.Anzahl_an_Wiederholungen);
                    }
                    if (Virtueller_Auftrag.Option == 3)
                    {
                        enddate = DateTime.ParseExact(Virtueller_Auftrag.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24).AddMilliseconds(-1);
                    }

                    int Count = count + 1;

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jeden Tag")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count = (enddate - current_day).Days;

                            Count = count + 1;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jede Woche")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jeden Monat")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddMonths(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Jedes Jahr")
                    {
                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1);
                                count++;
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        string Days = Virtueller_Auftrag.Speziell;

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

                        DateTime trans_dateTime = Datum;

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

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

                            bool indicator = false;

                            int follower = 0;

                            int follower2 = 0;

                            int follower3 = 0;

                            while (indicator == false)
                            {
                                dates.Add(sorted_date_List[follower2].AddDays(7 * follower3));

                                if (dates[follower] > DateTime.ParseExact(Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24))
                                {
                                    indicator = true;

                                    dates.RemoveAt(follower);

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
                                    count++;
                                }
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        int h = 0;

                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder Woche")
                        {
                            h = 1;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder zweiten Woche")
                        {
                            h = 2;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder dritten Woche")
                        {
                            h = 3;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jeder vierten Woche")
                        {
                            h = 4;
                        }

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddDays(7 * h);

                                count++;
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }

                            Count = count + 1;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        string Months = Virtueller_Auftrag.Speziell;

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

                        Count = (count + 1) * Month_List.Count;

                        DateTime trans_dateTime = Datum;

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

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            List<DateTime> dates = new List<DateTime>();

                            bool indicator = false;

                            Count = 0;

                            int follower = 0;

                            int follower2 = 0;

                            int follower3 = 0;

                            while (indicator == false)
                            {
                                dates.Add(sorted_Month_List[follower2].AddMonths(12 * follower3));

                                if (dates[follower] > enddate)
                                {
                                    indicator = true;

                                    dates.RemoveAt(follower);

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
                                    count++;
                                }
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }

                            Count = count;
                        }
                    }

                    if (Virtueller_Auftrag.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        int h = 0;

                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem Jahr")
                        {
                            h = 1;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem zweitem Jahr")
                        {
                            h = 2;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem drittem Jahr")
                        {
                            h = 3;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 4;
                        }
                        if (Virtueller_Auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 5;
                        }

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            while (current_day <= enddate)
                            {
                                current_day = current_day.AddYears(1 * h);
                                count++;
                            }

                            if (count == 0)
                            {
                                return Errorhandler.Errors[0];
                            }

                            Count = count + 1;
                        }
                    }


                    if (list_of_kinds.Contains(Virtuelle_Transaktion.Art_an_Wiederholungen) == true)
                    {
                        if (Count + 1 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(Count, transaktion_list2.Count - Count + 1));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (Count + 1 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, Count + 1 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        int w = 0;

                        DateTime trans_dateTime = Datum;

                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jedes Jahr")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddYears(w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Monat")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddMonths(w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jede Woche")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddDays(7 * w);

                                w++;
                            }
                        }
                        if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Jeden Tag")
                        {
                            foreach (Transaktion trans in transaktion_list2)
                            {
                                trans.Datum = trans_dateTime.AddDays(1 * w);

                                w++;
                            }
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        int count2 = 0;

                        while (Speziell.Length > 0)
                        {
                            if (Speziell.Length > 2)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 5);
                                count2++;
                            }
                            else
                            {
                                Speziell = null;
                                count2++;
                                break;
                            }
                        }

                        count2 *= Count;

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count2 = Count;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();


                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        Speziell = Virtuelle_Transaktion.Speziell;

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        List<int> Days_List = new List<int>();

                        List<DateTime> sorted_date_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Speziell))
                        {
                            if (Speziell.Substring(Speziell.Length - 2) == "So")
                            {
                                Days_List.Add(0);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Mo")
                            {
                                Days_List.Add(1);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Di")
                            {
                                Days_List.Add(2);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Mi")
                            {
                                Days_List.Add(3);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Do")
                            {
                                Days_List.Add(4);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Fr")
                            {
                                Days_List.Add(5);
                            }
                            if (Speziell.Substring(Speziell.Length - 2) == "Sa")
                            {
                                Days_List.Add(6);
                            }
                            if (Speziell.Length > 2)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 5);
                            }
                            else
                            {
                                Speziell = null;
                            }
                        }

                        int k = 0;

                        int w = 0;

                        DateTime trans_dateTime = Datum;

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

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_date_List[k].AddDays(7 * w);

                            if (k == sorted_date_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        int h = 0;

                        int count2 = Count;


                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder Woche")
                        {
                            h = 1;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder zweiten Woche")
                        {
                            h = 2;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder dritten Woche")
                        {
                            h = 3;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jeder vierten Woche")
                        {
                            h = 4;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        DateTime current_week = Datum;

                        foreach (Transaktion tran in transaktion_list2)
                        {
                            tran.Datum = current_week;

                            current_week = tran.Datum.AddDays(7 * h);
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        int count2 = 0;

                        while (Speziell.Length > 0)
                        {
                            if (Speziell.Length > 3)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 6);
                                count2++;
                            }
                            else
                            {
                                Speziell = null;
                                count2++;
                                break;
                            }
                        }

                        count2 *= Count;

                        if (Virtueller_Auftrag.Option == 3)
                        {
                            count2 = Count;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();


                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        Speziell = Virtuelle_Transaktion.Speziell;

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        List<int> Months_List = new List<int>();

                        List<DateTime> sorted_Month_List = new List<DateTime>();

                        while (!string.IsNullOrEmpty(Speziell))
                        {
                            if (Speziell.Substring(Speziell.Length - 3) == "Jan")
                            {
                                Months_List.Add(01);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Feb")
                            {
                                Months_List.Add(02);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Mrz")
                            {
                                Months_List.Add(03);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Apr")
                            {
                                Months_List.Add(04);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Mai")
                            {
                                Months_List.Add(05);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Jun")
                            {
                                Months_List.Add(06);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Jul")
                            {
                                Months_List.Add(07);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Aug")
                            {
                                Months_List.Add(08);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Sep")
                            {
                                Months_List.Add(09);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Okt")
                            {
                                Months_List.Add(10);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Nov")
                            {
                                Months_List.Add(11);
                            }
                            if (Speziell.Substring(Speziell.Length - 3) == "Dez")
                            {
                                Months_List.Add(12);
                            }
                            if (Speziell.Length > 3)
                            {
                                Speziell = Speziell.Remove(Speziell.Length - 6);
                            }
                            else
                            {
                                Speziell = null;
                            }
                        }

                        int k = 0;

                        int w = 0;

                        DateTime trans_dateTime = Datum;

                        foreach (int month in Months_List)
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

                        foreach (Transaktion trans in transaktion_list2)
                        {
                            trans.Datum = sorted_Month_List[k].AddMonths(12 * w);

                            if (k == sorted_Month_List.Count() - 1)
                            {
                                k = -1;

                                w++;
                            }

                            k++;
                        }
                    }

                    if (Virtuelle_Transaktion.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        int h = 0;

                        int count2 = Count;


                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem Jahr")
                        {
                            h = 1;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem zweitem Jahr")
                        {
                            h = 2;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem drittem Jahr")
                        {
                            h = 3;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 4;
                        }
                        if (Virtuelle_Transaktion.Speziell == "derselbe Tag in jedem viertem Jahr")
                        {
                            h = 5;
                        }

                        if (count2 < transaktion_list2.Count)
                        {
                            List<Transaktion> transaktions = new List<Transaktion>();
                            transaktions.AddRange(transaktion_list2.GetRange(count2, transaktion_list2.Count - count2));
                            foreach (Transaktion trans in transaktions)
                            {
                                await ContentService.Remove_Transaktion(trans);
                            }
                        }

                        if (count2 > transaktion_list2.Count)
                        {
                            List<Transaktion> wiedrholungen = Enumerable.Repeat(Virtuelle_Transaktion, count2 - transaktion_list2.Count).ToList();

                            int l = transaktion_list2.Count + 1;

                            foreach (Transaktion trans in wiedrholungen)
                            {
                                trans.Auftrags_id = "" + OrderId + "." + l + "";

                                await ContentService.Add_Transaktion(trans);
                                l++;
                            }
                        }

                        transaktion_list2.Clear();

                        transaktion_list.Clear();

                        transaktion_list = new List<Transaktion>(await ContentService.Get_all_enabeled_Transaktion());

                        foreach (Transaktion trans in transaktion_list)
                        {
                            if (string.IsNullOrEmpty(trans.Auftrags_id) == false)
                            {
                                if (trans.Auftrags_id.Substring(0, trans.Auftrags_id.IndexOf(".")) == OrderId.ToString())
                                {
                                    transaktion_list2.Add(trans);
                                }
                            }
                        }

                        DateTime current_year = Datum;

                        foreach (Transaktion tran in transaktion_list2)
                        {
                            tran.Datum = current_year;

                            current_year = tran.Datum.AddYears(1 * h);
                        }
                    }

                    List<Transaktion> transaktion_list3 = new List<Transaktion>();

                    foreach (Transaktion trans in transaktion_list2)
                    {
                        trans.Art_an_Wiederholungen = Virtuelle_Transaktion.Art_an_Wiederholungen;
                        trans.Auftrags_Option = Virtueller_Auftrag.Option;
                        trans.Speziell = Virtuelle_Transaktion.Speziell;
                        trans.Anzahl_an_Wiederholungen = Virtuelle_Transaktion.Anzahl_an_Wiederholungen;
                        trans.Betrag = Virtuelle_Transaktion.Betrag;
                        trans.Notiz = Virtuelle_Transaktion.Notiz;
                        trans.Zweck = Virtuelle_Transaktion.Zweck;
                        trans.Balance_Visibility = Virtuelle_Transaktion.Balance_Visibility;
                        transaktion_list3.Add(trans);
                    }

                    foreach (Transaktion trans in transaktion_list3)
                    {
                        await ContentService.Edit_Transaktion(trans);
                    }

                    transaktion_list3 = transaktion_list3.OrderBy(d => d.Datum).ToList();

                    if (false == await NotificationHelper.ModifyNotification(transaktion_list3.Last(), 0))
                    {
                        return Errorhandler.Errors[1];
                    }
                }
            }

            return "succesfull";
        }
    }
}
