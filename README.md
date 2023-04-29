# ASP.NET Core Vertical Slice Demo
Esta es una aplicación demo que estoy usando en mi [blog](https://dev.to/isaacOjeda), es una [serie de publicaciones](https://dev.to/isaacojeda/series/17547) que sigo actualizando día con día.

Cada publicación contiene el código que corresponde a esa publicación, de las partes 1 - 7 se encuentran en este [repositorio compartido](https://github.com/isaacOjeda/DevToPosts/tree/main/MediatrValidationExample). Partiendo del 8 en delante, se encontrarán en distintos branches de este repositorio (comentario random).

## Publicaciones
- [Implementando CQRS en una Web API](https://dev.to/isaacojeda/parte-1-cqrs-y-mediatr-implementando-cqrs-en-aspnet-56oe)
- [Validaciones con FluentValidation](https://dev.to/isaacojeda/parte-2-cqrs-y-mediatr-validando-con-fluentvalidation-14i0)
- [AutoMapper](https://dev.to/isaacojeda/parte-3-cqrs-y-mediatr-automapper-249n)
- [URLs seguros con HashIds](https://dev.to/isaacojeda/parte-4-cqrs-y-mediatr-urls-seguros-con-hashids-3dc9)
- [Identity Core y JWT](https://dev.to/isaacojeda/part-aspnet-identity-core-y-jwt-1l84)
- [Refactorizando a Vertical Slice Architecture](https://dev.to/isaacojeda/parte-6-aspnet-refactorizando-la-solucion-vertical-slice-architecture-d39)
- [Creando Sistemas Auditables](https://dev.to/isaacojeda/parte-7-aspnet-creando-un-sistema-auditable-31nf)
- [Logging con Serilog](https://dev.to/isaacojeda/parte-9-aspnet-core-logging-con-serilog-48o4)
- [Refresh Tokens](https://dev.to/isaacojeda/parte-10-aspnet-refresh-tokens-4em)
- [Application Performance Monitoring](https://dev.to/isaacojeda/parte-11-aspnet-core-application-insights-y-serilog-3103)
- [Background Jobs y Queues](https://dev.to/isaacojeda/parte-12-azure-functions-background-jobs-42ih)
- [EF Core: Dynamic sort](https://dev.to/isaacojeda/parte-13-ef-core-dynamic-sort-con-linq-expressi-nj9)
- Próximamente más⌛...

## Temas de las publicaciones
La intención de esta serie de posts es para aprender sobre muchos temas y conceptos de desarrollo web con ASP.NET Core. Todo se ha estado haciendo utilizando el mismo proyecto con la intención de llegar a una solución siguiendo todos principios DDD y Vertical Slice Architecture.

## Tecnologías usadas

- [MediatR](https://github.com/jbogard/MediatR) para CQRS
- [AutoMapper](https://github.com/AutoMapper/AutoMapper) para mapeo de objetos
- [FluentValidation](https://github.com/FluentValidation/FluentValidation) para validación de entrada
- [Hashids.net](https://github.com/ullmark/hashids.net) para crear IDs seguros
- [Audit.NET](https://github.com/thepirat000/Audit.NET) para auditar con persistencia en [blobs](https://github.com/thepirat000/Audit.NET/blob/master/src/Audit.NET.AzureStorageBlobs/README.md)
- [ASP.NET Identity Core](https://github.com/dotnet/aspnetcore/tree/main/src/Identity) para seguridad y usuarios
- [Serilog](https://github.com/serilog/serilog)
- [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview)
- [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview)
- Próximamente más⌛...
