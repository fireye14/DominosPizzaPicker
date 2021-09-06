using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace DominosPizzaPicker.Client.Models
{
    public class PizzaView
    {
        public string Id { get; set; }

        public string Sauce { get; set; }
        public string Topping1 { get; set; }
        public string Topping2 { get; set; }
        public string Topping3 { get; set; }
        public bool Eaten { get; set; }

        public DateTime DateEaten { get; set; }


        public float Rating { get; set; }
        public string Comment { get; set; }


        [Version]
        public string Version { get; set; }


        public bool IsRandomlyGenerated { get; set; }


        public override string ToString()
        {
            return string.Join(", ", new[] { Sauce, Topping1, Topping2, Topping3 });
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is PizzaView))
                return false;

            return Id == ((PizzaView)obj).Id;
        }

        public bool Equals(PizzaView other)
        {
            return other != null &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
        }
    }
}
