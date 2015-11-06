# jcDC
.NET Distributed Caching for MVC and WebAPI.

<h2>Upcoming Features</h2>
-Timed Based Caching

-Auto-purge of SQL based cache

-In-memory Table Support

<h2>How to Use</h2>
The library was designed for easy of use.  To help accelerate implementation inside the Tests.WebAPI project you'll find a complete example of how to use it.

At its heart there are 2 components to use:

1. The Attribute <b>jcCACHE</b>
2. The <b>Return</b> function inside the <b>jcCACHEController</b>

For implementation purposes, I'm assuming you have your own ApiController extension, so simply have your base classs extend the <b>jcCACHEController</b>.

Once implemented you can then begin to decorate Actions in your MVC and WebAPI controllers with ease.

I prefer to utilize Enumerations whereever possible, however the library was designed to allow strings or enums.  For the sake of the example below I will assume you are using an enum defined as:

```csharp
public enum REQUESTS {
    NO_CACHE,
    VALUES_GET,
    VALUES_ADD,
    VALUES_DELETE
}
```

To get started, let me assume you have a GET method that returns a collection of ints like so
```csharp
[jcCACHE(REQUESTS.VALUES_GET)]
public IEnumerable<int> Get() {
    return Return(val, REQUESTS.VALUES_GET, cacheObject: true);
}
```

What the attribute is doing in the background is caching that specific request and then checking to see if that request is in the cache and if so simply return the cache.  Using the <b>Return</b> wrapper it is saying to cache the result for future requests.

You might be thinking now about well, what if the collection gets modified?  I added in a lightweight dependency model in which in the attribute you can define which requests will be invalidated.

For instance, the example below says that upon execution, remove the VALUES_GET cache item:

```csharp
[HttpGet]
[jcCACHE(REQUESTS.NO_CACHE, new string[] { "VALUES_GET" })]
public bool Add(int a) {
    val.Add(a);

    return Return(true, REQUESTS.VALUES_ADD);
}
```

That's all there is to using the library.

<h2>Configuration</h2>
By default the library uses the ObjectCache which will get cleared upon your Application Pool being recycled or IIS being restarted.  You can use the SQL based cache by adding the following to your web.config:

```csharp
<add key="jcDC_CachePlatform" value="SQL"/>
```

Be sure to add Entity Framework via NuGet and that you have a connection string defined in your web.config.  For example for my local testing:

```csharp
  <connectionStrings>
    <add name="jcDCEFModel" connectionString="data source=localhost;initial catalog=jcDC;persist security info=True;user id=jcdcsa;password=jcdcsa;MultipleActiveResultSets=True;App=EntityFramework" providerName="System.Data.SqlClient" />
  </connectionStrings>
```

Be sure to run the SQL/DBCreate.sql script on your database server (SQL Server 2016 CTP in my case).

<h2>Contact</h2>
If you have suggestions, issues, concerns please email at jcapellman at hotmail dot com.
