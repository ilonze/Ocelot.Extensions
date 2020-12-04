# Ocelot.Extensions
Extensions for Ocelot


## Ocelot.Provider.Consul.Extensions
Reqair any issue for Ocelot.Provider.Consul.

```
services.AddOcelot().AddConsul().WithExtensions();
```

### issue:
DynamicRoute model with PollConsul, when an api is currently requested for the first time, the response statuscode is 404.

