using Chesterfield;
using Newtonsoft.Json.Linq;
using System;

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

        public void UpdateAccounts(Account[] accounts)
        {
            CouchClient couchClient = new CouchClient();

            string docId = "accounts-" + DateTime.Today.ToString("yyyy-MM-dd");

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc;

            // Check if todays document exists already, else create it.
            if (db.DocumentExists(docId))
            {
                doc = db.GetDocument<JDocument>(docId);
            }
            else
            {
                CreateTodaysAccountsDoc();

                // Get the docment from db
                doc = db.GetDocument<JDocument>(docId);
            }

            JToken token = doc.GetValue("accounts");
            token.Replace(JToken.Parse(Serialize.ToJson(accounts)));

            db.UpdateDocument(doc);

            couchClient = null;
        }

        public Account[] GetAccounts()
        {
            CouchClient couchClient = new CouchClient();

            string docId = "accounts-" + DateTime.Today.ToString("yyyy-MM-dd");

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc;

            // Check if todays document exists already, else create it.
            if (db.DocumentExists(docId))
            {
                doc = db.GetDocument<JDocument>(docId);
            }
            else
            {
                CreateTodaysAccountsDoc();

                // Get the docment from db
                doc = db.GetDocument<JDocument>(docId);
            }

            couchClient = null;

            return Account.FromJson(doc.GetValue("accounts").ToString());
        }

        private void CreateTodaysAccountsDoc()
        {
            CouchClient couchClient = new CouchClient();

            string docId = "accounts-" + DateTime.Today.ToString("yyyy-MM-dd");

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc;

            ViewOptions viewOptions = new ViewOptions();
            viewOptions.Descending = true;
            viewOptions.Limit = 1;

            // Get the id of the latest accounts document
            JObject viewResult = db.GetView("accountsDesignDoc", "getLastAccountsDocument", viewOptions);
            JToken rows = viewResult.GetValue("rows");
            string oldDocId = rows[0]["id"].ToString();

            // Get the latest accounts document
            doc = db.GetDocument<JDocument>(oldDocId);

            // Change the id of the document to the correct id for today
            doc.Id = docId;

            // Create the document with the new id
            db.CreateDocument(doc);

            couchClient = null;
        }

        public void UpdateItems(Item[] items)
        {
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>("items");

            JToken token = doc.GetValue("items");
            token.Replace(JToken.Parse(Serialize.ToJson(items)));

            db.UpdateDocument(doc);

            couchClient = null;
        }

        public Item[] GetItems()
        {
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>("items");

            couchClient = null;

            return Item.FromJson(doc.GetValue("items").ToString());
        }

        public void WriteSale(Sale sale)
        {
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(salesDB);
            db.CreateDocument(Serialize.ToJson(sale));

            couchClient = null;
        }
    }
}
