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
        private static PollConsul _pollConsul;
        private readonly static object _pollConsulLocker = new object();
        public static ServiceDiscoveryFinderDelegate Get = (services, config, route) =>
        {
            var factory = services.GetService<IOcelotLoggerFactory>();

            var consulFactory = services.GetService<IConsulClientFactory>();

            var consulRegistryConfiguration = new ConsulRegistryConfiguration(config.Scheme, config.Host, config.Port, route.ServiceName, config.Token);

            var consulServiceDiscoveryProvider = new Consul(consulRegistryConfiguration, factory, consulFactory);

            if (config.Type?.ToLower() == "pollconsul")
            {
                if(_pollConsul == null)
                {
                    lock (_pollConsulLocker)
                    {
                        if (_pollConsul == null)
                        {
                            _pollConsul = new PollConsul(config.PollingInterval, factory, consulServiceDiscoveryProvider);
                        }
                    }
                }
                return _pollConsul;
            }

            return consulServiceDiscoveryProvider;
        };
    }
}
