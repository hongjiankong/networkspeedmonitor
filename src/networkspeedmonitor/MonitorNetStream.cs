using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using log4net;
using Topshelf;

namespace networkspeedmonitor
{
    internal class MonitorNetStream
    {
        ILog _log = LogManager.GetLogger(typeof(MonitorNetStream));

        public MonitorNetStream()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));
            List<PerformanceCounter> pcs = new List<PerformanceCounter>();
            List<PerformanceCounter> pcs2 = new List<PerformanceCounter>();
            string[] names = GetAdapter();
            foreach (string name in names)
            {
                try
                {
                    PerformanceCounter pc = new PerformanceCounter("Network Interface", "Bytes Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    PerformanceCounter pc2 = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    pc.NextValue();
                    pcs.Add(pc);
                    pcs2.Add(pc2);
                }
                catch
                {
                    continue;
                }

            }
            ParameterizedThreadStart ts = new ParameterizedThreadStart(run);
            Thread monitor = new Thread(ts);
            List<PerformanceCounter>[] pcss = new List<PerformanceCounter>[2];
            pcss[0] = pcs;
            pcss[1] = pcs2;
            monitor.Start(pcss);
        }

        private string[] GetAdapter()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string[] name = new string[adapters.Length];
            int index = 0;
            foreach (NetworkInterface ni in adapters)
            {
                name[index] = ni.Description;
                index++;
            }
            return name;
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
                _log.Info("接收:" + recv + "mbps" + ",发送:" + sent + "mbps");
                Thread.Sleep(1000);
            }
        }

    }
}
