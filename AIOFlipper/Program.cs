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

            List<Thread> flipperThreads = new List<Thread>();

            List<Account> activeAccounts = new List<Account>();
            int i = 0;
            foreach (Account account in Accounts)
            {
                if (account.IsActive)
                {
                    if (i < 5)
                    {
                        activeAccounts.Add(account);
                        i++;
                    }
                    else
                    {
                        // 5 Accounts in the list, create a thread for those.
                        flipperThreads.Add(new Thread(() => StartFlipperThread(activeAccounts.ToArray())));

                        // Set the counter to 1
                        i = 1;

                        // Add the account to activeAccounts
                        activeAccounts.Clear();
                        activeAccounts.Add(account);
                    }
                    
                }
            }

            // Remaining Accounts in the list, create a thread for those.
            flipperThreads.Add(new Thread(() => StartFlipperThread(activeAccounts.ToArray())));

            foreach (Thread thread in flipperThreads)
            {
                thread.IsBackground = true;
                thread.Start();
            }

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

        public static List<Item> Items
        {
            get
            {
                CouchPortal couchPortal = new CouchPortal();
                return couchPortal.GetItems();
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
