using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(Viertuelle_TransaktionID), nameof(Viertuelle_TransaktionID))]
    [QueryProperty(nameof(OrderID), nameof(OrderID))]
    [QueryProperty(nameof(EditID), nameof(EditID))]
    public class Edit_Order_PageModel : FreshBasePageModel
    {
        public Edit_Order_PageModel()
        {
            Return_Command = new AsyncCommand(Return_Methode);

            ViewIsAppearing_Command = new AsyncCommand(ViewIsAppearing);

            Kind_Source = new ObservableRangeCollection<string>() { "Tag", "Woche", "Monat", "Jahr" };

            Count_Source = new ObservableRangeCollection<string>(Enumerable.Range(1, 356).Select(n => n.ToString()).ToList());

            Option_Source = new ObservableRangeCollection<string>() { "Einmalig", "Nach einer Anzahl", "Bis zu einem Datum" };
        }

        public async Task Return_Methode()
        {
            try
            {
                if (Option_Item == "Einmalig")
                {
                    Viertueller_Auftrag.Anzahl_an_Wiederholungen = "Einmalig";
                    Viertueller_Auftrag.Option = 1;
                }
                if (Option_Item == "Nach einer Anzahl")
                {
                    Viertueller_Auftrag.Anzahl_an_Wiederholungen = Count_Item.ToString();
                    Viertueller_Auftrag.Option = 2;
                }
                if (Option_Item == "Bis zu einem Datum")
                {
                    Viertueller_Auftrag.Anzahl_an_Wiederholungen = Date.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                    Viertueller_Auftrag.Option = 3;
                }

                if (Kind_Item == "Tag")
                {
                    List<bool> istoggledlist = new List<bool>()
                    {
                    Monday_Switch,
                    Tuesday_Switch,
                    Wednesday_Switch,
                    Thursday_Switch,
                    Friday_Switch,
                    Saturday_Switch,
                    Sunday_Switch
                    };

                    if (istoggledlist.Contains(true) == true)
                    {
                        List<string> Daylist = new List<string>();

                        string Days = "";

                        if (Monday_Switch == true)
                        {
                            Daylist.Add("Mo");
                        }
                        if (Tuesday_Switch == true)
                        {
                            Daylist.Add("Di");
                        }
                        if (Wednesday_Switch == true)
                        {
                            Daylist.Add("Mi");
                        }
                        if (Thursday_Switch == true)
                        {
                            Daylist.Add("Do");
                        }
                        if (Friday_Switch == true)
                        {
                            Daylist.Add("Fr");
                        }
                        if (Saturday_Switch == true)
                        {
                            Daylist.Add("Sa");
                        }
                        if (Sunday_Switch == true)
                        {
                            Daylist.Add("So");
                        }

                        try
                        {
                            foreach (string Day in Daylist)
                            {
                                Days += "" + Day + " , ";
                            }

                            Days = Days.Remove(Days.Length - 3);
                        }
                        catch { }

                        Viertueller_Auftrag.Art_an_Wiederholungen = "Spezielle Tage";

                        if (Daylist.Count() == 7 && Viertueller_Auftrag.Option == 2  && Count_Item == "364")
                        {
                            Viertueller_Auftrag.Art_an_Wiederholungen = "Jeden Tag";

                            Days = "jeder Tag in der Woche";
                        }

                        Viertueller_Auftrag.Speziell = Days;
                    }
                    else
                    {
                        await Return_Zero();
                        return;
                    }
                }
                if (Kind_Item == "Woche")
                {
                    List<bool> istoggledlist = new List<bool>()
                    {
                    Ever_Week_Switch,
                    Ever_Second_Week_Switch,
                    Ever_Third_Week_Switch,
                    Ever_Fourth_Week_Switch
                    };

                    if (istoggledlist.Contains(true) == true)
                    {
                        string week = null;

                        if (Ever_Week_Switch == true)
                        {
                            week = "derselbe Tag in jeder Woche";
                        }
                        if (Ever_Second_Week_Switch == true)
                        {
                            week = "derselbe Tag in jeder zweiten Woche";
                        }
                        if (Ever_Third_Week_Switch == true)
                        {
                            week = "derselbe Tag in jeder dritten Woche";
                        }
                        if (Ever_Fourth_Week_Switch == true)
                        {
                            week = "derselbe Tag in jeder vierten Woche";
                        }

                        Viertueller_Auftrag.Art_an_Wiederholungen = "Spezielle Wochen";

                        if(week == "derselbe Tag in jeder Woche" && Viertueller_Auftrag.Option == 2 && Count_Item == "51")
                        {
                            Viertueller_Auftrag.Art_an_Wiederholungen = "Jede Woche";
                        }

                        Viertueller_Auftrag.Speziell = week;
                    }
                    else
                    {
                        await Return_Zero();
                        return;
                    }
                }
                if (Kind_Item == "Monat")
                {
                    List<bool> istoggledlist = new List<bool>()
                    {
                    January_Switch,
                    February_Switch,
                    March_Switch,
                    April_Switch,
                    May_Switch,
                    June_Switch,
                    July_Switch,
                    August_Switch,
                    September_Switch,
                    October_Switch,
                    November_Switch,
                    December_Switch
                    };

                    if (istoggledlist.Contains(true) == true)
                    {
                        List<string> Monthlist = new List<string>();

                        string Month = null;

                        if (January_Switch == true)
                        {
                            Monthlist.Add("Jan");
                        }
                        if (February_Switch == true)
                        {
                            Monthlist.Add("Feb");
                        }
                        if (March_Switch == true)
                        {
                            Monthlist.Add("Mrz");
                        }
                        if (April_Switch == true)
                        {
                            Monthlist.Add("Apr");
                        }
                        if (May_Switch == true)
                        {
                            Monthlist.Add("Mai");
                        }
                        if (June_Switch == true)
                        {
                            Monthlist.Add("Jun");
                        }
                        if (July_Switch == true)
                        {
                            Monthlist.Add("Jul");
                        }
                        if (August_Switch == true)
                        {
                            Monthlist.Add("Aug");
                        }
                        if (September_Switch == true)
                        {
                            Monthlist.Add("Sep");
                        }
                        if (October_Switch == true)
                        {
                            Monthlist.Add("Okt");
                        }
                        if (November_Switch == true)
                        {
                            Monthlist.Add("Nov");
                        }
                        if (December_Switch == true)
                        {
                            Monthlist.Add("Dez");
                        }

                        try
                        {
                            foreach (string month in Monthlist)
                            {
                                Month += "" + month + " , ";
                            }

                            Month = Month.Remove(Month.Length - 3);
                        }
                        catch { }

                        Viertueller_Auftrag.Art_an_Wiederholungen = "Spezielle Monate";

                        if (Monthlist.Count() == 12 && Viertueller_Auftrag.Option == 2 && Count_Item == "11")
                        {
                            Viertueller_Auftrag.Art_an_Wiederholungen = "Jeden Monat";

                            Month = "derselbe Tag im Monat";
                        }

                        Viertueller_Auftrag.Speziell = Month;
                    }
                    else
                    {
                        await Return_Zero();
                        return;
                    }
                }
                if (Kind_Item == "Jahr")
                {
                    List<bool> istoggledlist = new List<bool>()
                {
                    Ever_Year_Switch,
                    Ever_Second_Year_Switch,
                    Ever_Third_Year_Switch,
                    Ever_Fourth_Year_Switch,
                    Ever_Fifth_Year_Switch
                };

                    if (istoggledlist.Contains(true) == true)
                    {
                        string year = null;

                        if (Ever_Year_Switch == true)
                        {
                            year = "derselbe Tag in jedem Jahr";
                        }
                        if (Ever_Second_Year_Switch == true)
                        {
                            year = "derselbe Tag in jedem zweitem Jahr";
                        }
                        if (Ever_Third_Year_Switch == true)
                        {
                            year = "derselbe Tag in jedem drittem Jahr";
                        }
                        if (Ever_Fourth_Year_Switch == true)
                        {
                            year = "derselbe Tag in jedem viertem Jahr";
                        }
                        if (Ever_Fifth_Year_Switch == true)
                        {
                            year = "derselbe Tag in jedem fünftem Jahr";
                        }

                        Viertueller_Auftrag.Art_an_Wiederholungen = "Spezielle Jahre";

                        if (year == "derselbe Tag in jedem Jahr" && Viertueller_Auftrag.Option == 2 && Count_Item == "4")
                        {
                            Viertueller_Auftrag.Art_an_Wiederholungen = "Jedes Jahr";

                            year = "derselbe Tag im Jahr";
                        }

                        Viertueller_Auftrag.Speziell = year;
                    }
                    else
                    {
                        await Return_Zero();
                        return;
                    }
                }

                Viertuelle_Transaktion.Anzahl_an_Wiederholungen = Viertueller_Auftrag.Anzahl_an_Wiederholungen;

                Viertuelle_Transaktion.Art_an_Wiederholungen = Viertueller_Auftrag.Art_an_Wiederholungen;

                Viertuelle_Transaktion.Speziell = Viertueller_Auftrag.Speziell;

                await PassingTransaktionService.Edit_Transaktion(Viertuelle_Transaktion);

                await PassingOrderService.Edit_Order(Viertueller_Auftrag);

                await Shell.Current.GoToAsync($"..?TransaktionID={Viertuelle_TransaktionID}&OrderID={OrderId}&EditID={EditID}");
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                await Shell.Current.GoToAsync($"..?TransaktionID={Viertuelle_TransaktionID}&OrderID={OrderID}&EditID={EditID}");
            }
        }

        private async Task Return_Zero()
        {
            await Shell.Current.GoToAsync($"..?TransaktionID={Viertuelle_TransaktionID}&OrderID={OrderID}&EditID={EditID}");
        }

        public async Task ViewIsAppearing()
        {
            try
            {
                if (int.TryParse(Viertuelle_TransaktionID, out var result1) == true)
                {
                    Viertuelle_Transaktion = await PassingTransaktionService.Get_specific_Transaktion(result1);

                    Minimum_Date = Viertuelle_Transaktion.Datum.Date;

                    Date = Viertuelle_Transaktion.Datum.Date;
                }

                else
                {
                    Minimum_Date = DateTime.Now;

                    Date = DateTime.Now;
                }

                if (int.TryParse(OrderID, out var result2) == true)
                {
                    OrderId = result2;

                    Viertueller_Auftrag = await PassingOrderService.Get_specific_Order(result2);

                    if (result2 != 0)
                    {
                        if (Viertueller_Auftrag.Option == 1)
                        {
                            Option_Item = "Einmalig";

                            Count_Item = null;
                        }
                        if (Viertueller_Auftrag.Option == 2)
                        {
                            Option_Item = "Nach einer Anzahl";

                            Amount_Option_Visibility = true;

                            Count_Item = Viertueller_Auftrag.Anzahl_an_Wiederholungen;
                        }
                        if (Viertueller_Auftrag.Option == 3)
                        {
                            Option_Item = "Bis zu einem Datum";

                            Date = DateTime.ParseExact(Viertueller_Auftrag.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE"));

                            Count_Item = null;
                        }
                    }

                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Jeden Tag")
                    {
                        Kind_Item = "Tag";

                        Days_Visibility = true;

                        Monday_Switch = true;
                        Tuesday_Switch = true;
                        Wednesday_Switch = true;
                        Thursday_Switch = true;
                        Friday_Switch = true;
                        Saturday_Switch = true;
                        Sunday_Switch = true;
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Jede Woche")
                    {
                        Kind_Item = "Woche";

                        Week_Visibility = true;

                        Ever_Week_Switch = true;
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Jeden Monat")
                    {
                        Kind_Item = "Monat";

                        Month_Visibility = true;

                        January_Switch = true;
                        February_Switch = true;
                        March_Switch = true;
                        April_Switch = true;
                        May_Switch = true;
                        June_Switch = true;
                        July_Switch = true;
                        August_Switch = true;
                        September_Switch = true;
                        October_Switch = true;
                        November_Switch = true;
                        December_Switch = true;
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Jedes Jahr")
                    {
                        Kind_Item = "Jahr";

                        Year_Visibility = true;

                        Ever_Year_Switch = true;
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Spezielle Tage")
                    {
                        Kind_Item = "Tag";
                        string Days = Viertueller_Auftrag.Speziell;

                        while (Days.Length >= 0)
                        {
                            if (Days.Substring(Days.Length - 2) == "Mo")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Monday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "Di")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Tuesday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "Mi")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Wednesday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "Do")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Thursday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "Fr")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Friday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "Sa")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Saturday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                            if (Days.Substring(Days.Length - 2) == "So")
                            {
                                if (Days.Length > 2)
                                {
                                    Days = Days.Remove(Days.Length - 5);
                                }
                                else
                                {
                                    Days = null;
                                }
                                Sunday_Switch = true;
                                if (String.IsNullOrEmpty(Days) == true)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Spezielle Wochen")
                    {
                        Kind_Item = "Woche";

                        if (String.IsNullOrEmpty(Viertueller_Auftrag.Speziell) == false)
                        {
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jeder Woche")
                            {
                                Ever_Week_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jeder zweiten Woche")
                            {
                                Ever_Second_Week_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jeder dritten Woche")
                            {
                                Ever_Third_Week_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jeder vierten Woche")
                            {
                                Ever_Fourth_Week_Switch = true;
                            }
                        }
                        else
                        {
                            Ever_Week_Switch = false;
                            Ever_Second_Week_Switch = false;
                            Ever_Third_Week_Switch = false;
                            Ever_Fourth_Week_Switch = false;
                        }
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Spezielle Monate")
                    {
                        Kind_Item = "Monat";

                        string Months = Viertueller_Auftrag.Speziell;

                        while (Months.Length >= 0)
                        {
                            if (Months.Substring(Months.Length - 3) == "Jan")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                January_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Feb")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                February_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Mrz")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                March_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Apr")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                April_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Mai")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                May_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Jun")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                June_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Jul")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                July_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Aug")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                August_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Sep")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                September_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Okt")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                October_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Nov")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                November_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                            if (Months.Substring(Months.Length - 3) == "Dez")
                            {
                                if (Months.Length > 3)
                                {
                                    Months = Months.Remove(Months.Length - 6);
                                }
                                else
                                {
                                    Months = null;
                                }
                                December_Switch = true;
                                if (String.IsNullOrEmpty(Months) == true)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (Viertueller_Auftrag.Art_an_Wiederholungen == "Spezielle Jahre")
                    {
                        Kind_Item = "Jahr";

                        if (String.IsNullOrEmpty(Viertueller_Auftrag.Speziell) == false)
                        {
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jedem Jahr")
                            {
                                Ever_Year_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jedem zweitem Jahr")
                            {
                                Ever_Second_Year_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jedem drittem Jahr")
                            {
                                Ever_Third_Year_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                            {
                                Ever_Fourth_Year_Switch = true;
                            }
                            if (Viertueller_Auftrag.Speziell == "derselbe Tag in jedem fünftem Jahr")
                            {
                                Ever_Fifth_Year_Switch = true;
                            }
                        }
                        else
                        {
                            Ever_Year_Switch = false;
                            Ever_Second_Year_Switch = false;
                            Ever_Third_Year_Switch = false;
                            Ever_Fourth_Year_Switch = false;
                            Ever_Fifth_Year_Switch = false;
                        }
                    }
                }

                if (int.TryParse(EditID, out var result3) == true)
                {
                    Edit_Version = result3;
                }           
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                await Shell.Current.GoToAsync($"..?TransaktionID={Viertuelle_TransaktionID}&OrderID={OrderID}&EditID={EditID}");
            }
        }

        public Auftrag Viertueller_Auftrag;

        public Transaktion transaktion;
        public Transaktion Viertuelle_Transaktion
        {
            get { return transaktion; }
            set
            {
                if (Viertuelle_Transaktion == value)
                    return;
                transaktion = value; RaisePropertyChanged();
            }
        }

        public string Viertuelle_TransaktionID { get; set; }
        public string OrderID { get; set; }
        public string EditID { get; set; }

        public int OrderId { get; set; }

        public int transaktion_Position = 1;

        public int Edit_Version { get; set; }

        public string kind_item;
        public string Kind_Item
        {
            get { return kind_item; }
            set
            {
                if (Kind_Item == value)
                    return;
                kind_item = value; RaisePropertyChanged();

                if (kind_item == "Tag")
                {
                    Days_Visibility = true;
                    Week_Visibility = false;
                    Month_Visibility = false;
                    Year_Visibility = false;
                }
                if (kind_item == "Woche")
                {
                    Days_Visibility = false;
                    Week_Visibility = true;
                    Month_Visibility = false;
                    Year_Visibility = false;
                }
                if (kind_item == "Monat")
                {
                    Days_Visibility = false;
                    Week_Visibility = false;
                    Month_Visibility = true;
                    Year_Visibility = false;
                }
                if (kind_item == "Jahr")
                {
                    Days_Visibility = false;
                    Week_Visibility = false;
                    Month_Visibility = false;
                    Year_Visibility = true;
                }
            }
        }

        public string count_item;
        public string Count_Item
        {
            get { return count_item; }
            set
            {
                if (Count_Item == value)
                    return;
                count_item = value; RaisePropertyChanged();
            }
        }

        public string option_item;
        public string Option_Item
        {
            get { return option_item; }
            set
            {
                if (Option_Item == value)
                    return;
                option_item = value; RaisePropertyChanged();
                if (value == "Einmalig")
                {
                    Date_Option_Visibility = false;

                    Amount_Option_Visibility = false;

                    Onc_Option_Visibility = true;
                }
                if (value == "Nach einer Anzahl")
                {
                    Date_Option_Visibility = false;

                    Amount_Option_Visibility = true;

                    Onc_Option_Visibility = false;
                }
                if (value == "Bis zu einem Datum")
                {
                    Date_Option_Visibility = true;

                    Amount_Option_Visibility = false;

                    Onc_Option_Visibility = false;
                }
            }
        }

        public ObservableRangeCollection<string> count_source;
        public ObservableRangeCollection<string> Count_Source
        {
            get { return count_source; }
            set
            {
                if (Count_Source == value)
                    return;
                count_source = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<string> kind_source;
        public ObservableRangeCollection<string> Kind_Source
        {
            get { return kind_source; }
            set
            {
                if (Kind_Source == value)
                    return;
                kind_source = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<string> Option_Source { get; set; }

        public AsyncCommand Return_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public bool onc_option_visibility = false;
        public bool Onc_Option_Visibility
        {
            get { return onc_option_visibility; }
            set
            {
                if (Onc_Option_Visibility == value)
                    return;
                onc_option_visibility = value; RaisePropertyChanged();
            }
        }

        public bool amount_option_visibility = false;
        public bool Amount_Option_Visibility
        {
            get { return amount_option_visibility; }
            set
            {
                if (Amount_Option_Visibility == value)
                    return;
                amount_option_visibility = value; RaisePropertyChanged();
            }
        }

        public bool date_option_visibility = false;
        public bool Date_Option_Visibility
        {
            get { return date_option_visibility; }
            set
            {
                if (Date_Option_Visibility == value)
                    return;
                date_option_visibility = value; RaisePropertyChanged();
            }
        }

        public bool days_visibility = false;
        public bool Days_Visibility
        {
            get { return days_visibility; }
            set
            {
                if (Days_Visibility == value)
                    return;
                days_visibility = value; RaisePropertyChanged();
                if(Days_Visibility == false)
                {
                    Monday_Switch = false;
                    Tuesday_Switch = false;
                    Wednesday_Switch = false;
                    Thursday_Switch = false;
                    Friday_Switch = false;
                    Saturday_Switch = false;
                    Sunday_Switch = false;
                }
            }
        }

        DateTime date = DateTime.Now;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (Date == value)
                    return;
                date = value; RaisePropertyChanged();
            }
        }

        DateTime minimum_date = DateTime.Now;
        public DateTime Minimum_Date
        {
            get { return minimum_date; }
            set
            {
                if (Minimum_Date == value)
                    return;
                minimum_date = value; RaisePropertyChanged();
            }
        }

        public bool monday_switch = false;
        public bool Monday_Switch
        {
            get { return monday_switch; }
            set
            {
                if (Monday_Switch == value)
                    return;
                monday_switch = value; RaisePropertyChanged();
            }
        }

        public bool tuesday_switch = false;
        public bool Tuesday_Switch
        {
            get { return tuesday_switch; }
            set
            {
                if (Tuesday_Switch == value)
                    return;
                tuesday_switch = value; RaisePropertyChanged();
            }
        }

        public bool wednesday_switch = false;
        public bool Wednesday_Switch
        {
            get { return wednesday_switch; }
            set
            {
                if (Wednesday_Switch == value)
                    return;
                wednesday_switch = value; RaisePropertyChanged();
            }
        }

        public bool thursday_switch = false;
        public bool Thursday_Switch
        {
            get { return thursday_switch; }
            set
            {
                if (Thursday_Switch == value)
                    return;
                thursday_switch = value; RaisePropertyChanged();
            }
        }

        public bool friday_switch = false;
        public bool Friday_Switch
        {
            get { return friday_switch; }
            set
            {
                if (Friday_Switch == value)
                    return;
                friday_switch = value; RaisePropertyChanged();
            }
        }

        public bool saturday_switch = false;
        public bool Saturday_Switch
        {
            get { return saturday_switch; }
            set
            {
                if (Saturday_Switch == value)
                    return;
                saturday_switch = value; RaisePropertyChanged();
            }
        }

        public bool sunday_switch = false;
        public bool Sunday_Switch
        {
            get { return sunday_switch; }
            set
            {
                if (Sunday_Switch == value)
                    return;
                sunday_switch = value; RaisePropertyChanged();
            }
        }

        public bool ever_week_switch = false;
        public bool Ever_Week_Switch
        {
            get { return ever_week_switch; }
            set
            {
                if (Ever_Week_Switch == value)
                    return;
                ever_week_switch = value; RaisePropertyChanged();
                if (ever_week_switch == true)
                {
                    Ever_Second_Week_Switch = false;
                    Ever_Third_Week_Switch = false;
                    Ever_Fourth_Week_Switch = false;
                }
            }
        }

        public bool ever_second_week_switch = false;
        public bool Ever_Second_Week_Switch
        {
            get { return ever_second_week_switch; }
            set
            {
                if (Ever_Second_Week_Switch == value)
                    return;
                ever_second_week_switch = value; RaisePropertyChanged();
                if (ever_second_week_switch == true)
                {
                    Ever_Week_Switch = false;
                    Ever_Third_Week_Switch = false;
                    Ever_Fourth_Week_Switch = false;
                }
            }
        }

        public bool ever_third_week_switch = false;
        public bool Ever_Third_Week_Switch
        {
            get { return ever_third_week_switch; }
            set
            {
                if (Ever_Third_Week_Switch == value)
                    return;
                ever_third_week_switch = value; RaisePropertyChanged();
                if (ever_third_week_switch == true)
                {
                    Ever_Second_Week_Switch = false;
                    Ever_Week_Switch = false;
                    Ever_Fourth_Week_Switch = false;
                }
            }
        }

        public bool ever_fourth_week_switch = false;
        public bool Ever_Fourth_Week_Switch
        {
            get { return ever_fourth_week_switch; }
            set
            {
                if (Ever_Fourth_Week_Switch == value)
                    return;
                ever_fourth_week_switch = value; RaisePropertyChanged();
                if (ever_fourth_week_switch == true)
                {
                    Ever_Second_Week_Switch = false;
                    Ever_Third_Week_Switch = false;
                    Ever_Week_Switch = false;
                }
            }
        }

        public bool ever_year_switch = false;
        public bool Ever_Year_Switch
        {
            get { return ever_year_switch; }
            set
            {
                if (Ever_Year_Switch == value)
                    return;
                ever_year_switch = value; RaisePropertyChanged();
                if (ever_year_switch == true)
                {
                    Ever_Second_Year_Switch = false;
                    Ever_Third_Year_Switch = false;
                    Ever_Fourth_Year_Switch = false;
                    Ever_Fifth_Year_Switch = false;
                }
            }
        }

        public bool ever_second_year_switch = false;
        public bool Ever_Second_Year_Switch
        {
            get { return ever_second_year_switch; }
            set
            {
                if (Ever_Second_Year_Switch == value)
                    return;
                ever_second_year_switch = value; RaisePropertyChanged();
                if (ever_second_year_switch == true)
                {
                    Ever_Year_Switch = false;
                    Ever_Third_Year_Switch = false;
                    Ever_Fourth_Year_Switch = false;
                    Ever_Fifth_Year_Switch = false;
                }
            }
        }

        public bool ever_third_year_switch = false;
        public bool Ever_Third_Year_Switch
        {
            get { return ever_third_year_switch; }
            set
            {
                if (Ever_Third_Year_Switch == value)
                    return;
                ever_third_year_switch = value; RaisePropertyChanged();
                if (ever_third_year_switch == true)
                {
                    Ever_Second_Year_Switch = false;
                    Ever_Year_Switch = false;
                    Ever_Fourth_Year_Switch = false;
                    Ever_Fifth_Year_Switch = false;
                }
            }
        }

        public bool ever_fourth_year_switch = false;
        public bool Ever_Fourth_Year_Switch
        {
            get { return ever_fourth_year_switch; }
            set
            {
                if (Ever_Fourth_Year_Switch == value)
                    return;
                ever_fourth_year_switch = value; RaisePropertyChanged();
                if (ever_fourth_year_switch == true)
                {
                    Ever_Second_Year_Switch = false;
                    Ever_Third_Year_Switch = false;
                    Ever_Year_Switch = false;
                    Ever_Fifth_Year_Switch = false;
                }
            }
        }

        public bool ever_fifth_year_switch = false;
        public bool Ever_Fifth_Year_Switch
        {
            get { return ever_fifth_year_switch; }
            set
            {
                if (Ever_Fifth_Year_Switch == value)
                    return;
                ever_fifth_year_switch = value; RaisePropertyChanged();
                if (ever_fifth_year_switch == true)
                {
                    Ever_Second_Year_Switch = false;
                    Ever_Third_Year_Switch = false;
                    Ever_Year_Switch = false;
                    Ever_Fourth_Year_Switch = false;
                }
            }
        }

        public bool january_switch = false;
        public bool January_Switch
        {
            get { return january_switch; }
            set
            {
                if (January_Switch == value)
                    return;
                january_switch = value; RaisePropertyChanged();
            }
        }

        public bool february_switch = false;
        public bool February_Switch
        {
            get { return february_switch; }
            set
            {
                if (February_Switch == value)
                    return;
                february_switch = value; RaisePropertyChanged();
            }
        }

        public bool march_switch = false;
        public bool March_Switch
        {
            get { return march_switch; }
            set
            {
                if (March_Switch == value)
                    return;
                march_switch = value; RaisePropertyChanged();
            }
        }

        public bool april_switch = false;
        public bool April_Switch
        {
            get { return april_switch; }
            set
            {
                if (April_Switch == value)
                    return;
                april_switch = value; RaisePropertyChanged();
            }
        }

        public bool may_switch = false;
        public bool May_Switch
        {
            get { return may_switch; }
            set
            {
                if (May_Switch == value)
                    return;
                may_switch = value; RaisePropertyChanged();
            }
        }

        public bool june_switch = false;
        public bool June_Switch
        {
            get { return june_switch; }
            set
            {
                if (June_Switch == value)
                    return;
                june_switch = value; RaisePropertyChanged();
            }
        }

        public bool july_switch = false;
        public bool July_Switch
        {
            get { return july_switch; }
            set
            {
                if (July_Switch == value)
                    return;
                july_switch = value; RaisePropertyChanged();
            }
        }

        public bool august_switch = false;
        public bool August_Switch
        {
            get { return august_switch; }
            set
            {
                if (August_Switch == value)
                    return;
                august_switch = value; RaisePropertyChanged();
            }
        }

        public bool september_switch = false;
        public bool September_Switch
        {
            get { return september_switch; }
            set
            {
                if (September_Switch == value)
                    return;
                september_switch = value; RaisePropertyChanged();
            }
        }

        public bool october_switch = false;
        public bool October_Switch
        {
            get { return october_switch; }
            set
            {
                if (October_Switch == value)
                    return;
                october_switch = value; RaisePropertyChanged();
            }
        }

        public bool november_switch = false;
        public bool November_Switch
        {
            get { return november_switch; }
            set
            {
                if (November_Switch == value)
                    return;
                november_switch = value; RaisePropertyChanged();
            }
        }

        public bool december_switch = false;
        public bool December_Switch
        {
            get { return december_switch; }
            set
            {
                if (December_Switch == value)
                    return;
                december_switch = value; RaisePropertyChanged();
            }
        }

        public bool week_visibility = false;
        public bool Week_Visibility
        {
            get { return week_visibility; }
            set
            {
                if (Week_Visibility == value)
                    return;
                week_visibility = value; RaisePropertyChanged();
            }
        }

        public bool month_visibility = false;
        public bool Month_Visibility
        {
            get { return month_visibility; }
            set
            {
                if (Month_Visibility == value)
                    return;
                month_visibility = value; RaisePropertyChanged();
            }
        }

        public bool year_visibility = false;
        public bool Year_Visibility
        {
            get { return year_visibility; }
            set
            {
                if (Year_Visibility == value)
                    return;
                year_visibility = value; RaisePropertyChanged();
            }
        }
    }
}
