
using Topshelf;

namespace networkspeedmonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceRunner>(s =>
                {
                    s.ConstructUsing(name => new ServiceRunner());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("网速监控服务");
                x.SetDisplayName("网速监控服务");
                x.SetServiceName("网速监控服务");
            });
        }
    }
}
