using Android.Webkit;
using EasyLife.Models;
using EasyLife.PageModels;
using EasyLife.Pages.To_Do;
using EasyLife.Services;
using FontAwesome;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.BehaviorsPack;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using static SQLite.SQLite3;
using ShellItem = Xamarin.Forms.ShellItem;

namespace EasyLife.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Master_To_Do_Page : Shell
    {
        CustomeShellContent shellContent1 = new CustomeShellContent();
        public Master_To_Do_Page()
        {

            InitializeComponent();

            Routing.RegisterRoute(nameof(To_Do_Home_Page), typeof(To_Do_Home_Page));
            Routing.RegisterRoute(nameof(Edit_Category_Page), typeof(Edit_Category_Page));
            Routing.RegisterRoute(nameof(To_Do_Detail_Page), typeof(To_Do_Detail_Page));

            CustomeShellContent shellContent = new CustomeShellContent() { Title = "Alle To-Dos", Option = 1, ClassId = "alle to-do", IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };
            shellContent.FlyoutIcon = new FontImageSource() { FontFamily = "FAS", Glyph = FontAwesomeIcons.ClipboardList };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { Title = "Zuletzt gelöscht", Option = 2, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Deleted_Page)) };
            shellContent.FlyoutIcon = new FontImageSource() { FontFamily = "FAS", Glyph = FontAwesomeIcons.TrashCan };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "Spacer" };
            this.Items.Add(shellContent);

            shellContent = new CustomeShellContent() { Title = "KATEGORIEN", CustomeRoute = "Header", IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };
            this.Items.Add(shellContent);


        }

        public async Task Create_Missing_Flyoutitems()
        {
            if(this.Items.Count<=4)
            {
                List<Category> categories = await CategoryService.Get_all_Categorys();
                List<To_Do_Item> to_dos = await To_DoService.Get_all_To_DOs();
                CustomeShellContent shellContent = new CustomeShellContent() { Title = "Alle To-Dos", Option = 1, ClassId = "alle to-do", IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)) };

                if (categories != null)
                {
                    foreach (Category category in categories)
                    {
                        List<To_Do_Item> result4 = to_dos.Where(x => x.CategoryID == category.Id && x.Is_Removed == false).ToList();
                        shellContent = new CustomeShellContent() { Option = 3, Category = category, ItemCount = result4.Count, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "Item" };
                        this.Items.Add(shellContent);
                    }
                }

                shellContent = new CustomeShellContent() { IsEnabled = false, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "New" };
                this.Items.Add(shellContent);
            }
        }

        private void Header_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("Start_Option", 0);

            App.Current.MainPage = new Start_Page();
        }

        public async void Edit_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(Edit_Category_Page));
            }
            catch { }
        }

        private async void New_Tapped(object sender, EventArgs e)
        {
            try
            {
                Category result = await Shell.Current.ShowPopupAsync(new Add_Category_Popup()) as Category;

                if (result != null)
                {
                    if (this.Items.Count != 0)
                    {
                        List<int> itemlist = new List<int>();

                        for (int i = 0; i < this.Items.Count; i++)
                        {
                            var result1 = (CustomeShellContent)this.Items[i].CurrentItem.CurrentItem;
                            if (result1.Option == 3)
                            {
                                itemlist.Add(i);
                            }
                        }

                        this.Items.Insert(itemlist.LastOrDefault() +1, new CustomeShellContent() { Option = 3, Category = result, ItemCount = 0, IsTabStop = false, IsEnabled = true, ContentTemplate = new DataTemplate(typeof(To_Do_Home_Page)), CustomeRoute = "Item" });
                    }
                }
            }
            catch (Exception ex) { }
        }

        public async Task Update_FlyoutConten()
        {
            List<Category> categories = await CategoryService.Get_all_Categorys();
            List<To_Do_Item> result2 = await To_DoService.Get_all_To_DOs();

            var customeShell = Shell.Current.CurrentItem.CurrentItem.CurrentItem;

            List<CustomeShellContent> shellcontentlist = new List<CustomeShellContent>();

            if (this.Items.Count != 0)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    var result = (CustomeShellContent)this.Items[i].CurrentItem.CurrentItem;
                    if (result.Option != 0)
                    {
                        if (result.Option == 1)
                        {
                            result.ItemCount = result2.Where(x => x.Is_Removed == false).Count();
                        }
                        if (result.Option == 2)
                        {
                            result.ItemCount = result2.Where(x => x.Is_Removed == true).Count();
                        }
                        if (result.Option == 3)
                        {
                            result.ItemCount = result2.Where(x => x.Is_Removed == false && x.CategoryID == result.Category.Id).Count();
                            if(categories.Where(x=>x.Id == result.Category.Id).FirstOrDefault() != null)
                            {
                                result.Category= categories.Where(x => x.Id == result.Category.Id).FirstOrDefault();
                            }
                        }
                        this.Items[i].CurrentItem.CurrentItem = result;
                    }
                }
            }
        }

        public async Task Update_FlyoutConten2()
        {
            List<Category> categories = await CategoryService.Get_all_Categorys();
            if(categories.Count != 0)
            {
                List<Category> categories2 = await CategoryService.Get_all_Categorys();

                Dictionary<int,int> itemdic = new Dictionary<int, int>();

                for (int i = 0; i < this.Items.Count; i++)
                {
                    var result1 = (CustomeShellContent)this.Items[i].CurrentItem.CurrentItem;
                    if (result1.Option == 3)
                    {
                        itemdic.Add(result1.Category.Id,i);
                    }
                }

                foreach (Category category in categories)
                {
                    if(categories2.Where(x=>x.Id == category.Id).Count() == 0)
                    {
                        this.Items.RemoveAt(itemdic[category.Id]);
                    }
                }

                await Update_FlyoutConten();
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }

    public class CustomeShellContent : ShellContent,INotifyPropertyChanged
    {

        public int Option { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// 0 = nichts | 1 = Alle | 2 = gelöschte | 3 = spezielle Kategorie
        /// </summary>
        int itemcount;
        public int ItemCount
        {
            get { return itemcount; }
            set
            {
                if (ItemCount == value)
                    return;
                itemcount = value; OnPropertyChanged(nameof(ItemCount));
            }
        }
        public Category category;
        public Category Category
        {
            get { return category; }
            set
            {
                if (Category == value)
                    return;
                category = value; OnPropertyChanged(nameof(Category));
            }
        }

        public string CustomeRoute { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}