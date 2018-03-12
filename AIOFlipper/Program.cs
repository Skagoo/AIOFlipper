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


            // Assign 5 accounts or less per thread
            CouchPortal couchPortal = new CouchPortal();

            int i = 0;
            int flipperThreadId = 0;
            foreach (Account account in Accounts)
            {
                if (i >= 5)
                {
                    flipperThreadId++;
                    i = 0;
                }

                account.FlipperThreadId = flipperThreadId;
                couchPortal.UpdateAccount(account);

                i++;
            }            

            List<Thread> flipperThreads = new List<Thread>();
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

            Thread thread1 = new Thread(() => StartFlipperThread(0, optionsQueue.Dequeue()));
            thread1.IsBackground = true;
            thread1.Start();

            Thread thread2 = new Thread(() => StartFlipperThread(1, optionsQueue.Dequeue()));
            thread2.IsBackground = true;
            thread2.Start();

            Thread thread3 = new Thread(() => StartFlipperThread(2, optionsQueue.Dequeue()));
            thread3.IsBackground = true;
            thread3.Start();

            Thread thread4 = new Thread(() => StartFlipperThread(3, optionsQueue.Dequeue()));
            thread4.IsBackground = true;
            thread4.Start();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Application.Run(form);
        }

        public static void StartFlipperThread(int id, ChromeOptions options)
        {
            Flipper flipper = new Flipper(id, options);
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
