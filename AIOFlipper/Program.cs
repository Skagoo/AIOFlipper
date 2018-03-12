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

            Account[] accountPack1 = Accounts.GetRange(0, 5).ToArray();
            Account[] accountPack2 = Accounts.GetRange(5, 5).ToArray();
            Account[] accountPack3 = Accounts.GetRange(10, 5).ToArray();
            Account[] accountPack4 = Accounts.GetRange(15, 2).ToArray();

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

            FlippingGroup flippingGroup1 = new FlippingGroup(optionsQueue.Dequeue(), accountPack1);
            FlippingGroup flippingGroup2 = new FlippingGroup(optionsQueue.Dequeue(), accountPack2);
            FlippingGroup flippingGroup3 = new FlippingGroup(optionsQueue.Dequeue(), accountPack3);
            FlippingGroup flippingGroup4 = new FlippingGroup(optionsQueue.Dequeue(), accountPack4);

            Thread thread = new Thread(() => StartFlipperThread(new FlippingGroup[] { flippingGroup1, flippingGroup2, flippingGroup3, flippingGroup4}));
            thread.IsBackground = true;
            thread.Start();

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            Application.Run(form);
        }

        public static void StartFlipperThread(FlippingGroup[] flippingGroups)
        {
            Flipper flipper = new Flipper(flippingGroups);
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
