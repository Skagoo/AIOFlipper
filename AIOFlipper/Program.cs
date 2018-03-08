using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;

namespace AIOFlipper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static Form1 form;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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

        public static List<Account> Accounts
        {
            get
            {
                CouchPortal couchPortal = new CouchPortal();
                return couchPortal.GetAccounts();
            }
        }

        public static Item[] Items
        {
            get
            {
                CouchPortal couchPortal = new CouchPortal();
                return couchPortal.GetItems();
            }

            set
            {
                CouchPortal couchPortal = new CouchPortal();
                couchPortal.UpdateItems(value);
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
