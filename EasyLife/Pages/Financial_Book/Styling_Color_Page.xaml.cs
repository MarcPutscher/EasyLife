using EasyLife.Cells;
using EasyLife.Cells.Styling;
using EasyLife.CustomeEventArgs;
using EasyLife.Helpers;
using EasyLife.PageModels;
using EasyLife.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.BehaviorsPack;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Styling_Color_Page : ContentPage
    {
        public Styling_Color_PageModel Styling_Color_PageModel => ((Styling_Color_PageModel)BindingContext);
        public Styling_Color_Page()
        {
            List<StylingCell> myCarousel = new List<StylingCell>();

            InitializeComponent();

            Styling_Color_PageModel.OnMessageRaised += OnStylingMessageRaised;

            myCarousel.Add(new StylingCell { Cellname = "Home_View", Items = HomeView.Home_View_Item_List, Title = "Home" });

            myCarousel.Add(new StylingCell { Cellname = "Hinzufügen_View", Items = HinzufügenView.Hinzufügen_View_Item_List, Title = "Hinzufügen/Bearbeiten" });

            myCarousel.Add(new StylingCell { Cellname = "Bilanz_View", Items = BilanzView.Bilanz_View_Item_List, Title = "Bilanz" });

            myCarousel.Add(new StylingCell { Cellname = "Popup_View", Items = PopupView.Popup_View_Item_List, Title = "Popup" });

            carouselView.ItemsSource = myCarousel;
        }

        private void ColorWheel1_SelectedColorChanged(object sender, ColorPicker.BaseClasses.ColorPickerEventArgs.ColorChangedEventArgs e)
        {
            Styling_Color_PageModel.SelectedColorChanged_CommandExecuted(sender, e);
        }

        private void carouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            Styling_Color_PageModel.CurrentItemChangedCommand_CommandExecuted(sender, e);
        }

        private void OnStylingMessageRaised(object sender, StylingMessageEventArgs e)
        {
            if (e.Message != null)
            {
                ColorWheel1.SelectedColor = Color.FromHex(e.Message);
            }
        }
    }
}