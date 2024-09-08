using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.To_Do
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Change_Color_Popup : Popup
	{
		public Change_Color_Popup ()
		{
			InitializeComponent ();
		}
	}
}