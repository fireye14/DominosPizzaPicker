using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominosPizzaPicker.Client.Models.Managers
{
    public class ToppingManager
    {
        static ToppingManager defaultInstance = new ToppingManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Topping> sauceTable;
#else
        IMobileServiceTable<Topping> toppingTable;
#endif


        private ToppingManager()
        {
            SetConnection();
        }


        public static ToppingManager DefaultManager
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


        public void SetConnection()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            toppingTable = client.GetSyncTable<Topping>();
#else
            toppingTable = client.GetTable<Topping>();
#endif
        }

        public async Task<ObservableCollection<Topping>> GetToppingsAsync(bool usedOnly = false, bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                        if (syncItems)
                        {
                            await this.SyncAsync();
                        }
#endif
                IEnumerable<Topping> toppings;
                // for some god damn reason, you can't use a (true ? a : b) operator in a where clause? 
                if (usedOnly)
                    toppings = await toppingTable.Where(x => x.Used).OrderBy(x => x.Name).ToEnumerableAsync();
                else
                    toppings = await toppingTable.OrderBy(x => x.Name).ToEnumerableAsync();

                return new ObservableCollection<Topping>(toppings);
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




        public async Task<Topping> GetTopping(string Id, bool syncItems = false)
        {
            try
            {
                IEnumerable<Topping> topping = await toppingTable.Where(x => x.Id == Id).ToEnumerableAsync();
                return topping.FirstOrDefault();
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

    }
}
