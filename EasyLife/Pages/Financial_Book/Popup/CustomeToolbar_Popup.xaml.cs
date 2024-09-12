using EasyLife.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomeToolbar_Popup : Popup
	{
		public CustomeToolbar_Popup (List<Action_Button> toolbarItems, double width)
		{
			InitializeComponent ();

			toolbaritemlist.ItemsSource = toolbarItems;

            this.Size = new Size(width, 50*toolbarItems.Count+50);
		}

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listview = sender as ListView;

            Action_Button button = listview.SelectedItem as Action_Button;

            if (button != null)
            {
                Dismiss(button);
            }
        }
    }
}