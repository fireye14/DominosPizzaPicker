using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DominosPizzaPicker.Backend.DataObjects
{
    public class Topping : EntityData
    {
        public string Name { get; set; }
        public bool IsMeat { get; set; }
        public bool IsCheese { get; set; }
        public bool Used { get; set; }
    }
}