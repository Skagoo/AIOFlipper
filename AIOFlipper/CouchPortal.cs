using Chesterfield;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AIOFlipper
{
    class CouchPortal: CouchClient
    {
        private string accountsDB = "aio_flipper_accounts";
        private string itemsDB = "aio_flipper_items";
        private string salesDB = "aio_flipper_sales";

        public CouchPortal() : base()
        {
        }

        public void UpdateAccount(Account account)
        {
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            string docId = account.Email;

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc = db.GetDocument<JDocument>(docId);

            JDocument newDoc = new JDocument(Serialize.ToJson(account));
            newDoc.Rev = doc.Rev;

            db.UpdateDocument(newDoc);

            couchClient = null;
        }

        public List<Account> GetAccounts()
        {
            List<Account> accounts = new List<Account>();
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JObject viewResult = db.GetView("accountsDesignDoc", "getAllAccounts");

            foreach (JObject doc in viewResult.GetValue("rows"))
            {
                JDocument accountDoc = db.GetDocument<JDocument>((doc.GetValue("id").ToString()));
                accounts.Add(Account.FromJson(accountDoc.ToString()));
            }

            couchClient = null;

            return accounts;
        }

        public Account GetAccount(string email)
        {
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc = db.GetDocument<JDocument>(email);

            return Account.FromJson(doc.ToString());
        }

        public void UpdateItem(Item item)
        {
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            string docId = item.Name;

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>(docId);

            JDocument newDoc = new JDocument(Serialize.ToJson(item));
            newDoc.Rev = doc.Rev;

            db.UpdateDocument(newDoc);

            couchClient = null;
        }

        public List<Item> GetItems()
        {
            List<Item> items = new List<Item>();
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JObject viewResult = db.GetView("itemsDesignDoc", "getAllItems");

            foreach (JObject doc in viewResult.GetValue("rows"))
            {
                JDocument itemstDoc = db.GetDocument<JDocument>((doc.GetValue("id").ToString()));
                items.Add(Item.FromJson(itemstDoc.ToString()));
            }
            couchClient = null;

            return items;
        }

        public Item GetItem(string name)
        {
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>(name);

            return Item.FromJson(doc.ToString());
        }

        public void WriteSale(Sale sale)
        {
            CouchClient couchClient = new CouchClient("192.168.1.100", 5984, null, null);

            CouchDatabase db = couchClient.GetDatabase(salesDB);
            db.CreateDocument(Serialize.ToJson(sale));

            couchClient = null;
        }
    }
}
