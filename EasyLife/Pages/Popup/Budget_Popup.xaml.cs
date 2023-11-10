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
            var input = sender as SwipeItem;

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
            var input = sender as SwipeItem;

            Budget_Progessbar budget = input.CommandParameter as Budget_Progessbar;

            if (budget != null)
            {
                bool indicator = false;

                while (indicator == false)
                {
                    var result = await Shell.Current.DisplayPromptAsync("Budget ändern", "Das aktuelle Budget liegt bei " + budget.Budget.Goal.ToString().Replace(".", ",") + " €.", "Verändern", "Abbrechen", "Hier das neue Budget eingeben.", 20, null, null);

                    if (result != null)
                    {
                        if (double.TryParse(result, NumberStyles.Any, new CultureInfo("de-DE"), out double result1) == true)
                        {
                            result = result1.ToString("F2");

                            double result0 = double.Parse(result.Replace(".", ","), NumberStyles.Any, new CultureInfo("de-DE"));

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

            budget_name_list = budget_names.ToArray();

            var result0 = await Shell.Current.DisplayActionSheet("Budget benennen", "Verwerfen", null, budget_name_list);

            if (result0 == null)
            {
                return;
            }

            if (result0 == "Verwerfen")
            {
                return;
            }
            else
            {
                new_budget.Name = result0;

                bool indicator = false;

                while (indicator == false)
                {
                    var result = await Shell.Current.DisplayPromptAsync("Budget festlegen", "Das Budget sollte größer 0 € sein.", "Hinzufügen", "Verwerfen", "Hier das Budget eingeben.", 20, null, null);

                    if (result != null)
                    {
                        if (double.TryParse(result, NumberStyles.Any, new CultureInfo("de-DE"), out double result1) == true)
                        {
                            result = result1.ToString("F2");

                            double result2 = double.Parse(result.Replace(".", ","), NumberStyles.Any, new CultureInfo("de-DE"));

                            if (result2 <= 0)
                            {
                                await ToastHelper.ShowToast("Das Budget darf nicht kleiner gleich 0 € betragen.");
                            }
                            else
                            {
                                new_budget.Goal = result2;

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