using System;
using System.Collections.Generic;
using System.Text;

namespace EasyLife.CustomeEventArgs
{
    public class ItemChangedEventArgs : EventArgs
    {
        public ItemChangedEventArgs(string oldColor, string newColor)
        {
            OldItem = oldColor;
            NewItem = newColor;
        }

        public string OldItem { get; private set; }
        public string NewItem { get; private set; }
    }
}
