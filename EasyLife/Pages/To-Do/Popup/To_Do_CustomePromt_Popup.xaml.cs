using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.To_Do
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class To_Do_CustomePromt_Popup : Popup
    {
        public To_Do_Item To_Do = null;
        public List<To_Do_Item> To_Dos = new List<To_Do_Item>();
        public bool Option2;
        public To_Do_CustomePromt_Popup(To_Do_Item to_Do_Item, String message, bool is_option2)
        {
            To_Do = to_Do_Item;
            Option2 = is_option2;
            InitializeComponent();
            promt_lable.Text = message;
            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 95);
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
        }
        public To_Do_CustomePromt_Popup(List<To_Do_Item> to_Do_Items, String message,bool is_option2)
        {
            To_Dos = to_Do_Items;
            Option2 = is_option2;
            InitializeComponent();
            promt_lable.Text = message;
            this.Size = new Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 95);
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
        }

        private async void Abord_Clicked(object sender, EventArgs e)
        {
            try
            {
                Dismiss(null);
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Delet_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(Option2 == false)
                {
                    if (To_Do == null)
                    {
                        if (To_Dos.Count != 0)
                        {
                            foreach (To_Do_Item item in To_Dos)
                            {
                                item.Is_Select = false;
                                item.Is_Removed = true;
                                await To_DoService.Edit_To_Do(item);
                            }
                        }

                    }
                    else
                    {
                        To_Do.Is_Select = false;
                        To_Do.Is_Removed = true;
                        await To_DoService.Edit_To_Do(To_Do);
                    }
                }
                else
                {
                    if (To_Do == null)
                    {
                        if (To_Dos.Count != 0)
                        {
                            foreach (To_Do_Item item in To_Dos)
                            {
                                item.Is_Select = false;
                                await To_DoService.Remove_To_Do(item);
                            }
                        }

                    }
                    else
                    {
                        To_Do.Is_Select = false;
                        await To_DoService.Remove_To_Do(To_Do);
                    }
                }

                Dismiss("Deleted");
            }
            catch (Exception ex)
            {       
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
                Dismiss(null);
            }
        }
    }
}