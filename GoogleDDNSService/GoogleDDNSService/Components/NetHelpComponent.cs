using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NLog;

namespace GoogleDDNSService.Components
{
    [UsedImplicitly]
    public class NetHelpComponent: NetQueryBase
    {
        public NetHelpComponent(ILogger logger) : base(logger)
        {

        }

        public override async Task<IPAddress> GetExternalIpAddress()
        {
            return await base.GetExternalIpAddress("http://myexternalip.com/raw");
        }
    }
}
