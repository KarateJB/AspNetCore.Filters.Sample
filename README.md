# ASP.NET Core Filter Sample

## Tutorials

- [[MVC] Pass parameters to ActionFilterAttribute](https://karatejb.blogspot.com/2017/09/mvc-pass-parameters-to.html)
- [[ASP.NET Core] Action Filter with Parameter(s)](https://karatejb.blogspot.com/2019/07/aspnet-core-action-filter-with.html)
- [[ASP.NET Core] Hybrid Filter](https://karatejb.blogspot.com/2019/07/aspnet-core-hybrid-filter.html)
- [[ASP.NET Core] Feature toggle](https://karatejb.blogspot.com/2020/12/aspnet-core-feature-toggle.html)



## Feature Toggle

### Remote feature definition provider (Get feature flags from other service)

To test custom feature definition provider, use [json-server](https://github.com/typicode/json-server) to load the `feature_management.json` and listen on port: `3000`.

```s
$ cd src\AspNetCore.Filters.WebApi\Assets
$ json-server --watch feature_management.json
```

And set your `ASPNETCORE_ENVIRONMENT` to `RemoteFeatureFlags` in `launchSettings.json`.

```json
"AspNetCore.Filters.WebApi": {
    "commandName": "Project",
    "launchBrowser": true,
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "RemoteFeatureFlags" // <----
    },
    "applicationUrl": "https://localhost:5001;http://localhost:5000"
}
```

See the article [[ASP.NET Core] Feature toggle](https://karatejb.blogspot.com/2020/12/aspnet-core-feature-toggle.html) for more details.




