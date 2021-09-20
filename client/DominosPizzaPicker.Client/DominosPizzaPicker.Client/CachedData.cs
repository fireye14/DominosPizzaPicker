using DominosPizzaPicker.Client.Models;
using DominosPizzaPicker.Client.Models.Managers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DominosPizzaPicker.Client.Helpers.Comparers;

namespace DominosPizzaPicker.Client
{
    public static class CachedData
    {
        /// <summary>
        /// Cached data for populating the recent eaten pizzas page
        /// </summary>
        public static ObservableCollection<PizzaView> RecentEatenPizzaCache { get; set; } = new ObservableCollection<PizzaView>();

        /// <summary>
        /// 
        /// </summary>
        public static async Task AddToEatenPizzaCache(PizzaView pizza)
        {
            if (!pizza.Eaten)
                return;

            // If pizza already exists in cache, then something else has been updated besides eaten status (rating or comment)
            // remove the pizza and add it back so the cache will include the updated version
            var addPizza = RecentEatenPizzaCache.FirstOrDefault(x => x.Id == pizza.Id);
            if (addPizza != null)
            {
                RecentEatenPizzaCache.Remove(addPizza);
            }

            var pizzaMan = PizzaViewManager.DefaultManager;
            addPizza = await pizzaMan.GetSinglePizza(pizza.Id);

            RecentEatenPizzaCache.Add(addPizza);
            RecentEatenPizzaCache = new ObservableCollection<PizzaView>(RecentEatenPizzaCache
                .OrderByDescending(x => x.DateEaten).ThenByDescending(x => x.Version, new ByteArrayComparer()));

            while (RecentEatenPizzaCache.Count > Constants.RecentEatenListCount)
            {
                RecentEatenPizzaCache.RemoveAt(RecentEatenPizzaCache.Count - 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static async Task RemoveFromEatenPizzaCache(PizzaView pizza)
        {
            if (pizza.Eaten)
                return;
            
            var removePizza = RecentEatenPizzaCache.FirstOrDefault(x => x.Id == pizza.Id);
            if (removePizza == null)
                return;

            RecentEatenPizzaCache.Remove(removePizza);

            var pizzaMan = PizzaViewManager.DefaultManager;
            var newPizza = await pizzaMan.GetReplacementPizzaForCache();

            if (newPizza != null)
                RecentEatenPizzaCache.Add(newPizza);

            RecentEatenPizzaCache = new ObservableCollection<PizzaView>(RecentEatenPizzaCache
                .OrderByDescending(x => x.DateEaten).ThenByDescending(x => x.Version, new ByteArrayComparer()));

        }

        public static async Task<bool> ResetConnection()
        {
            PizzaViewManager.DefaultManager.SetConnection();
            PizzaManager.DefaultManager.SetConnection();
            SauceManager.DefaultManager.SetConnection();
            SauceManager.DefaultManager.SetConnection();

            if (RecentEatenPizzaCache == null || RecentEatenPizzaCache.Count == 0)
            {
                RecentEatenPizzaCache = await PizzaViewManager.DefaultManager.GetRecentAsync();
            }

            // false if not able to connect
            return await PizzaViewManager.DefaultManager.PingConnection();
        }
    }
}
