using FreshMvvm;
using Command = MvvmHelpers.Commands.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using System;
using System.Runtime.CompilerServices;
using EasyLife.Pages;
using Xamarin.CommunityToolkit.Extensions;
using EasyLife.Services;
using System.Linq;
using EasyLife.Models;
using System.Reactive.Linq;
using EasyLife.Helpers;
using System.Collections.ObjectModel;
using MvvmHelpers;
using Org.BouncyCastle.Tls;
using ColorPicker.Interfaces;
using ColorPicker.BaseClasses.ColorPickerEventArgs;
using EasyLife.Cells;
using EasyLife.Cells.Styling;
using ColorMine.ColorSpaces;
using static SQLite.SQLite3;
using EasyLife.CustomeEventArgs;

namespace EasyLife.PageModels
{
    public class Styling_Color_PageModel : FreshBasePageModel
    {
        public Styling_Color_PageModel()
        {
            Save_Command = new AsyncCommand(Save_Methode);

            Load_Command = new AsyncCommand(Load_Methode);

            Default_Command = new AsyncCommand(Default_Methode);

            View_IsAppering_Command = new AsyncCommand(View_IsAppering_MethodeAsync);

            Choose_Command = new AsyncCommand<List<object>>(Choose_Methode);

            Colorsname.AddRange(HomeView.Home_View_Item_List);

            Colorsname.AddRange(HinzufügenView.Hinzufügen_View_Item_List);

            Colorsname.AddRange(BilanzView.Bilanz_View_Item_List);

            Colorsname.AddRange(PopupView.Popup_View_Item_List);

            Create_Default_Colordic();
        }

        private async Task View_IsAppering_MethodeAsync()
        {
            try
            {
                if(Preferences.Get("Stylingprofil",-1) != -1)
                {
                    Stylingprofile stylingprofile = new Stylingprofile();

                    stylingprofile = await StylingService.Get_specific_Stylingprofile(Preferences.Get("Stylingprofil", -1));

                    if(stylingprofile != null)
                    {
                        Currentstylingprofile = stylingprofile;
                    }

                    ColorWheel_State = false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Load_Methode()
        {
            try
            {
                var result = await StylingService.Get_all_Stylingprofile();

                if(result.Count() == 0)
                {
                    await ToastHelper.ShowToast("Es gibt kein Stylingprofil was geladen werden kann.");

                    return;
                }
                else
                {
                    List<Stylingprofile> list = new List<Stylingprofile>();

                    Dictionary<string,string> colors = new Dictionary<string, string>();

                    List<string> buttons = new List<string>();

                    foreach (var s in result)
                    {
                        list.Add(s);

                        buttons.Add("Stylingprofil "+s.Id+"");
                    }

                    string title;

                    if(Currentstylingprofile.Id != 0)
                    {
                        title = "Stylingprofile laden\nAktuell: Stylingprofil " + Currentstylingprofile.Id + "";
                    }
                    else
                    {
                        title = "Stylingprofile laden";
                    }

                    var result1 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup(title, 400, buttons));

                    if(result1 == null)
                    {
                        return;
                    }
                    else
                    {
                        string reslutstring = result1 as string;

                        int id = int.Parse(reslutstring.Substring(reslutstring.IndexOf(" ")+1));

                        Stylingprofile output = list.Where(sp => sp.Id == id).First();

                        colors = Stylingprofile_Konverter.Deserilize(output);

                        SelectedColor = Color.Black;

                        if (colors.Count() != 0)
                        {
                            foreach (var color in colors)
                            {
                                App.Current.Resources[color.Key] = color.Value;
                                Preferences.Set(color.Key, color.Value);
                            }
                        }

                        ColorWheel_State = false;

                        Currentstylingprofile = output;

                        Preferences.Set("Stylingprofil", Currentstylingprofile.Id);

                        await ToastHelper.ShowToast("Die Einstellungen wurden erfolgreich aus dem Stylingprofil "+id+" geladen.");
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Save_Methode()
        {
            try
            {
                if(Colordic.Count != 0)
                {
                    Dictionary<string,string> colordict = new Dictionary<string,string>();

                    foreach (string name in Colorsname)
                    {
                        colordict.Add(name, App.Current.Resources[name].ToString());
                    }

                    Stylingprofile placeholder  = new Stylingprofile() { Colors = Stylingprofile_Konverter.Serilize(colordict) };

                    var result = await StylingService.Get_all_Stylingprofile();

                    if (result.Count() == 0)
                    {
                        var result1 = await StylingService.Add_Stylingprofile(placeholder);

                        if (result1 == -1)
                        {
                            await ToastHelper.ShowToast("Es ist beim speichern ein Fehler aufgetreten.");
                        }

                        if (result1 == 0)
                        {
                            await ToastHelper.ShowToast("Das Stylingprofil konnte nicht gespeichert werden, da es leer ist.");
                        }
                        else
                        {
                            ColorWheel_State = false;

                            Currentstylingprofile = placeholder;

                            Preferences.Set("Stylingprofil", Currentstylingprofile.Id);

                            await ToastHelper.ShowToast("Das Stylingprofil " + result1 + " wurde erfolgreich erstellt und die Einstellungen wurden erfolgreich darin gespeichert.");
                        }
                    }
                    else
                    {
                        var result2 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Speichern",330,new List<string>() { "Im Stylingprofil "+Currentstylingprofile.Id+" speichern.", "Neues Stylingprofil erstellen."}));

                        if(result2 == null)
                        {
                            return;
                        }
                        if((string)result2 == "Im Stylingprofil " + Currentstylingprofile.Id + " speichern.")
                        {
                            ColorWheel_State = false;

                            Currentstylingprofile.Colors = placeholder.Colors;

                            await StylingService.Edit_Stylingprofile(Currentstylingprofile);

                            await ToastHelper.ShowToast("Die Einstellungen wurden erfolgreich im Stylingprofil " + Currentstylingprofile.Id + " gespeichert.");
                        }
                        else
                        {
                            var result1 = await StylingService.Add_Stylingprofile(placeholder);

                            if (result1 == -1)
                            {
                                await ToastHelper.ShowToast("Es ist beim speichern ein Fehler aufgetreten.");
                            }

                            if (result1 == 0)
                            {
                                await ToastHelper.ShowToast("Das Stylingprofil konnte nicht gespeichert werden, da es leer ist.");
                            }
                            else
                            {
                                ColorWheel_State = false;

                                Currentstylingprofile = placeholder;

                                Preferences.Set("Stylingprofil", Currentstylingprofile.Id);

                                await ToastHelper.ShowToast("Das Stylingprofil " + result1 + " wurde erfolgreich erstellt und die Einstellungen wurden erfolgreich darin gespeichert.");
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

        public async Task Default_Methode()
        {
            try
            {
                if(CurrentCell != null)
                {
                    string message = "Wollen Sie wirklich die Änderungen vom " + CurrentCell.Title + " zurücksetzen?";

                    if (CurrentCell.Title == "Bilanz")
                    {
                        message = "Wollen Sie wirklich die Änderungen von der " + CurrentCell.Title + " zurücksetzen?";
                    }
                    var result = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Zurücksetzen",300,300,"Ja","Nein",message));

                    if(result == null)
                    {
                        return;
                    }

                    if((bool)result == true)
                    {
                        SelectedColor = Color.Black;

                        if (CurrentCell.Items.Count() != 0)
                        {
                            foreach (var color in Colordic)
                            {
                                if (CurrentCell.Items.Contains(color.Key) == true)
                                {
                                    App.Current.Resources[color.Key] = color.Value;
                                    Preferences.Set(color.Key, color.Value);
                                }
                            }
                        }
                    }

                    ColorWheel_State = false;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public void Create_Default_Colordic()
        {
            try
            {
                //Popup
                Colordic.Add("Vordergrund_Cancel_Popup", "#710117");
                Colordic.Add("Hintergrund_Cancel_Popup", Color.Black.ToHex());
                Colordic.Add("Text_Titel_Popup", "#cbcdcb");
                Colordic.Add("Hintergrund_Content_Popup", "#0b1c48");
                Colordic.Add("Rand_Content_Popup", "#2b6ad0");
                Colordic.Add("Text_Content_Popup", "#ecd5bb");
                Colordic.Add("Subtext_Content_Popup", "#ecd5bb");
                Colordic.Add("Hintertgrund_Button_Popup", "#0b1c48");
                Colordic.Add("Rand_Button_Popup", "#2b6ad0");
                Colordic.Add("Text_Button_Popup", "#138b83");
                Colordic.Add("Aktiv_Schalter_Popup", Color.ForestGreen.ToHex());
                Colordic.Add("Deaktiv_Schalter_Popup", Color.DarkRed.ToHex());

                //Home
                Colordic.Add("Hintergrund_Seite_Home", Color.DarkSlateGray.ToHex());
                Colordic.Add("Hintergrund_Bearbeiten_Home", Color.Green.ToHex());
                Colordic.Add("Rand_Bearbeiten_Home", Color.DarkGreen.ToHex());
                Colordic.Add("Text_Bearbeiten_Home", Color.White.ToHex());
                Colordic.Add("Hintergrund_Löschen_Home", Color.Red.ToHex());
                Colordic.Add("Rand_Löschen_Home", Color.DarkRed.ToHex());
                Colordic.Add("Text_Löschen_Home", Color.White.ToHex());
                Colordic.Add("Hintergrund_Transaktion_Home", Color.Gray.ToHex());
                Colordic.Add("Rand_Transaktion_Home", Color.DarkGray.ToHex());
                Colordic.Add("Text_Transaktion_Home", Color.Black.ToHex());
                Colordic.Add("Hintergrund_Detail_Transaktion_Home", Color.LightGray.ToHex());
                Colordic.Add("Text_Detail_Transaktion_Home", Color.Black.ToHex());
                Colordic.Add("Hintergrund_Positiver_Betrag_Transaktion_Home", Color.MediumSeaGreen.ToHex());
                Colordic.Add("Rand_Positiver_Betrag_Transaktion_Home", Color.ForestGreen.ToHex());
                Colordic.Add("Text_Positiver_Betrag_Transaktion_Home", Color.Black.ToHex());
                Colordic.Add("Hintergrund_Negativer_Betrag_Transaktion_Home", Color.Salmon.ToHex());
                Colordic.Add("Rand_Negativer_Betrag_Transaktion_Home", Color.Red.ToHex());
                Colordic.Add("Text_Negativer_Betrag_Transaktion_Home", Color.Black.ToHex());
                Colordic.Add("Text_MehrLaden_Home", Color.Black.ToHex());
                Colordic.Add("Vordergrund_Budget_Home", Color.SaddleBrown.ToHex());
                Colordic.Add("Hintergrund_Saldo_Home", Color.Black.ToHex());
                Colordic.Add("Text_Saldo_Home", Color.White.ToHex());
                Colordic.Add("Vordergrund_Hinzufügen_Home", Color.Orange.ToHex());

                //Hinzufügen/Bearbeiten
                Colordic.Add("Hintergrund_Hinzufügen", Color.DarkSlateGray.ToHex());
                Colordic.Add("Hintergrund_Eingabefeld_Hinzufügen", Color.Orange.ToHex());
                Colordic.Add("Rand_Eingabefeld_Hinzufügen", Color.Coral.ToHex());
                Colordic.Add("Title_Eingabefeld_Hinzufügen", Color.White.ToHex());
                Colordic.Add("Text_Eingabefeld_Hinzufügen", Color.Black.ToHex());
                Colordic.Add("Platzhater_Eingabefeld_Hinzufügen", "#525252");
                Colordic.Add("Aktiv_Schalter_Eingabefeld_Hinzufügen", Color.ForestGreen.ToHex());
                Colordic.Add("Deaktiv_Schalter_Eingabefeld_Hinzufügen", Color.DarkRed.ToHex());
                Colordic.Add("Hintergrund_Wahlfeld_Hinzufügen", Color.Orange.ToHex());
                Colordic.Add("Haubttext_Wahlfeld_Hinzufügen", Color.Black.ToHex());
                Colordic.Add("Nebentext_Wahlfeld_Hinzufügen", Color.Black.ToHex());
                Colordic.Add("Hintergrund_Option_Wahlfeld_Hinzufügen", Color.SkyBlue.ToHex());
                Colordic.Add("Rand_Option_Wahlfeld_Hinzufügen", Color.Blue.ToHex());
                Colordic.Add("Text_Option_Wahlfeld_Hinzufügen", Color.OrangeRed.ToHex());
                Colordic.Add("Hintergrund_Schalter_Hinzufügen", Color.SkyBlue.ToHex());
                Colordic.Add("Rand_Schalter_Hinzufügen", Color.Blue.ToHex());
                Colordic.Add("Text_Schalter_Hinzufügen", Color.OrangeRed.ToHex());

                //Bilanz
                Colordic.Add("Hintergrund_Bilanz", Color.DarkSlateGray.ToHex());
                Colordic.Add("Hintergrund_Kopfzeile_Bilanz", Color.DarkGray.ToHex());
                Colordic.Add("Text_Kopfzeile_Bilanz", Color.Black.ToHex());
                Colordic.Add("Hintergrund_Fußzeile_Bilanz", Color.DarkGray.ToHex());
                Colordic.Add("Text_Fußzeile_Bilanz", Color.Black.ToHex());
                Colordic.Add("Text_Zusammenfassung_Bilanz", Color.Black.ToHex());
                Colordic.Add("Text_Zweck_Stack_Bilanz", Color.White.ToHex());
                Colordic.Add("Text_Anzahl_Stack_Bilanz", Color.White.ToHex());
            }
            catch
            {
            }
        }

        public async Task Choose_Methode(List<object> input)
        {
            if(input.Count() >= 2)
            {
                Dictionary<string, string> input1 = (Dictionary<string,string>)input[0];

                if(input1 != null)
                {
                    CurrentItemList = input1.Keys.ToList();
                }
                else
                {
                    CurrentItemList = null;

                }

                string input2 = (string)input[1];

                string input3 = (string)input[2];

                if(CurrentItemList.Count() != 1)
                {
                    var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup(""+ input2 + " bestimmen", 350, CurrentItemList));
                    
                    if(result == null)
                    {
                        return;
                    }
                    else
                    {
                        CurrentColorname = input1[(string)result];
                    }
                }
                else
                {
                    CurrentColorname = input1.Values.First();
                }

                SelectedColor = Color.FromHex(App.Current.Resources[CurrentColorname].ToString());

                RaiseMessage(App.Current.Resources[CurrentColorname].ToString());

                ColorWheel_State = true;

                CurrentItem = input3;
            }
        }

        internal async void SelectedColorChanged_CommandExecuted(object sender, ColorChangedEventArgs e)
        {
            try
            {
                SelectedColor = e.NewColor;

                if (CurrentColorname != null)
                {
                    App.Current.Resources[CurrentColorname] = SelectedColor.ToHex();

                    Preferences.Set(CurrentColorname, SelectedColor.ToHex());
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        internal async void CurrentItemChangedCommand_CommandExecuted(object sender, CurrentItemChangedEventArgs e)
        {
            try
            {
                CurrentCell = e.CurrentItem as StylingCell;

                ColorWheel_State = false;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private void RaiseMessage (string message)
        {
            OnMessageRaised?.Invoke(this, new StylingMessageEventArgs(message));
        }

        public string CurrentItem { get; set; }

        public StylingCell currentcell;
        public StylingCell CurrentCell
        {
            get { return currentcell; }
            set
            {
                if (CurrentCell == value)
                    return;
                currentcell = value; RaisePropertyChanged();
            }
        }
        public string CurrentColorname { get; set; }

        public List<string> CurrentItemList = new List<string>();

        public Color selectedcolor = Color.Black;
        public Color SelectedColor
        {
            get { return selectedcolor; }
            set
            {
                if (SelectedColor == value)
                    return;
                selectedcolor = value; RaisePropertyChanged();
            }
        }

        public Dictionary<string, string> Colordic = new Dictionary<string, string>();

        public string Colorstring { get; set; }

        public List<string> Colorsname = new List<string>();

        public Stylingprofile Currentstylingprofile { get; set; }

        public bool colorwheel_state = false;
        public bool ColorWheel_State
        {
            get { return colorwheel_state; }
            set
            {
                if (ColorWheel_State == value)
                    return;
                colorwheel_state = value; RaisePropertyChanged();
            }
        }

        public AsyncCommand Back_Command { get; }
        public AsyncCommand Default_Command { get; }
        public AsyncCommand Save_Command { get; }
        public AsyncCommand Load_Command { get; }
        public AsyncCommand View_IsAppering_Command { get; }
        public AsyncCommand<object> CurrentItemChangedCommand_Command { get; }
        public AsyncCommand<List<object>> Choose_Command { get; }

        public EventHandler<StylingMessageEventArgs> OnMessageRaised;
    }
}
