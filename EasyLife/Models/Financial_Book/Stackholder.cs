using System.Collections.Generic;
using System.Drawing;

namespace EasyLife.Models
{
    class Stackholder 
    {
        public string Value { get; set; }

        public string  Reason { get; set; }

        public int Count { get; set; }

        public Xamarin.Forms.Color Evaluating { get; set; }

        public List<Transaktion> Detail { get; set; }
    }
}
