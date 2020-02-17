using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DominosPizzaPicker.Client.Models.Managers;
using Microsoft.WindowsAzure.MobileServices;

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
        public DateTime DateEaten { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }


        [Version]
        public string Version { get; set; }


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
            if(string.IsNullOrEmpty(Topping1Id) || string.IsNullOrEmpty(Topping2Id) || string.IsNullOrEmpty(Topping3Id))
                return base.ToString();

            var s = await SauceManager.DefaultManager.GetSauce(SauceId);
            var t1 = await ToppingManager.DefaultManager.GetTopping(Topping1Id);
            var t2 = await ToppingManager.DefaultManager.GetTopping(Topping2Id);
            var t3 = await ToppingManager.DefaultManager.GetTopping(Topping3Id);

            return string.Join(", ", new[] { s.Name, t1.Name, t2.Name, t3.Name });
        }

        public async Task<string> GetDebugDisplay()
        {
            return await ToStringAsync();
        }
        #endregion
    }
}
