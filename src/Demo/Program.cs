using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using NetWorkSpeedMonitor;

namespace Demo
{
    class Program
    {


        static void Main(string[] args)
        {
            NetworkAdapter[] adapters;
            NetworkMonitor monitor;
            monitor = new NetworkMonitor();
            adapters = monitor.Adapters;
            /* If the length of adapters is zero, then no instance 
             * exists in the networking category of performance console.*/
            if (adapters.Length == 0)
            {
                //                this.ListAdapters.Enabled = false;
                Console.WriteLine("No network adapters found on this computer.");
                return;
            }

            ParameterizedThreadStart ts = new ParameterizedThreadStart(run);
            foreach (NetworkAdapter adapter in adapters)
            {
                //                monitor.StartMonitoring(adapter);
                Thread thread = new Thread(ts);
                thread.Start(adapter);
//                Console.WriteLine("{0:n} kbps", adapter.DownloadSpeedKbps);
//                Console.WriteLine("{0:n} kbps", adapter.UploadSpeedKbps);
            }
            //
            //            Console.ReadLine();
        }

        public void run(object obj)
        {
            List<PerformanceCounter>[] pcss = (List<PerformanceCounter>[])obj;
            List<PerformanceCounter> pcs = pcss[0];
            List<PerformanceCounter> pcs2 = pcss[1];
            while (true)
            {
                long recv = 0;
                long sent = 0;
                foreach (PerformanceCounter pc in pcs)
                {
                    recv += Convert.ToInt32(pc.NextValue()) / 1000;
                }
                foreach (PerformanceCounter pc in pcs2)
                {
                    sent += Convert.ToInt32(pc.NextValue()) / 1000;
                }
                Console.WriteLine("recv:" + recv + "mbps" + ",send:" + sent + "mbps");
                Thread.Sleep(500);
                Console.WriteLine("---------------------------------------");
            }
        }
    }
}
