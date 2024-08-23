using EasyLife.Helpers;
using EasyLife.Models;
using EasyLife.Services;
using iText.Layout.Borders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
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
    public partial class CustomeAlert_Popup : Popup
    {
        public List<Alert_Notification> Alert_List = new List<Alert_Notification>();

        /// <summary>
        /// Ist ein Popup was eine Nachricht anzeigt.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="height">Ist die Höhe die das Popup benötigt</param>
        /// <param name="accept">Akzeptiert</param>
        /// <param name="refuse">Verweigert</param>
        /// <param name="notification">Ist eine Nachricht die angezeigt wird.</param>
        public CustomeAlert_Popup(string title, double width, double height, string accept, string refuse, string notification)
        {
            if (String.IsNullOrWhiteSpace(notification) == false)
            {
                Alert_List.Add(new Alert_Notification() { Message = notification, NotificationVisibilit = true });
            }

            Alert_Notification buttons = new Alert_Notification() { NotificationVisibilit = false };

            if (String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.AcceptVisibility = false;
            }
            else
            {
                buttons.AcceptVisibility = true;

                buttons.AcceptTitle = accept;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true)
            {
                buttons.RefuseVisibility = false;
            }
            else
            {
                buttons.RefuseVisibility = true;

                buttons.RefuseTitle = refuse;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true && String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.ButtonVisibilit = false;
            }
            else
            {
                buttons.ButtonVisibilit = true;
            }

            Alert_List.Add(buttons);

            InitializeComponent();

            Title.Text = title;

            if (Alert_List.Count() == 0)
            {
                AlertList.IsVisible = false;
            }
            else
            {
                AlertList.IsVisible = true;

                AlertList.ItemsSource = Alert_List;
            }

            if (height == 0)
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

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

            Alert_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        /// <summary>
        /// Ist ein Popup was eine Nachricht anzeigt.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="height">Ist die Höhe die das Popup benötigt</param>
        /// <param name="accept">Akzeptiert</param>
        /// <param name="refuse">Verweigert</param>
        /// <param name="notification">Ist eine Nachricht mit Subnachricht die angezeigt wird.</param>
        public CustomeAlert_Popup(string title, double width, double height, string accept, string refuse, string[] notification)
        {
            if (notification != null)
            {
                Alert_List.Add(new Alert_Notification() { Message = notification[0], NotificationVisibilit = true, Submessage = notification[1], SubmessageVisibilit = true });
            }

            Alert_Notification buttons = new Alert_Notification() { NotificationVisibilit = false };

            if (String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.AcceptVisibility = false;
            }
            else
            {
                buttons.AcceptVisibility = true;

                buttons.AcceptTitle = accept;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true)
            {
                buttons.RefuseVisibility = false;
            }
            else
            {
                buttons.RefuseVisibility = true;

                buttons.RefuseTitle = refuse;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true && String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.ButtonVisibilit = false;
            }
            else
            {
                buttons.ButtonVisibilit = true;
            }

            Alert_List.Add(buttons);

            InitializeComponent();

            Title.Text = title;

            if (Alert_List.Count() == 0)
            {
                AlertList.IsVisible = false;
            }
            else
            {
                AlertList.IsVisible = true;

                AlertList.ItemsSource = Alert_List;
            }

            if (height == 0)
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

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

            Alert_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        /// <summary>
        /// Ist ein Popup was eine Liste an Nachrichten anzeigt.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="height">Ist die Höhe die das Popup benötigt</param>
        /// <param name="accept">Akzeptiert</param>
        /// <param name="refuse">Verweigert</param>
        /// <param name="notification_list">Ist eine Liste an Nachricht die angezeigt wird.</param>
        public CustomeAlert_Popup(string title, double width, double height, string accept, string refuse, List<string> notification_list)
        {
            if (notification_list != null)
            {
                if (notification_list.Count() != 0)
                {
                    foreach (var notification in notification_list)
                    {
                        Alert_List.Add(new Alert_Notification() { Message = notification, NotificationVisibilit = true });
                    }
                }
            }

            Alert_Notification buttons = new Alert_Notification() { NotificationVisibilit = false };

            if (String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.AcceptVisibility = false;
            }
            else
            {
                buttons.AcceptVisibility = true;

                buttons.AcceptTitle = accept;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true)
            {
                buttons.RefuseVisibility = false;
            }
            else
            {
                buttons.RefuseVisibility = true;

                buttons.RefuseTitle = refuse;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true && String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.ButtonVisibilit = false;
            }
            else
            {
                buttons.ButtonVisibilit = true;
            }

            Alert_List.Add(buttons);

            InitializeComponent();

            Title.Text = title;

            if (Alert_List.Count() == 0)
            {
                AlertList.IsVisible = false;
            }
            else
            {
                AlertList.IsVisible = true;

                AlertList.ItemsSource = Alert_List;
            }

            if (height == 0)
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

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

            Alert_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        /// <summary>
        /// Ist ein Popup was eine Liste an Nachrichten anzeigt.
        /// </summary>
        /// <param name="title">Ist der Name des Popups und wird neben dem Abbrechenbutton und oberhalb der Optionen angezeigt.</param>
        /// <param name="width">Ist die Breite die das Popup benötigt</param>
        /// <param name="height">Ist die Höhe die das Popup benötigt</param>
        /// <param name="accept">Akzeptiert</param>
        /// <param name="refuse">Verweigert</param>
        /// <param name="notification_list">Ist eine Liste an Nachricht die angezeigt wird.</param>
        public CustomeAlert_Popup(string title, double width, double height, string accept, string refuse, List<string[]> notification_list)
        {
            if (notification_list != null)
            {
                if (notification_list.Count() != 0)
                {
                    foreach (var notification in notification_list)
                    {
                        Alert_List.Add(new Alert_Notification() { Message = notification[0], NotificationVisibilit = true, Submessage = notification[1], SubmessageVisibilit = true });
                    }
                }
            }

            Alert_Notification buttons = new Alert_Notification() { NotificationVisibilit = false };

            if (String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.AcceptVisibility = false;
            }
            else
            {
                buttons.AcceptVisibility = true;

                buttons.AcceptTitle = accept;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true)
            {
                buttons.RefuseVisibility = false;
            }
            else
            {
                buttons.RefuseVisibility = true;

                buttons.RefuseTitle = refuse;
            }

            if (String.IsNullOrWhiteSpace(refuse) == true && String.IsNullOrWhiteSpace(accept) == true)
            {
                buttons.ButtonVisibilit = false;
            }
            else
            {
                buttons.ButtonVisibilit = true;
            }

            Alert_List.Add(buttons);

            InitializeComponent();

            Title.Text = title;

            if (Alert_List.Count() == 0)
            {
                AlertList.IsVisible = false;
            }
            else
            {
                AlertList.IsVisible = true;

                AlertList.ItemsSource = Alert_List;
            }

            if (height == 0)
            {
                height = 0.9 * (DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density);
            }

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

            Alert_Popup.Size = new Size(width, height);

            Title.WidthRequest = width - 75;
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void AcceptButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }

        private void RefuseButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(false);
        }
    }

    public class Alert_Notification
    {
        public string Message { get; set; }

        public string Submessage { get; set; }

        public bool NotificationVisibilit { get; set; }

        public bool SubmessageVisibilit { get; set; }

        public bool ButtonVisibilit { get; set; }

        public bool AcceptVisibility { get; set; }

        public bool RefuseVisibility { get; set; }

        public string AcceptTitle { get; set; }

        public string RefuseTitle { get; set; }
    }
}