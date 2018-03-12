using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;

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
            Queue<ChromeOptions> optionsQueue = new Queue<ChromeOptions>();

            // Fill the optionsQueue with ChromeOptions
            for (int j = 0; j < 5; j++)
            {
                string proxyIP = Environment.GetEnvironmentVariable("PROXY_IP_" + j);
                string proxyPort = Environment.GetEnvironmentVariable("PROXY_PORT_" + j);

                Proxy proxy = new Proxy();
                proxy.HttpProxy = proxyIP + ":" + proxyPort;
                proxy.SslProxy = proxyIP + ":" + proxyPort;

                ChromeOptions options = new ChromeOptions();
                options.Proxy = proxy;

                optionsQueue.Enqueue(options);
            }

            Account[] accountPack1 = Accounts.GetRange(0, 5).ToArray();
            Account[] accountPack2 = Accounts.GetRange(5, 5).ToArray();
            Account[] accountPack3 = Accounts.GetRange(10, 5).ToArray();
            Account[] accountPack4 = Accounts.GetRange(15, 2).ToArray();

            Thread thread1 = new Thread(() => StartFlipperThread(accountPack1, optionsQueue.Dequeue()));
            thread1.IsBackground = true;
            thread1.Start();

            Thread thread2 = new Thread(() => StartFlipperThread(accountPack2, optionsQueue.Dequeue()));
            thread2.IsBackground = true;
            thread2.Start();

            Thread thread3 = new Thread(() => StartFlipperThread(accountPack3, optionsQueue.Dequeue()));
            thread3.IsBackground = true;
            thread3.Start();

            Thread thread4 = new Thread(() => StartFlipperThread(accountPack4, optionsQueue.Dequeue()));
            thread4.IsBackground = true;
            thread4.Start();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Application.Run(form);
        }

        public static void StartFlipperThread(Account[] accounts, ChromeOptions options)
        {
            Flipper flipper = new Flipper(accounts, options);
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
