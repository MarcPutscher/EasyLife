using FreshMvvm;
using Command = MvvmHelpers.Commands.Command;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using MvvmHelpers.Commands;
using System;
using System.Runtime.CompilerServices;

namespace EasyLife.PageModels
{
    public class Styling_Color_PageModel : FreshBasePageModel
    {
        public Styling_Color_PageModel()
        {
            Apply_Command = new Command(Apply_Methode);

            Default_Command = new AsyncCommand(Default_MehtodeAsync);

            View_IsAppering_Command = new AsyncCommand(View_IsAppering_MethodeAsync);

            Page_BackgroundColor_Changed_Command = new Command(Page_BackgroundColor_Changed_Methode);

            App_BackgroundColor_Changed_Command = new Command(App_BackgroundColor_Changed_Methode);

            Transaktion_Backgroundcolor_Changed_Command = new Command(Transaktion_Backgroundcolor_Changed_Methode);

            Transaktion_Bordercolor_Changed_Command = new Command(Transaktion_Bordercolor_Changed_Methode);

            Transaktion_Textcolor_Changed_Command = new Command(Transaktion_Textcolor_Changed_Methode);

            EntryFrame_Backgroundcolor_Changed_Command = new Command(EntryFrame_Backgroundcolor_Changed_Methode);

            EntryFrame_Bordercolor_Changed_Command = new Command(EntryFrame_Bordercolor_Changed_Methode);

            EntryFrame_Textcolor_Changed_Command = new Command(EntryFrame_Textcolor_Changed_Methode);

            Button_Backgroundcolor_Changed_Command = new Command(Button_Backgroundcolor_Changed_Methode);

            Button_Bordercolor_Changed_Command = new Command(Button_Bordercolor_Changed_Methode);

            Button_Textcolor_Changed_Command = new Command(Button_Textcolor_Changed_Methode);

            Edit_Backgroundcolor_Changed_Command = new Command(Edit_Backgroundcolor_Changed_Methode);

            Edit_Bordercolor_Changed_Command = new Command(Edit_Bordercolor_Changed_Methode);

            Edit_Textcolor_Changed_Command = new Command(Edit_Textcolor_Changed_Methode);

            Remove_Backgroundcolor_Changed_Command = new Command(Remove_Backgroundcolor_Changed_Methode);

            Remove_Bordercolor_Changed_Command = new Command(Remove_Bordercolor_Changed_Methode);

            Remove_Textcolor_Changed_Command = new Command(Remove_Textcolor_Changed_Methode);

            Revive_Backgroundcolor_Changed_Command = new Command(Revive_Backgroundcolor_Changed_Changed_Methode);

            Revive_Bordercolor_Changed_Command = new Command(Revive_Bordercolor_Changed_Methode);

            Revive_Textcolor_Changed_Command = new Command(Revive_Textcolor_Changed_Methode);

            Delete_Backgroundcolor_Changed_Command = new Command(Delete_Backgroundcolor_Changed_Methode);

            Delete_Bordercolor_Changed_Command = new Command(Delete_Bordercolor_Changed_Methode);

            Delete_Textcolor_Changed_Command = new Command(Delete_Textcolor_Changed_Methode);

            Order_Backgroundcolor_Changed_Command = new Command(Order_Backgroundcolor_Changed_Methode);

            Order_Bordercolor_Changed_Command = new Command(Order_Bordercolor_Changed_Methode);

            Order_Textcolor_Changed_Command = new Command(Order_Textcolor_Changed_Methode);

            Saldo_Backgroundcolor_Changed_Command = new Command(Saldo_Backgroundcolor_Changed_Methode);

            Saldo_Textcolor_Changed_Command = new Command(Saldo_Textcolor_Changed_Methode);

            Label_Textcolor_Changed_Command = new Command(Label_Textcolor_Changed_Methode);

            Grouping_Textcolor_Changed_Command = new Command(Grouping_Textcolor_Changed_Methode);

            Refresh_Color_Changed_Command = new Command(Refresh_Color_Changed_Methode);

            Flyout_Backgroundcolor_Changed_Command = new Command(Flyout_Backgroundcolor_Changed_Methode);

            Flyout_Selectcolor_Changed_Command = new Command(Flyout_Selectcolor_Changed_Methode);

            Flyout_Textcolor_Changed_Command = new Command(Flyout_Textcolor_Changed_Methode);

            Flyout_Iconcolor_Changed_Command = new Command(Flyout_Iconcolor_Changed_Methode);


            Color_List = new List<string>();

            _ = Create_DictonaryAsync();

            foreach (string colorName in Color_Dictionary.Keys)
            {
                Color_List.Add(colorName);
            }
        }

        private async Task View_IsAppering_MethodeAsync()
        {
            try
            {
                App_Backgroundcolor = Color.FromHex(App.Current.Resources["App_Backgroundcolor"].ToString());

                Page_Backgroundcolor = Color.FromHex(App.Current.Resources["Page_Backgroundcolor"].ToString());

                Transaktion_Backgroundcolor = Color.FromHex(App.Current.Resources["Transaktion_Backgroundcolor"].ToString());
                Transaktion_Bordercolor = Color.FromHex(App.Current.Resources["Transaktion_Bordercolor"].ToString());
                Transaktion_Textcolor = Color.FromHex(App.Current.Resources["Transaktion_Textcolor"].ToString());

                EntryFrame_Backgroundcolor = Color.FromHex(App.Current.Resources["EntryFrame_Backgroundcolor"].ToString());
                EntryFrame_Bordercolor = Color.FromHex(App.Current.Resources["EntryFrame_Bordercolor"].ToString());
                EntryFrame_Textcolor = Color.FromHex(App.Current.Resources["EntryFrame_Textcolor"].ToString());

                Button_Backgroundcolor = Color.FromHex(App.Current.Resources["Button_Backgroundcolor"].ToString());
                Button_Bordercolor = Color.FromHex(App.Current.Resources["Button_Bordercolor"].ToString());
                Button_Textcolor = Color.FromHex(App.Current.Resources["Button_Textcolor"].ToString());

                Label_Textcolor = Color.FromHex(App.Current.Resources["Label_Textcolor"].ToString());

                Grouping_Textcolor = Color.FromHex(App.Current.Resources["Grouping_Textcolor"].ToString());

                Refresh_Color = Color.FromHex(App.Current.Resources["Refresh_Color"].ToString());

                Edit_Backgroundcolor = Color.FromHex(App.Current.Resources["Edit_Backgroundcolor"].ToString());
                Edit_Bordercolor = Color.FromHex(App.Current.Resources["Edit_Bordercolor"].ToString());
                Edit_Textcolor = Color.FromHex(App.Current.Resources["Edit_Textcolor"].ToString());

                Remove_Backgroundcolor = Color.FromHex(App.Current.Resources["Remove_Backgroundcolor"].ToString());
                Remove_Bordercolor = Color.FromHex(App.Current.Resources["Remove_Bordercolor"].ToString());
                Remove_Textcolor = Color.FromHex(App.Current.Resources["Remove_Textcolor"].ToString());

                Delete_Backgroundcolor = Color.FromHex(App.Current.Resources["Delete_Backgroundcolor"].ToString());
                Delete_Bordercolor = Color.FromHex(App.Current.Resources["Delete_Bordercolor"].ToString());
                Delete_Textcolor = Color.FromHex(App.Current.Resources["Delete_Textcolor"].ToString());

                Revive_Backgroundcolor = Color.FromHex(App.Current.Resources["Revive_Backgroundcolor"].ToString());
                Revive_Bordercolor = Color.FromHex(App.Current.Resources["Revive_Bordercolor"].ToString());
                Revive_Textcolor = Color.FromHex(App.Current.Resources["Revive_Textcolor"].ToString());

                Order_Backgroundcolor = Color.FromHex(App.Current.Resources["Order_Backgroundcolor"].ToString());
                Order_Bordercolor = Color.FromHex(App.Current.Resources["Order_Bordercolor"].ToString());
                Order_Textcolor = Color.FromHex(App.Current.Resources["Order_Textcolor"].ToString());

                Saldo_Backgroundcolor = Color.FromHex(App.Current.Resources["Saldo_Backgroundcolor"].ToString());
                Saldo_Textcolor = Color.FromHex(App.Current.Resources["Saldo_Textcolor"].ToString());

                Flyout_Backgroundcolor = Color.FromHex(App.Current.Resources["Flyout_Backgroundcolor"].ToString());
                Flyout_Selectcolor = Color.FromHex(App.Current.Resources["Flyout_Selectcolor"].ToString());
                Flyout_Textcolor = Color.FromHex(App.Current.Resources["Flyout_Textcolor"].ToString());
                Flyout_Iconcolor = Color.FromHex(App.Current.Resources["Flyout_Iconcolor"].ToString());
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Transaktion_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
               if (Transaktion_Backgroundcolor_String == null)
                {
                    Transaktion_Backgroundcolor = Defauft_Transaktion_Backgroundcolor;
                }
                else
                {
                    Transaktion_Backgroundcolor = Color_Dictionary[Transaktion_Backgroundcolor_String];
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Transaktion_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if(Transaktion_Bordercolor_String == null)
                {
                    Transaktion_Bordercolor = Defauft_Transaktion_Bordercolor;
                }
                else
                {
                    Transaktion_Bordercolor = Color_Dictionary[Transaktion_Bordercolor_String];
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Transaktion_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Transaktion_Textcolor_String == null)
                {
                    Transaktion_Textcolor = Defauft_Transaktion_Textcolor;
                }
                else
                {
                    Transaktion_Textcolor = Color_Dictionary[Transaktion_Textcolor_String];
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void EntryFrame_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (EntryFrame_Backgroundcolor_String == null)
                {
                    EntryFrame_Backgroundcolor = Defauft_EntryFrame_Backgroundcolor;
                }
                else
                {
                    EntryFrame_Backgroundcolor = Color_Dictionary[EntryFrame_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void EntryFrame_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (EntryFrame_Bordercolor_String == null)
                {
                    EntryFrame_Bordercolor = Defauft_EntryFrame_Bordercolor;
                }
                else
                {
                    EntryFrame_Bordercolor = Color_Dictionary[EntryFrame_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void EntryFrame_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (EntryFrame_Textcolor_String == null)
                {
                    EntryFrame_Textcolor = Defauft_EntryFrame_Textcolor;
                }
                else
                {
                    EntryFrame_Textcolor = Color_Dictionary[EntryFrame_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Button_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Button_Backgroundcolor_String == null)
                {
                    Button_Backgroundcolor = Defauft_Button_Backgroundcolor;
                }
                else
                {
                    Button_Backgroundcolor = Color_Dictionary[Button_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Button_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Button_Bordercolor_String == null)
                {
                    Button_Bordercolor = Defauft_Button_Bordercolor;
                }
                else
                {
                    Button_Bordercolor = Color_Dictionary[Button_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Button_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Button_Textcolor_String == null)
                {
                    Button_Textcolor = Defauft_Button_Textcolor;
                }
                else
                {
                    Button_Textcolor = Color_Dictionary[Button_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Edit_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Edit_Backgroundcolor_String == null)
                {
                    Edit_Backgroundcolor = Defauft_Edit_Backgroundcolor;
                }
                else
                {
                    Edit_Backgroundcolor = Color_Dictionary[Edit_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Edit_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Edit_Bordercolor_String == null)
                {
                    Edit_Bordercolor = Defauft_Edit_Bordercolor;
                }
                else
                {
                    Edit_Bordercolor = Color_Dictionary[Edit_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Edit_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Edit_Textcolor_String == null)
                {
                    Edit_Textcolor = Defauft_Edit_Textcolor;
                }
                else
                {
                    Edit_Textcolor = Color_Dictionary[Edit_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Remove_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Remove_Backgroundcolor_String == null)
                {
                    Remove_Backgroundcolor = Defauft_Remove_Backgroundcolor;
                }
                else
                {
                    Remove_Backgroundcolor = Color_Dictionary[Remove_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Remove_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Remove_Bordercolor_String == null)
                {
                    Remove_Bordercolor = Defauft_Remove_Bordercolor;
                }
                else
                {
                    Remove_Bordercolor = Color_Dictionary[Remove_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Remove_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Remove_Textcolor_String == null)
                {
                    Remove_Textcolor = Defauft_Remove_Textcolor;
                }
                else
                {
                    Remove_Textcolor = Color_Dictionary[Remove_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Revive_Backgroundcolor_Changed_Changed_Methode(object obj)
        {
            try
            {
                if (Revive_Backgroundcolor_String == null)
                {
                    Revive_Backgroundcolor = Defauft_Revive_Backgroundcolor;
                }
                else
                {
                    Revive_Backgroundcolor = Color_Dictionary[Revive_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Revive_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Revive_Bordercolor_String == null)
                {
                    Revive_Bordercolor = Defauft_Revive_Bordercolor;
                }
                else
                {
                    Revive_Bordercolor = Color_Dictionary[Revive_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Revive_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Revive_Textcolor_String == null)
                {
                    Revive_Textcolor = Defauft_Revive_Textcolor;
                }
                else
                {
                    Revive_Textcolor = Color_Dictionary[Revive_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Delete_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Delete_Backgroundcolor_String == null)
                {
                    Delete_Backgroundcolor = Defauft_Delete_Backgroundcolor;
                }
                else
                {
                    Delete_Backgroundcolor = Color_Dictionary[Delete_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Delete_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Delete_Bordercolor_String == null)
                {
                    Delete_Bordercolor = Defauft_Delete_Bordercolor;
                }
                else
                {
                    Delete_Bordercolor = Color_Dictionary[Delete_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Delete_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Delete_Textcolor_String == null)
                {
                    Delete_Textcolor = Defauft_Delete_Textcolor;
                }
                else
                {
                    Delete_Textcolor = Color_Dictionary[Delete_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Order_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Order_Backgroundcolor_String == null)
                {
                    Order_Backgroundcolor = Defauft_Order_Backgroundcolor;
                }
                else
                {
                    Order_Backgroundcolor = Color_Dictionary[Order_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Order_Bordercolor_Changed_Methode(object obj)
        {
            try
            {
                if (Order_Bordercolor_String == null)
                {
                    Order_Bordercolor = Defauft_Order_Bordercolor;
                }
                else
                {
                    Order_Bordercolor = Color_Dictionary[Order_Bordercolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Order_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Order_Textcolor_String == null)
                {
                    Order_Textcolor = Defauft_Order_Textcolor;
                }
                else
                {
                    Order_Textcolor = Color_Dictionary[Order_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Saldo_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Saldo_Backgroundcolor_String == null)
                {
                    Saldo_Backgroundcolor = Defauft_Saldo_Backgroundcolor;
                }
                else
                {
                    Saldo_Backgroundcolor = Color_Dictionary[Saldo_Backgroundcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Saldo_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Saldo_Textcolor_String == null)
                {
                    Saldo_Textcolor = Defauft_Saldo_Textcolor;
                }
                else
                {
                    Saldo_Textcolor = Color_Dictionary[Saldo_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Label_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Label_Textcolor_String == null)
                {
                    Label_Textcolor = Defauft_Label_Textcolor;
                }
                else
                {
                    Label_Textcolor = Color_Dictionary[Label_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Grouping_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Grouping_Textcolor_String == null)
                {
                    Grouping_Textcolor = Defauft_Grouping_Textcolor;
                }
                else
                {
                    Grouping_Textcolor = Color_Dictionary[Grouping_Textcolor_String];

                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Refresh_Color_Changed_Methode(object obj)
        {
            try
            {
                if (Refresh_Color_String == null)
                {
                    Refresh_Color = Defauft_Refresh_Color;
                }
                else
                {
                    Refresh_Color = Color_Dictionary[Refresh_Color_String];

                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Flyout_Backgroundcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Flyout_Backgroundcolor_String == null)
                {
                    Flyout_Backgroundcolor = Defauft_Flyout_Backgroundcolor;
                }
                else
                {
                    Flyout_Backgroundcolor = Color_Dictionary[Flyout_Backgroundcolor_String];

                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Flyout_Selectcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Flyout_Selectcolor_String == null)
                {
                    Flyout_Selectcolor = Defauft_Flyout_Selectcolor;
                }
                else
                {
                    Flyout_Selectcolor = Color_Dictionary[Flyout_Selectcolor_String];

                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Flyout_Textcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Flyout_Textcolor_String == null)
                {
                    Flyout_Textcolor = Defauft_Flyout_Textcolor;
                }
                else
                {
                    Flyout_Textcolor = Color_Dictionary[Flyout_Textcolor_String];

                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Flyout_Iconcolor_Changed_Methode(object obj)
        {
            try
            {
                if (Flyout_Iconcolor_String == null)
                {
                    Flyout_Iconcolor = Defauft_Flyout_Iconcolor;
                }
                else
                {
                    Flyout_Iconcolor = Color_Dictionary[Flyout_Iconcolor_String];
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void App_BackgroundColor_Changed_Methode()
        {
            try
            {
                if (App_BackgroundColor_String == null)
                {
                    App_Backgroundcolor = Defauft_App_Backgroundcolor;
                }
                else
                {
                    App_Backgroundcolor = Color_Dictionary[App_BackgroundColor_String];
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async void Page_BackgroundColor_Changed_Methode()
        {
            try
            {
                if (Page_BackgroundColor_String == null)
                {
                    Page_Backgroundcolor = Defauft_Page_Backgroundcolor;
                }
                else
                {
                    Page_Backgroundcolor = Color_Dictionary[Page_BackgroundColor_String];
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Default_MehtodeAsync()
        {
            try
            {
                App.Current.Resources["App_Backgroundcolor"] = Defauft_App_Backgroundcolor.ToHex();
                App.Current.Resources["Page_Backgroundcolor"] = Defauft_Page_Backgroundcolor.ToHex();
                App.Current.Resources["Transaktion_Backgroundcolor"] = Defauft_Transaktion_Backgroundcolor.ToHex();
                App.Current.Resources["Transaktion_Bordercolor"] = Defauft_Transaktion_Bordercolor.ToHex();
                App.Current.Resources["Transaktion_Textcolor"] = Defauft_Transaktion_Textcolor.ToHex();
                App.Current.Resources["EntryFrame_Backgroundcolor"] = Defauft_EntryFrame_Backgroundcolor.ToHex();
                App.Current.Resources["EntryFrame_Bordercolor"] = Defauft_EntryFrame_Bordercolor.ToHex();
                App.Current.Resources["EntryFrame_Textcolor"] = Defauft_EntryFrame_Textcolor.ToHex();
                App.Current.Resources["Button_Backgroundcolor"] = Defauft_Button_Backgroundcolor.ToHex();
                App.Current.Resources["Button_Bordercolor"] = Defauft_Button_Bordercolor.ToHex();
                App.Current.Resources["Button_Textcolor"] = Defauft_Button_Textcolor.ToHex();
                App.Current.Resources["Edit_Backgroundcolor"] = Defauft_Edit_Backgroundcolor.ToHex();
                App.Current.Resources["Edit_Bordercolor"] = Defauft_Edit_Bordercolor.ToHex();
                App.Current.Resources["Edit_Textcolor"] = Defauft_Edit_Textcolor.ToHex();
                App.Current.Resources["Remove_Backgroundcolor"] = Defauft_Remove_Backgroundcolor.ToHex();
                App.Current.Resources["Remove_Bordercolor"] = Defauft_Remove_Bordercolor.ToHex();
                App.Current.Resources["Remove_Textcolor"] = Defauft_Remove_Textcolor.ToHex();
                App.Current.Resources["Revive_Backgroundcolor"] = Defauft_Revive_Backgroundcolor.ToHex();
                App.Current.Resources["Revive_Bordercolor"] = Defauft_Revive_Bordercolor.ToHex();
                App.Current.Resources["Revive_Textcolor"] = Defauft_Revive_Textcolor.ToHex();
                App.Current.Resources["Delete_Backgroundcolor"] = Defauft_Delete_Backgroundcolor.ToHex();
                App.Current.Resources["Delete_Bordercolor"] = Defauft_Delete_Bordercolor.ToHex();
                App.Current.Resources["Delete_Textcolor"] = Defauft_Delete_Textcolor.ToHex();
                App.Current.Resources["Order_Backgroundcolor"] = Defauft_Order_Backgroundcolor.ToHex();
                App.Current.Resources["Order_Bordercolor"] = Defauft_Order_Bordercolor.ToHex();
                App.Current.Resources["Order_Textcolor"] = Defauft_Order_Textcolor.ToHex();
                App.Current.Resources["Saldo_Backgroundcolor"] = Defauft_Saldo_Backgroundcolor.ToHex();
                App.Current.Resources["Saldo_Textcolor"] = Defauft_Saldo_Textcolor.ToHex();
                App.Current.Resources["Label_Textcolor"] = Defauft_Label_Textcolor.ToHex();
                App.Current.Resources["Refresh_Color"] = Defauft_Refresh_Color.ToHex();
                App.Current.Resources["Grouping_Textcolor"] = Defauft_Grouping_Textcolor.ToHex();
                App.Current.Resources["Flyout_Backgroundcolor"] = Defauft_Flyout_Backgroundcolor.ToHex();
                App.Current.Resources["Flyout_Selectcolor"] = Defauft_Flyout_Selectcolor.ToHex();
                App.Current.Resources["Flyout_Textcolor"] = Defauft_Flyout_Textcolor.ToHex();
                App.Current.Resources["Flyout_Iconcolor"] = Defauft_Flyout_Iconcolor.ToHex();

                App_Backgroundcolor = Color.FromHex(App.Current.Resources["App_Backgroundcolor"].ToString());

                Page_Backgroundcolor = Color.FromHex(App.Current.Resources["Page_Backgroundcolor"].ToString());

                Transaktion_Backgroundcolor = Color.FromHex(App.Current.Resources["Transaktion_Backgroundcolor"].ToString());
                Transaktion_Bordercolor = Color.FromHex(App.Current.Resources["Transaktion_Bordercolor"].ToString());
                Transaktion_Textcolor = Color.FromHex(App.Current.Resources["Transaktion_Textcolor"].ToString());

                EntryFrame_Backgroundcolor = Color.FromHex(App.Current.Resources["EntryFrame_Backgroundcolor"].ToString());
                EntryFrame_Bordercolor = Color.FromHex(App.Current.Resources["EntryFrame_Bordercolor"].ToString());
                EntryFrame_Textcolor = Color.FromHex(App.Current.Resources["EntryFrame_Textcolor"].ToString());

                Button_Backgroundcolor = Color.FromHex(App.Current.Resources["Button_Backgroundcolor"].ToString());
                Button_Bordercolor = Color.FromHex(App.Current.Resources["Button_Bordercolor"].ToString());
                Button_Textcolor = Color.FromHex(App.Current.Resources["Button_Textcolor"].ToString());

                Label_Textcolor = Color.FromHex(App.Current.Resources["Label_Textcolor"].ToString());

                Grouping_Textcolor = Color.FromHex(App.Current.Resources["Grouping_Textcolor"].ToString());

                Refresh_Color = Color.FromHex(App.Current.Resources["Refresh_Color"].ToString());

                Edit_Backgroundcolor = Color.FromHex(App.Current.Resources["Edit_Backgroundcolor"].ToString());
                Edit_Bordercolor = Color.FromHex(App.Current.Resources["Edit_Bordercolor"].ToString());
                Edit_Textcolor = Color.FromHex(App.Current.Resources["Edit_Textcolor"].ToString());

                Remove_Backgroundcolor = Color.FromHex(App.Current.Resources["Remove_Backgroundcolor"].ToString());
                Remove_Bordercolor = Color.FromHex(App.Current.Resources["Remove_Bordercolor"].ToString());
                Remove_Textcolor = Color.FromHex(App.Current.Resources["Remove_Textcolor"].ToString());

                Delete_Backgroundcolor = Color.FromHex(App.Current.Resources["Delete_Backgroundcolor"].ToString());
                Delete_Bordercolor = Color.FromHex(App.Current.Resources["Delete_Bordercolor"].ToString());
                Delete_Textcolor = Color.FromHex(App.Current.Resources["Delete_Textcolor"].ToString());

                Revive_Backgroundcolor = Color.FromHex(App.Current.Resources["Revive_Backgroundcolor"].ToString());
                Revive_Bordercolor = Color.FromHex(App.Current.Resources["Revive_Bordercolor"].ToString());
                Revive_Textcolor = Color.FromHex(App.Current.Resources["Revive_Textcolor"].ToString());

                Order_Backgroundcolor = Color.FromHex(App.Current.Resources["Order_Backgroundcolor"].ToString());
                Order_Bordercolor = Color.FromHex(App.Current.Resources["Order_Bordercolor"].ToString());
                Order_Textcolor = Color.FromHex(App.Current.Resources["Order_Textcolor"].ToString());

                Saldo_Backgroundcolor = Color.FromHex(App.Current.Resources["Saldo_Backgroundcolor"].ToString());
                Saldo_Textcolor = Color.FromHex(App.Current.Resources["Saldo_Textcolor"].ToString());

                Flyout_Backgroundcolor = Color.FromHex(App.Current.Resources["Flyout_Backgroundcolor"].ToString());
                Flyout_Selectcolor = Color.FromHex(App.Current.Resources["Flyout_Selectcolor"].ToString());
                Flyout_Textcolor = Color.FromHex(App.Current.Resources["Flyout_Textcolor"].ToString());
                Flyout_Iconcolor = Color.FromHex(App.Current.Resources["Flyout_Iconcolor"].ToString());

                Apply_Methode();
            }
            catch 
            {
                await Shell.Current.DisplayAlert("Error", "Ein Fehler ist aufgetaucht.", "Okay");
            }
        }

        private void Apply_Methode()
        {
            try
            {
                App.Current.Resources["App_Backgroundcolor"] = App_Backgroundcolor.ToHex();
                Preferences.Set("App_Backgroundcolor", App_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["App_Backgroundcolor"] = Defauft_App_Backgroundcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Page_Backgroundcolor"] = Page_Backgroundcolor.ToHex();
                Preferences.Set("Page_Backgroundcolor", Page_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Page_Backgroundcolor"] = Defauft_Page_Backgroundcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Transaktion_Backgroundcolor"] = Transaktion_Backgroundcolor.ToHex();
                Preferences.Set("Transaktion_Backgroundcolor", Transaktion_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Transaktion_Backgroundcolor"] = Defauft_Transaktion_Backgroundcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Transaktion_Bordercolor"] = Transaktion_Bordercolor.ToHex();
                Preferences.Set("Transaktion_Bordercolor", Transaktion_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Transaktion_Bordercolor"] = Defauft_Transaktion_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Transaktion_Textcolor"] = Transaktion_Textcolor.ToHex();
                Preferences.Set("Transaktion_Textcolor", Transaktion_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Transaktion_Textcolor"] = Defauft_Transaktion_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["EntryFrame_Backgroundcolor"] = EntryFrame_Backgroundcolor.ToHex();
                Preferences.Set("EntryFrame_Backgroundcolor", EntryFrame_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["EntryFrame_Backgroundcolor"] = Defauft_EntryFrame_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["EntryFrame_Bordercolor"] = EntryFrame_Bordercolor.ToHex();
                Preferences.Set("EntryFrame_Bordercolor", EntryFrame_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["EntryFrame_Bordercolor"] = Defauft_EntryFrame_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["EntryFrame_Textcolor"] = EntryFrame_Textcolor.ToHex();
                Preferences.Set("EntryFrame_Textcolor", EntryFrame_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["EntryFrame_Textcolor"] = Defauft_EntryFrame_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Button_Backgroundcolor"] = Button_Backgroundcolor.ToHex();
                Preferences.Set("Button_Backgroundcolor", Button_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Button_Backgroundcolor"] = Defauft_Button_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Button_Bordercolor"] = Button_Bordercolor.ToHex();
                Preferences.Set("Button_Bordercolor", Button_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Button_Bordercolor"] = Defauft_Button_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Button_Textcolor"] = Button_Textcolor.ToHex();
                Preferences.Set("Button_Textcolor", Button_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Button_Textcolor"] = Defauft_Button_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Edit_Backgroundcolor"] = Edit_Backgroundcolor.ToHex();
                Preferences.Set("Edit_Backgroundcolor", Edit_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Edit_Backgroundcolor"] = Defauft_Edit_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Edit_Bordercolor"] = Edit_Bordercolor.ToHex();
                Preferences.Set("Edit_Bordercolor", Edit_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Edit_Bordercolor"] = Defauft_Edit_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Edit_Textcolor"] = Edit_Textcolor.ToHex();
                Preferences.Set("Edit_Textcolor", Edit_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Edit_Textcolor"] = Defauft_Edit_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Remove_Backgroundcolor"] = Remove_Backgroundcolor.ToHex();
                Preferences.Set("Remove_Backgroundcolor", Remove_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Remove_Backgroundcolor"] = Defauft_Remove_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Remove_Bordercolor"] = Remove_Bordercolor.ToHex();
                Preferences.Set("Remove_Bordercolor", Remove_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Remove_Bordercolor"] = Defauft_Remove_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Remove_Textcolor"] = Remove_Textcolor.ToHex();
                Preferences.Set("Remove_Textcolor", Remove_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Remove_Textcolor"] = Defauft_Remove_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Revive_Backgroundcolor"] = Revive_Backgroundcolor.ToHex();
                Preferences.Set("Revive_Backgroundcolor", Revive_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Revive_Backgroundcolor"] = Defauft_Revive_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Revive_Bordercolor"] = Revive_Bordercolor.ToHex();
                Preferences.Set("Revive_Bordercolor", Revive_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Revive_Bordercolor"] = Defauft_Revive_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Revive_Textcolor"] = Revive_Textcolor.ToHex();
                Preferences.Set("Revive_Textcolor", Revive_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Revive_Textcolor"] = Defauft_Revive_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Delete_Backgroundcolor"] = Delete_Backgroundcolor.ToHex();
                Preferences.Set("Delete_Backgroundcolor", Delete_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Delete_Backgroundcolor"] = Defauft_Delete_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Delete_Bordercolor"] = Delete_Bordercolor.ToHex();
                Preferences.Set("Delete_Bordercolor", Delete_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Delete_Bordercolor"] = Defauft_Delete_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Delete_Textcolor"] = Delete_Textcolor.ToHex();
                Preferences.Set("Delete_Textcolor", Delete_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Delete_Textcolor"] = Defauft_Delete_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Order_Backgroundcolor"] = Order_Backgroundcolor.ToHex();
                Preferences.Set("Order_Backgroundcolor", Order_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Order_Backgroundcolor"] = Defauft_Order_Backgroundcolor.ToHex();
            }
            try
            {
                App.Current.Resources["Order_Bordercolor"] = Order_Bordercolor.ToHex();
                Preferences.Set("Order_Bordercolor", Order_Bordercolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Order_Bordercolor"] = Defauft_Order_Bordercolor.ToHex();
            }

            try
            {
                App.Current.Resources["Order_Textcolor"] = Order_Textcolor.ToHex();
                Preferences.Set("Order_Textcolor", Order_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Order_Textcolor"] = Defauft_Order_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Saldo_Backgroundcolor"] = Saldo_Backgroundcolor.ToHex();
                Preferences.Set("Saldo_Backgroundcolor", Saldo_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Saldo_Backgroundcolor"] = Defauft_Saldo_Backgroundcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Saldo_Textcolor"] = Saldo_Textcolor.ToHex();
                Preferences.Set("Saldo_Textcolor", Saldo_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Saldo_Textcolor"] = Defauft_Saldo_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Label_Textcolor"] = Label_Textcolor.ToHex();
                Preferences.Set("Label_Textcolor", Label_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Label_Textcolor"] = Defauft_Label_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Grouping_Textcolor"] = Grouping_Textcolor.ToHex();
                Preferences.Set("Grouping_Textcolor", Grouping_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Grouping_Textcolor"] = Defauft_Grouping_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Refresh_Color"] = Refresh_Color.ToHex();
                Preferences.Set("Refresh_Color", Refresh_Color.ToHex());
            }
            catch
            {
                App.Current.Resources["Refresh_Color"] = Defauft_Refresh_Color.ToHex();
            }

            try
            {
                App.Current.Resources["Flyout_Backgroundcolor"] = Flyout_Backgroundcolor.ToHex();
                Preferences.Set("Flyout_Backgroundcolor", Flyout_Backgroundcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Flyout_Backgroundcolor"] = Defauft_Flyout_Backgroundcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Flyout_Selectcolor"] = Flyout_Selectcolor.ToHex();
                Preferences.Set("Flyout_Selectcolor", Flyout_Selectcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Flyout_Selectcolor"] = Defauft_Flyout_Selectcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Flyout_Textcolor"] = Flyout_Textcolor.ToHex();
                Preferences.Set("Flyout_Textcolor", Flyout_Textcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Flyout_Textcolor"] = Defauft_Flyout_Textcolor.ToHex();
            }

            try
            {
                App.Current.Resources["Flyout_Iconcolor"] = Flyout_Iconcolor.ToHex();
                Preferences.Set("Flyout_Iconcolor", Flyout_Iconcolor.ToHex());
            }
            catch
            {
                App.Current.Resources["Flyout_Iconcolor"] = Defauft_Flyout_Iconcolor.ToHex();
            }
        }

        public async Task Create_DictonaryAsync()
        {
            try
            {
                Color_Dictionary.Add("Alice Blue", Color.AliceBlue);
                Color_Dictionary.Add("Antique White ", Color.AntiqueWhite);
                Color_Dictionary.Add("Aqua", Color.Aqua);
                Color_Dictionary.Add("Aquamarine", Color.Aquamarine);
                Color_Dictionary.Add("Azure", Color.Azure);
                Color_Dictionary.Add("Beige", Color.Beige);
                Color_Dictionary.Add("Bisque", Color.Bisque);
                Color_Dictionary.Add("Black", Color.Black);
                Color_Dictionary.Add("Blanched Almond", Color.BlanchedAlmond);
                Color_Dictionary.Add("Blue", Color.Blue);
                Color_Dictionary.Add("Blue Violet", Color.BlueViolet);
                Color_Dictionary.Add("Brown", Color.Brown);
                Color_Dictionary.Add("Burly Wood", Color.BurlyWood);
                Color_Dictionary.Add("Cadet blue", Color.CadetBlue);
                Color_Dictionary.Add("Chartreuse", Color.Chartreuse);
                Color_Dictionary.Add("Chocolate", Color.Chocolate);
                Color_Dictionary.Add("Coral", Color.Coral);
                Color_Dictionary.Add("Cornflower Blue", Color.CornflowerBlue);
                Color_Dictionary.Add("Cornsilk", Color.Cornsilk);
                Color_Dictionary.Add("Crimson", Color.Crimson);
                Color_Dictionary.Add("Cyan", Color.Cyan);
                Color_Dictionary.Add("Dark blue", Color.DarkBlue);
                Color_Dictionary.Add("Dark cyan", Color.DarkCyan);
                Color_Dictionary.Add("Dark golden dod", Color.DarkGoldenrod);
                Color_Dictionary.Add("Dark gray", Color.DarkGray);
                Color_Dictionary.Add("Dark green", Color.DarkGreen);
                Color_Dictionary.Add("Dark khaki", Color.DarkKhaki);
                Color_Dictionary.Add("Dark magenta", Color.DarkMagenta);
                Color_Dictionary.Add("Dark olive green", Color.DarkOliveGreen);
                Color_Dictionary.Add("Dark orange", Color.DarkOrange);
                Color_Dictionary.Add("Dark orchid", Color.DarkOrchid);
                Color_Dictionary.Add("Dark red", Color.DarkRed);
                Color_Dictionary.Add("Dark salmon", Color.DarkSalmon);
                Color_Dictionary.Add("Dark sea green", Color.DarkSeaGreen);
                Color_Dictionary.Add("Dark slate blue", Color.DarkSlateBlue);
                Color_Dictionary.Add("Dark slate gray", Color.DarkSlateGray);
                Color_Dictionary.Add("Dark turquoise", Color.DarkTurquoise);
                Color_Dictionary.Add("Dark violet", Color.DarkViolet);
                Color_Dictionary.Add("Deep pink", Color.DeepPink);
                Color_Dictionary.Add("Deep sky blue", Color.DeepSkyBlue);
                Color_Dictionary.Add("Dim gray", Color.DimGray);
                Color_Dictionary.Add("Dodger blue", Color.DodgerBlue);
                Color_Dictionary.Add("Firebrick", Color.Firebrick);
                Color_Dictionary.Add("Floral white", Color.FloralWhite);
                Color_Dictionary.Add("Forest green", Color.ForestGreen);
                Color_Dictionary.Add("Fuchsia", Color.Fuchsia);
                Color_Dictionary.Add("Gainsboro", Color.Gainsboro);
                Color_Dictionary.Add("Ghost white", Color.GhostWhite);
                Color_Dictionary.Add("Gold", Color.Gold);
                Color_Dictionary.Add("Golden rod", Color.Goldenrod);
                Color_Dictionary.Add("Gray", Color.Gray);
                Color_Dictionary.Add("Green", Color.Green);
                Color_Dictionary.Add("Greenyellow", Color.GreenYellow);
                Color_Dictionary.Add("Honeydew", Color.Honeydew);
                Color_Dictionary.Add("Hot pink", Color.HotPink);
                Color_Dictionary.Add("Indian red", Color.IndianRed);
                Color_Dictionary.Add("Indigo", Color.Indigo);
                Color_Dictionary.Add("Ivory", Color.Ivory);
                Color_Dictionary.Add("Khaki", Color.Khaki);
                Color_Dictionary.Add("Lavender", Color.Lavender);
                Color_Dictionary.Add("Lavender blush", Color.LavenderBlush);
                Color_Dictionary.Add("Lawn green", Color.LawnGreen);
                Color_Dictionary.Add("Lemon chiffon", Color.LemonChiffon);
                Color_Dictionary.Add("Light blue", Color.LightBlue);
                Color_Dictionary.Add("Light coral", Color.LightCoral);
                Color_Dictionary.Add("Light cyan", Color.LightCyan);
                Color_Dictionary.Add("Light golden rod yellow", Color.LightGoldenrodYellow);
                Color_Dictionary.Add("Light gray", Color.LightGray);
                Color_Dictionary.Add("Light green", Color.LightGreen);
                Color_Dictionary.Add("Light pink", Color.LightPink);
                Color_Dictionary.Add("Light salmon", Color.LightSalmon);
                Color_Dictionary.Add("Light sea green", Color.LightSeaGreen);
                Color_Dictionary.Add("Light sky blue", Color.LightSkyBlue);
                Color_Dictionary.Add("Light slate gray", Color.LightSlateGray);
                Color_Dictionary.Add("Light steel blue", Color.LightSteelBlue);
                Color_Dictionary.Add("Light yellow", Color.LightYellow);
                Color_Dictionary.Add("Lime", Color.Lime);
                Color_Dictionary.Add("Lime green", Color.LimeGreen);
                Color_Dictionary.Add("Linen", Color.Linen);
                Color_Dictionary.Add("Magenta", Color.Magenta);
                Color_Dictionary.Add("Maroon", Color.Maroon);
                Color_Dictionary.Add("Medium aquamarine", Color.MediumAquamarine);
                Color_Dictionary.Add("Medium blue", Color.MediumBlue);
                Color_Dictionary.Add("Medium orchid", Color.MediumOrchid);
                Color_Dictionary.Add("Medium purple", Color.MediumPurple);
                Color_Dictionary.Add("Medium sea green", Color.MediumSeaGreen);
                Color_Dictionary.Add("Medium slate blue", Color.MediumSlateBlue);
                Color_Dictionary.Add("Medium spring green", Color.MediumSpringGreen);
                Color_Dictionary.Add("Medium turquoise", Color.MediumTurquoise);
                Color_Dictionary.Add("Medium violet red", Color.MediumVioletRed);
                Color_Dictionary.Add("Midnight blue", Color.MidnightBlue);
                Color_Dictionary.Add("Mint cream", Color.MintCream);
                Color_Dictionary.Add("Misty rose", Color.MistyRose);
                Color_Dictionary.Add("Moccasin", Color.Moccasin);
                Color_Dictionary.Add("Navajo white", Color.NavajoWhite);
                Color_Dictionary.Add("Navy", Color.Navy);
                Color_Dictionary.Add("Old lace", Color.OldLace);
                Color_Dictionary.Add("Olive", Color.Olive);
                Color_Dictionary.Add("Olive drab", Color.OliveDrab);
                Color_Dictionary.Add("Orange", Color.Orange);
                Color_Dictionary.Add("Orange red", Color.OrangeRed);
                Color_Dictionary.Add("Orchid", Color.Orchid);
                Color_Dictionary.Add("Pale golden rod", Color.PaleGoldenrod);
                Color_Dictionary.Add("Pale green", Color.PaleGreen);
                Color_Dictionary.Add("Pale turquoise", Color.PaleTurquoise);
                Color_Dictionary.Add("Paleviolet red", Color.PaleVioletRed);
                Color_Dictionary.Add("Papaya whip", Color.PapayaWhip);
                Color_Dictionary.Add("Peach puff", Color.PeachPuff);
                Color_Dictionary.Add("Peru", Color.Peru);
                Color_Dictionary.Add("Pink", Color.Pink);
                Color_Dictionary.Add("Plum", Color.Plum);
                Color_Dictionary.Add("Powder blue", Color.PowderBlue);
                Color_Dictionary.Add("Purple", Color.Purple);
                Color_Dictionary.Add("Red", Color.Red);
                Color_Dictionary.Add("Rosy brown", Color.RosyBrown);
                Color_Dictionary.Add("Royal blue", Color.RoyalBlue);
                Color_Dictionary.Add("Saddle brown", Color.SaddleBrown);
                Color_Dictionary.Add("Salmon", Color.Salmon);
                Color_Dictionary.Add("Sandy brown", Color.SandyBrown);
                Color_Dictionary.Add("Sea green", Color.SeaGreen);
                Color_Dictionary.Add("Sea shell", Color.SeaShell);
                Color_Dictionary.Add("Sienna", Color.Sienna);
                Color_Dictionary.Add("Silver", Color.Silver);
                Color_Dictionary.Add("Sky blue", Color.SkyBlue);
                Color_Dictionary.Add("Slate blue", Color.SlateBlue);
                Color_Dictionary.Add("Slate gray", Color.SlateGray);
                Color_Dictionary.Add("Snow", Color.Snow);
                Color_Dictionary.Add("Spring green", Color.SpringGreen);
                Color_Dictionary.Add("Steel blue", Color.SteelBlue);
                Color_Dictionary.Add("Tan", Color.Tan);
                Color_Dictionary.Add("Teal", Color.Teal);
                Color_Dictionary.Add("Thistle", Color.Thistle);
                Color_Dictionary.Add("Tomato", Color.Tomato);
                Color_Dictionary.Add("Transparent", Color.Transparent);
                Color_Dictionary.Add("Turquoise", Color.Turquoise);
                Color_Dictionary.Add("Violet", Color.Violet);
                Color_Dictionary.Add("Wheat", Color.Wheat);
                Color_Dictionary.Add("White", Color.White);
                Color_Dictionary.Add("White smoke", Color.WhiteSmoke);
                Color_Dictionary.Add("Yellow", Color.Yellow);
                Color_Dictionary.Add("Yellow green", Color.YellowGreen);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }
        private async Task Notificater(string v)
        {
            IsNotificater_Visibel = true;

            Notificater_Text = v;

            await Task.Delay(5000);

            IsNotificater_Visibel = false;
        }


        public Xamarin.Forms.Color Defauft_Transaktion_Backgroundcolor = Xamarin.Forms.Color.Gray;

        public string transaktion_Backgroundcolor_String;
        public string Transaktion_Backgroundcolor_String
        {
            get { return transaktion_Backgroundcolor_String; }
            set
            {
                if (Transaktion_Backgroundcolor_String == value)
                    return;
                transaktion_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color transaktion_Backgroundcolor;
        public Color Transaktion_Backgroundcolor
        {
            get { return transaktion_Backgroundcolor; }
            set
            {
                if (Transaktion_Backgroundcolor == value)
                    return;
                transaktion_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Transaktion_Bordercolor = Xamarin.Forms.Color.DarkGray;

        public string transaktion_Bordercolor_String;
        public string Transaktion_Bordercolor_String
        {
            get { return transaktion_Bordercolor_String; }
            set
            {
                if (Transaktion_Bordercolor_String == value)
                    return;
                transaktion_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color transaktion_Bordercolor;
        public Color Transaktion_Bordercolor
        {
            get { return transaktion_Bordercolor; }
            set
            {
                if (Transaktion_Bordercolor == value)
                    return;
                transaktion_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Transaktion_Textcolor = Xamarin.Forms.Color.Black;

        public string transaktion_Textcolor_String;
        public string Transaktion_Textcolor_String
        {
            get { return transaktion_Textcolor_String; }
            set
            {
                if (Transaktion_Textcolor_String == value)
                    return;
                transaktion_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color transaktion_Textcolor;
        public Color Transaktion_Textcolor
        {
            get { return transaktion_Textcolor; }
            set
            {
                if (Transaktion_Textcolor == value)
                    return;
                transaktion_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_EntryFrame_Backgroundcolor = Xamarin.Forms.Color.Orange;

        public string entryFrame_Backgroundcolor_String;
        public string EntryFrame_Backgroundcolor_String
        {
            get { return entryFrame_Backgroundcolor_String; }
            set
            {
                if (EntryFrame_Backgroundcolor_String == value)
                    return;
                entryFrame_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color entryFrame_Backgroundcolor;
        public Color EntryFrame_Backgroundcolor
        {
            get { return entryFrame_Backgroundcolor; }
            set
            {
                if (EntryFrame_Backgroundcolor == value)
                    return;
                entryFrame_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_EntryFrame_Bordercolor = Xamarin.Forms.Color.Coral;

        public string entryFrame_Bordercolor_String;
        public string EntryFrame_Bordercolor_String
        {
            get { return entryFrame_Bordercolor_String; }
            set
            {
                if (EntryFrame_Bordercolor_String == value)
                    return;
                entryFrame_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color entryFrame_Bordercolor;
        public Color EntryFrame_Bordercolor
        {
            get { return entryFrame_Bordercolor; }
            set
            {
                if (EntryFrame_Bordercolor == value)
                    return;
                entryFrame_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_EntryFrame_Textcolor = Xamarin.Forms.Color.Black;

        public string entryFrame_Textcolor_String;
        public string EntryFrame_Textcolor_String
        {
            get { return entryFrame_Textcolor_String; }
            set
            {
                if (EntryFrame_Textcolor_String == value)
                    return;
                entryFrame_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color entryFrame_Textcolor;
        public Color EntryFrame_Textcolor
        {
            get { return entryFrame_Textcolor; }
            set
            {
                if (EntryFrame_Textcolor == value)
                    return;
                entryFrame_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Button_Backgroundcolor = Xamarin.Forms.Color.SkyBlue;

        public string button_Backgroundcolor_String;
        public string Button_Backgroundcolor_String
        {
            get { return button_Backgroundcolor_String; }
            set
            {
                if (Button_Backgroundcolor_String == value)
                    return;
                button_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color button_Backgroundcolor;
        public Color Button_Backgroundcolor
        {
            get { return button_Backgroundcolor; }
            set
            {
                if (Button_Backgroundcolor == value)
                    return;
                button_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Button_Bordercolor = Xamarin.Forms.Color.Blue;

        public string button_Bordercolor_String;
        public string Button_Bordercolor_String
        {
            get { return button_Bordercolor_String; }
            set
            {
                if (Button_Bordercolor_String == value)
                    return;
                button_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color button_Bordercolor;
        public Color Button_Bordercolor
        {
            get { return button_Bordercolor; }
            set
            {
                if (Button_Bordercolor == value)
                    return;
                button_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Button_Textcolor = Xamarin.Forms.Color.OrangeRed;

        public string button_Textcolor_String;
        public string Button_Textcolor_String
        {
            get { return button_Textcolor_String; }
            set
            {
                if (Button_Textcolor_String == value)
                    return;
                button_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color button_Textcolor;
        public Color Button_Textcolor
        {
            get { return button_Textcolor; }
            set
            {
                if (Button_Textcolor == value)
                    return;
                button_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Edit_Backgroundcolor = Xamarin.Forms.Color.Green;

        public string edit_Backgroundcolor_String;
        public string Edit_Backgroundcolor_String
        {
            get { return edit_Backgroundcolor_String; }
            set
            {
                if (Edit_Backgroundcolor_String == value)
                    return;
                edit_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color edit_Backgroundcolor;
        public Color Edit_Backgroundcolor
        {
            get { return edit_Backgroundcolor; }
            set
            {
                if (Edit_Backgroundcolor == value)
                    return;
                edit_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Edit_Bordercolor = Xamarin.Forms.Color.DarkGreen;

        public string edit_Bordercolor_String;
        public string Edit_Bordercolor_String
        {
            get { return edit_Bordercolor_String; }
            set
            {
                if (Edit_Bordercolor_String == value)
                    return;
                edit_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color edit_Bordercolor;
        public Color Edit_Bordercolor
        {
            get { return edit_Bordercolor; }
            set
            {
                if (Edit_Bordercolor == value)
                    return;
                edit_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Edit_Textcolor = Xamarin.Forms.Color.White;

        public string edit_Textcolor_String;
        public string Edit_Textcolor_String
        {
            get { return edit_Textcolor_String; }
            set
            {
                if (Edit_Textcolor_String == value)
                    return;
                edit_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color edit_Textcolor;
        public Color Edit_Textcolor
        {
            get { return edit_Textcolor; }
            set
            {
                if (Edit_Textcolor == value)
                    return;
                edit_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Remove_Backgroundcolor = Xamarin.Forms.Color.Red;

        public string remove_Backgroundcolor_String;
        public string Remove_Backgroundcolor_String
        {
            get { return remove_Backgroundcolor_String; }
            set
            {
                if (Remove_Backgroundcolor_String == value)
                    return;
                remove_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color remove_Backgroundcolor;
        public Color Remove_Backgroundcolor
        {
            get { return remove_Backgroundcolor; }
            set
            {
                if (Remove_Backgroundcolor == value)
                    return;
                remove_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Remove_Bordercolor = Xamarin.Forms.Color.DarkRed;

        public string remove_Bordercolor_String;
        public string Remove_Bordercolor_String
        {
            get { return remove_Bordercolor_String; }
            set
            {
                if (Remove_Bordercolor_String == value)
                    return;
                remove_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color remove_Bordercolor;
        public Color Remove_Bordercolor
        {
            get { return remove_Bordercolor; }
            set
            {
                if (Remove_Bordercolor == value)
                    return;
                remove_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Remove_Textcolor = Xamarin.Forms.Color.White;

        public string remove_Textcolor_String;
        public string Remove_Textcolor_String
        {
            get { return remove_Textcolor_String; }
            set
            {
                if (Remove_Textcolor_String == value)
                    return;
                remove_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color remove_Textcolor;
        public Color Remove_Textcolor
        {
            get { return remove_Textcolor; }
            set
            {
                if (Remove_Textcolor == value)
                    return;
                remove_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Revive_Backgroundcolor = Xamarin.Forms.Color.Blue;

        public string revive_Backgroundcolor_String;
        public string Revive_Backgroundcolor_String
        {
            get { return revive_Backgroundcolor_String; }
            set
            {
                if (Revive_Backgroundcolor_String == value)
                    return;
                revive_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color revive_Backgroundcolor;
        public Color Revive_Backgroundcolor
        {
            get { return revive_Backgroundcolor; }
            set
            {
                if (Revive_Backgroundcolor == value)
                    return;
                revive_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Revive_Bordercolor = Xamarin.Forms.Color.DarkBlue;

        public string revive_Bordercolor_String;
        public string Revive_Bordercolor_String
        {
            get { return revive_Bordercolor_String; }
            set
            {
                if (Revive_Bordercolor_String == value)
                    return;
                revive_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color revive_Bordercolor;
        public Color Revive_Bordercolor
        {
            get { return revive_Bordercolor; }
            set
            {
                if (Revive_Bordercolor == value)
                    return;
                revive_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Revive_Textcolor = Xamarin.Forms.Color.White;

        public string revive_Textcolor_String;
        public string Revive_Textcolor_String
        {
            get { return revive_Textcolor_String; }
            set
            {
                if (Revive_Textcolor_String == value)
                    return;
                revive_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color revive_Textcolor;
        public Color Revive_Textcolor
        {
            get { return revive_Textcolor; }
            set
            {
                if (Revive_Textcolor == value)
                    return;
                revive_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Delete_Backgroundcolor = Xamarin.Forms.Color.Red;

        public string delete_Backgroundcolor_String;
        public string Delete_Backgroundcolor_String
        {
            get { return delete_Backgroundcolor_String; }
            set
            {
                if (Delete_Backgroundcolor_String == value)
                    return;
                delete_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color delete_Backgroundcolor;
        public Color Delete_Backgroundcolor
        {
            get { return delete_Backgroundcolor; }
            set
            {
                if (Delete_Backgroundcolor == value)
                    return;
                delete_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Delete_Bordercolor = Xamarin.Forms.Color.DarkRed;

        public string delete_Bordercolor_String;
        public string Delete_Bordercolor_String
        {
            get { return delete_Bordercolor_String; }
            set
            {
                if (Delete_Bordercolor_String == value)
                    return;
                delete_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color delete_Bordercolor;
        public Color Delete_Bordercolor
        {
            get { return delete_Bordercolor; }
            set
            {
                if (Delete_Bordercolor == value)
                    return;
                delete_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Delete_Textcolor = Xamarin.Forms.Color.White;

        public string delete_Textcolor_String;
        public string Delete_Textcolor_String
        {
            get { return delete_Textcolor_String; }
            set
            {
                if (Delete_Textcolor_String == value)
                    return;
                delete_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color delete_Textcolor;
        public Color Delete_Textcolor
        {
            get { return delete_Textcolor; }
            set
            {
                if (Delete_Textcolor == value)
                    return;
                delete_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Order_Backgroundcolor = Xamarin.Forms.Color.Blue;

        public string order_Backgroundcolor_String;
        public string Order_Backgroundcolor_String
        {
            get { return order_Backgroundcolor_String; }
            set
            {
                if (Order_Backgroundcolor_String == value)
                    return;
                order_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color order_Backgroundcolor;
        public Color Order_Backgroundcolor
        {
            get { return order_Backgroundcolor; }
            set
            {
                if (Order_Backgroundcolor == value)
                    return;
                order_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Order_Bordercolor = Xamarin.Forms.Color.DarkBlue;

        public string order_Bordercolor_String;
        public string Order_Bordercolor_String
        {
            get { return order_Bordercolor_String; }
            set
            {
                if (Order_Bordercolor_String == value)
                    return;
                order_Bordercolor_String = value; RaisePropertyChanged();
            }
        }

        public Color order_Bordercolor;
        public Color Order_Bordercolor
        {
            get { return order_Bordercolor; }
            set
            {
                if (Order_Bordercolor == value)
                    return;
                order_Bordercolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Order_Textcolor = Xamarin.Forms.Color.White;

        public string order_Textcolor_String;
        public string Order_Textcolor_String
        {
            get { return order_Textcolor_String; }
            set
            {
                if (Order_Textcolor_String == value)
                    return;
                order_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color order_Textcolor;
        public Color Order_Textcolor
        {
            get { return order_Textcolor; }
            set
            {
                if (Order_Textcolor == value)
                    return;
                order_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Saldo_Backgroundcolor = Xamarin.Forms.Color.Black;

        public string saldo_Backgroundcolor_String;
        public string Saldo_Backgroundcolor_String
        {
            get { return saldo_Backgroundcolor_String; }
            set
            {
                if (Saldo_Backgroundcolor_String == value)
                    return;
                saldo_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color saldo_Backgroundcolor;
        public Color Saldo_Backgroundcolor
        {
            get { return saldo_Backgroundcolor; }
            set
            {
                if (Saldo_Backgroundcolor == value)
                    return;
                saldo_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Saldo_Textcolor = Xamarin.Forms.Color.White;

        public string saldo_Textcolor_String;
        public string Saldo_Textcolor_String
        {
            get { return saldo_Textcolor_String; }
            set
            {
                if (Saldo_Textcolor_String == value)
                    return;
                saldo_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color saldo_Textcolor;
        public Color Saldo_Textcolor
        {
            get { return saldo_Textcolor; }
            set
            {
                if (Saldo_Textcolor == value)
                    return;
                saldo_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Label_Textcolor = Xamarin.Forms.Color.White;

        public string label_Textcolor_String;
        public string Label_Textcolor_String
        {
            get { return label_Textcolor_String; }
            set
            {
                if (Label_Textcolor_String == value)
                    return;
                label_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color label_Textcolor;
        public Color Label_Textcolor
        {
            get { return label_Textcolor; }
            set
            {
                if (Label_Textcolor == value)
                    return;
                label_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Grouping_Textcolor = Xamarin.Forms.Color.White;

        public string grouping_Textcolor_String;
        public string Grouping_Textcolor_String
        {
            get { return grouping_Textcolor_String; }
            set
            {
                if (Grouping_Textcolor_String == value)
                    return;
                grouping_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color grouping_Textcolor;
        public Color Grouping_Textcolor
        {
            get { return grouping_Textcolor; }
            set
            {
                if (Grouping_Textcolor == value)
                    return;
                grouping_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Refresh_Color = Xamarin.Forms.Color.Blue;

        public string refresh_Color_String;
        public string Refresh_Color_String
        {
            get { return refresh_Color_String; }
            set
            {
                if (Refresh_Color_String == value)
                    return;
                refresh_Color_String = value; RaisePropertyChanged();
            }
        }

        public Color refresh_Color;
        public Color Refresh_Color
        {
            get { return refresh_Color; }
            set
            {
                if (Refresh_Color == value)
                    return;
                refresh_Color = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Flyout_Backgroundcolor = Xamarin.Forms.Color.DarkGray;

        public string flyout_Backgroundcolor_String;
        public string Flyout_Backgroundcolor_String
        {
            get { return flyout_Backgroundcolor_String; }
            set
            {
                if (Flyout_Backgroundcolor_String == value)
                    return;
                flyout_Backgroundcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color flyout_Backgroundcolor;
        public Color Flyout_Backgroundcolor
        {
            get { return flyout_Backgroundcolor; }
            set
            {
                if (Flyout_Backgroundcolor == value)
                    return;
                flyout_Backgroundcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Flyout_Selectcolor = Xamarin.Forms.Color.LightGray;

        public string flyout_Selectcolor_String;
        public string Flyout_Selectcolor_String
        {
            get { return flyout_Selectcolor_String; }
            set
            {
                if (Flyout_Selectcolor_String == value)
                    return;
                flyout_Selectcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color flyout_Selectcolor;
        public Color Flyout_Selectcolor
        {
            get { return flyout_Selectcolor; }
            set
            {
                if (Flyout_Selectcolor == value)
                    return;
                flyout_Selectcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Flyout_Textcolor = Xamarin.Forms.Color.Black;

        public string flyout_Textcolor_String;
        public string Flyout_Textcolor_String
        {
            get { return flyout_Textcolor_String; }
            set
            {
                if (Flyout_Textcolor_String == value)
                    return;
                flyout_Textcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color flyout_Textcolor;
        public Color Flyout_Textcolor
        {
            get { return flyout_Textcolor; }
            set
            {
                if (Flyout_Textcolor == value)
                    return;
                flyout_Textcolor = value; RaisePropertyChanged();
            }
        }


        public Xamarin.Forms.Color Defauft_Flyout_Iconcolor = Xamarin.Forms.Color.Black;

        public string flyout_Iconcolor_String;
        public string Flyout_Iconcolor_String
        {
            get { return flyout_Iconcolor_String; }
            set
            {
                if (Flyout_Iconcolor_String == value)
                    return;
                flyout_Iconcolor_String = value; RaisePropertyChanged();
            }
        }

        public Color flyout_Iconcolor;
        public Color Flyout_Iconcolor
        {
            get { return flyout_Iconcolor; }
            set
            {
                if (Flyout_Iconcolor == value)
                    return;
                flyout_Iconcolor = value; RaisePropertyChanged();
            }
        }


        public bool isnotificater_visibel = false;
        public bool IsNotificater_Visibel
        {
            get { return isnotificater_visibel; }
            set
            {
                if (IsNotificater_Visibel == value)
                    return;
                isnotificater_visibel = value; RaisePropertyChanged();
            }
        }

        string notificater_text;
        public string Notificater_Text
        {
            get { return notificater_text; }
            set
            {
                if (Notificater_Text == value)
                    return;
                notificater_text = value; RaisePropertyChanged();
            }
        }

        public Xamarin.Forms.Color Defauft_Page_Backgroundcolor = Xamarin.Forms.Color.DarkSlateGray;

        public Xamarin.Forms.Color Defauft_App_Backgroundcolor = Xamarin.Forms.Color.FromHex("#1d0e21");

        public string page_backgroundcolor_string;
        public string Page_BackgroundColor_String
        {
            get { return page_backgroundcolor_string; }
            set
            {
                if (Page_BackgroundColor_String == value)
                    return;
                page_backgroundcolor_string = value; RaisePropertyChanged();
            }
        }

        public string app_backgroundcolor_string;
        public string App_BackgroundColor_String
        {
            get { return app_backgroundcolor_string; }
            set
            {
                if (App_BackgroundColor_String == value)
                    return;
                app_backgroundcolor_string = value; RaisePropertyChanged();
            }
        }

        public Color app_backgroundColor;
        public Color App_Backgroundcolor
        {
            get { return app_backgroundColor; }
            set
            {
                if (App_Backgroundcolor == value)
                    return;
                app_backgroundColor = value; RaisePropertyChanged();
            }
        }

        public Color page_backgroundColor;
        public Color Page_Backgroundcolor
        {
            get { return page_backgroundColor; }
            set
            {
                if (Page_Backgroundcolor == value)
                    return;
                page_backgroundColor = value; RaisePropertyChanged();
            }
        }

        public List<string> Color_List { get; set; }

        public Dictionary<string, Color> Color_Dictionary = new Dictionary<string, Color>();

        public Command Apply_Command { get; }
        public AsyncCommand Default_Command { get; }

        public AsyncCommand View_IsAppering_Command { get; }
        public Command Page_BackgroundColor_Changed_Command { get; }
        public Command App_BackgroundColor_Changed_Command { get; }
        public Command Transaktion_Backgroundcolor_Changed_Command { get; }
        public Command Transaktion_Bordercolor_Changed_Command { get; }
        public Command Transaktion_Textcolor_Changed_Command { get; }
        public Command EntryFrame_Backgroundcolor_Changed_Command { get; }
        public Command EntryFrame_Bordercolor_Changed_Command { get; }
        public Command EntryFrame_Textcolor_Changed_Command { get; }
        public Command Button_Backgroundcolor_Changed_Command { get; }
        public Command Button_Bordercolor_Changed_Command { get; }
        public Command Button_Textcolor_Changed_Command { get; }
        public Command Edit_Backgroundcolor_Changed_Command { get; }
        public Command Edit_Bordercolor_Changed_Command { get; }
        public Command Edit_Textcolor_Changed_Command { get; }
        public Command Remove_Backgroundcolor_Changed_Command { get; }
        public Command Remove_Bordercolor_Changed_Command { get; }
        public Command Remove_Textcolor_Changed_Command { get; }
        public Command Revive_Backgroundcolor_Changed_Command { get; }
        public Command Revive_Bordercolor_Changed_Command { get; }
        public Command Revive_Textcolor_Changed_Command { get; }
        public Command Delete_Backgroundcolor_Changed_Command { get; }
        public Command Delete_Bordercolor_Changed_Command { get; }
        public Command Delete_Textcolor_Changed_Command { get; }
        public Command Order_Backgroundcolor_Changed_Command { get; }
        public Command Order_Bordercolor_Changed_Command { get; }
        public Command Order_Textcolor_Changed_Command { get; }
        public Command Saldo_Backgroundcolor_Changed_Command { get; }
        public Command Saldo_Textcolor_Changed_Command { get; }
        public Command Label_Textcolor_Changed_Command { get; }
        public Command Grouping_Textcolor_Changed_Command { get; }
        public Command Refresh_Color_Changed_Command { get; }
        public Command Flyout_Backgroundcolor_Changed_Command { get; }
        public Command Flyout_Selectcolor_Changed_Command { get; }
        public Command Flyout_Textcolor_Changed_Command { get; }
        public Command Flyout_Iconcolor_Changed_Command { get; }
    }
}
