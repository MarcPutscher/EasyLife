using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
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

    }
}
