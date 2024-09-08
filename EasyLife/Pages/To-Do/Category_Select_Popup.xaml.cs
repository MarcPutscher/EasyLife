using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.PageModels.To_Do
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Category_Select_Popup : Popup
	{
		public Category_Select_Popup ()
		{
			InitializeComponent ();
		}
	}
}