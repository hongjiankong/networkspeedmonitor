using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace NetworkService
{
    public static class AdapterHelper
    {
        public static List<NetworkInterface> GetGetAllNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces().ToList();
        }
    }
}
