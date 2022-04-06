using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace DominosPizzaPicker.Backend.DataObjects
{
    public class PizzaView : EntityData
    {
        public string Sauce { get; set; }
        public string Topping1 { get; set; }
        public bool Topping1IsMeat { get; set; }
        public bool Topping1IsCheese { get; set; }
        public string Topping2 { get; set; }
        public bool Topping2IsMeat { get; set; }
        public bool Topping2IsCheese { get; set; }
        public string Topping3 { get; set; }
        public bool Topping3IsMeat { get; set; }
        public bool Topping3IsCheese { get; set; }
        public bool Eaten { get; set; }
        public DateTime DateEaten { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public bool IsRandomlyGenerated { get; set; }
    }
}