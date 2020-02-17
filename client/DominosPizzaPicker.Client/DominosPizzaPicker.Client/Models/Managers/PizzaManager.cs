using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DominosPizzaPicker.Client.Models.Managers
{
    public class PizzaManager
    {
        #region Fields
        static PizzaManager defaultInstance = new PizzaManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Pizza> pizzaTable;
#else
        IMobileServiceTable<Pizza> pizzaTable;
#endif
        #endregion

        #region Properties
        public static PizzaManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }
        #endregion

        #region Constructor
        private PizzaManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            pizzaTable = client.GetSyncTable<Pizza>();
#else
            pizzaTable = client.GetTable<Pizza>();
#endif
        }
        #endregion

        #region Methods
        public async Task<ObservableCollection<Pizza>> GetPizzasAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                IEnumerable<Pizza> pizzas = await pizzaTable.ToEnumerableAsync();

                return new ObservableCollection<Pizza>(pizzas);
          
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }



        public async Task SavePizzaAsync(Pizza pizza)
        {
            try
            {
                if (pizza.Id == null)
                {
                    await pizzaTable.InsertAsync(pizza);
                }
                else
                {
                    await pizzaTable.UpdateAsync(pizza);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
        }

        public async Task<Pizza> GetRandomUneatenPizza(bool syncItems = false)
        {
            try
            {
                var uneatenCount = await GetUneatenPizzaCount();

                // random int between 0 and uneatenCount
                var rand = (new Random()).Next(Convert.ToInt32(uneatenCount));

                var pizza = await pizzaTable.Skip(rand).Take(1).Where(x => !x.Eaten).ToEnumerableAsync();
                
                return pizza.FirstOrDefault();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine("Invalid sync operation: {0}", new[] { msioe.Message });
            }
            catch (Exception e)
            {
                Debug.WriteLine("Sync error: {0}", new[] { e.Message });
            }
            return null;
        }


        public async Task<long> GetUneatenPizzaCount()
        {
            // Take(0) ensures no actual records are returned     
            return ((IQueryResultEnumerable<Pizza>)await pizzaTable.Take(0).Where(x => !x.Eaten).IncludeTotalCount().ToEnumerableAsync()).TotalCount; 
        }

        public async Task<ObservableCollection<Pizza>> GetRecentAsync(int count, bool syncItems = false)
        {
            try
            {
                // get up to 10 pizzas that are eaten but don't have either rating or comment entered
                //IEnumerable<Pizza> pizzas = await pizzaTable.Take(count).Where(x => x.Eaten && (x.Rating == 0 || x.Comment == string.Empty)).OrderByDescending(x => x.DateEaten).ToEnumerableAsync();
                IEnumerable<Pizza> pizzas = await pizzaTable.Take(count).Where(x => x.Eaten).OrderByDescending(x => x.DateEaten).ToEnumerableAsync();

                return new ObservableCollection<Pizza>(pizzas);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
            return null;
        }

        public async Task<Pizza> GetSinglePizza(string Id)
        {
            try
            {
                IEnumerable<Pizza> pizza = await pizzaTable.Take(1).Where(x => x.Id == Id).ToEnumerableAsync();

                return pizza.FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
            return null;
        }

        public async Task<Pizza> GetSinglePizza(string sauceId, List<string> toppingIds)
        {
            try
            {
                if (toppingIds.Count != 3) return null;
                IEnumerable<Pizza> pizza = await pizzaTable.Take(1).Where(x => x.SauceId == sauceId && toppingIds.Contains(x.Topping1Id) && toppingIds.Contains(x.Topping2Id) && toppingIds.Contains(x.Topping3Id)).ToEnumerableAsync();

                return pizza.FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
            return null;
        }

        #endregion
    }
}
