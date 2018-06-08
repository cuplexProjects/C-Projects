using System.Threading.Tasks;

namespace GoogleDDNSService.PropagationModules
{
    public abstract class PropagationModuleBase
    {
        public virtual string DDNSServiceType { get; set; }

        protected abstract Task UpdateIp(string ipnumber);
    }
}
