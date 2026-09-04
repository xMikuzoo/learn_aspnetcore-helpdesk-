# ASP.NET Core learning path for a Vue/TypeScript developer — verified sources

Research date: 2026-09-04. Every factual claim below carries an inline citation `[n]` to a source that was actually fetched during this research session (see **Sources** at the end). Anything that could not be confirmed against a fetched primary source is marked **UNVERIFIED**.

Target reader: a frontend developer (JS/TS, Vue) who must become productive in an ASP.NET Core backend that uses DI, minimal APIs and/or controllers, Clean Architecture / Vertical Slices, CQRS with MediatR-style handlers, FluentValidation pipeline behaviors, and EF Core with SQLite.

Local machine context: .NET SDK 10.0.400 and 8.0.424 are installed. See section 0 for what that means.

---

## 0. Which .NET to target in 2026

**Best primary source:** Microsoft .NET support policy page [11] — the official, machine-independent statement of which versions are supported and for how long.

**Supporting:** `dotnet/core` release notes README for .NET 10 [12]; the .NET 10 download page (which maps SDK band → runtime patch) [13].

**Key facts**

- .NET 10 is **LTS**. Released **November 11, 2025**, end of support **November 14, 2028**; latest patch at time of research is **10.0.11 (August 11, 2026)** [11]. The release notes README states verbatim: ".NET 10 is a Long Term Support (LTS) release" [12].
- LTS definition, quoted: "LTS releases are supported for three years after the initial release." STS definition, quoted: "STS releases are supported for one year after a subsequent release. Releases happen every 12 months so the support period for STS is two years." [11]
- .NET 9 (STS) and .NET 8 (LTS) **both** reach end of support on **November 10, 2026** [11]. The installed 8.0.424 SDK is therefore ~2 months from end of life; new work should target `net10.0`.
- The installed **SDK 10.0.400** corresponds to **.NET Runtime 10.0.11**, released August 11, 2026, and ships C# 14.0 [13]. SDK 10.0.400 is listed as requiring Visual Studio 2026 (v18.9) if you use VS; VS Code + C# Dev Kit does not have that constraint [13][6].
- The `dotnet new` template docs confirm the default target framework for `web`, `webapi`, `webapiaot`, `mvc`/`webapp` under the 10.0 SDK is `net10.0` [8].

---

## 1. Dependency injection and service lifetimes

**Best primary source:** ".NET dependency injection" on Microsoft Learn [1] together with its sibling page "Service lifetimes" [2]. These are the canonical framework docs, maintained by the .NET team; they define the vocabulary (service, container, `IServiceCollection`, `IServiceProvider`) and each lifetime precisely.

**Supporting:** "Dependency injection guidelines" [3] (anti-patterns incl. captive dependency, disposal rules); "Dependency injection in ASP.NET Core" [4] (ASP.NET-specific: `builder.Services.AddScoped<...>()`, request scope, middleware).

**Key facts**

- Concept: "A *dependency* is an object that another object depends on." DI "addresses hard-coded dependency problems through: The use of an interface or base class to abstract the dependency implementation. Registration of the dependency in a *service container*." ".NET provides a built-in service container, `IServiceProvider`. Services are typically registered at the app's start-up and appended to an `IServiceCollection`." Injection happens via the constructor: "The framework takes on the responsibility of creating an instance of the dependency and disposing of it when it's no longer needed." [1]
- The three lifetimes, quoted from [2]:
  - **Transient**: "A service with a *transient* lifetime is created each time it's requested from the service container. To register a service as transient, call `AddTransient`." "In apps that process requests, transient services are disposed at the end of the request."
  - **Scoped**: "For web applications, a *scoped* lifetime indicates that services are created once per client request (connection). In apps that process requests, scoped services are disposed at the end of the request. Register scoped services by calling `AddScoped`."
  - **Singleton**: "Singleton lifetime services are created either: The first time they're requested. By the developer, when providing an implementation instance directly to the container." "Every subsequent request of the service implementation from the dependency injection container uses the same instance." "Singleton services must be thread safe and are often used in stateless services."
- EF Core note (same page): "When using Entity Framework Core, the `AddDbContext` extension method registers `DbContext` types with a scoped lifetime by default." [2]
- ASP.NET Core registration syntax (from [4]):
  ```csharp
  builder.Services.AddScoped<IMyDependency, MyDependency>();
  ```
  and the lifetime comparison sample:
  ```csharp
  builder.Services.AddTransient<IOperationTransient, Operation>();
  builder.Services.AddScoped<IOperationScoped, Operation>();
  builder.Services.AddSingleton<IOperationSingleton, Operation>();
  ```
  Observed output: "*Transient* objects are always different... *Scoped* objects are the same for a given request... *Singleton* objects are the same for every request" [4].
- **Captive dependency** (the classic lifetime bug), quoted from [3]: "The term 'captive dependency', coined by Mark Seemann, refers to the misconfiguration of service lifetimes, where a longer-lived service holds a shorter-lived service captive." Example: `Foo` registered singleton depends on `Bar` registered scoped — "`Foo` is only instantiated once, and it holds onto `Bar` for its lifetime, which is longer than the intended scoped lifetime of `Bar`." With scope validation "you get an `InvalidOperationException` with a message similar to 'Cannot consume scoped service 'Bar' from singleton 'Foo'.'" [3]
- Scope validation is on by default in Development: "When an app runs in the development environment and calls `CreateApplicationBuilder` to build the host, the default service provider performs checks to verify that: Scoped services aren't resolved from the root service provider. Scoped services aren't injected into singletons." [1]
- Rule of thumb from [2]: "Do ***not*** resolve a scoped service directly from a singleton... Doing so causes the scoped service to behave like a singleton." "It's also fine to: Resolve a singleton service from a scoped or transient service. Resolve a scoped service from another scoped or transient service."
- Other guidelines worth teaching early [3]: "Avoid using the *service locator pattern*"; "Avoid calls to `BuildServiceProvider` when configuring services"; "Only use singleton lifetime for services with their own state that is expensive to create or globally shared"; "`async/await` and `Task` based service resolution isn't supported."
- Disposal: "Services resolved from the container should never be disposed by the developer." Transient and scoped are disposed at end of scope/request; singletons at container disposal [3].
- Keyed services exist (`AddKeyedSingleton<IMessageWriter, MemoryMessageWriter>("memory")` + `[FromKeyedServices("queue")]`) [1] — useful to recognise in code, not a day-one topic.

**Mental model for a Vue dev:** `builder.Services` is a typed provide/inject registry that the framework resolves for you per request; "scoped" ≈ one instance per HTTP request, "singleton" ≈ module-level constant, "transient" ≈ `new` every time.

---

## 2. Minimal APIs vs controllers, and the .NET 10 templates

**Best primary source:** "APIs overview" (ASP.NET Core 10 docs) [5] — the official comparison and the official recommendation.

**Supporting:** "Tutorial: Create a Minimal API with ASP.NET Core" (.NET 10) [6]; "How to create responses in Minimal API apps" (`TypedResults` vs `Results`) [7]; "Default templates for `dotnet new`" [8]; the actual `web` template source in `dotnet/aspnetcore` release/10.0 [9]; controller tutorial [10].

**Key facts — the recommendation**

- Quoted from [5]: "ASP.NET Core provides two approaches for building HTTP APIs: **Minimal APIs** and controller-based APIs. **For new projects, we recommend using Minimal APIs** as they provide a simplified, high-performance approach for building APIs with minimal code and configuration."
- When to still choose controllers, quoted from [5]: "Consider controller-based APIs if you need: Model binding extensibility (`IModelBinderProvider`, `IModelBinder`); Advanced validation features (`IModelValidator`); Application parts or the application model; OData support." And: "Most of these features can be implemented in Minimal APIs with custom solutions, but controllers provide them out of the box."
- Smallest possible minimal API (from [5]):
  ```csharp
  var app = WebApplication.Create(args);

  app.MapGet("/", () => "Hello World!");

  app.Run();
  ```
  Route parameters bind by name and type:
  ```csharp
  app.MapGet("/users/{userId}/books/{bookId}",
      (int userId, int bookId) => $"The user id is {userId} and book id is {bookId}");
  ```
- Controller shape (from [5]): `builder.Services.AddControllers(); ... app.MapControllers();` in `Program.cs`, and a class decorated `[ApiController] [Route("[controller]")] public class WeatherForecastController : ControllerBase` with `[HttpGet]` methods.

**Key facts — `MapGet` / `MapPost` / `TypedResults` in .NET 10 (from the tutorial [6])**

- Endpoints with DI-injected `DbContext` and body binding:
  ```csharp
  app.MapGet("/todoitems", async (TodoDb db) =>
      await db.Todos.ToListAsync());

  app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
      await db.Todos.FindAsync(id)
          is Todo todo
              ? Results.Ok(todo)
              : Results.NotFound());

  app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
  {
      db.Todos.Add(todo);
      await db.SaveChangesAsync();

      return Results.Created($"/todoitems/{todo.Id}", todo);
  });
  ```
- Route groups: "`var todoItems = app.MapGroup("/todoitems");`" then `todoItems.MapPost("/", CreateTodo);` [6].
- `TypedResults`: "Returning `TypedResults` rather than `Results` has several advantages, including testability and automatically returning the response type metadata for OpenAPI to describe the endpoint." [6] Example from the tutorial:
  ```csharp
  static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
  {
      db.Todos.Add(todo);
      await db.SaveChangesAsync();

      return TypedResults.Created($"/todoitems/{todo.Id}", todo);
  }
  ```
- From the responses page [7]: "The `TypedResults` class is the *typed* equivalent of the `Results` class. However, the `Results` helpers' return type is `IResult`, while each `TypedResults` helper's return type is one of the `IResult` implementation types." Also: "`TypedResults` requires the use of `Results<T1, TN>` from such delegates" when one handler returns several result types. Return-value rules: `string` → `text/plain`; any other `T` → JSON; `IResult` → `ExecuteAsync` [7].
- .NET 10 behaviour change worth knowing: "Starting with ASP.NET Core 10, known API endpoints no longer redirect to login pages when using cookie authentication. Instead, they return 401/403 status codes." [7]

**Key facts — `dotnet new` templates in the .NET 10 SDK (from [8])**

| Short name | Template | Notes |
|---|---|---|
| `web` | ASP.NET Core Empty | Options: `-f`, `--no-https`, `--use-program-main`, `--exclude-launch-settings`, Kestrel port options |
| `webapi` | ASP.NET Core Web API | **Minimal API by default.** `-minimal\|--use-minimal-apis` and `-controllers\|--use-controllers` exist; `--no-openapi` turns off `AddOpenApi`/`MapOpenApi` |
| `webapiaot` | ASP.NET Core API (Native AOT) | Introduced 8.0 |
| `apicontroller` | API controller item template | `-ac\|--actions` |
| `mvc` | Web App (MVC) | |
| `webapp` / `razor` | Web App (Razor Pages) | |

Quoted from the `webapi` section [8]: "`-minimal|--use-minimal-apis` — Create a project that uses the ASP.NET Core minimal API. Default is `false`, but this option is overridden by `-controllers`. Since the default for `-controllers` is `false`, entering `dotnet new webapi` without specifying either option creates a minimal API project." And: "`-controllers|--use-controllers` — Whether to use controllers instead of minimal APIs. If both this option and `-minimal` are specified, this option overrides the value specified by `-minimal`. Default is `false`. Available since .NET 8 SDK."

So the three practical commands are:

```
dotnet new web -o MyApp                    # empty: one MapGet("/")
dotnet new webapi -o MyApi                 # minimal API + OpenAPI (default)
dotnet new webapi --use-controllers -o MyApi   # controllers
```

The controller tutorial [10] uses exactly `dotnet new webapi --use-controllers -o TodoApi`; the minimal tutorial [6] uses `dotnet new webapi -o TodoApi`.

**Verification of the `dotnet new web` Program.cs.** The `EmptyWeb-CSharp` template content in the `dotnet/aspnetcore` repo, branch `release/10.0`, is verbatim [9]:

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
```

This matches the expected snippet exactly (modulo the template engine's conditional `--use-program-main` variant, which is not shown here). Note the .NET 10 minimal API *tutorial* on Learn does **not** use `dotnet new web`; it uses `dotnet new webapi`, whose generated `Program.cs` is larger (it adds `builder.Services.AddOpenApi();`, `app.MapOpenApi()` in Development, `app.UseHttpsRedirection();` and a `/weatherforecast` `MapGet`) [6].

---

## 3. Clean Architecture

**Best primary source:** "Common web application architectures" from Microsoft's *Architect Modern Web Applications with ASP.NET Core and Azure* e-book on Learn [14] — an official, vendor-maintained explanation of Clean Architecture in ASP.NET Core terms, including which types live in which project.

**Supporting:** `ardalis/CleanArchitecture` template repo [15] (Microsoft's own doc links to it as *the* starter template); `jasontaylordev/CleanArchitecture` [16] (the other widely used template, on .NET 10, ships MediatR + FluentValidation + EF Core); Milan Jovanović's folder-structure post [17].

**Key facts**

- Naming and lineage, quoted [14]: "Applications that follow the Dependency Inversion Principle as well as the Domain-Driven Design (DDD) principles tend to arrive at a similar architecture. This architecture has gone by many names over the years. One of the first names was Hexagonal Architecture, followed by Ports-and-Adapters. More recently, it's been cited as the Onion Architecture or Clean Architecture."
- The rule, quoted [14]: "Clean architecture puts the business logic and application model at the center of the application. Instead of having business logic depend on data access or other infrastructure concerns, this dependency is inverted: infrastructure and implementation details depend on the Application Core. This functionality is achieved by defining abstractions, or interfaces, in the Application Core, which are then implemented by types defined in the Infrastructure layer."
- What goes where [14]:
  - *Application Core*: "Entities... Aggregates... Interfaces, Domain Services, Specifications, Custom Exceptions and Guard Clauses, Domain Events and Handlers."
  - *Infrastructure*: "EF Core types (`DbContext`, `Migration`), Data access implementation types (Repositories), Infrastructure-specific services (for example, `FileLogger` or `SmtpNotifier`)."
  - *UI layer*: "Controllers, Custom Filters, Custom Middleware, Views, ViewModels, Startup." "The `Startup` class or *Program.cs* file is responsible for configuring the application, and for wiring up implementation types to interfaces. The place where this logic is performed is known as the app's *composition root*."
  - The pragmatic exception: "In order to wire up dependency injection during app startup, the UI layer project may need to reference the Infrastructure project... developers should limit actual references to types in the Infrastructure project to the app's composition root." [14]
- Microsoft points to the template: "You can find a solution template you can use as a starting point for your own ASP.NET Core solutions in the ardalis/cleanarchitecture GitHub repository or by installing the template from NuGet." [14] The template's own README describes projects **Core, UseCases, Infrastructure, Web**, installs as NuGet package `Ardalis.CleanArchitecture.Template`, and (at fetch time) states "The main branch is now using **.NET 9**. This corresponds with NuGet package version 10.x." [15]
- Jason Taylor's template: `dotnet new install Clean.Architecture.Solution.Template`; main branch supports ".NET 10.0 SDK or later"; technologies listed include ASP.NET Core 10, Entity Framework Core 10, MediatR, AutoMapper, FluentValidation, NUnit/Shouldly/Moq/Respawn, Scalar [16]. (Its README has no note about MediatR licensing [16] — see section 5; there is a GitHub discussion on its repo about the license warning, surfaced via search but **not fetched**, so treat as UNVERIFIED.)
- Four-layer vocabulary common in blogs (Domain / Application / Infrastructure / Presentation) [17]: "The Domain layer sits at the core of the Clean Architecture." "The Domain layer is not allowed to reference other projects in your solution." "The Application layer sits right above the Domain layer. It acts as an orchestrator for the Domain layer, containing the most important use cases in your application." (post dated 2022-09-24) [17].

---

## 4. Vertical Slice Architecture

**Best primary source:** Jimmy Bogard, "Vertical Slice Architecture" (2018-04-19) [18] — the post that coined/popularised the term in the .NET world; Bogard is also the author of MediatR.

**Supporting:** Bogard's reference implementation `jbogard/ContosoUniversityDotNetCore-Pages` (now on ASP.NET Core 10 / .NET 10; README lists "CQRS and MediatR", "Vertical slice architecture", "Fluent Validation", "Entity Framework Core") [20]; Milan Jovanović, "Vertical Slice Architecture" (2023-11-04) [19].

**Key facts**

- Definition, quoted [18]: the architecture is "built around distinct requests, encapsulating and grouping all concerns from front-end to back."
- The guiding rule, quoted [18]: "Minimize coupling between slices, and maximize coupling in a slice."
- Approach, quoted [18]: treat "each request as a distinct use case in how to approach its code" — each slice may pick its own implementation strategy rather than being forced through mandated repository/service abstractions; the post describes removing "the gates and barriers across those layers" [18].
- Trade-off, paraphrased from [18]: it requires a team comfortable with refactoring and recognising code smells; without that it degrades.
- Jovanović's framing [19]: "All the files for a single use case live in one folder. This minimizes coupling between unrelated features and maximizes coupling inside a single feature." Layered architecture yields "high coupling inside a layer and low coupling between layers," which vertical slices invert. Benefit: "New features only add code, you're not changing shared code and worrying about side effects." Drawback: slices can accumulate too much and need refactoring into the domain model. Example structure: `Features/<Entity>/<Operation>/` holding request, endpoint, handler and validator files (REPR pattern) [19].

**How it relates to CQRS/MediatR:** In practice a "slice" is one request type + its handler (+ validator) — which is precisely the `IRequest`/`IRequestHandler` pair from MediatR (section 5). Bogard's own reference app combines the two [20].

---

## 5. CQRS with separate command/query handlers

**Best primary source:** Microsoft's *.NET Microservices* e-book chapter "Apply simplified CQRS and DDD patterns in a microservice" [21] — official, defines CQS vs CQRS and the "simplified CQRS" that most ASP.NET codebases actually use.

**Supporting:** Milan Jovanović, "CQRS Pattern With MediatR" (2023-10-21) [22]; Bogard's ContosoUniversity sample [20].

**Key facts**

- CQS origin, quoted [21]: "The basic idea is that you can divide a system's operations into two sharply separated categories: Queries. These queries return a result and don't change the state of the system, and they're free of side effects. Commands. These commands change the state of a system."
- CQRS, quoted [21]: "Command and Query Responsibility Segregation (CQRS) was introduced by Greg Young and strongly promoted by Udi Dahan and others. It's based on the CQS principle, although it's more detailed." And, importantly for expectations: "This guide uses the simplest CQRS approach, which consists of just separating the queries from the commands." "CQRS means having two objects for a read/write operation where in other contexts there's one."
- Same database is fine: "Each layer has its own data model (note that we say model, not necessarily a different database)." [21]
- Typical .NET shape [22]: "Command Query Responsibility Segregation (CQRS) gives commands and queries their own models, so you can optimize writes and reads independently." Practice: define `ICommand`/`IQuery` marker interfaces that extend MediatR's `IRequest`, and `ICommandHandler`/`IQueryHandler` that extend `IRequestHandler`; route via `ISender` [22].

---

## 6. MediatR — what it is, the 2025 licensing change, current version, alternatives

### 6.1 The library

**Best primary source:** the MediatR repository README (github.com/jbogard/MediatR now redirects to `LuckyPennySoftware/MediatR`) [23][24] plus the `IPipelineBehavior.cs` source file [25].

**Key facts**

- Registration [23]:
  ```csharp
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
  // or
  services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
  ```
  Behaviors are registered with `cfg.AddBehavior<>`, `cfg.AddStreamBehavior<>`, or `cfg.AddOpenBehavior(typeof(...<,>))` [23].
- `IPipelineBehavior` signature in the current source (verbatim from [25]):
  ```csharp
  public delegate Task<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken t = default);

  public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : notnull
  {
      Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken);
  }
  ```
  Note the `RequestHandlerDelegate` now takes an optional `CancellationToken` parameter; older blog samples call `next()` with no argument, which still compiles.
- NuGet packages: `MediatR` and `MediatR.Contracts` [24]. Latest version on NuGet: **14.2.0 (2026-07-02)**; recent history 14.1.0 (2026-03-03), 14.0.0 (2025-12-03), 13.1.0 (2025-10-24), **13.0.0 (2025-07-02)**, 12.5.0 (2025-04-01). Owners: `jbogard`, `LuckyPennySoftware` [26].
- License key wiring [24]:
  ```csharp
  services.AddMediatR(cfg =>
  {
      cfg.LicenseKey = "<license key here>";
  })
  ```
  Keys are also discovered from `MEDIATR_LICENSE_KEY` or `LUCKYPENNY_LICENSE_KEY` environment variables, and "The license key does not need to be set on client applications (such as Blazor WASM)" [23][24].

### 6.2 What happened with licensing in 2025 (verified)

**Best primary sources:** Jimmy Bogard's two posts [27][28], the Lucky Penny Software FAQ [30], the repository `LICENSE.md` [29], and the launch discussion on the repo [31].

- **2025-04-02 — announcement.** Bogard: "In order to ensure the long-term sustainability of my OSS projects, I will be commercializing AutoMapper and MediatR." Reason given: since leaving Headspring in 2020 he had no sponsor for OSS time; "I need to be able to pay for my time to work on these projects." At that point "nothing will change" short-term and the model was undecided [27].
- **2025-07-02 — commercial editions launched.** "dual-license model": Reciprocal Public License 1.5 (RPL-1.5) **or** the Lucky Penny Software Commercial License. First commercial versions: **MediatR v13.0** and AutoMapper v15.0 [28]. The repo `LICENSE.md` is titled "Reciprocal Public License 1.5 (RPL1.5)" and says that if you do not want RPL terms you may use the code under the commercial agreement at luckypennysoftware.com/license [29]. The repo discussion confirms: "Version 13.0.0 is the dual-licensed version, which has both a FOSS license and commercial license." [31]
- **Community (free) edition criteria** [28][30]: "Companies and individuals **under $5,000,000** in gross annual revenue"; "Non-profits **under $5,000,000** in annual total budget"; "Educational/classroom use"; "Non-production environments" [28]. The FAQ adds: annual gross revenue < $5,000,000 USD **and** "Has not received more than $10,000,000 USD in outside capital"; not government/quasi-government; universities excluded for "institutional / operational software" but classroom/research use stays eligible [30].
- **Commercial tiers** by team size: Standard (1–10 developers), Professional (11–50), Enterprise (unlimited); "within your tier, you can grow your team without buying additional licenses." Prices are not on the FAQ page [30].
- **Behaviour without a key**: "Log warnings only. The library continues to function normally – no degraded performance, no feature lockout." [30]
- **Older versions**: "Two packages: AutoMapper (version 15.0.0 and later) and MediatR (version 13.0.0 and later)" require the new license; earlier versions "remain under their original open-source licenses (Apache 2.0 or MIT)" [30]. Bogard: "Per those existing license agreements, you're free to fork, download, print out and read by the fireplace." [28] Support for v12 and earlier is "the same support as they did before this change—that is, none" [31].

**Practical implication for the learner:** the concepts (request → handler, pipeline behaviors) are the same in MediatR 12 (Apache-2.0), MediatR 13/14 (RPL/commercial with free community tier), and the alternatives below. Learn the pattern, then check which package and version the employer's repo pins and whether the company falls under the $5M community threshold.

### 6.3 Free alternatives

**(a) Mediator by Martin Othamar — `martinothamar/Mediator`** [32][33]

- Note: the task brief spelled the author as "martindevans"; `github.com/martindevans/Mediator` returns **404**. The library is **`martinothamar/Mediator`** (NuGet owner `martinothamar`) [32][33].
- "A high performance .NET implementation of the Mediator pattern using source generators." It offers "a similar API to the great MediatR library while delivering better performance and full Native AOT support." License: **MIT** [32].
- Packages: `Mediator.SourceGenerator` (in the executable project) and `Mediator.Abstractions` (where messages/handlers live). Latest NuGet: **3.0.2 (2026-03-22)**, with 3.1.0-rc.1 prerelease [33].
- Registration: `services.AddMediator();` (options include `options.ServiceLifetime = ServiceLifetime.Singleton;`) [32].
- Pipeline behavior signature differs from MediatR (ValueTask, `MessageHandlerDelegate`):
  ```csharp
  public ValueTask<TResponse> Handle(
      TMessage message,
      MessageHandlerDelegate<TMessage, TResponse> next,
      CancellationToken cancellationToken)
  ```
  [32]

**(b) Wolverine — `JasperFx/wolverine`, NuGet `WolverineFx`** [34][35][36][37][38][39][40]

- "Released under the MIT License. Copyright © Jeremy D. Miller and contributors." [34] NuGet `WolverineFx` latest **6.33.0 (2026-09-03)**, MIT, targets .NET 9.0 and higher [38].
- Handlers are discovered by convention, no interfaces: "Handler type names should be suffixed with either `Handler` or `Consumer`" and "Handler method names should be either `Handle()` or `Consume()`." [34] "There are no required interfaces on either the message type or the handler type." [35]
- Mediator usage: "Wolverine's `IMessageBus.InvokeAsync()` is the direct equivalent to MediatR's `IMediator.Send()`" [35]. `bus.InvokeAsync(cmd)` / `bus.InvokeAsync<ItemCreated>(cmd)`; set `opts.Durability.Mode = DurabilityMode.MediatorOnly;` when using it only in-process [36]. Example from [37]:
  ```csharp
  await bus.InvokeAsync(new Message1());
  ```
- Different middleware model: Wolverine generates code and "is able to peek into the IoC configuration and 'know' whether there are registered validators" instead of wrapping every request in an `IPipelineBehavior` [39]. Author's framing (2026-01-26): "Idiomatic Wolverine results in potentially less repetitive code, less code ceremony, and less layering than MediatR idioms." [40]
- Caveat for a beginner: Wolverine is a full messaging framework (outbox, queues, HTTP endpoints via `[WolverinePost]`), so it is a larger conceptual surface than MediatR [35][40].

**(c) Hand-rolled dispatcher**

- There is no primary/official source for "the" hand-rolled dispatcher; the pattern is ~100 lines of your own code. A representative, recent, code-complete community write-up (Mukesh Murugan, 2026-04-12, **not a primary source**) defines MediatR-shaped abstractions and a `FrozenDictionary`-backed `ISender` [41]:
  ```csharp
  public interface IRequest<out TResponse>;

  public interface IRequestHandler<in TRequest, TResponse>
      where TRequest : IRequest<TResponse>
  {
      ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
  }

  public interface ISender
  {
      ValueTask<TResponse> Send<TResponse>(
          IRequest<TResponse> request,
          CancellationToken cancellationToken = default);
  }
  ```
  Registration scans an assembly, `services.AddScoped(iface, type)` for each `IRequestHandler<,>` implementation, and registers `ISender` scoped [41]. Its performance claims (4.4x faster than MediatR 12.4.1) are the author's own benchmarks — **UNVERIFIED** here.
- Minimal alternative that needs no dispatcher at all: inject the concrete handler (`ICommandHandler<CreateTodo, int>`) directly into the minimal-API lambda. This is just section 1 DI; no citation needed beyond [4].

---

## 7. FluentValidation, including MediatR pipeline-behavior validation

**Best primary source:** FluentValidation official docs — "Creating your first validator" [42], "Installation" [43], "Dependency Injection" [44], "ASP.NET Core integration" [45]. Maintained by the library author (Jeremy Skinner) at docs.fluentvalidation.net.

**Supporting:** NuGet pages [46][47]; Milan Jovanović, "CQRS Validation Pipeline with MediatR and FluentValidation" (2023-09-30) [48].

**Key facts**

- Install: `dotnet add package FluentValidation` [43]. Latest NuGet: **12.1.1 (2025-12-03)**, license **Apache-2.0** [46]. DI package `FluentValidation.DependencyInjectionExtensions` **12.1.1**, Apache-2.0 [47]. Upgrading from 11.x: read the "upgrading-to-12" notes [43].
- Validator definition (verbatim from [42]):
  ```csharp
  public class CustomerValidator : AbstractValidator<Customer>
  {
    public CustomerValidator()
    {
      RuleFor(customer => customer.Surname).NotNull();
    }
  }
  ```
  `RuleFor` takes "a lambda expression that indicates the property that you wish to validate"; validators chain: `RuleFor(customer => customer.Surname).NotNull().NotEqual("foo")` [42].
- Running it: "instantiate the validator object and call the Validate method, passing in the object to validate"; the `ValidationResult` has "`IsValid` - a boolean that says whether the validation succeeded" and "`Errors` - a collection of `ValidationFailure` objects"; `validator.ValidateAndThrow(customer)` throws instead [42].
- DI registration [44]: `services.AddValidatorsFromAssemblyContaining<UserValidator>()` or `services.AddValidatorsFromAssembly(Assembly.Load("SomeAssembly"))`; "By default, these will be registered as Scoped"; consume via constructor injection of `IValidator<T>` (e.g. `public UserService(IValidator<User> validator)`).
- ASP.NET auto-validation is **legacy**: "We no longer recommend using this approach for new projects but it is still available for legacy implementations." Reasons: synchronous only, MVC-only (no Minimal APIs), hard to debug. Recommended: manual validation by injecting the validator [45].
- **MediatR pipeline behavior** (the pattern most Clean/Vertical-slice repos use), from [48]:
  ```csharp
  public sealed class ValidationBehavior<TRequest, TResponse>
      : IPipelineBehavior<TRequest, TResponse>
      where TRequest : ICommandBase
  {
      private readonly IEnumerable<IValidator<TRequest>> _validators;

      public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
      {
          _validators = validators;
      }

      public async Task<TResponse> Handle(
          TRequest request,
          RequestHandlerDelegate<TResponse> next,
          CancellationToken cancellationToken)
      {
          var context = new ValidationContext<TRequest>(request);

          var validationFailures = await Task.WhenAll(
              _validators.Select(validator => validator.ValidateAsync(context)));

          var errors = validationFailures
              .Where(validationResult => !validationResult.IsValid)
              .SelectMany(validationResult => validationResult.Errors)
              .Select(validationFailure => new ValidationError(
                  validationFailure.PropertyName,
                  validationFailure.ErrorMessage))
              .ToList();

          if (errors.Any())
          {
              throw new Exceptions.ValidationException(errors);
          }

          var response = await next();

          return response;
      }
  }
  ```
  Registration: `config.AddOpenBehavior(typeof(ValidationBehavior<,>));` inside `AddMediatR`, plus `services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly);` [48]. (`ValidationError`, `ICommandBase` and `Exceptions.ValidationException` are the blog's own types.) This compiles against the current `IPipelineBehavior` signature in [25].

---

## 8. EF Core basics: `DbContext`, SQLite provider, migrations

**Best primary source:** "Getting Started with EF Core" [49] — the official tutorial, and it uses **SQLite** specifically. Together with "DbContext Lifetime, Configuration, and Initialization" [50] for the ASP.NET Core wiring.

**Supporting:** "Migrations Overview" [51]; "EF Core tools reference – .NET CLI" [52]; NuGet pages for `Microsoft.EntityFrameworkCore.Sqlite` [53] and `dotnet-ef` [54].

**Key facts**

- Provider package: `dotnet add package Microsoft.EntityFrameworkCore.Sqlite` [49]. Latest stable **10.0.11 (2026-08-11)**, MIT, targets .NET 10 [53]. The tutorial explains: "This tutorial uses SQLite because it runs on all platforms that .NET supports." [49]
- Minimal `DbContext` (from [49]):
  ```csharp
  public class BloggingContext : DbContext
  {
      public DbSet<Blog> Blogs { get; set; }
      public DbSet<Post> Posts { get; set; }
      ...
      protected override void OnConfiguring(DbContextOptionsBuilder options)
          => options.UseSqlite($"Data Source={DbPath}");
  }
  ```
- ASP.NET Core registration (from [50], SQL Server in the doc; swap `UseSqlite` per the provider table on the same page):
  ```csharp
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(connectionString));
  ```
  "The preceding code registers `ApplicationDbContext`, a subclass of `DbContext`, as a scoped service." The context "must expose a public constructor with a `DbContextOptions<ApplicationDbContext>` parameter." The provider table lists SQLite as `.UseSqlite(connectionString)` with package `Microsoft.EntityFrameworkCore.Sqlite` [50]. So for SQLite:
  ```csharp
  builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite("Data Source=app.db"));
  ```
- Lifetime rules [50]: "A `DbContext` instance is designed to be used for a *single* unit-of-work"; "DbContext is **Not thread-safe**"; "Always await EF Core asynchronous methods immediately." In-memory provider warning: "The EF Core in-memory database is not designed for production use. In addition, it may not be the best choice even for testing." [50]
- Migrations tooling — the exact sequence from the official tutorial [49]:
  ```
  dotnet tool install --global dotnet-ef
  dotnet add package Microsoft.EntityFrameworkCore.Design
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
  "This installs dotnet ef and the design package which is required to run the command on a project. The `migrations` command scaffolds a migration to create the initial set of tables for the model. The `database update` command creates the database and applies the new migration to it." [49]
- The CLI reference [52] confirms: "`dotnet ef` can be installed as either a global or local tool"; "Before you can use the tools on a specific project, you'll need to add the `Microsoft.EntityFrameworkCore.Design` package to it." Update with `dotnet tool update --global dotnet-ef`. `dotnet-ef` latest **10.0.11 (2026-08-11)** [54].
- Migrations model, quoted [51]: "EF Core compares the current model against a snapshot of the old model to determine the differences, and generates migration source files; the files can be tracked in your project's source control like any other source file." Applying via `dotnet ef database update` "is ideal for local development, but is less suitable for production environments." Multi-project tip: `--project` / `--startup-project` when the `DbContext` lives in a class library (typical in Clean Architecture's Infrastructure project) [52].
- EF Core 11 preview additions visible in the CLI docs (not yet stable at research time): `dotnet ef database update <Name> --add` (create + apply in one step) and a `.config/dotnet-ef.json` defaults file [52]. Not needed for .NET 10 work.

---

## 9. Communities for asking questions

Verified against fetched pages:

- **Official .NET community page** [55] lists: Discord at `https://aka.ms/csharp-discord`; Stack Overflow `https://stackoverflow.com/questions/tagged/.net`; Microsoft Q&A `https://learn.microsoft.com/answers/products/dotnet`; GitHub `https://github.com/dotnet`; .NET Virtual User Group meetup; .NET Blog `https://devblogs.microsoft.com/dotnet/`.
- **C# Discord** — `aka.ms/csharp-discord` 301-redirects to `discord.gg/ccyrDKv`, whose invite page title is "C#" [57]. This is the one Microsoft's own community page links to [55].
- **".NET Discord" (aka.ms/dotnet-discord)** — 301-redirects to `discord.gg/HSuhTyG`, whose invite page title is "DotNetEvolution" [56]. Its "official Microsoft" status is asserted by search-engine summaries only — **UNVERIFIED**; the C# Discord above is the one Microsoft's community page cites.
- **Stack Overflow** — tag pages could not be fetched from this environment (blocked). Relevant tag URLs, **not verified by fetch**: `https://stackoverflow.com/questions/tagged/asp.net-core`, `.../entity-framework-core`, `.../mediatr`, `.../fluentvalidation`. The `.net` tag is linked by Microsoft's community page [55].
- **Reddit r/dotnet, r/csharp** — reddit.com could not be fetched from this environment. A third-party directory (thehiveindex, seen only in search results, **not fetched**) reports ~241K members for r/dotnet — **UNVERIFIED**.
- **Polish communities:**
  - **Dotnetos** (`dotnetos.org`) — a .NET education/community organisation ("help companies build correct, robust and efficient .NET solutions"), offers online courses (Async Expert, .NET Diagnostics Expert, .NET Memory Expert…), a "Dotnetos Conference", and "an active Discord server" [58]. Invite link not captured — **UNVERIFIED**.
  - **Programistok** (`programistok.org`) — "Ogólnopolska konferencja technologii i biznesu organizowana w sercu Podlaskiego"; next edition **25–26 September 2026, Białystok**; organised with Fundacja INFOTECH and Wydział Informatyki Politechniki Białostockiej; grew out of local meetups [59]. It is a conference, not a Q&A forum.
  - **devstyle.pl** — "Portal i szkolenia dla każdego programisty", founded 2008 by Maciej Aniserowicz, run by DEVSTYLE sp. z o.o. (Białystok); newsletter of "ponad 40 000 programistów"; community interaction is via course comments, "grupy dyskusyjne", live sessions and social media — the page does not mention a public Discord/Slack [60].
  - Other Polish .NET communities (e.g. local .NET user groups, Szkoła Dotneta, Polish Discord servers) appeared only in search results — **UNVERIFIED**.

---

## 10. Suggested sequencing (derived from the sources above)

1. **Scaffold and read** — `dotnet new web`, then `dotnet new webapi`; compare the two `Program.cs` files [8][9][6]. Learn `MapGet/MapPost`, route params, `TypedResults` [5][6][7].
2. **DI** — register a service three ways, watch the `OperationId` demo, deliberately create a captive dependency and see the scope-validation exception [4][3][2].
3. **EF Core + SQLite** — `AddDbContext` + `UseSqlite`, `dotnet-ef`, first migration [49][50][52].
4. **Controllers** — only enough to read existing code: `[ApiController]`, `[Route]`, `[HttpGet]`, `ControllerBase` [5][10].
5. **CQRS + handlers** — one command, one query, without any library (plain interfaces) [21][41]; then swap in MediatR (or the employer's choice) and add `ValidationBehavior` with FluentValidation [23][25][42][44][48].
6. **Architecture** — read Microsoft's Clean Architecture chapter [14], then Bogard's vertical-slice post [18]; open `ardalis/CleanArchitecture` or `jasontaylordev/CleanArchitecture` [15][16] and Bogard's ContosoUniversity [20] to see both styles in real repos.
7. **Licensing awareness** — check the MediatR version pinned in the work repo against the 13.0.0 boundary and the $5M community threshold [30][31].

---

## Sources

All URLs below were fetched on 2026-09-04 unless marked otherwise.

1. https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection — ".NET dependency injection" (Microsoft Learn)
2. https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/service-lifetimes — "Service lifetimes" (Microsoft Learn)
3. https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines — "Dependency injection guidelines" (Microsoft Learn)
4. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0 — "Dependency injection in ASP.NET Core" (Microsoft Learn; retrieved via docs search excerpts)
5. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-10.0 — "APIs overview" (Microsoft Learn)
6. https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0 — "Tutorial: Create a Minimal API with ASP.NET Core" (Microsoft Learn)
7. https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0 — "How to create responses in Minimal API apps" (Microsoft Learn)
8. https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates — "Default templates for dotnet new" (Microsoft Learn)
9. https://raw.githubusercontent.com/dotnet/aspnetcore/release/10.0/src/ProjectTemplates/Web.ProjectTemplates/content/EmptyWeb-CSharp/Program.cs — `web` template source, dotnet/aspnetcore release/10.0
10. https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-10.0 — "Tutorial: Create a controller-based web API" (Microsoft Learn; retrieved via docs search excerpts)
11. https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core — ".NET and .NET Core Support Policy"
12. https://github.com/dotnet/core/blob/main/release-notes/10.0/README.md — .NET 10 release notes index
13. https://dotnet.microsoft.com/en-us/download/dotnet/10.0 — .NET 10 download page (SDK ↔ runtime mapping)
14. https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures — "Common web application architectures" (Microsoft Learn e-book chapter)
15. https://github.com/ardalis/CleanArchitecture — Ardalis Clean Architecture template README
16. https://github.com/jasontaylordev/CleanArchitecture — Jason Taylor Clean Architecture template README
17. https://www.milanjovanovic.tech/blog/clean-architecture-folder-structure — Milan Jovanović, "Clean Architecture Folder Structure" (2022-09-24)
18. https://www.jimmybogard.com/vertical-slice-architecture/ — Jimmy Bogard, "Vertical Slice Architecture" (2018-04-19)
19. https://www.milanjovanovic.tech/blog/vertical-slice-architecture — Milan Jovanović, "Vertical Slice Architecture" (2023-11-04)
20. https://github.com/jbogard/ContosoUniversityDotNetCore-Pages — Bogard's vertical-slice reference app (ASP.NET Core 10)
21. https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns — "Apply simplified CQRS and DDD patterns in a microservice" (Microsoft Learn e-book chapter)
22. https://www.milanjovanovic.tech/blog/cqrs-pattern-with-mediatr — Milan Jovanović, "CQRS Pattern With MediatR" (2023-10-21)
23. https://github.com/jbogard/MediatR — MediatR README (redirects to LuckyPennySoftware/MediatR)
24. https://raw.githubusercontent.com/LuckyPennySoftware/MediatR/main/README.md — MediatR README raw
25. https://raw.githubusercontent.com/LuckyPennySoftware/MediatR/main/src/MediatR/IPipelineBehavior.cs — `IPipelineBehavior` source
26. https://www.nuget.org/packages/MediatR — MediatR on NuGet (version history)
27. https://www.jimmybogard.com/automapper-and-mediatr-going-commercial/ — Jimmy Bogard, "AutoMapper and MediatR Going Commercial" (2025-04-02)
28. https://www.jimmybogard.com/automapper-and-mediatr-commercial-editions-launch-today/ — Jimmy Bogard, "AutoMapper and MediatR Commercial Editions Launch Today" (2025-07-02)
29. https://github.com/LuckyPennySoftware/MediatR/blob/main/LICENSE.md — MediatR LICENSE.md (RPL-1.5)
30. https://luckypennysoftware.com/faq — Lucky Penny Software Licensing FAQ
31. https://github.com/LuckyPennySoftware/MediatR/discussions/1123 — "MediatR commercial version launched" discussion
32. https://github.com/martinothamar/Mediator — Mediator (source-generator) README
33. https://www.nuget.org/packages/Mediator.SourceGenerator — Mediator.SourceGenerator on NuGet
34. https://wolverinefx.net/guide/handlers/ — Wolverine docs, "Message Handlers"
35. https://wolverinefx.net/introduction/from-mediatr — Wolverine docs, "Wolverine for MediatR Users"
36. https://wolverinefx.net/tutorials/mediator — Wolverine docs, "Wolverine as Mediator"
37. https://wolverinefx.net/guide/messaging/message-bus.html — Wolverine docs, message bus / `InvokeAsync`
38. https://www.nuget.org/packages/WolverineFx — WolverineFx on NuGet
39. https://jeremydmiller.com/2025/01/28/wolverine-for-mediatr-users/ — Jeremy D. Miller, "Wolverine for MediatR Users" (2025-01-28)
40. https://jeremydmiller.com/2026/01/26/wolverine-idioms-for-mediatr-users/ — Jeremy D. Miller, "Wolverine Idioms for MediatR Users" (2026-01-26)
41. https://codewithmukesh.com/blog/cqrs-without-mediatr/ — Mukesh Murugan, "Build Your Own CQRS Dispatcher in .NET 10 (No MediatR)" (2026-04-12) — community blog, not a primary source
42. https://docs.fluentvalidation.net/en/latest/start.html — FluentValidation docs, "Creating your first validator"
43. https://docs.fluentvalidation.net/en/latest/installation.html — FluentValidation docs, "Installation"
44. https://docs.fluentvalidation.net/en/latest/di.html — FluentValidation docs, "Dependency Injection"
45. https://docs.fluentvalidation.net/en/latest/aspnet.html — FluentValidation docs, "ASP.NET Core integration"
46. https://www.nuget.org/packages/FluentValidation — FluentValidation on NuGet
47. https://www.nuget.org/packages/FluentValidation.DependencyInjectionExtensions — DI extensions on NuGet
48. https://www.milanjovanovic.tech/blog/cqrs-validation-with-mediatr-pipeline-and-fluentvalidation — Milan Jovanović, "CQRS Validation Pipeline with MediatR and FluentValidation" (2023-09-30)
49. https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli — "Getting Started with EF Core" (Microsoft Learn)
50. https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/ — "DbContext Lifetime, Configuration, and Initialization" (Microsoft Learn)
51. https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli — "Migrations Overview" (Microsoft Learn)
52. https://learn.microsoft.com/en-us/ef/core/cli/dotnet — "EF Core tools reference – .NET CLI" (Microsoft Learn)
53. https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite — SQLite provider on NuGet
54. https://www.nuget.org/packages/dotnet-ef — dotnet-ef on NuGet
55. https://dotnet.microsoft.com/en-us/platform/community — ".NET Community" (official)
56. https://aka.ms/dotnet-discord → https://discord.gg/HSuhTyG → https://discord.com/invite/HSuhTyG — invite page title "DotNetEvolution"
57. https://aka.ms/csharp-discord → https://discord.gg/ccyrDKv → https://discord.com/invite/ccyrDKv — invite page title "C#"
58. https://dotnetos.org/ — Dotnetos
59. https://programistok.org/ — Programistok
60. https://devstyle.pl/start — devstyle.pl

Not fetched (blocked or 404) and therefore not used as evidence: reddit.com, stackoverflow.com tag pages, github.com/martindevans/Mediator (404), wolverinefx.net/guide/mediator.html (404), jasontaylordev/CleanArchitecture discussion #1413 (seen in search only).
