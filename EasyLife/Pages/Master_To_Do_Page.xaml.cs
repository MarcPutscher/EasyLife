using EasyLife.Models;
using EasyLife.PageModels;
using EasyLife.Pages.To_Do;
using EasyLife.Services;
using FontAwesome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master_To_Do_Page : Shell
    {
        CustomeShellContent shellContent1 = new CustomeShellContent();

        public Master_To_Do_Page(List<Category> categories, List<To_Do_Item> result2)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(To_Do_Home_Page), typeof(To_Do_Home_Page));
            Routing.RegisterRoute(nameof(To_Do_Settings_Page), typeof(To_Do_Settings_Page));

            CustomeShellContent shellContent = new CustomeShellContent() { Title = "Alle To-Dos", Option = 1, Count = result2.Count, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };
            shellContent.FlyoutIcon = new FontImageSource() { FontFamily = "FAS", Glyph = FontAwesomeIcons.ClipboardList };
            this.Items.Add(shellContent);

            List<To_Do_Item> result3 = result2.Where(x=>x.Is_Removed == true).ToList();
            shellContent = new CustomeShellContent() { Title = "Zuletzt gelöscht", Option = 2, Count = result3.Count, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };
            shellContent.FlyoutIcon = new FontImageSource() { FontFamily = "FAS", Glyph = FontAwesomeIcons.TrashCan };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() {IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "Spacer" };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { Title = "KATEGORIEN", CustomeRoute = "Header", IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };
            this.Items.Add(shellContent);

            if(categories != null)
            {
                foreach (Category category in categories)
                {
                    List<To_Do_Item> result4 = result2.Where(x => x.CategoryID == category.Id).ToList();
                    shellContent = new CustomeShellContent() {Option = 3, Category = category, Count = result4.Count, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) , CustomeRoute = "Item" };
                    this.Items.Add(shellContent);
                }
            }


            shellContent = new CustomeShellContent() { IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute ="New"};
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "Spacer" };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { Title = "Einstellungen", IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Settings_Page)), CustomeRoute="Settings"};
            shellContent.FlyoutIcon = new FontImageSource() { FontFamily = "FAS", Glyph = FontAwesomeIcons.Gear };
            this.Items.Add(shellContent);
        }

        public async Task Appearingd()
        {

            await CategoryService.Add_Category(new Category() { Count = 10, Color = Color.Blue.ToHex(), Title = "Garten" });
            await CategoryService.Add_Category(new Category() { Count = 450, Color = Color.Red.ToHex(), Title = "Arbeit" });
            await CategoryService.Add_Category(new Category() { Count = 3, Color = Color.Green.ToHex(), Title = "Zuhause" });


            //categories = await CategoryService.Get_all_Categorys();
            //ObservableCollection<Category> categories = new ObservableCollection<Category>();
            //foreach(Category category in await CategoryService.Get_all_Categorys())
            //{
            //    categories.Add(category);
            //}
            //shellContent1.Categorys = categories;

            //shellContent1.Height = categories.Count * 45;
        }

        private void Header_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new Start_Page();
        }

        private void Edit_Tapped(object sender, EventArgs e)
        {

        }

        private void New_Tapped(object sender, EventArgs e)
        {

        }
    }

    public class CustomeShellContent : ShellContent
    {
        //public ObservableCollection<Category> Categorys { get; set; }

        public int Count { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// 0 = nichts | 1 = Alle | 2 = gelöschte | 3 = spezielle Kategorie
        /// </summary>
        public int Option { get; set; }

        public Category Category { get; set; }

        public string CustomeRoute { get; set; }
    }

}