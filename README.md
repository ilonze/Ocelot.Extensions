# Ocelot.Extensions
Extensions for Ocelot

[![NuGet](https://img.shields.io/nuget/v/Ocelot.Provider.Consul.Extensions.svg)](https://www.nuget.org/packages/Ocelot.Provider.Consul.Extensions)

## Ocelot.Provider.Consul.Extensions
Reqair any issue for Ocelot.Provider.Consul.

```
services.AddOcelot().AddConsul().WithExtensions();
```

### issue:
DynamicRoute model with PollConsul, when an api is currently requested for the first time, the response statuscode is 404.

