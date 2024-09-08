using EasyLife.Models;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Color = System.Drawing.Color;

namespace EasyLife.Pages.To_Do
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Add_Category_Popup : Popup
	{
		public Add_Category_Popup ()
		{
			InitializeComponent ();
            this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 160);
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);
        }

        private async void Appeared(object sender , EventArgs e)
        {
            List<Color> colors = GetAllColors();
            Random random = new Random();
            Color color = colors.ElementAtOrDefault<Color>(random.Next(0, colors.Count));

            foreach (Category ca in await CategoryService.Get_all_Categorys())
            {
                if (ca.Color == new ColorToHexRgbaStringConverter().ConvertFrom(color))
                {
                    color = colors.ElementAtOrDefault<Color>(random.Next(0, colors.Count));
                }
            }

            icon.TextColor = color;
        }
        private void Abord_Clicked(object sender, EventArgs e)
        {
            Dismiss(null);
        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(category_entry.Text) == false)
            {

                Category category = new Category() { Title = category_entry.Text, Color = icon.TextColor.ToHex() , Count = 0};

                int result = await CategoryService.Add_Category(category);

                if ((int)result == 2)
                {
                    return;
                }
                if((int)result == 0)
                {
                    Dismiss(await CategoryService.Get_specific_Category(category.Title));

                }
                if ((int)result == 1)
                {
                    return;
                }
            }
        }
        private void category_entry_Changed(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(category_entry.Text) == false)
            {
                save_button.IsEnabled = true;
            }
            else
            {
                save_button.IsEnabled = false;
            }
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            List<Color> colors = GetAllColors();
            Random random = new Random();
            Color color = colors.ElementAtOrDefault<Color>(random.Next(0, colors.Count));

            foreach (Category ca in await CategoryService.Get_all_Categorys())
            {
                if (ca.Color == new ColorToHexRgbaStringConverter().ConvertFrom(color))
                {
                    color = colors.ElementAtOrDefault<Color>(random.Next(0, colors.Count));
                }
            }

            icon.TextColor = color;
        }
        private List<Color> GetAllColors()
        {
            List<Color> allColors = new List<Color>();

            foreach (PropertyInfo property in typeof(Color).GetProperties())
            {
                if (property.PropertyType == typeof(Color))
                {
                    allColors.Add((Color)property.GetValue(null));
                }
            }

            return allColors;
        }
    }
}