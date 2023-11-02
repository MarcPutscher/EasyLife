using MvvmHelpers;
using System.Threading.Tasks;
using EasyLife.Models;
using Xamarin.Forms;
using Command = MvvmHelpers.Commands.Command;
using System;
using System.Collections.Generic;
using EasyLife.Services;
using MvvmHelpers.Commands;
using FreshMvvm;
using System.Linq;
using EasyLife.Pages;
using System.Globalization;
using Plugin.LocalNotification;
using System.Reactive;
using EasyLife.Helpers;
using Xamarin.CommunityToolkit.Extensions;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(TransaktionID), nameof(TransaktionID))]
    [QueryProperty(nameof(OrderID), nameof(OrderID))]
    public class Add_Item_PageModel : FreshBasePageModel
    {
        public string TransaktionID { get; set; }

        public string orderid;
        public string OrderID
        {
            get
            {
                return orderid;
            }
            set
            {
                orderid = value;
                if(String.IsNullOrEmpty(value) == false)
                {
                    Return_from_Repeat_Customized();
                }
                else
                {
                    Is_Expended = false;

                    Height = 50;

                    Wiederholungs_Text = string.Empty;

                    ActivityIndicator_IsRunning = false;

                    Wiederholungs_Text_Visibility = false;

                    Wiederholungs_Text_Visibility_2 = false;

                    Virtueller_Auftrag = null;

                    Wiederholungs_Header = "Auftrag hinzufügen";
                }
            }
        }

        public AsyncCommand Add_Item { get; }

        public AsyncCommand Repeat_Every_Day_Command { get; }

        public AsyncCommand Repeat_Every_Week_Command { get; }

        public AsyncCommand Repeat_Every_Month_Command { get; }

        public AsyncCommand Repeat_Every_Year_Command { get; }

        public AsyncCommand Repeat_Customised_Command { get; }

        public AsyncCommand Repeat_Never_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public AsyncCommand Reasons_Settings_Command { get; }

        public Transaktion transaktion;
        public Transaktion Transaktion
        {
            get { return transaktion; }
            set
            {
                if (Transaktion == value)
                    return;
                transaktion = value; RaisePropertyChanged();
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
                if (double.TryParse(value, out double result) == true)
                {
                    betrag = value.Replace(".", ",").Trim();
                }
                else
                {
                    if (value == null)
                    {
                        betrag = "";
                    }
                    else
                    {
                        Betrag = betrag;
                    }
                }

                RaisePropertyChanged();
            }
        }

        DateTime datum = DateTime.Now;
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

        Auftrag auftrag;
        public Auftrag Virtueller_Auftrag
        {
            get { return auftrag; }
            set
            {
                if (Virtueller_Auftrag == value)
                    return;
                auftrag = value; RaisePropertyChanged();
            }
        }

        string wiederholungs_text;
        public string Wiederholungs_Text
        {
            get { return wiederholungs_text; }
            set
            {
                if (Wiederholungs_Text == value)
                    return;
                wiederholungs_text = value; RaisePropertyChanged();
            }
        }

        string wiederholungs_header = "Auftrag hinzufügen";
        public string Wiederholungs_Header
        {
            get { return wiederholungs_header; }
            set
            {
                if (Wiederholungs_Header == value)
                    return;
                wiederholungs_header = value; RaisePropertyChanged();
            }
        }

        bool is_expended = false;
        public bool Is_Expended
        {
            get { return is_expended; }
            set
            {
                if (Is_Expended == value)
                    return;
                is_expended = value; RaisePropertyChanged();
            }
        }

        TextAlignment horizontaltextalignment = TextAlignment.End;
        public TextAlignment HorizontalTextAlignment
        {
            get { return horizontaltextalignment; }
            set
            {
                if (HorizontalTextAlignment == value)
                    return;
                horizontaltextalignment = value; RaisePropertyChanged();
            }
        }

        int height = 50;
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

        public bool wiederholungs_text_visibility;
        public bool Wiederholungs_Text_Visibility
        {
            get { return wiederholungs_text_visibility; }
            set
            {
                if (Wiederholungs_Text_Visibility == value)
                    return;
                wiederholungs_text_visibility = value; RaisePropertyChanged();
                Wiederholungs_Text_Visibility_2 = !value;
            }
        }

        public bool wiederholungs_text_visibility_2;
        public bool Wiederholungs_Text_Visibility_2
        {
            get { return wiederholungs_text_visibility_2; }
            set
            {
                if (Wiederholungs_Text_Visibility_2 == value)
                    return;
                wiederholungs_text_visibility_2 = value; RaisePropertyChanged();
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

        public int placeholder_Auftrag = 0;

        public int placeholder_Transaktion = 0;

        public static Dictionary<string, string> Entscheider_ob_Einnahme_oder_Ausgabe = new Dictionary<string, string>();

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

        public bool show_hide_balance = true;
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

        public bool show_hide_saldo = true;
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



        public Add_Item_PageModel()
        {
            Add_Item = new AsyncCommand(Add);

            Repeat_Never_Command = new AsyncCommand(Repeat_Never_MethodeAsync);
            Repeat_Every_Day_Command = new AsyncCommand(Repeat_Every_Day_Methode);
            Repeat_Every_Week_Command = new AsyncCommand(Repeat_Every_Week_Methode);
            Repeat_Every_Month_Command = new AsyncCommand(Repeat_Every_Month_Methode);
            Repeat_Every_Year_Command = new AsyncCommand(Repeat_Every_Year_Methode);
            Repeat_Customised_Command = new AsyncCommand(Repeat_Customised_Methode);
            Reasons_Settings_Command = new AsyncCommand(Reasons_Settings_Methode);

            ViewIsAppearing_Command = new AsyncCommand(Get_Reasons_Liste);

            Zweck_Liste = new ObservableRangeCollection<string>();

            Transaktion = new Transaktion();
        }

        private async Task Reasons_Settings_Methode()
        {
            try
            {
                var result = await Shell.Current.DisplayActionSheet("Einstellungen für Zwecke" , "Zurück" , null , new string[] {"Zweck hinzufügen" , "Zweck entfernen" , "Zweck wiederherstellen"});

                if(result == "Zweck hinzufügen")
                {
                    await Add_Reason_Methode();
                }
                if(result == "Zweck entfernen")
                {
                    await Remove_Reason_Methode();
                }
                if(result == "Zweck wiederherstellen")
                {
                    await Revive_Reason_Methode();
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Add_Reason_Methode()
        {
            try
            {
                var result = await Shell.Current.DisplayPromptAsync("Zweck erstellen", null, "Hinzufügen", "Verwerfen", "Zweck hier eingeben", 30);

                if (String.IsNullOrWhiteSpace(result) == false)
                {
                    if (result == "Verwerfen")
                    {
                        await Notificater("Der Zweck wurde verworfen.");

                        return;
                    }

                    var result2 = await Shell.Current.DisplayActionSheet("Zweck zuordnen", "Verwerfen", null, new string[] { "Einnahmen", "Ausgaben" });

                    if (String.IsNullOrEmpty(result2) == false)
                    {
                        if (result2 == "Verwerfen")
                        {
                            await Notificater("Der Zweck " + result.Trim() + " wurde verworfen.");

                            return;
                        }

                        var result3 = await ReasonService.Add_Reason(result, result2);

                        if (result3 == true)
                        {
                            await Get_Reasons_Liste();

                            await Notificater("Der Zweck " + result.Trim() + " wurde hinzugefügt.");

                            return;
                        }
                        else
                        {
                            await Notificater("Der Zweck " + result.Trim() + " wurde verworfen.");

                            return;
                        }
                    }
                }

                await Notificater("Der Zweck wurde verworfen.");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Remove_Reason_Methode()
        {
            try
            {
                Dictionary<string, string> zwecke = (Dictionary<string, string>)await ReasonService.Get_Enable_ReasonDictionary();

                List<string> zwecke2 = new List<string>();

                string[] zweck_string = new string[] { };

                foreach (var item in zwecke)
                {
                    zwecke2.Add(item.Key + ":" + item.Value);
                }

                zweck_string = zwecke2.ToArray();

                if (zwecke2.Count != 0)
                {
                    var result = await Shell.Current.DisplayActionSheet("Angezeigte Zwecke", "Zurück", null, zweck_string);

                    if (String.IsNullOrWhiteSpace(result) == false)
                    {
                        if (result == "Zurück")
                        {
                            return;
                        }
                        Zweck item = await ReasonService.Get_specific_Reason(result);

                        bool in_use = false;

                        var transaktion_list = await ContentService.Get_all_Transaktion();

                        foreach (var trans in transaktion_list)
                        {
                            if(trans.Zweck == item.zweck.Substring(0,item.zweck.IndexOf(":")))
                            {
                                in_use = true;
                            }
                        }

                        if (in_use == false)
                        {
                            item.Reason_Visibility = false;

                            await ReasonService.Edit_Reason(item);

                            await Get_Reasons_Liste();

                            await Notificater("Der Zweck wurde erfolgreich entfernt.");
                        }
                        else
                        {
                            await Get_Reasons_Liste();

                            await Notificater("Der Zweck kann nicht entfernt werden, da er noch benutzt wird.");
                        }
                    }
                }
                else
                {
                    await Notificater("Es gibt keine Zwecke die angezeigt werden können.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Revive_Reason_Methode()
        {
            try
            {
                Dictionary<string, string> zwecke = (Dictionary<string, string>)await ReasonService.Get_Disable_ReasonDictionary();

                List<string> zwecke2 = new List<string>();

                string[] zweck_string = new string[] { };

                foreach (var item in zwecke)
                {
                    zwecke2.Add(item.Key + ":" + item.Value);
                }

                zweck_string = zwecke2.ToArray();

                if (zwecke2.Count != 0)
                {
                    var result = await Shell.Current.DisplayActionSheet("Entfernte Zwecke", "Zurück", null, zweck_string);

                    if (String.IsNullOrEmpty(result) == false)
                    {
                        if (result == "Zurück")
                        {
                            return;
                        }

                        Zweck item = await ReasonService.Get_specific_Reason(result);

                        item.Reason_Visibility = true;

                        await ReasonService.Edit_Reason(item);

                        await Get_Reasons_Liste();

                        await Notificater("Der Zweck wurde erfolgreich wiederhergestellt.");
                    }
                }
                else
                {
                    await Notificater("Es gibt keine Zwecke die wiederhergestellt werden können.");
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task Get_Reasons_Liste()
        {
            try
            {
                string pseudozweck = null;

                Zweck_IsEnable = false;

                if (String.IsNullOrEmpty(Zweck) == false)
                {
                    if (Zweck_Liste.Contains(Zweck) == true)
                    {
                        pseudozweck = Zweck;
                    }
                }

                Entscheider_ob_Einnahme_oder_Ausgabe = (Dictionary<string, string>)await ReasonService.Get_Enable_ReasonDictionary();

                Zweck_Liste.Clear();

                Zweck_Liste.AddRange(Entscheider_ob_Einnahme_oder_Ausgabe.Keys.ToArray());

                if(Transaktion != null)
                {
                    if (Transaktion.Zweck != null)
                    {
                        Zweck = Transaktion.Zweck;
                    }
                }

                Zweck_IsEnable = true;

                if (Zweck_Liste.Contains(pseudozweck) == true)
                {
                    Zweck = pseudozweck;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Zweck_IsEnable = true;

                Fehler();
            }
        }

        async Task Add()
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
                    if(0>result || result>9999999)
                    {
                        Betrag = null;

                        await Notificater("Der Betrag ist nicht zwischen 0€ und 9999999€");

                        return;
                    }


                    Auftrag order = new Auftrag();

                    if (Entscheider_ob_Einnahme_oder_Ausgabe[Zweck] == "Einnahmen")
                    {
                        result = Math.Abs(result);
                    }
                    else
                    {
                        result = -Math.Abs(result);
                    }

                    if(result == 0)
                    {
                        await Notificater("Es wurde kein Betrag eingegeben.");
                        return;
                    }

                    if (Virtueller_Auftrag == null)
                    {
                        Transaktion transaktion = new Transaktion() { Betrag = result.ToString(), Datum = Datum.Date, Zweck = Zweck, Notiz = Notiz, Auftrags_id = null, Anzahl_an_Wiederholungen = null, Art_an_Wiederholungen = null, Speziell = null, Order_Visibility = false, Content_Visibility = true , Balance_Visibility = Show_Hide_Balance , Saldo_Visibility = Show_Hide_Saldo};

                        await ContentService.Add_Transaktion(transaktion);

                        placeholder_Transaktion = ContentService.Get_last_Transaktion().Id;
                    }
                    else
                    {
                        ActivityIndicator_IsRunning = true;

                        Virtueller_Auftrag.Id = 0;

                        Transaktion transaktion = new Transaktion() { Betrag = result.ToString(), Datum = Datum.Date, Zweck = Zweck, Notiz = Notiz, Auftrags_id = Virtueller_Auftrag.Id.ToString() , Anzahl_an_Wiederholungen = Virtueller_Auftrag.Anzahl_an_Wiederholungen , Art_an_Wiederholungen = Virtueller_Auftrag.Art_an_Wiederholungen, Speziell = Virtueller_Auftrag.Speziell, Order_Visibility = true, Content_Visibility = true , Balance_Visibility = Show_Hide_Balance , Saldo_Visibility = Show_Hide_Saldo};

                        await OrderService.Add_Order(Virtueller_Auftrag);

                        bool result1 = await AddRange(transaktion, Virtueller_Auftrag);

                        if(result1 == false)
                        {
                            ActivityIndicator_IsRunning = false;

                            await OrderService.Remove_Order(Virtueller_Auftrag);

                            Virtueller_Auftrag = await PassingOrderService.Get_specific_Order(int.Parse(OrderID));

                            await Notificater("Es gab ein Logikfehler.");

                            return;
                        }

                        await OrderService.Edit_Order(Virtueller_Auftrag);

                        ActivityIndicator_IsRunning = false;

                        Transaktion trans = await ContentService.Get_last_Transaktion();

                        if(false == await NotificationHelper.RequestNotification(Virtueller_Auftrag,trans))
                        {
                            await Notificater("Es konnte keine Benachrichtigung erstellt werden");
                        }
                    }

                    Virtueller_Auftrag = null;

                    Transaktion = null;

                    Betrag = null;

                    Datum = DateTime.Now;

                    Zweck = null;

                    Notiz = null;

                    Wiederholungs_Text = null;

                    Show_Hide_Balance = true;

                    Show_Hide_Saldo = true;

                    OrderID = null;

                    TransaktionID = null;

                    HorizontalTextAlignment = TextAlignment.End;

                    Height = 50;

                    TransaktionID = null;

                    await PassingOrderService.Remove_All_Order();
                    await PassingTransaktionService.Remove_All_Transaktion();

                    await Notificater("Erfolgreich hinzugefügt");
                }
                else
                {
                    Betrag = null;

                    await Notificater("Der Betrag ist fehlerhaft");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                if (placeholder_Auftrag == 0)
                {
                    try
                    {
                        await ContentService.Remove_Transaktion(ContentService.Get_specific_Transaktion(placeholder_Transaktion).Result);
                    }
                    catch { }
                }

                else
                {
                    try
                    {
                        var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                        foreach (var trans in transaktionscontent)
                        {
                            if (trans.Auftrags_id != null)
                            {
                                if (Math.Truncate(decimal.Parse(trans.Auftrags_id)) == placeholder_Auftrag)
                                {
                                    await ContentService.Remove_Transaktion(trans);
                                }
                            }
                        }
                    }
                    catch { }
                }

                Fehler();
            }
        }

        async Task<bool> AddRange(Transaktion transaktion, Auftrag auftrag)
        {
            try
            {
                int count = 0;

                DateTime current_day = transaktion.Datum;

                DateTime enddate = DateTime.Today.AddDays(+1).AddMilliseconds(-1) ;

                if (auftrag.Option == 1)
                {
                    transaktion.Auftrags_Option = 1;
                }
                if (auftrag.Option == 2)
                {
                    count = int.Parse(auftrag.Anzahl_an_Wiederholungen);

                    transaktion.Auftrags_Option = 2;
                }
                if (auftrag.Option == 3)
                {
                    enddate = DateTime.ParseExact(auftrag.Anzahl_an_Wiederholungen, "dddd, d.M.yyyy", new CultureInfo("de-DE")).AddHours(24).AddMilliseconds(-1);

                    transaktion.Auftrags_Option = 3;
                }

                List<string> wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count+1).ToList();

                List<string> Benutzerdefiniert = new List<string>() { "Tag", "Woche", "Monat", "Jahr" };

                int K = 1;

                if (Benutzerdefiniert.Contains(auftrag.Art_an_Wiederholungen))
                {
                    if (auftrag.Art_an_Wiederholungen == "Tag")
                    {
                        auftrag.Art_an_Wiederholungen = "Jeden Tag";
                    }

                    if (auftrag.Art_an_Wiederholungen == "Woche")
                    {
                        auftrag.Art_an_Wiederholungen = "Jede Woche";
                    }

                    if (auftrag.Art_an_Wiederholungen == "Monat")
                    {
                        auftrag.Art_an_Wiederholungen = "Jeden Monat";
                    }

                    if (auftrag.Art_an_Wiederholungen == "Jahr")
                    {
                        auftrag.Art_an_Wiederholungen = "Jedes Jahr";
                    }
                }

                if (auftrag.Art_an_Wiederholungen == "Jeden Tag")
                {
                    if(auftrag.Option == 3)
                    {
                        count = (enddate - current_day).Days;

                        wiedrholungen.Clear();

                        wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count + 1).ToList();
                    }

                    foreach (string st in wiedrholungen)
                    {
                        string auftrags_id = st + "." + K + "";

                        transaktion.Auftrags_id = auftrags_id;

                        transaktion.Datum = current_day;

                        await ContentService.Add_Transaktion(transaktion);

                        current_day = transaktion.Datum.AddDays(1);

                        K++;
                    }
                    return true;
                }

                if (auftrag.Art_an_Wiederholungen == "Jede Woche")
                {
                    if (auftrag.Option == 3)
                    {
                        while(current_day<=enddate)
                        {
                            current_day = current_day.AddDays(7);
                            count++;
                        }

                        current_day = transaktion.Datum;

                        wiedrholungen.Clear();

                        wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count).ToList();
                    }

                    foreach (string st in wiedrholungen)
                    {
                        string auftrags_id = st + "." + K + "";

                        transaktion.Auftrags_id = auftrags_id;

                        transaktion.Datum = current_day;

                        await ContentService.Add_Transaktion(transaktion);

                        current_day = transaktion.Datum.AddDays(7);

                        K++;
                    }
                    return true;
                }

                if (auftrag.Art_an_Wiederholungen == "Jeden Monat")
                {
                    if (auftrag.Option == 3)
                    {
                        while (current_day <= enddate)
                        {
                            current_day = current_day.AddMonths(1);
                            count++;
                        }

                        current_day = transaktion.Datum;

                        wiedrholungen.Clear();

                        wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count).ToList();
                    }

                    foreach (string st in wiedrholungen)
                    {
                        string auftrags_id = st + "." + K + "";

                        transaktion.Auftrags_id = auftrags_id;

                        transaktion.Datum = current_day;

                        await ContentService.Add_Transaktion(transaktion);

                        current_day = transaktion.Datum.AddMonths(1);

                        K++;
                    }

                    return true;
                }

                if (auftrag.Art_an_Wiederholungen == "Jedes Jahr")
                {
                    if (auftrag.Option == 3)
                    {
                        while (current_day <= enddate)
                        {
                            current_day = current_day.AddYears(1);
                            count++;
                        }

                        current_day = transaktion.Datum;

                        wiedrholungen.Clear();

                        wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count).ToList();
                    }

                    foreach (string st in wiedrholungen)
                    {
                        string auftrags_id = st + "." + K + "";

                        transaktion.Auftrags_id = auftrags_id;

                        transaktion.Datum = current_day;

                        await ContentService.Add_Transaktion(transaktion);

                        current_day = transaktion.Datum.AddYears(1);

                        K++;
                    }

                    return true;
                }

                else
                {
                    string s = auftrag.Art_an_Wiederholungen.Remove(18);
                    if (s == "Benutzerdefiniert:")
                    {
                        auftrag.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen.Remove(0, 18);

                        if (auftrag.Art_an_Wiederholungen == "Tag")
                        {
                            auftrag.Art_an_Wiederholungen = "Spezielle Tage";

                            transaktion.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen;

                            string Days = auftrag.Speziell;

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

                            int k = 0;

                            int w = 0;

                            wiedrholungen.Clear();

                            wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), (count+1) * Days_List.Count).ToList();

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

                            if (auftrag.Option == 3)
                            {
                                List<DateTime> dates = new List<DateTime>();

                                bool indicator = false;

                                count = 0;

                                int follower = 0;

                                int follower2 = 0;

                                int follower3 = 0;

                                while (indicator == false)
                                {
                                    dates.Add(sorted_date_List[follower2].AddDays(7*follower3));

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
                                    return false;
                                }

                                int follower4 = 1;

                                foreach (DateTime time in dates)
                                {
                                    string auftrags_id = auftrag.Id + "." + follower4 + "";

                                    transaktion.Auftrags_id = auftrags_id;

                                    transaktion.Datum = time;

                                    await ContentService.Add_Transaktion(transaktion);

                                    follower4++;
                                }
                            }
                            else
                            {
                                foreach (string st in wiedrholungen)
                                {
                                    string auftrags_id = st + "." + K + "";

                                    transaktion.Auftrags_id = auftrags_id;

                                    transaktion.Datum = sorted_date_List[k].AddDays(7 * w);

                                    await ContentService.Add_Transaktion(transaktion);

                                    if (k == sorted_date_List.Count() - 1)
                                    {
                                        k = -1;

                                        w++;
                                    }

                                    k++;

                                    K++;
                                }
                            }
                        }

                        if (auftrag.Art_an_Wiederholungen == "Woche")
                        {
                            auftrag.Art_an_Wiederholungen = "Spezielle Wochen";

                            transaktion.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen;

                            DateTime current_week = transaktion.Datum;

                            int h = 0;

                            if (auftrag.Speziell == "derselbe Tag in jeder Woche")
                            {
                                h = 1;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder zweiten Woche")
                            {
                                h = 2;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder dritten Woche")
                            {
                                h = 3; 
                            }
                            if (auftrag.Speziell == "derselbe Tag in jeder vierten Woche")
                            {
                                h = 4;
                            }

                            if (auftrag.Option == 3)
                            {
                                while (current_day < enddate)
                                {
                                    current_day = current_day.AddDays(7*h);
                                    count++;
                                }

                                if (count == 0)
                                {
                                    return false;
                                }

                                current_day = transaktion.Datum;

                                wiedrholungen.Clear();

                                wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count+1).ToList();
                            }

                            foreach (string st in wiedrholungen)
                            {
                                string auftrags_id = st + "." + K + "";

                                transaktion.Auftrags_id = auftrags_id;

                                transaktion.Datum = current_week;

                                await ContentService.Add_Transaktion(transaktion);

                                current_week = transaktion.Datum.AddDays(7*h);

                                K++;
                            }
                        }

                        if (auftrag.Art_an_Wiederholungen == "Monat")
                        {
                            auftrag.Art_an_Wiederholungen = "Spezielle Monate";

                            transaktion.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen;

                            string Months = auftrag.Speziell;

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

                            int k = 0;

                            int w = 0;

                            wiedrholungen.Clear();

                            wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), (count+1) * Month_List.Count).ToList();

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

                            if (auftrag.Option == 3)
                            {
                                List<DateTime> dates = new List<DateTime>();

                                bool indicator = false;

                                count = 0;

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
                                    return false;
                                }

                                int follower4 = 1;

                                foreach (DateTime time in dates)
                                {
                                    string auftrags_id = auftrag.Id + "." + follower4 + "";

                                    transaktion.Auftrags_id = auftrags_id;

                                    transaktion.Datum = time;

                                    await ContentService.Add_Transaktion(transaktion);

                                    follower4++;
                                }
                            }
                            else
                            {
                                foreach (string st in wiedrholungen)
                                {
                                    string auftrags_id = st + "." + K + "";

                                    transaktion.Auftrags_id = auftrags_id;

                                    transaktion.Datum = sorted_Month_List[k].AddMonths(12 * w);

                                    await ContentService.Add_Transaktion(transaktion);

                                    if (k == sorted_Month_List.Count() - 1)
                                    {
                                        k = -1;

                                        w++;
                                    }

                                    k++;

                                    K++;
                                }
                            }
                        }

                        if (auftrag.Art_an_Wiederholungen == "Jahr")
                        {
                            auftrag.Art_an_Wiederholungen = "Spezielle Jahre";

                            transaktion.Art_an_Wiederholungen = auftrag.Art_an_Wiederholungen;

                            DateTime current_year = transaktion.Datum;

                            int h = 0;

                            if (auftrag.Speziell == "derselbe Tag in jedem Jahr")
                            {
                                h = 1;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem zweitem Jahr")
                            {
                                h = 2;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem drittem Jahr")
                            {
                                h = 3;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                            {
                                h = 4;
                            }
                            if (auftrag.Speziell == "derselbe Tag in jedem viertem Jahr")
                            {
                                h = 5;
                            }

                            if (auftrag.Option == 3)
                            {
                                while (current_day < enddate)
                                {
                                    current_day = current_day.AddYears(1 * h);
                                    count++;
                                }

                                if (count == 0)
                                {
                                    return false;
                                }

                                current_day = transaktion.Datum;

                                wiedrholungen.Clear();

                                wiedrholungen = Enumerable.Repeat(auftrag.Id.ToString(), count+1).ToList();
                            }

                            foreach (string st in wiedrholungen)
                            {
                                string auftrags_id = st + "." + K + "";

                                transaktion.Auftrags_id = auftrags_id;

                                transaktion.Datum = current_year;

                                await ContentService.Add_Transaktion(transaktion);

                                current_year = transaktion.Datum.AddYears(1*h);

                                K++;
                            }
                        }
                    }
                    else
                    {
                        auftrag.Art_an_Wiederholungen = "Fehler";
                        auftrag.Anzahl_an_Wiederholungen = null;
                        auftrag.Speziell = "Fehler";
                    }
                }

                await PassingOrderService.Remove_All_Order();

                await PassingTransaktionService.Remove_All_Transaktion();

                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();

                return false;
            }
        }

        async Task Repeat_Customised_Methode()
        {
            try
            {
                if (double.TryParse(Betrag, out double result) == false)
                {
                    Betrag = null;
                }

                Transaktion = new Transaktion() { Betrag = Betrag, Datum = Datum, Zweck = Zweck, Notiz = Notiz , Balance_Visibility = Show_Hide_Balance , Saldo_Visibility = Show_Hide_Saldo};

                if (Transaktion.Zweck == null && Transaktion.Notiz == null && Transaktion.Betrag == null)
                {
                    TransaktionID = null;
                }
                else
                {
                    await PassingTransaktionService.Add_Transaktion(Transaktion);

                    TransaktionID = Transaktion.Id.ToString();
                }

                ViewIsDisappearing();

                await Shell.Current.GoToAsync($"{nameof(Add_Order_Page)}?TransaktionID={TransaktionID}&OrderID={OrderID}");

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        private async Task Repeat_Every_Year_Methode()
        {
            try
            {
                await Notificater("Die Transaktion wird jedes Jahr an demselben Tag erstellt.\nDauer: 5 Jahre");

                Virtueller_Auftrag = new Auftrag { Anzahl_an_Wiederholungen = "4", Art_an_Wiederholungen = "Jedes Jahr", Speziell = "derselbe Tag im Jahr", Option = 2 };

                Wiederholungs_Text = "Jedes Jahr";

                Wiederholungs_Text_Visibility = true;

                HorizontalTextAlignment = TextAlignment.End;

                Wiederholungs_Header = "Wiederholung :";

                Height = 50;

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        private async Task Repeat_Every_Month_Methode()
        {
            try
            {
                await Notificater("Die Transaktion wird jeden Monat an demselben Tag erstellt.\nDauer: 1 Jahr");

                Virtueller_Auftrag = new Auftrag { Anzahl_an_Wiederholungen = "11", Art_an_Wiederholungen = "Jeden Monat", Speziell = "derselbe Tag im Monat", Option = 2 };

                Wiederholungs_Text = "Jeden Monat";

                Wiederholungs_Text_Visibility = true;

                Wiederholungs_Header = "Wiederholung :";

                HorizontalTextAlignment = TextAlignment.End;

                Height = 50;

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        private async Task Repeat_Every_Week_Methode()
        {
            try
            {
                await Notificater("Die Transaktion wird jede Woche an demselben Tag erstellt.\nDauer: 1 Jahr");

                Virtueller_Auftrag = new Auftrag { Anzahl_an_Wiederholungen = "51", Art_an_Wiederholungen = "Jede Woche", Speziell = "derselbe Tag in der Woche", Option = 2 };

                Wiederholungs_Text = "Jede Woche";

                Wiederholungs_Text_Visibility = true;

                Wiederholungs_Header = "Wiederholung :";

                HorizontalTextAlignment = TextAlignment.End;

                Height = 50;

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        public async Task Repeat_Every_Day_Methode()
        {
            try
            {
                await Notificater("Die Transaktion wird jeden Tag erstellt.\nDauer: 1 Jahr");

                Virtueller_Auftrag = new Auftrag { Anzahl_an_Wiederholungen = "364", Art_an_Wiederholungen = "Jeden Tag", Speziell = "jeder Tag in der Woche", Option = 2 };

                Wiederholungs_Text = "Jeden Tag";

                Wiederholungs_Text_Visibility = true;

                Wiederholungs_Header = "Wiederholung :";

                HorizontalTextAlignment = TextAlignment.End;

                Height = 50;

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        public async Task Repeat_Never_MethodeAsync()
        {
            try
            {
                Virtueller_Auftrag = null;

                Wiederholungs_Text = null;

                Wiederholungs_Text_Visibility = false;

                Wiederholungs_Header = "Auftrag hinzufügen";

                HorizontalTextAlignment = TextAlignment.End;

                Height = 50;

                Is_Expended = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        public void Fehler()
        {
            Is_Expended = false;

            Zweck = null;

            Betrag = string.Empty;

            Datum = DateTime.Now;

            Height = 50;

            Wiederholungs_Text = string.Empty;

            ActivityIndicator_IsRunning = false;

            Wiederholungs_Text_Visibility = false;

            Wiederholungs_Text_Visibility_2 = false;

            Show_Hide_Balance = true;

            Show_Hide_Saldo = true;

            Virtueller_Auftrag = null;
        }

        private async Task Notificater(string v)
        {
            await Shell.Current.DisplayToastAsync(v, 5000);
        }

        public async void Return_from_Repeat_Customized()
        {
            try
            {
                if (int.TryParse(TransaktionID, out var result1) == true)
                {
                    Transaktion = await PassingTransaktionService.Get_specific_Transaktion(result1);
                    Transaktion.Id = 0;
                }

                if (int.TryParse(OrderID, out var result2) == true)
                {
                    Virtueller_Auftrag = await PassingOrderService.Get_specific_Order(result2);
                    Virtueller_Auftrag.Id = 0;
                }
                else
                {
                    Virtueller_Auftrag = null;
                }

                if (string.IsNullOrEmpty(Zweck) == true && string.IsNullOrEmpty(Transaktion.Zweck) == false)
                {
                    Zweck = Transaktion.Zweck;
                }

                if (string.IsNullOrEmpty(Betrag) == true && string.IsNullOrEmpty(Transaktion.Betrag) == false)
                {
                    Betrag = Transaktion.Betrag;

                    Datum = Transaktion.Datum;

                    Notiz = Transaktion.Notiz;
                }

                if (Virtueller_Auftrag != null)
                {
                    HorizontalTextAlignment = TextAlignment.Start;

                    Height = 110;

                    Wiederholungs_Text_Visibility = false;

                    Wiederholungs_Text_Visibility_2 = true;

                    string placeholder = null;

                    try
                    {
                        string s = Virtueller_Auftrag.Art_an_Wiederholungen.Remove(0, 18);


                        if (s == "Tag")
                        {
                            placeholder = "Spezielle Tage";
                        }
                        if (s == "Woche")
                        {
                            placeholder = "Spezielle Woche";
                        }
                        if (s == "Monat")
                        {
                            placeholder = "Spezielle Monate";
                        }
                        if (s == "Jahr")
                        {
                            placeholder = "Spezielle Jahre";
                        }

                        Virtueller_Auftrag.Anzahl_an_Wiederholungen = Virtueller_Auftrag.Anzahl_an_Wiederholungen;
                    }
                    catch
                    {
                        placeholder = Virtueller_Auftrag.Art_an_Wiederholungen;
                    }

                    if (Virtueller_Auftrag.Option == 1)
                    {
                        Wiederholungs_Text = "Anzahl = " + Virtueller_Auftrag.Anzahl_an_Wiederholungen + "\nArt der Wiederholung = " + placeholder +
                                             "\nSpeziell = " + Virtueller_Auftrag.Speziell + "";
                    }
                    if (Virtueller_Auftrag.Option == 2)
                    {
                        Wiederholungs_Text = "Anzahl an Wiederholungen = " + Virtueller_Auftrag.Anzahl_an_Wiederholungen + "\nArt der Wiederholung = " + placeholder +
                                             "\nSpeziell = " + Virtueller_Auftrag.Speziell + "";
                    }
                    if (Virtueller_Auftrag.Option == 3)
                    {
                        Wiederholungs_Text = "Enddatum = " + Virtueller_Auftrag.Anzahl_an_Wiederholungen + "\nArt der Wiederholung = " + placeholder +
                                             "\nSpeziell = " + Virtueller_Auftrag.Speziell + "";
                    }
                    if (Virtueller_Auftrag.Option == 0)
                    {
                        Wiederholungs_Text = "Anzahl an Wiederholungen = \nArt der Wiederholung = " + placeholder +
                                             "\nSpeziell = " + Virtueller_Auftrag.Speziell + "";
                    }

                    if(placeholder == null && Virtueller_Auftrag.Speziell == null && Virtueller_Auftrag.Anzahl_an_Wiederholungen == null)
                    {
                        Is_Expended = false;

                        Height = 50;

                        Wiederholungs_Text = string.Empty;

                        ActivityIndicator_IsRunning = false;

                        Wiederholungs_Text_Visibility = false;

                        Wiederholungs_Text_Visibility_2 = false;

                        Virtueller_Auftrag = null;

                        Wiederholungs_Header = "Auftrag hinzufügen";
                    }

                    Wiederholungs_Header = "Wiederholung :";
                }
                else
                {
                    Is_Expended = false;

                    Height = 50;

                    Wiederholungs_Text = string.Empty;

                    ActivityIndicator_IsRunning = false;

                    Wiederholungs_Text_Visibility = false;

                    Wiederholungs_Text_Visibility_2 = false;

                    Virtueller_Auftrag = null;

                    Wiederholungs_Header = "Auftrag hinzufügen";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }

        public async void ViewIsDisappearing()
        {
            try
            {
                if (String.IsNullOrEmpty(TransaktionID) == true)
                {
                    Is_Expended = false;

                    Zweck = null;

                    Betrag = null;

                    Notiz = null;

                    Datum = DateTime.Now;

                    Height = 50;

                    Wiederholungs_Text = null;

                    ActivityIndicator_IsRunning = false;

                    Wiederholungs_Text_Visibility = false;

                    Wiederholungs_Text_Visibility_2 = false;

                    Virtueller_Auftrag = null;

                    Show_Hide_Saldo = true;

                    Show_Hide_Balance = true;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Fehler();
            }
        }
    }
}