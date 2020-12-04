using Ocelot.Logging;
using Ocelot.ServiceDiscovery.Providers;
using Ocelot.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ocelot.Provider.Consul.Extensions
{
    public class PollConsul : IServiceDiscoveryProvider, IDisposable
    {
        private readonly IOcelotLogger _logger;
        private readonly IServiceDiscoveryProvider _consulServiceDiscoveryProvider;
        private Timer _timer;
        private bool _polling;
        private List<Service> _services;

        public PollConsul(int pollingInterval, IOcelotLoggerFactory factory, IServiceDiscoveryProvider consulServiceDiscoveryProvider)
        {
            _logger = factory.CreateLogger<PollConsul>();
            _consulServiceDiscoveryProvider = consulServiceDiscoveryProvider;
            _services = new List<Service>();

            Task.WaitAll(Poll());

            _timer = new Timer(async x =>
            {
                if (_polling)
                {
                    return;
                }

                _polling = true;
                await Poll();
                _polling = false;
            }, null, pollingInterval, pollingInterval);
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _timer = null;
            _logger.LogDebug("Pollconsul disposed.");
        }

        public Task<List<Service>> Get()
        {
            return Task.FromResult(_services);
        }

        private async Task Poll()
        {
            _logger.LogDebug("services updated.");
            _services = await _consulServiceDiscoveryProvider.Get();
        }
    }
}
