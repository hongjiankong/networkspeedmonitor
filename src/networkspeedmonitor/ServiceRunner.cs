using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace networkspeedmonitor
{
    class ServiceRunner
    {
        

        public ServiceRunner()
        {

          
        }

        public void Start()
        {
            new MonitorNetStream();
        }

        public void Stop()
        {
        }
    }
}
