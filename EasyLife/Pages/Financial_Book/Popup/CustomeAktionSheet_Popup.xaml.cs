using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using iText.Layout.Borders;
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
    public partial class CustomeAktionSheet_Popup : Popup
    {
        public List<Action_Button> ActionButtonList = new List<Action_Button>();

        /// <summary>
        /// Ist ein Popup was eine Liste an Optionen anzeigt die wenn man sie triggert ihren Titel wiedergeben sonst wird null zurückgegeben.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="button_list">Ist eine Liste an Optionen die in dem Popup untereinander angezeigt wird.</param>
        public CustomeAktionSheet_Popup(string title, double width, List<string> button_list)
        {
            if (button_list != null)
            {
                if (button_list.Count() != 0)
                {
                    foreach (string button in button_list)
                    {
                        ActionButtonList.Add(new Action_Button() { Title = button, Description = null, Description_Visibility = false });
                    }
                }
            }

            InitializeComponent();

            Title.Text = title;

            if (ActionButtonList.Count() == 0)
            {
                ButtonList.IsVisible = false;
            }
            else
            {
                ButtonList.IsVisible = true;

                ButtonList.ItemsSource = ActionButtonList;
            }

            double height = 70 * ActionButtonList.Count() + 110;

            if (height > 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density))
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width == 0)
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width > 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density))
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            ActionSheet_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        /// <summary>
        /// Ist ein Popup was eine Liste an Optionen anzeigt die wenn man sie triggert ihren Titel wiedergeben sonst wird null zurückgegeben.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="button_with_description_list">Ist eine Liste an Optionen mit Beschreibung die in dem Popup untereinander angezeigt wird.</param>
        public CustomeAktionSheet_Popup(string title, double width, List<string[]> button_with_description_list)
        {
            if (button_with_description_list != null)
            {
                if (button_with_description_list.Count() != 0)
                {
                    foreach (string[] button in button_with_description_list)
                    {
                        if (button.Count() == 2)
                        {
                            ActionButtonList.Add(new Action_Button() { Title = button[0], Description = button[1], Description_Visibility = true });
                        }

                        if (button.Count() == 1)
                        {
                            ActionButtonList.Add(new Action_Button() { Title = button[0], Description = null, Description_Visibility = true });
                        }
                    }
                }
            }

            InitializeComponent();

            Title.Text = title;

            if (ActionButtonList.Count() == 0)
            {
                ButtonList.IsVisible = false;
            }
            else
            {
                ButtonList.IsVisible = true;

                ButtonList.ItemsSource = ActionButtonList;
            }

            double height = 85 * ActionButtonList.Count() + 110;

            if (height > 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density))
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width == 0)
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            if (width > 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density))
            {
                width = 0.9 * (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density);
            }

            ActionSheet_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void ButtonList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;

            Action_Button button = listview.SelectedItem as Action_Button;

            if (button != null)
            {
                Dismiss(button.Title);
            }
        }
    }


}