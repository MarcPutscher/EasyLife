using EasyLife.Models;
using EasyLife.Pages.To_Do;
using EasyLife.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.PageModels.To_Do
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Category_Select_Popup : Popup
	{
        public Category_Select_Popup(List<Models.Category> categories, int categoryid)
        {
            Categories.AddRange(categories);
            InitializeComponent();

            if (Categories.Count > 3)
            {
                this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 250);
            }
            else
            {
                this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 150 + Categories.Count * 50);
            }
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);

            Categories.Find(c => c.Id == categoryid).Is_Select = true;
            category_list.ItemsSource = Categories;
        }
		
        public Category_Select_Popup(List<Models.Category> categories)
        {
            Categories.AddRange(categories);
            InitializeComponent();

            if (Categories.Count > 3)
            {
                this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 250);
            }
            else
            {
                this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 150 + Categories.Count * 50);
            }
            this.VerticalOptions = new LayoutOptions(LayoutAlignment.End, true);

            category_list.ItemsSource = Categories;
        }


        private async void Category_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is ListView && sender != null)
                {
                    Dismiss(category_list.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void New_Tapped(object sender, EventArgs e)
        {
            try
            {
                Category result = await Shell.Current.ShowPopupAsync(new Add_Category_Popup()) as Category;

                if (result != null)
                {
                    category_list.ItemsSource = null;
                    Categories.Add(result);
                    if (Categories.Count > 3)
                    {
                        this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 250);
                    }
                    else
                    {
                        this.Size = new Xamarin.Forms.Size(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density, 150 + Categories.Count * 50);
                    }
                    category_list.ItemsSource = Categories;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }
        private async void Abord_Clicked(object sender, EventArgs e)
        {
            try
            {
                Dismiss((Category)null);
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public List<Category> Categories = new List<Category>();
    }
}