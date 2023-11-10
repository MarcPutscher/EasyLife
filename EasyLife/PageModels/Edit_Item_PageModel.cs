using EasyLife.Models;
using EasyLife.Services;
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
using Xamarin.CommunityToolkit.Extensions;
using EasyLife.Pages;

namespace EasyLife.PageModels
{
    [QueryProperty(nameof(TransaktionID), nameof(TransaktionID))]
    class Edit_Item_PageModel : FreshBasePageModel
    {
        public AsyncCommand Edit_Item_Command { get; }

        public AsyncCommand ViewIsAppearing_Command { get; }

        public string TransaktionID { get; set; }

        public Transaktion Transaktion { get; set; }


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

        Transaktion placeholder = new Transaktion();

        public Edit_Item_PageModel()
        {
            Edit_Item_Command = new AsyncCommand(Edit);
            Zweck_Liste = new ObservableRangeCollection<string>();
            ViewIsAppearing_Command = new AsyncCommand(ViewIsAppearing);
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

                StringValidator myStrValidator = new StringValidator(1, 8);

                try
                {
                    myStrValidator.Validate(Betrag);
                }
                catch
                {
                    Zweck = placeholder.Zweck;
                    Betrag = placeholder.Betrag;
                    Datum = placeholder.Datum;
                    Notiz = placeholder.Notiz;

                    await Notificater("Die Eingaben sind fehlerhaft.");
                }

                if (double.TryParse(Betrag, NumberStyles.Any, new CultureInfo("de-DE"), out double result) == true)
                {
                    if (Entscheider_ob_Einnahme_oder_Ausgabe[Zweck] == "Einnahmen")
                    {
                        result = Math.Abs(result);
                    }
                    else
                    {
                        result = -Math.Abs(result);
                    }

                    if (result == 0)
                    {
                        await Notificater("Es wurde kein Betrag eingegeben.");
                        return;
                    }

                    Betrag = result.ToString();

                    Transaktion.Betrag = Betrag;
                    Transaktion.Zweck = Zweck;
                    Transaktion.Datum = Datum;
                    Transaktion.Notiz = Notiz;
                    Transaktion.Balance_Visibility = Show_Hide_Balance;
                    Transaktion.Saldo_Visibility = Show_Hide_Saldo;

                    await ContentService.Edit_Transaktion(Transaktion);

                    await Notificater("Erfolgreich bearbeitet");

                    Return();
                }
                else
                {
                    Zweck = placeholder.Zweck;
                    Betrag = placeholder.Betrag;
                    Datum = placeholder.Datum;
                    Notiz = placeholder.Notiz;

                    await Notificater("Die Eingaben sind fehlerhaft.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                await ContentService.Edit_Transaktion(placeholder);

                Transaktion = placeholder;
                Zweck = placeholder.Zweck;
                Betrag = placeholder.Betrag;
                Datum = placeholder.Datum;
                Notiz = placeholder.Notiz;
            }
        }

        public void Return()
        {
            Shell.Current.GoToAsync("..");
        }

        public async Task ViewIsAppearing()
        {
            try
            {
                await Get_Zweck_Liste();

                if (int.TryParse(TransaktionID, out var result) == true)
                {
                    Transaktion = await ContentService.Get_specific_Transaktion(result);
                    Betrag = Math.Abs(double.Parse(Transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE"))).ToString();
                    Datum = Transaktion.Datum;
                    Notiz = Transaktion.Notiz;
                    Zweck = Transaktion.Zweck;
                    Show_Hide_Balance = Transaktion.Balance_Visibility;
                    Show_Hide_Saldo = Transaktion.Saldo_Visibility;

                    placeholder = Transaktion;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Notificater(string v)
        {
            await Shell.Current.DisplayToastAsync(v, 5000);
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
    }
}
