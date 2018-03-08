﻿using Chesterfield;
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
            CouchClient couchClient = new CouchClient();

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
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JObject viewResult = db.GetView("accountsDesignDoc", "getAllAccounts");

            foreach (JDocument doc in viewResult.GetValue("rows"))
            {
                accounts.Add(Account.FromJson(doc.ToString()));
            }
            couchClient = null;

            return accounts;
        }

        public Account GetAccount(string email)
        {
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(accountsDB);
            JDocument doc = db.GetDocument<JDocument>(email);

            return Account.FromJson(doc.ToString());
        }

        public void UpdateItem(Item item)
        {
            CouchClient couchClient = new CouchClient();

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
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JObject viewResult = db.GetView("itemsDesignDoc", "getAllItems");

            foreach (JDocument doc in viewResult.GetValue("rows"))
            {
                items.Add(Item.FromJson(doc.ToString()));
            }
            couchClient = null;

            return items;
        }

        public Item GetItem(string name)
        {
            CouchClient couchClient = new CouchClient();

            CouchDatabase db = couchClient.GetDatabase(itemsDB);
            JDocument doc = db.GetDocument<JDocument>(name);

            return Item.FromJson(doc.ToString());
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
