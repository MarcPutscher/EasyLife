using System;
using System.Collections.Generic;
using System.Text;

namespace EasyLife.CustomeEventArgs
{
    public class ChangeFlyoutMenuEventArgs
    {
        public ChangeFlyoutMenuEventArgs(bool isvisible)
        {
            IsVisible = isvisible;
        }

        public bool IsVisible { get; private set; }
    }
}
