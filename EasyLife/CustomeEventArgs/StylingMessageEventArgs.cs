using System;
using System.Collections.Generic;
using System.Text;

namespace EasyLife.CustomeEventArgs
{
    public class StylingMessageEventArgs : System.EventArgs
    {
        public string Message { get; set; }
        public StylingMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}