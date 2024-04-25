# ASP.NET Core Filter Sample

## Tutorials

- [[MVC] Pass parameters to ActionFilterAttribute](https://karatejb.blogspot.com/2017/09/mvc-pass-parameters-to.html)
- [[ASP.NET Core] Action Filter with Parameter(s)](https://karatejb.blogspot.com/2019/07/aspnet-core-action-filter-with.html)
- [[ASP.NET Core] Hybrid Filter](https://karatejb.blogspot.com/2019/07/aspnet-core-hybrid-filter.html)
- [[ASP.NET Core] Feature toggle](https://karatejb.blogspot.com/2020/12/aspnet-core-feature-toggle.html)


## Demo

```s
curl -X GET https://localhost:5001/api/Demo/MyAction1 --head
curl -X GET https://localhost:5001/api/Demo/MyAction1 --head
curl -X GET https://localhost:5001/api/Demo/MyAction1 --head
curl -X GET https://localhost:5001/api/Demo/SignIn --head
curl -X GET https://localhost:5001/api/Demo/TestDisableApiFilter --head
```

```s
curl -X GET https://localhost:5001/api/DemoGlobal/MyAction1 --head
curl -X GET https://localhost:5001/api/DemoGlobal/MyAction2 --head
curl -X GET https://localhost:5001/api/DemoGlobal/MyAction3 --head
```

## Feature Toggle

### TimeWindow

```json
{
  "FeatureManagement": {
    "Demo": {
      "RequirementType": "All", // All, Any
      "EnabledFor": [
        {
          "Name": "TimeWindow",
          "Parameters": {
            "Start": "2024-04-24T06:30:00Z",
            "End": "2024-04-24T15:50:00Z"
          }
        }
      ]
    }
  }
}
```

- `RequirementType`: 
    - "All": should meet all conditions in `EnableFor` to enable the feature.
    - "Any": meet any condition in `EnableFor` to enable the feature.
- TimeWindow condition:
    - If only `Start` is set, the feature will be enabled at the `Start` datetime and will never be diabled after it.
    - If only `End` is set, the feature will be enabled right away and will be disabled at and after `End` datetime.
    - If both `Start` and `End` are set, the feature will be enabled between the two datetimes.
    - The datetime is GMT, and the format can be "2024-04-24T06:30:00Z" or "Wed, 24 April 2024 06:30:00 GMT".


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

## Reference

- [microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet)


