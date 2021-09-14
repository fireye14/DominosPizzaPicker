using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

/*
 * To add Offline Sync Support:
 *  1) Add the NuGet package Microsoft.Azure.Mobile.Client.SQLiteStore (and dependencies) to all client projects
 *  2) Uncomment the #define OFFLINE_SYNC_ENABLED
 *
 * For more information, see: http://go.microsoft.com/fwlink/?LinkId=717898
 */
//#define OFFLINE_SYNC_ENABLED

namespace DominosPizzaPicker.Client.Models.Managers
{
    public class PizzaViewManager
    {
        #region Fields
        static PizzaViewManager defaultInstance = new PizzaViewManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Pizza> pizzaTable;
#else
        IMobileServiceTable<PizzaView> pizzaViewTable;

#endif
        #endregion

        #region Properties
        public static PizzaViewManager DefaultManager
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
        private PizzaViewManager()
        {
            SetConnection();
        }
        #endregion

        #region Methods

        public void SetConnection()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);


#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            pizzaTable = client.GetSyncTable<Pizza>();
#else
            pizzaViewTable = client.GetTable<PizzaView>();
#endif
        }

        /// <summary>
        /// Test connection using current Application URL
        /// </summary>
        /// <returns></returns>
        public async Task<bool> PingConnection()
        {

            try
            {
                await pizzaViewTable.Take(0).ToEnumerableAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ObservableCollection<PizzaView>> GetAllPizzasAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif

                //var test = await pizzaTable.ToEnumerableAsync().Result
                //IEnumerable<Pizza> pizzas = await pizzaTable.Take(10120).ToEnumerableAsync();
                //var pizzas = await pizzaTable.Take(500).Where(x => true).ToListAsync();

                var totalCount = Convert.ToInt16(((IQueryResultEnumerable<Pizza>)await pizzaViewTable.Take(0).IncludeTotalCount().ToEnumerableAsync()).TotalCount);
                var pageSize = 1000;
                var pizzaList = new List<PizzaView>();
                for (var i = 0; i < totalCount; i = pizzaList.Count)
                {
                    var pizzas = await pizzaViewTable.Skip(i).Take(pageSize).ToEnumerableAsync();
                    pizzaList.AddRange(pizzas);
                }


                return new ObservableCollection<PizzaView>(pizzaList);
                //return new ObservableCollection<Pizza>(pizzas);

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



        public async Task<PizzaView> GetRandomUneatenPizza(bool syncItems = false)
        {
            try
            {
                var uneatenCount = await GetUneatenPizzaCount();

                // random int between 0 and uneatenCount
                var rand = (new Random()).Next(Convert.ToInt32(uneatenCount));

                // IsRandomlyGenerated = true if its sauce and all of its toppings have Used = true
                var pizza = await pizzaViewTable.Skip(rand).Take(1).Where(x => !x.Eaten && x.IsRandomlyGenerated).ToEnumerableAsync();

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
            return await GetCountWithCondition(x => !x.Eaten && x.IsRandomlyGenerated);
        }

        public async Task<long> GetEatenPizzaCount()
        {
            return await GetCountWithCondition(x => x.Eaten && x.IsRandomlyGenerated);
        }

        public async Task<long> GetTotalPizzaCount()
        {
            return await GetCountWithCondition(x => x.IsRandomlyGenerated);
        }

        public async Task<long> GetCountWithCondition(Expression<Func<PizzaView, bool>> conditionExpression)
        {
            // Take(0) ensures no actual records are returned     
            return ((IQueryResultEnumerable<PizzaView>)await pizzaViewTable.Take(0).Where(conditionExpression).IncludeTotalCount().ToEnumerableAsync()).TotalCount;
        }

        public async Task<ObservableCollection<PizzaView>> GetRecentAsync(bool syncItems = false)
        {
            try
            {
                // get up to 10 pizzas that are eaten but don't have either rating or comment entered
                //IEnumerable<Pizza> pizzas = await pizzaTable.Take(count).Where(x => x.Eaten && (x.Rating == 0 || x.Comment == string.Empty)).OrderByDescending(x => x.DateEaten).ToEnumerableAsync();
                //var t = pizzaViewTable.Take(Constants.RecentEatenListCount);

                // Get the top 'RecentEatenListCount' recently eaten pizzas. if date eaten is the same, order by row version
                IEnumerable<PizzaView> pizzas = await pizzaViewTable.Take(Constants.RecentEatenListCount).Where(x => x.Eaten).OrderByDescending(x => x.DateEaten).ThenByDescending(x => x.Version).ToEnumerableAsync();

                return new ObservableCollection<PizzaView>(pizzas);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
            return null;
        }

        public async Task<PizzaView> GetSinglePizza(string Id)
        {
            try
            {
                IEnumerable<PizzaView> pizza = await pizzaViewTable.Take(1).Where(x => x.Id == Id).ToEnumerableAsync();

                return pizza.FirstOrDefault();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Save error: {0}", new[] { e.Message });
            }
            return null;
        }


        /// <summary>
        /// Called after a pizza is changed from eaten to not eaten; need to update the cache with a new pizza
        /// </summary>
        public async Task<PizzaView> GetReplacementPizzaForCache()
        {
            try
            {
                var eatenCount = ((IQueryResultEnumerable<Pizza>)await pizzaViewTable.Take(0).Where(x => x.Eaten).IncludeTotalCount().ToEnumerableAsync()).TotalCount;
                if (eatenCount < Constants.RecentEatenListCount)
                    return null;

                // Get the last pizza in the list to replace the one that was removed prior to this
                var pizza = await pizzaViewTable.Skip(Constants.RecentEatenListCount - 1).Take(1).Where(x => x.Eaten).OrderByDescending(x => x.DateEaten).ThenByDescending(x => x.Version).ToEnumerableAsync();

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

        #endregion
    }
}

