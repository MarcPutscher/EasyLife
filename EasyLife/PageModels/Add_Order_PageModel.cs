using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using static Xamarin.Forms.Internals.Profile;
using AsyncCommand = MvvmHelpers.Commands.AsyncCommand;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(TransaktionID), nameof(TransaktionID))]
    [QueryProperty(nameof(OrderID), nameof(OrderID))]
    public class Add_Order_PageModel : FreshBasePageModel
    {
        public Add_Order_PageModel()
        {
            Count_Source = new ObservableRangeCollection<int>(Enumerable.Range(1, 365).ToList());
            Return_Command = new AsyncCommand(Return_Methode);
            ViewIsAppearing_Command = new AsyncCommand(ViewIsAppearing);
            Kind_Source = new ObservableRangeCollection<string>() { "Tag", "Woche", "Monat", "Jahr" };
            Option_Source = new ObservableRangeCollection<string>() { "Einmalig", "Nach einer Anzahl" , "Bis zu einem Datum"};
        }

        public async Task Return_Methode()
        {
            try
            {
                Auftrag auftrag = new Auftrag();

                string amount_text = null;

                int option = 0;

                if (Option_Item == "Einmalig")
                {
                    amount_text = "Einmalig";
                    option = 1;
                }
                if (Option_Item == "Nach einer Anzahl")
                {
                    amount_text = Count_Item.ToString();
                    option = 2;
                }
                if (Option_Item == "Bis zu einem Datum")
                {
                    amount_text = Date.ToString("dddd, d.M.yyyy", new CultureInfo("de-DE"));
                    option = 3;
                }
                if(Option_Item == null)
                {
                    await Return_Zero();
                    return;
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

                    if(istoggledlist.Contains(true) == true)
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

                        auftrag = new Auftrag { Anzahl_an_Wiederholungen = amount_text, Art_an_Wiederholungen = "Benutzerdefiniert:" + Kind_Item + "", Speziell = Days , Option = option};
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

                    if(istoggledlist.Contains(true) == true)
                    {
                        string week = null;

                        if(Ever_Week_Switch == true)
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

                        auftrag = new Auftrag { Anzahl_an_Wiederholungen = amount_text, Art_an_Wiederholungen = "Benutzerdefiniert:" + Kind_Item + "", Speziell = week , Option = option };
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

                        auftrag = new Auftrag { Anzahl_an_Wiederholungen = amount_text, Art_an_Wiederholungen = "Benutzerdefiniert:" + Kind_Item + "", Speziell = Month , Option = option };
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

                        auftrag = new Auftrag { Anzahl_an_Wiederholungen = amount_text, Art_an_Wiederholungen = "Benutzerdefiniert:" + Kind_Item + "", Speziell = year , Option = option };
                    }
                    else
                    {
                        await Return_Zero();
                        return;
                    }
                }

                try
                {
                    if(String.IsNullOrEmpty(OrderID) == true && auftrag != null)
                    {
                        await PassingOrderService.Add_Order(auftrag);

                        OrderID = auftrag.Id.ToString();
                    }
                    if(String.IsNullOrEmpty(OrderID) == false && auftrag != null)
                    {
                        auftrag.Id = int.Parse(OrderID);
                        await PassingOrderService.Edit_Order(auftrag);
                    }
                    await Shell.Current.GoToAsync($"..?TransaktionID={TransaktionID}&OrderID={OrderID}");

                }
                catch(Exception e)
                {
                    await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + e.ToString() + ""));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        public async Task Return_Zero()
        {
            await Shell.Current.GoToAsync($"..?TransaktionID={TransaktionID}&OrderID={OrderID}");
        }
        public async Task ViewIsAppearing()
        {
            try
            {
                if(int.TryParse(TransaktionID, out var result1) == true)
                {
                    Transaktion transaktion = await PassingTransaktionService.Get_specific_Transaktion(result1);
                    
                    Minimum_Date = transaktion.Datum.Date;

                    Date = transaktion.Datum.Date;
                }
                else
                {
                    Minimum_Date = DateTime.Now;

                    Date = DateTime.Now;
                }

                if (int.TryParse(OrderID, out var result2) == true)
                {
                    Auftrag auftrag = await PassingOrderService.Get_specific_Order(result2);

                    if (result2 != 0)
                    {
                        if (auftrag.Option == 1)
                        {
                            Option_Item = "Einmalig";

                            Count_Item = -1;
                        }
                        if (auftrag.Option == 2)
                        {
                            Option_Item = "Nach einer Anzahl";

                            Amount_Option_Visibility = true;

                            Count_Item = int.Parse(auftrag.Anzahl_an_Wiederholungen);
                        }
                        if (auftrag.Option == 3)
                        {
                            Option_Item = "Bis zu einem Datum";

                            Date = DateTime.ParseExact(auftrag.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE"));

                            Count_Item = -1;
                        }
                    }

                    try
                    {
                        auftrag.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen.Remove(0, 18);
                    }
                    catch
                    {
                    }

                    Kind_Item = auftrag.Art_an_Wiederholungen;

                    if (auftrag.Art_an_Wiederholungen == "Tag")
                    {
                        Kind_Item = "Tag";

                        string Days = auftrag.Speziell;

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
                    if(auftrag.Art_an_Wiederholungen == "Woche")
                    {
                        Kind_Item = "Woche";

                        if (String.IsNullOrEmpty(auftrag.Speziell) == false)
                        {
                            if(auftrag.Speziell == "derselbe Tag in jeder Woche")
                            {
                                Ever_Week_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder zweiten Woche")
                            {
                                Ever_Second_Week_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder dritten Woche")
                            {
                                Ever_Third_Week_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder vierten Woche")
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
                    if (auftrag.Art_an_Wiederholungen == "Monat")
                    {
                        Kind_Item = "Monat";

                        string Months = auftrag.Speziell;

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
                    if (auftrag.Art_an_Wiederholungen == "Jahr")
                    {
                        Kind_Item = "Jahr";

                        if (String.IsNullOrEmpty(auftrag.Speziell) == false)
                        {
                            if (auftrag.Speziell == "derselbe Tag in jedem Jahr")
                            {
                                Ever_Year_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem zweitem Jahr")
                            {
                                Ever_Second_Year_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem drittem Jahr")
                            {
                                Ever_Third_Year_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                            {
                                Ever_Fourth_Year_Switch = true;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem fünftem Jahr")
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
                else
                {
                    Count_Item = -1;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }



        public string TransaktionID { get; set; }
        public string OrderID { get; set; }

        public ObservableRangeCollection<int> Count_Source { get; set; }

        public ObservableRangeCollection<string> Kind_Source { get; set; }

        public ObservableRangeCollection<string> Option_Source { get; set; }

        public string order_settings_text;
        public string Order_settings_Text
        {
            get { return order_settings_text; }
            set
            {
                if (Order_settings_Text == value)
                    return;
                order_settings_text = value; RaisePropertyChanged();
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
                if(ever_week_switch == true)
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

        public int count_item;
        public int Count_Item
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
                if( value == "Einmalig")
                {
                    Date_Option_Visibility = false;

                    Amount_Option_Visibility = false;

                    Onc_Option_Visibility = true;
                }
                if( value == "Nach einer Anzahl")
                {
                    Date_Option_Visibility = false;

                    Amount_Option_Visibility = true;

                    Onc_Option_Visibility = false;
                }
                if( value == "Bis zu einem Datum")
                {
                    Date_Option_Visibility = true;

                    Amount_Option_Visibility = false;

                    Onc_Option_Visibility = false;
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

        public AsyncCommand Return_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }
    }
}
