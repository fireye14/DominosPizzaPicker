using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DominosPizzaPicker.Client.Models.Managers;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace DominosPizzaPicker.Client.Models
{
    //[DebuggerDisplay("{GetDebugDisplay()}")]
    public class Pizza
    {
        public string Id { get; set; }

        public string SauceId { get; set; }
        public string Topping1Id { get; set; }
        public string Topping2Id { get; set; }
        public string Topping3Id { get; set; }
        public bool Eaten { get; set; }


        // when setting this property, make sure the time is set to UTC
        // for some reason, when this object is mapped back to the Backend, it automatically converts to UTC, but then when retrieving the data, it never gets converted back
        // set the Kind to UTC here so that no automatic conversion happens
        private DateTime _dateEaten;
        public DateTime DateEaten
        {
            get { return _dateEaten; }
            set
            {
                if (value.Kind == DateTimeKind.Utc)
                    _dateEaten = value;
                else
                {
                    _dateEaten = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, DateTimeKind.Utc);
                }
            }
        }

        public float Rating { get; set; }
        public string Comment { get; set; }


        [Version]
        public byte[] Version { get; set; }


        #region Methods
        public async Task<string> GetSauceName()
        {
            if (string.IsNullOrEmpty(SauceId))
                return string.Empty;

            var s = await SauceManager.DefaultManager.GetSauce(SauceId);
            return s.Name;
        }

        public async Task<string> GetTopping1Name()
        {
            if (string.IsNullOrEmpty(Topping1Id))
                return string.Empty;

            var t = await ToppingManager.DefaultManager.GetTopping(Topping1Id);
            return t.Name;
        }

        public async Task<string> GetTopping2Name()
        {
            if (string.IsNullOrEmpty(Topping2Id))
                return string.Empty;

            var t = await ToppingManager.DefaultManager.GetTopping(Topping2Id);
            return t.Name;
        }

        public async Task<string> GetTopping3Name()
        {
            if (string.IsNullOrEmpty(Topping3Id))
                return string.Empty;

            var t = await ToppingManager.DefaultManager.GetTopping(Topping3Id);
            return t.Name;
        }

        public async Task<string> ToStringAsync()
        {
            if (string.IsNullOrEmpty(Topping1Id) || string.IsNullOrEmpty(Topping2Id) || string.IsNullOrEmpty(Topping3Id))
                return this.ToString();

            var s = await SauceManager.DefaultManager.GetSauce(SauceId);
            var t1 = await ToppingManager.DefaultManager.GetTopping(Topping1Id);
            var t2 = await ToppingManager.DefaultManager.GetTopping(Topping2Id);
            var t3 = await ToppingManager.DefaultManager.GetTopping(Topping3Id);

            return string.Join(", ", new[] { s.Name, t1.Name, t2.Name, t3.Name });
        }

        public override string ToString()
        {
            return string.Join(", ", new[] { SauceId, Topping1Id, Topping2Id, Topping3Id });
        }

        public async Task<string> GetDebugDisplay()
        {
            return await ToStringAsync();
        }
        #endregion
    }
}
