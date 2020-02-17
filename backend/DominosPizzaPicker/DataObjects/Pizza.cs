using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DominosPizzaPicker.Backend.DataObjects
{
    public class Pizza : EntityData
    {
        public string SauceId { get; set; }
        public string Topping1Id { get; set; }
        public string Topping2Id { get; set; }
        public string Topping3Id { get; set; }
        public bool Eaten { get; set; }
        public DateTime DateEaten { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }


        // Equal if each pizza contains the same sauce and same toppings in ANY order
        public override bool Equals(object obj)
        {
            try
            {
                var pizza = (Pizza)obj;

                if (!pizza.SauceId.Equals(this.SauceId)) return false;

                // compare arrays of toppings. if p2 has a topping that p1 doesn't have, pizzas are not equal
                var p1 = new string[] { this.Topping1Id, this.Topping2Id, this.Topping3Id };
                var p2 = new string[] { pizza.Topping1Id, pizza.Topping2Id, pizza.Topping3Id };

                var result = true;
                foreach (var t in p1)
                {
                    if (!p2.Any(y => y.Equals(t)))
                    {
                        result = false;
                        break;
                    }
                }

                return result;
            }
            catch
            {
                // if any error, return false
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = 863397964;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SauceId);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Topping1Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Topping2Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Topping3Id);
            hashCode = hashCode * -1521134295 + Eaten.GetHashCode();
            hashCode = hashCode * -1521134295 + DateEaten.GetHashCode();
            hashCode = hashCode * -1521134295 + Rating.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Comment);
            return hashCode;
        }
    }
}