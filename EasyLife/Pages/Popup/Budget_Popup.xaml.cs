using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Budget_Popup : Popup
    {
        public List<Budget> budgetList = new List<Budget>();

        public List<Budget_Progessbar> budget_ProgessbarsList = new List<Budget_Progessbar>();

        public Viewtime Current_Viewtime { get; set; }

        public List<Transaktion> transaktionslist { get; set; }

        public Budget_Popup(List<Budget> input, Viewtime input2, List<Transaktion> input3)
        {
            budgetList = input;

            if (budgetList.Count() != 0)
            {
                foreach (Budget bg in budgetList)
                {
                    Budget_Progessbar placeholder = new Budget_Progessbar();

                    if ((bg.Current / bg.Goal) < 1)
                    {
                        placeholder = new Budget_Progessbar() { Budget = bg, Progress = new Rectangle(0, 0, (bg.Current / bg.Goal), 1), Rest_Progress = new Rectangle(1, 0, 1 - (bg.Current / bg.Goal), 1), Rest_Budget = bg.Goal - bg.Current };

                        if ((bg.Current / bg.Goal) < 0.2)
                        {
                            placeholder.Progress_visibility = false;
                        }
                        else
                        {
                            placeholder.Progress_visibility = true;
                        }

                        if ((bg.Current / bg.Goal) > 0.8)
                        {
                            placeholder.Rest_Progress_visibility = false;
                        }
                        else
                        {
                            placeholder.Rest_Progress_visibility = true;
                        }

                        placeholder.Overload = false;

                        placeholder.Successful = false;
                    }
                    else
                    {
                        placeholder = new Budget_Progessbar() { Budget = bg, Progress = new Rectangle(0, 0, (bg.Current / bg.Goal), 1), Rest_Progress = new Rectangle(1, 0, 0, 1), Rest_Budget = 0, Rest_Progress_visibility = false };

                        if (bg.Current > bg.Goal)
                        {
                            placeholder.Progress_visibility = false;

                            placeholder.Overload = true;

                            placeholder.Successful = false;
                        }
                        else
                        {
                            placeholder.Progress_visibility = false;

                            placeholder.Overload = false;

                            placeholder.Successful = true;
                        }
                    }

                    budget_ProgessbarsList.Add(placeholder);
                }
            }

            Current_Viewtime = input2;

            transaktionslist = input3;

            InitializeComponent();

            BudgetList.ItemsSource = budget_ProgessbarsList;

            if (budget_ProgessbarsList.Count() == 0)
            {
                BudgetList.IsVisible = false;

                Rahmen.HeightRequest = 50;
            }
            else
            {
                BudgetList.IsVisible = true;

                Rahmen.HeightRequest = 105 * budget_ProgessbarsList.Count() + 70;
            }

            Budget_Popup_Size.Size = new Size(400, 900);

            Budget_Popup_Size.HorizontalOptions = LayoutOptions.Center;

            Budget_Popup_Size.VerticalOptions = LayoutOptions.Center;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                Dismiss(null);
            }
            catch
            {
                Dismiss(null);
            }
        }

        private async void SwipeItemLeft_Invoked(object sender, EventArgs e)
        {
            var input = sender as SwipeItemView;

            Budget_Progessbar budget = input.CommandParameter as Budget_Progessbar;

            if (budget != null)
            {
                await BudgetService.Remove_Budget(budget.Budget);

                budget_ProgessbarsList.Remove(budget);

                if (budget_ProgessbarsList.Count() != 0)
                {
                    Rahmen.HeightRequest = 105 * budget_ProgessbarsList.Count() + 70;
                }
                else
                {
                    BudgetList.IsVisible = false;

                    Rahmen.HeightRequest = 50;
                }

                BudgetList.ItemsSource = null;

                BudgetList.ItemsSource = budget_ProgessbarsList;
            }
        }

        private async void SwipeItemRight_Invoked(object sender, EventArgs e)
        {
            var input = sender as SwipeItemView;

            Budget_Progessbar budget = input.CommandParameter as Budget_Progessbar;

            if (budget != null)
            {
                bool indicator = false;

                while (indicator == false)
                {
                    var result = await Shell.Current.ShowPopupAsync(new CustomePromt_Popup("Budget ändern", 350, 320, "Verändern", "Abbrechen", "Das Budget liegt bei " + budget.Budget.Goal.ToString().Replace(".", ",") + " €."));

                    if (result != null)
                    {
                        if (double.TryParse((string)result, NumberStyles.Any, new CultureInfo("de-DE"), out double result1) == true)
                        {
                            string resultstring = result1.ToString("F2");

                            double result0 = double.Parse(resultstring.Replace(".", ","), NumberStyles.Any, new CultureInfo("de-DE"));

                            if (result0 <= 0)
                            {
                                await ToastHelper.ShowToast("Das Budget darf nicht kleiner gleich 0 € betragen.");
                            }
                            else
                            {
                                budget.Budget.Goal = result0;

                                await BudgetService.Edit_Budget(budget.Budget);

                                foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                {
                                    if (bg.Budget.Id == budget.Budget.Id)
                                    {
                                        bg.Budget.Goal = budget.Budget.Goal;
                                    }

                                    bg.Budget.Current = 0;
                                }

                                foreach (Transaktion transaktion in transaktionslist)
                                {
                                    foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                    {
                                        if (transaktion.Zweck == bg.Budget.Name)
                                        {
                                            bg.Budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                        }

                                        if (bg.Budget.Name == "Monat")
                                        {
                                            if (double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                            {
                                                bg.Budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                            }
                                        }
                                    }
                                }

                                foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                {
                                    if ((bg.Budget.Current / bg.Budget.Goal) < 1)
                                    {
                                        bg.Progress = new Rectangle(0, 0, (bg.Budget.Current / bg.Budget.Goal), 1);

                                        bg.Rest_Progress = new Rectangle(1, 0, 1 - (bg.Budget.Current / bg.Budget.Goal), 1);

                                        bg.Rest_Budget = bg.Budget.Goal - bg.Budget.Current;

                                        if ((bg.Budget.Current / bg.Budget.Goal) < 0.2)
                                        {
                                            bg.Progress_visibility = false;
                                        }
                                        else
                                        {
                                            bg.Progress_visibility = true;
                                        }

                                        if ((bg.Budget.Current / bg.Budget.Goal) > 0.8)
                                        {
                                            bg.Rest_Progress_visibility = false;
                                        }
                                        else
                                        {
                                            bg.Rest_Progress_visibility = true;
                                        }

                                        bg.Overload = false;

                                        bg.Successful = false;
                                    }
                                    else
                                    {
                                        bg.Progress = new Rectangle(0, 0, 1, 1);

                                        bg.Rest_Progress = new Rectangle(1, 0, 0, 1);

                                        bg.Rest_Budget = 0;

                                        bg.Rest_Progress_visibility = false;

                                        if (bg.Budget.Current > bg.Budget.Goal)
                                        {
                                            bg.Progress_visibility = false;

                                            bg.Overload = true;

                                            bg.Successful = false;
                                        }
                                        else
                                        {
                                            bg.Progress_visibility = false;

                                            bg.Overload = false;

                                            bg.Successful = true;
                                        }
                                    }
                                }

                                BudgetList.ItemsSource = null;

                                BudgetList.ItemsSource = budget_ProgessbarsList;

                                indicator = true;
                            }
                        }
                    }
                    else
                    {
                        indicator = true;
                    }
                }

                if (budget.Budget.Name == "Monat")
                {
                    Budget monat_budget = budget.Budget;

                    if (monat_budget != null)
                    {
                        if (monat_budget.Name_Of_Enabled_Reasons == null || monat_budget.Name_Of_Enabled_Reasons == "leer")
                        {
                            var result0 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Monat eingrenzen", 350, 250, "Ja", "Nein", "Wollen Sie bestimmte Zwecke aus dem Monats-Budget entfernen?"));

                            List<string> diabled_reaons = new List<string>();

                            if (result0 != null)
                            {
                                if ((bool)result0 == true)
                                {
                                    var enablereason = await ReasonService.Get_Enable_ReasonDictionary();

                                    List<string> enablereasonlist = new List<string>();

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
                                                var result1 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup((string)result, 350, 300, "Ja", "Nein", "Wollen Sie den Zweck " + (string)result + " wieder in das Monats-Budget einführen?"));

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
                                        monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                    }
                                    else
                                    {
                                        diabled_reaons.Add("leer");

                                        monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                    }
                                }
                                else
                                {
                                    monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                }
                            }
                            else
                            {
                                monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                            }

                            if (diabled_reaons.Count() != 0)
                            {
                                monat_budget.Current = 0.0;

                                foreach (Transaktion trans in transaktionslist)
                                {
                                    if (double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                    {
                                        if (diabled_reaons.Contains(trans.Zweck) == false)
                                        {
                                            monat_budget.Current += Math.Abs(double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                        }
                                    }
                                }
                            }

                            await BudgetService.Edit_Budget(monat_budget);
                        }
                        else
                        {
                            var result0 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Monat eingrenzen", 350, 300, "Ja", "Nein", "Wollen Sie bestimmte Zwecke aus dem Monats-Budget entfernen oder wieder einführen?"));

                            List<string> diabled_reaons = Budget_Konverter.Deserilize(monat_budget);

                            if (result0 != null)
                            {
                                if ((bool)result0 == true)
                                {
                                    var enablereason = await ReasonService.Get_Enable_ReasonDictionary();

                                    List<string> enablereasonlist = new List<string>();

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

                                    bool indkator = false;

                                    while (indkator == false)
                                    {
                                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Zweck der ausgeschlossen oder eingeführt werden sollen", 400, enablereasonlist));

                                        if (result == null)
                                        {
                                            indkator = true; break;
                                        }
                                        else
                                        {
                                            if (diabled_reaons.Contains((string)result) == true)
                                            {
                                                var result1 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup((string)result, 300, 350, "Ja", "Nein", "Wollen Sie den Zweck " + (string)result + " wieder in das Monats-Budget einführen?"));

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
                                        monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                    }
                                    else
                                    {
                                        diabled_reaons.Add("leer");

                                        monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                    }
                                }
                                else
                                {
                                    monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                }
                            }
                            else
                            {
                                monat_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                            }

                            if (diabled_reaons.Count() != 0)
                            {
                                monat_budget.Current = 0.0;

                                foreach (Transaktion trans in transaktionslist)
                                {
                                    if (double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                    {
                                        if (diabled_reaons.Contains(trans.Zweck) == false)
                                        {
                                            monat_budget.Current += Math.Abs(double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                        }
                                    }
                                }
                            }

                            await BudgetService.Edit_Budget(monat_budget);
                        }
                    }

                    budget.Budget = monat_budget;
                }
            }
        }

        private async void Add_Budget(object sender, EventArgs e)
        {
            Budget new_budget = new Budget();

            string[] budget_name_list = { };

            var enablereason = (Dictionary<string, string>)await ReasonService.Get_Enable_ReasonDictionary();

            List<string> budget_names = new List<string>() { "Monat" };

            if (enablereason.Count() != 0)
            {
                foreach (var value in enablereason)
                {
                    if (value.Value == "Ausgaben")
                    {
                        budget_names.Add(value.Key);
                    }
                }
            }

            var result0 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Budget benennen", 400, budget_names));

            if (result0 == null)
            {
                return;
            }
            else
            {
                new_budget.Name = (string)result0;

                bool indicator = false;

                while (indicator == false)
                {
                    var result = await Shell.Current.ShowPopupAsync(new CustomePromt_Popup("Budget festlegen", 350, 320, "Hinzufügen", "Verwerfen", "Hier das Budget eingeben."));

                    if (result != null)
                    {
                        string resultstring = (string)result;

                        if (double.TryParse(resultstring, NumberStyles.Any, new CultureInfo("de-DE"), out double result1) == true)
                        {
                            result = result1.ToString("F2");

                            double result2 = double.Parse(resultstring.Replace(".", ","), NumberStyles.Any, new CultureInfo("de-DE"));

                            if (result2 <= 0)
                            {
                                await ToastHelper.ShowToast("Das Budget darf nicht kleiner gleich 0 € betragen.");
                            }
                            else
                            {
                                new_budget.Goal = result2;

                                if (new_budget.Name == "Monat")
                                {
                                    if (new_budget.Name_Of_Enabled_Reasons == null)
                                    {
                                        var result4 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Monat eingrenzen", 350, 250, "Ja", "Nein", "Wollen Sie bestimmte Zwecke aus dem Monats-Budget entfernen?"));

                                        List<string> diabled_reaons = new List<string>();

                                        if (result4 != null)
                                        {
                                            if ((bool)result4 == true)
                                            {
                                                bool indkator = false;

                                                while (indkator == false)
                                                {
                                                    var result5 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Zweck der ausgeschlossen werden sollen", 400, budget_names));

                                                    if (result5 == null)
                                                    {
                                                        indkator = true; break;
                                                    }
                                                    else
                                                    {
                                                        if (diabled_reaons.Contains((string)result5) == true)
                                                        {
                                                            var result6 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup((string)result5, 350, 300, "Ja", "Nein", "Wollen Sie den Zweck " + (string)result5 + " wieder in das Monats-Budget einführen?"));

                                                            if (result6 != null)
                                                            {
                                                                if ((bool)result6 == true)
                                                                {
                                                                    diabled_reaons.Remove((string)result5);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            diabled_reaons.Add((string)result5);
                                                        }
                                                    }
                                                }

                                                if (diabled_reaons.Count() != 0)
                                                {
                                                    new_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                                }
                                                else
                                                {
                                                    diabled_reaons.Add("leer");

                                                    new_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(diabled_reaons);
                                                }
                                            }
                                            else
                                            {
                                                new_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                            }
                                        }
                                        else
                                        {
                                            new_budget.Name_Of_Enabled_Reasons = Budget_Konverter.Serilize(new List<string>() { "leer" });
                                        }

                                        if (diabled_reaons.Count() != 0)
                                        {
                                            new_budget.Current = 0.0;

                                            foreach (Transaktion trans in transaktionslist)
                                            {
                                                if (double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                                {
                                                    if (diabled_reaons.Contains(trans.Zweck) == false)
                                                    {
                                                        new_budget.Current += Math.Abs(double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                var result3 = await BudgetService.Add_Budget(new_budget);

                                if (result3 == 0)
                                {
                                    budget_ProgessbarsList.Add(new Budget_Progessbar() { Budget = new_budget });

                                    foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                    {
                                        bg.Budget.Current = 0;
                                    }

                                    foreach (Transaktion transaktion in transaktionslist)
                                    {
                                        foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                        {
                                            if (transaktion.Zweck == bg.Budget.Name)
                                            {
                                                bg.Budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                            }

                                            if (bg.Budget.Name == "Monat")
                                            {
                                                if (double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")) < 0)
                                                {
                                                    bg.Budget.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                                }
                                            }
                                        }
                                    }

                                    foreach (Budget_Progessbar bg in budget_ProgessbarsList)
                                    {
                                        if ((bg.Budget.Current / bg.Budget.Goal) < 1)
                                        {
                                            bg.Progress = new Rectangle(0, 0, (bg.Budget.Current / bg.Budget.Goal), 1);

                                            bg.Rest_Progress = new Rectangle(1, 0, 1 - (bg.Budget.Current / bg.Budget.Goal), 1);

                                            bg.Rest_Budget = bg.Budget.Goal - bg.Budget.Current;

                                            if ((bg.Budget.Current / bg.Budget.Goal) < 0.2)
                                            {
                                                bg.Progress_visibility = false;
                                            }
                                            else
                                            {
                                                bg.Progress_visibility = true;
                                            }

                                            if ((bg.Budget.Current / bg.Budget.Goal) > 0.8)
                                            {
                                                bg.Rest_Progress_visibility = false;
                                            }
                                            else
                                            {
                                                bg.Rest_Progress_visibility = true;
                                            }

                                            bg.Overload = false;

                                            bg.Successful = false;
                                        }
                                        else
                                        {
                                            bg.Progress = new Rectangle(0, 0, 1, 1);

                                            bg.Rest_Progress = new Rectangle(1, 0, 0, 1);

                                            bg.Rest_Budget = 0;

                                            bg.Rest_Progress_visibility = false;

                                            if (bg.Budget.Current > bg.Budget.Goal)
                                            {
                                                bg.Progress_visibility = false;

                                                bg.Overload = true;

                                                bg.Successful = false;
                                            }
                                            else
                                            {
                                                bg.Progress_visibility = false;

                                                bg.Overload = false;

                                                bg.Successful = true;
                                            }
                                        }
                                    }

                                    BudgetList.ItemsSource = null;

                                    BudgetList.ItemsSource = budget_ProgessbarsList;

                                    BudgetList.IsVisible = true;

                                    Rahmen.HeightRequest = 105 * budget_ProgessbarsList.Count() + 70;

                                    indicator = true;
                                }
                                else
                                {
                                    if (result3 == 1)
                                    {
                                        await ToastHelper.ShowToast("Es gabe ein Fehler und das Budget konnte nicht hinzugefügt werden.");
                                    }
                                    else
                                    {
                                        await ToastHelper.ShowToast("Das Budget ist bereits vorhanden.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            await ToastHelper.ShowToast("Die Eingabe war inkorrekt.");
                        }
                    }
                    else
                    {
                        indicator = true;
                    }
                }
            }
        }
    }
}