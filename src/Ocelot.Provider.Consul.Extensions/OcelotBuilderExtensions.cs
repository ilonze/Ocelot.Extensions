using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ocelot.DependencyInjection;
using Ocelot.ServiceDiscovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ocelot.Provider.Consul.Extensions
{
    public static class OcelotBuilderExtensions
    {
        public static IOcelotBuilder WithExtensions(this IOcelotBuilder builder)
        {
            builder.Services.Replace(new ServiceDescriptor(typeof(ServiceDiscoveryFinderDelegate), PollConsulProviderFactory.Get));
            return builder;
        }
    }
}
