using EasyLife.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EasyLife.Cells
{
    public class FlyoutItemTemplateSelector :DataTemplateSelector
    {
        public DataTemplate NavigationHeaderTemplate { get; set; }
        public DataTemplate NavigationItemTemplate { get; set; }
        public DataTemplate NavigationSpaceTemplate { get; set; }
        public DataTemplate NavigationNewTemplate { get; set; }
        public DataTemplate NavigationItemsTemplate { get; set; }
        public DataTemplate NavigationCategoryItemTemplate { get; set; }
        public DataTemplate NavigationSettingsTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is CustomeShellContent)
            {
                var item1 = (CustomeShellContent)item;

                if (item1.CustomeRoute == "Header")
                {
                    return NavigationHeaderTemplate;
                }
                if (item1.CustomeRoute == "Spacer")
                {
                    return NavigationSpaceTemplate;
                }
                if (item1.CustomeRoute == "New")
                {
                    return NavigationNewTemplate;
                }
                if (item1.CustomeRoute == "Items")
                {
                    return NavigationItemsTemplate;
                }
                if (item1.CustomeRoute == "Item")
                {
                    return NavigationCategoryItemTemplate;
                }
                if (item1.CustomeRoute == "Settings")
                {
                    return NavigationSettingsTemplate;
                }
            }
            return NavigationItemTemplate;
        }
    }
}
