using EasyLife.PageModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master_Page : Shell
    {
        public Master_Page()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Home_Page), typeof(Home_Page));
            Routing.RegisterRoute(nameof(Add_Item_Page), typeof(Add_Item_Page));
            Routing.RegisterRoute(nameof(Viewtime_Popup), typeof(Viewtime_Popup));
            Routing.RegisterRoute(nameof(Settings_Page), typeof(Settings_Page));
            Routing.RegisterRoute(nameof(Trashcan_Page), typeof(Trashcan_Page));
            Routing.RegisterRoute(nameof(Edit_Item_Page), typeof(Edit_Item_Page));
            Routing.RegisterRoute(nameof(Edit_Item_With_Order_Page), typeof(Edit_Item_With_Order_Page));
            Routing.RegisterRoute(nameof(Edit_Order_Page), typeof(Edit_Order_Page));
            Routing.RegisterRoute(nameof(Add_Order_Page), typeof(Add_Order_Page));
            Routing.RegisterRoute(nameof(Balance_Page), typeof(Balance_Page));
            Routing.RegisterRoute(nameof(Styling_Color_Page), typeof(Styling_Color_Page));
            Routing.RegisterRoute(nameof(Compare_Page), typeof(Compare_Page));
        }
    }
}