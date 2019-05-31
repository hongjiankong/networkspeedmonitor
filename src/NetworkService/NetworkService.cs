using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using System.Text;

namespace NetworkService
{
    public partial class NetworkService : ServiceBase
    {
        public NetworkService()
        {
            InitializeComponent();
        }
        string filePath = @"D:\MyServiceLog.txt";
        protected override void OnStart(string[] args)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},服务启动！");
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();

                writer.WriteLine("适配器个数：" + adapters.Length);
                int index = 0;
                foreach (NetworkInterface adapter in adapters)
                {
                    index++;
                    //显示网络适配器描述信息、名称、类型、速度、MAC 地址
                    writer.WriteLine("---------------------第" + index + "个适配器信息---------------------");
                    writer.WriteLine("描述信息：" + adapter.Name);
                    writer.WriteLine("类型：" + adapter.NetworkInterfaceType);
                    writer.WriteLine("速度：" + adapter.Speed / 1000 / 1000 + "MB");
                    writer.WriteLine("MAC 地址：" + adapter.GetPhysicalAddress());

                    //获取IPInterfaceProperties实例
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                    //获取并显示DNS服务器IP地址信息
                    IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                    if (dnsServers.Count > 0)
                    {
                        foreach (IPAddress dns in dnsServers)
                        {
                            writer.WriteLine("DNS 服务器IP地址：" + dns + "\n");
                        }
                    }
                    else
                    {
                        writer.WriteLine("DNS 服务器IP地址：" + "\n");
                    }
                }
                writer.WriteLine($"{DateTime.Now},服务启动！");
            }
        }

        protected override void OnStop()
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine($"{DateTime.Now},服务停止！");
            }
        }
    }
}
