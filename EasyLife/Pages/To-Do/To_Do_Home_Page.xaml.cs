using EasyLife.Models;
using EasyLife.PageModels.To_Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages.To_Do
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class To_Do_Home_Page : ContentPage
    {
        public To_Do_Home_Page()
        {
            InitializeComponent();

            var customeShell = Shell.Current.CurrentItem.CurrentItem.CurrentItem;

            CustomeShellContent customeShellContent = customeShell as CustomeShellContent;

            this.BindingContext = new PageModels.To_Do_Home_PageModel(customeShellContent.Option,customeShellContent.Category, customeShellContent.Title, this);
        }
    }
}