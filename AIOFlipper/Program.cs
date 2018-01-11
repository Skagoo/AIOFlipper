using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using Chesterfield;

namespace AIOFlipper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static Form1 form;

        private static Queue<Account> updateAccountQueue;
        private static Queue<Item> updateItemQueue;

        private static CouchPortal couchClient;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize the update queues.
            updateAccountQueue = new Queue<Account>();
            updateItemQueue = new Queue<Item>();

            // Initialize the CouchDB client
            couchClient = new CouchPortal();

            form = new Form1();

            List<Account> activeAccounts = new List<Account>();
            foreach (Account account in Accounts)
            {
                if (account.IsActive)
                {
                    activeAccounts.Add(account);
                }
            }

            Thread myThread = new Thread(() => StartFlipperThread(activeAccounts.ToArray()));
            myThread.IsBackground = true;
            myThread.Start();

            //for (int i = 0; i < activeAccounts.Count - 1; i++)
            //{
            //    Thread myThread = new Thread(() => StartFlipperThread(new Account[] { activeAccounts[i]}));
            //    myThread.IsBackground = true;
            //    myThread.Start();
            //}

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Application.Run(form);
        }

        public static void StartFlipperThread(Account[] accounts)
        {
            Flipper flipper = new Flipper(accounts);
            flipper.Start();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        public static Account[] Accounts
        {
            get
            {
                return couchClient.GetAccounts();
            }

            set
            {
                couchClient.UpdateAccounts(value);
            }
        }

        public static Item[] Items
        {
            get
            {
                return couchClient.GetItems();
            }

            set
            {
                couchClient.UpdateItems(value);
            }
        }

        public static JObject Elements
        {
            get
            {
                string json = File.ReadAllText(@"C:\Users\Sacha\Documents\development\csharp\AIOFlipper\AIOFlipper\data\elements.json");
                return JObject.Parse(json);
            }
        }

    }
}
