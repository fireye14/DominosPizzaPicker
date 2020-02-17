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
    public class SauceManager
    {
        static SauceManager defaultInstance = new SauceManager();
        MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<Sauce> sauceTable;
#else
        IMobileServiceTable<Sauce> sauceTable;
#endif


        public static SauceManager DefaultManager
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

        private SauceManager()
        {
            this.client = new MobileServiceClient(Constants.ApplicationURL);

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore(offlineDbPath);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            sauceTable = client.GetSyncTable<Sauce>();
#else
            sauceTable = client.GetTable<Sauce>();
#endif
        }

        #region Methods
        public async Task<ObservableCollection<Sauce>> GetSaucesAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<Sauce> sauces = await sauceTable.OrderBy(x =>x.Name).ToEnumerableAsync();

                return new ObservableCollection<Sauce>(sauces);
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

        public async Task<Sauce> GetSauce(string Id, bool syncItems = false)
        {
            try
            {
                IEnumerable<Sauce> sauce = await sauceTable.Where(x => x.Id == Id).ToEnumerableAsync();
                return sauce.FirstOrDefault();
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
