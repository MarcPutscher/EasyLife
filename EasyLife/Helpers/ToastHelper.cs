using Android.Content;
using Android.Views;
using Android.Widget;
using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyLife.Helpers
{
    /// <summary>
    /// Helferklasse für eine Benachrichtigung die angezeigt werden soll.
    /// </summary>
    public class ToastHelper
    {
        /// <summary>
        /// Zeigt eine Benachrichtigung auf der aktuellen Seite.
        /// </summary>
        /// <param name="input">Der Text der in der Benachrichtigung angezeigt werden soll.</param>
        /// <returns></returns>
        public static async Task ShowToast(string input) 
        {
            await Shell.Current.DisplayToastAsync(input, 5000);
        }

        public static async Task Show_To_Do_Toast(string input)
        {
            var toast = Toast.MakeText(Android.App.Application.Context,input,0);
            toast.SetGravity(GravityFlags.Center, 0,(int)Math.Round(DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density*0.95,0));
            toast.Show();
            //ToastOptions toastOptions = new ToastOptions()
            //{
            //    BackgroundColor = Color.Gray,
            //    CornerRadius = 10,
            //    MessageOptions = new MessageOptions()
            //    { Message = input, Padding = 5 },
            //    Duration = new TimeSpan(0, 0, 3)

            //};

            //await Shell.Current.DisplayToastAsync(toastOptions);
        }

    }
}
