using Chesterfield;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIOFlipper
{
    class CouchPortal: CouchClient
    {
        private CouchClient couchClient;

        private string accountsDB = "aio_flipper_accounts";
        private string itemsDB = "aio_flipper_items";
        private string salesDB = "aio_flipper_sales";

        public CouchPortal() : base()
        {
            couchClient = new CouchClient();
        }

        public void UpdateAccounts(Account[] accounts)
        {
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
        }

        public Account[] GetAccounts()
        {
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

            return Account.FromJson(doc.GetValue("accounts").ToString());
        }

        private void CreateTodaysAccountsDoc()
        {
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
        }

        public void UpdateItems(Item[] items)
        {
            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>("items");

            JToken token = doc.GetValue("items");
            token.Replace(JToken.Parse(Serialize.ToJson(items)));

            db.UpdateDocument(doc);
        }

        public Item[] GetItems()
        {
            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>("items");

            return Item.FromJson(doc.GetValue("items").ToString());
        }

        public void WriteSale(Sale sale)
        {
            CouchDatabase db = couchClient.GetDatabase(salesDB);
            db.CreateDocument(Serialize.ToJson(sale));
        }
    }
}
