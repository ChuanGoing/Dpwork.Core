using Microsoft.Extensions.DependencyInjection;

namespace Dpwork.Core.Dependence
{
    public interface IModule
    {
        void Load(IServiceCollection services);
    }
}
