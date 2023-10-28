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

        public Viewtime Current_Viewtime { get; set; }

        public List<Transaktion> transaktionslist { get; set; }

        public Budget_Popup(List<Budget> input, Viewtime input2, List<Transaktion> input3)
        {
            budgetList = input;

            Current_Viewtime = input2;

            transaktionslist = input3;

            InitializeComponent();

            BudgetList.ItemsSource = budgetList;

            if (budgetList.Count() == 0)
            {
                BudgetList.IsVisible = false;
            }
            else
            {
                BudgetList.IsVisible = true;

                Budget_Popup_Size.Size = new Size(400, 600);

                Rahmen.HeightRequest = 65 * budgetList.Count() + 50;
            }
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

            Budget budget = input.CommandParameter as Budget;

            if (budget != null)
            {
                await BudgetService.Remove_Budget(budget);

                budgetList.Remove(budget);

                if(budgetList.Count() != 0)
                {
                    Rahmen.HeightRequest = 65 * budgetList.Count() + 50;
                }
                else
                {
                    BudgetList.IsVisible = false;

                    Rahmen.HeightRequest = 50;
                }

                BudgetList.ItemsSource = null;

                BudgetList.ItemsSource = budgetList;
            }
        }

        private async void SwipeItemRight_Invoked(object sender, EventArgs e)
        {
            var input = sender as SwipeItem;

            Budget budget = input.CommandParameter as Budget;

            if (budget != null)
            {
                bool indicator = false;

                while(indicator == false)
                {
                    var result = await Shell.Current.DisplayPromptAsync("Budget ändern", "Das aktuelle Budget liegt bei " + budget.Goal.ToString().Replace(".", ",") + " €.", "Verändern", "Abbrechen", "Hier das neue Budget eingeben.", 20, null, null);

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
                                budget.Goal = result0;

                                await BudgetService.Edit_Budget(budget);

                                foreach (Budget bg in budgetList)
                                {
                                    if (bg.Id == budget.Id)
                                    {
                                        bg.Goal = budget.Goal;
                                    }

                                    bg.Current = 0;
                                }

                                foreach (Transaktion transaktion in transaktionslist)
                                {
                                    foreach (Budget bg in budgetList)
                                    {
                                        if (transaktion.Zweck == bg.Name)
                                        {
                                            bg.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                        }

                                        if (bg.Name == "Monat")
                                        {
                                            bg.Current += double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                        }
                                    }
                                }

                                foreach (Budget bg in budgetList)
                                {
                                    bg.Red = Math.Round((bg.Current / bg.Goal) * 360, 0);

                                    bg.Visibility = true;

                                    if (bg.Red <= 0)
                                    {
                                        bg.Red = 0;

                                        bg.Visibility = false;
                                    }
                                    if (bg.Red > 360)
                                    {
                                        bg.Red = 360;
                                    }
                                }

                                BudgetList.ItemsSource = null;

                                BudgetList.ItemsSource = budgetList;

                                indicator = true;
                            }
                        }
                    }
                    else
                    {
                        await ToastHelper.ShowToast("Die Eingabe war inkorrekt.");
                    }
                }

            }
        }

        private async void Add_Budget(object sender, EventArgs e)
        {
            Budget new_budget = new Budget();

            string[] budget_name_list = { };

            var enablereason = (Dictionary<string, string>)await ReasonService.Get_Enable_ReasonDictionary();

            List<string> budget_names = new List<string>() {"Monat"};

            budget_names.AddRange(enablereason.Keys.ToList());

            budget_name_list = budget_names.ToArray();

            var result0 = await Shell.Current.DisplayActionSheet("Budget benennen","Verwerfen",null,budget_name_list);

            if(result0 == null)
            {
                return;
            }

            if(result0 == "Verwerfen")
            {
                return;
            }
            else
            {
                new_budget.Name = result0;

                bool indicator = false;

                while(indicator == false)
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

                                if(result3 == 0)
                                {
                                    budgetList.Add(new_budget);

                                    foreach(Budget bg in budgetList)
                                    {
                                        bg.Current = 0;
                                    }

                                    foreach (Transaktion transaktion in transaktionslist)
                                    {
                                        foreach (Budget bg in budgetList)
                                        {
                                            if (transaktion.Zweck == bg.Name)
                                            {
                                                bg.Current += Math.Abs(double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE")));
                                            }

                                            if (bg.Name == "Monat")
                                            {
                                                bg.Current += double.Parse(transaktion.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                            }
                                        }
                                    }

                                    foreach (Budget bg in budgetList)
                                    {
                                        bg.Red = Math.Round((bg.Current / bg.Goal) * 360, 0);

                                        bg.Visibility = true;

                                        if (bg.Red <= 0)
                                        {
                                            bg.Red = 0;

                                            bg.Visibility = false;
                                        }
                                        if (bg.Red > 360)
                                        {
                                            bg.Red = 360;
                                        }
                                    }

                                    BudgetList.ItemsSource = null;

                                    BudgetList.ItemsSource = budgetList;

                                    BudgetList.IsVisible = true;

                                    Rahmen.HeightRequest = 65 * budgetList.Count() + 50;

                                    indicator = true;
                                }
                                else
                                {
                                    if(result3 == 1)
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