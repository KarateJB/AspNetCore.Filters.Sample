# ASP.NET Core Filter Sample

## Tutorials

- [[MVC] Pass parameters to ActionFilterAttribute](https://karatejb.blogspot.com/2017/09/mvc-pass-parameters-to.html)
- [[ASP.Net Core] Action Filter with Parameter(s)](https://karatejb.blogspot.com/2019/07/aspnet-core-action-filter-with.html)
- [[ASP.Net Core] Hybrid Filter](https://karatejb.blogspot.com/2019/07/aspnet-core-hybrid-filter.html)


## Feature Management

### Custom feature provider

To test custom feature provider, use [json-server](https://github.com/typicode/json-server) to load the `feature_management.json` and listen on port: `3000`.

```s
$ cd src\AspNetCore.Filters.WebApi\Assets
$ json-server --watch feature_management.json
```