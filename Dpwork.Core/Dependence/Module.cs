using Microsoft.Extensions.DependencyInjection;

namespace Dpwork.Core.Dependence
{
    public abstract class Module : IModule
    {
        public virtual void Load(IServiceCollection services) { }
    }
}
