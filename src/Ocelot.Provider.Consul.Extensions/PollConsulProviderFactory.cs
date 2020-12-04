using Microsoft.Extensions.DependencyInjection;
using Ocelot.Logging;
using Ocelot.ServiceDiscovery;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Ocelot.Provider.Consul.Extensions
{
    public static class PollConsulProviderFactory
    {
        private static ConcurrentDictionary<string, PollConsul> _cacheProviders = new ConcurrentDictionary<string, PollConsul>();
        public static ServiceDiscoveryFinderDelegate Get = (services, config, route) =>
        {
            var factory = services.GetService<IOcelotLoggerFactory>();

            var consulFactory = services.GetService<IConsulClientFactory>();

            var consulRegistryConfiguration = new ConsulRegistryConfiguration(config.Scheme, config.Host, config.Port, route.ServiceName, config.Token);

            var consulServiceDiscoveryProvider = new Consul(consulRegistryConfiguration, factory, consulFactory);

            if (config.Type?.ToLower() == "pollconsul")
            {
                if (_cacheProviders.ContainsKey(route.ServiceName))
                {
                    if (_cacheProviders.TryGetValue(route.ServiceName, out var sprovider))
                    {
                        return sprovider;
                    }
                }
                var provider = new PollConsul(config.PollingInterval, factory, consulServiceDiscoveryProvider);
                if (_cacheProviders.TryRemove(route.ServiceName, out var oldprovider))
                {
                    oldprovider.Dispose();
                }
                _cacheProviders.TryAdd(route.ServiceName, provider);
            }

            return consulServiceDiscoveryProvider;
        };
    }
}
