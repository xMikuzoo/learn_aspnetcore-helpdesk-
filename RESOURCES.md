# ASP.NET Core dla frontendowca — Resources

Pełne notatki z cytatami: [research/aspnet-learning-path.md](research/aspnet-learning-path.md) (60 źródeł, zweryfikowane 2026-09-04). Poniżej tylko to, po co sięgamy w lekcjach.

## Knowledge

### Szkielet, minimal API, .NET 10
- [Microsoft Learn: Tutorial — Create a Minimal API with ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-10.0)
  Oficjalny tutorial: Todo API z EF Core in-memory. Use for: lekcja 0001 (anatomia Program.cs), lekcja 0003 (MapGet/MapPost, TypedResults), lekcja 0008 (EF Core). Uwaga: tutorial używa `dotnet new webapi`, my zaczynamy od `dotnet new web`.
- [Microsoft Learn: APIs overview — minimal vs controllers](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-10.0)
  Stanowisko Microsoftu: „For new projects, we recommend using Minimal APIs”. Use for: rozmowy o tym, czemu firmowy kod ma kontrolery, a my nie.
- [Microsoft Learn: How to create responses in Minimal API apps](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/responses?view=aspnetcore-10.0)
  `Results.*` vs `TypedResults.*`, kody statusu. Use for: lekcja 0003.
- [dotnet/aspnetcore — źródło szablonu `web` (release/10.0)](https://raw.githubusercontent.com/dotnet/aspnetcore/release/10.0/src/ProjectTemplates/Web.ProjectTemplates/content/EmptyWeb-CSharp/Program.cs)
  Dowód, że cztery linijki z lekcji 0001 to dokładnie to, co generuje SDK.
- [Microsoft Learn: Default templates for dotnet new](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new-sdk-templates)
  `web`, `webapi` (domyślnie minimal, `-controllers` przełącza), `webapiaot`, `apicontroller`.
- [.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)
  .NET 10 = LTS do 2028-11-14. .NET 8 kończy wsparcie 2026-11-10. Use for: uzasadnienie `net10.0`.

### Dependency Injection
- [Microsoft Learn: .NET dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
  Podstawa: co to serwis, kontener, rejestracja. Use for: lekcja 0002.
- [Microsoft Learn: Service lifetimes](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/service-lifetimes)
  Definicje Singleton/Scoped/Transient słowo w słowo. Use for: lekcja 0002 — quiz i tabela.
- [Microsoft Learn: Dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-guidelines)
  Captive dependency, wyjątek „Cannot consume scoped service … from singleton …”, anti-patterny. Use for: lekcja 0002 (celowy błąd), przegląd firmowego kodu.
- [Microsoft Learn: Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-10.0)
  Wersja specyficzna dla web: scope = request, wstrzykiwanie do endpointów. Use for: lekcja 0002–0003.

### Vertical slices, Clean Architecture, CQRS
- [Jimmy Bogard: Vertical Slice Architecture (2018)](https://www.jimmybogard.com/vertical-slice-architecture/)
  Tekst źródłowy pojęcia. „Minimize coupling between slices, and maximize coupling in a slice.” Use for: lekcja 0004.
- [jbogard/ContosoUniversityDotNetCore-Pages](https://github.com/jbogard/ContosoUniversityDotNetCore-Pages)
  Referencyjna aplikacja Bogarda w slices, na ASP.NET Core 10. Use for: podglądanie, jak wygląda „prawdziwy” slice.
- [Milan Jovanović: Vertical Slice Architecture (2023)](https://www.milanjovanovic.tech/blog/vertical-slice-architecture)
  Nowocześniejszy zapis z minimal API. Use for: lekcja 0004, struktura folderów.
- [Microsoft Learn: Common web application architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
  Rozdział e-booka Microsoftu o Clean Architecture; kierunek zależności, warstwy. Use for: lekcja 0009.
- [Milan Jovanović: Clean Architecture Folder Structure](https://www.milanjovanovic.tech/blog/clean-architecture-folder-structure)
  Konkretna struktura Domain/Application/Infrastructure/Presentation. Use for: lekcja 0009, mapowanie na firmowe repo.
- [ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture) · [jasontaylordev/CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture)
  Dwa najpopularniejsze szablony. Jason Taylor: .NET 10 + MediatR + FluentValidation + EF Core 10 — prawdopodobnie najbliższy temu, co masz w pracy. Use for: lekcja 0009, czytanie cudzego kodu.
- [Microsoft Learn: Apply simplified CQRS and DDD patterns](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/apply-simplified-microservice-cqrs-ddd-patterns)
  „Simplified CQRS” = osobne klasy, wspólna baza. Use for: lekcja 0005 — żeby nie przesadzić.
- [Milan Jovanović: CQRS Pattern With MediatR](https://www.milanjovanovic.tech/blog/cqrs-pattern-with-mediatr)
  `ICommand`/`IQuery` markery, handlery. Use for: lekcja 0005–0006.

### Mediator / dispatcher
- [MediatR README (LuckyPennySoftware)](https://raw.githubusercontent.com/LuckyPennySoftware/MediatR/main/README.md) · [IPipelineBehavior.cs](https://raw.githubusercontent.com/LuckyPennySoftware/MediatR/main/src/MediatR/IPipelineBehavior.cs)
  Aktualna wersja 14.2.0. Sygnatura: `Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)`. Use for: lekcja 0006–0007.
- [Jimmy Bogard: AutoMapper and MediatR Going Commercial (2025-04)](https://www.jimmybogard.com/automapper-and-mediatr-going-commercial/) · [Launch (2025-07)](https://www.jimmybogard.com/automapper-and-mediatr-commercial-editions-launch-today/) · [Licensing FAQ](https://luckypennysoftware.com/faq)
  MediatR ≥ 13 = RPL-1.5 lub licencja komercyjna. Darmowe (Community) dla firm < 5M USD przychodu i do nauki. 12.x zostaje Apache-2.0. Use for: lekcja 0006 — decyzja, co wybrać; pytanie do zespołu, jakiej wersji używacie.
- [martinothamar/Mediator](https://github.com/martinothamar/Mediator)
  MIT, source generator, `services.AddMediator()`, prawie to samo API co MediatR. Use for: alternatywa w lekcji 0006, jeśli firma ucieka od MediatR.
- [Wolverine: for MediatR users](https://wolverinefx.net/introduction/from-mediatr) · [Wolverine as Mediator](https://wolverinefx.net/tutorials/mediator)
  MIT, konwencje zamiast interfejsów, dużo większy. Use for: tylko do wiadomości; nie w kursie, chyba że firma go używa.

### Walidacja
- [FluentValidation: Creating your first validator](https://docs.fluentvalidation.net/en/latest/start.html) · [Installation](https://docs.fluentvalidation.net/en/latest/installation.html) · [DI](https://docs.fluentvalidation.net/en/latest/di.html)
  `AbstractValidator<T>`, `RuleFor`, `AddValidatorsFromAssembly`. Wersja 12.1.1, Apache-2.0. Use for: lekcja 0007.
- [FluentValidation: ASP.NET Core integration](https://docs.fluentvalidation.net/en/latest/aspnet.html)
  Docs mówią wprost: auto-walidacja ASP.NET to legacy, używaj ręcznej / pipeline behavior. Use for: lekcja 0007 — czemu walidacja siedzi w behaviorze, nie w endpoincie.
- [Milan Jovanović: CQRS Validation Pipeline with MediatR and FluentValidation](https://www.milanjovanovic.tech/blog/cqrs-validation-with-mediatr-pipeline-and-fluentvalidation)
  Kompletny `ValidationBehavior<TRequest,TResponse>` + `AddOpenBehavior(typeof(ValidationBehavior<,>))`. Use for: lekcja 0007 — wzorzec, który najpewniej macie w pracy.

### EF Core
- [Microsoft Learn: Getting Started with EF Core](https://learn.microsoft.com/en-us/ef/core/get-started/overview/first-app?tabs=netcore-cli)
  SQLite, `DbContext`, `DbSet`, pierwsza migracja. Use for: lekcja 0008.
- [Microsoft Learn: DbContext lifetime, configuration](https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/)
  `AddDbContext` rejestruje jako Scoped — łączy się z lekcją 0002. Use for: lekcja 0008.
- [Microsoft Learn: Migrations overview](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli) · [EF Core CLI reference](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
  `dotnet ef migrations add` / `database update`; wymaga pakietu `Microsoft.EntityFrameworkCore.Design`. Use for: lekcja 0008.

## Wisdom (Communities)

- [C# Discord (oficjalny link Microsoftu: aka.ms/csharp-discord)](https://aka.ms/csharp-discord)
  Duży, moderowany, kanały pomocowe. Use for: szybkie pytania o składnię i DI.
- [.NET Community — strona Microsoftu](https://dotnet.microsoft.com/en-us/platform/community)
  Lista oficjalnych kanałów (Discord, YouTube, .NET Foundation). Use for: punkt startowy.
- r/dotnet, r/csharp, Stack Overflow tag `asp.net-core` — NIEZWERYFIKOWANE (strony zablokowane w tym środowisku), ale powszechnie uznawane. Use for: pytania architektoniczne (r/dotnet lubi dyskusje o CQRS/MediatR).
- Polska scena: [Dotnetos](https://dotnetos.org/) (konferencja + społeczność, Discord istnieje, link do zdobycia), [Programistok](https://programistok.org/) (konferencja, 25–26.09.2026, Białystok), [devstyle.pl](https://devstyle.pl/start) (kursy/społeczność). Use for: networking po polsku.
- **Najlepsza społeczność: twój własny zespół backendowy.** Każda lekcja daje pytanie do zadania seniorowi („czemu u nas X?”). To najkrótsza droga do wisdom.

## Gaps
- Brak pierwotnego źródła dla „hand-rolled dispatcher” (jest tylko blog społecznościowy, codewithmukesh). Lekcja 0006 opiera własny dispatcher na sygnaturach MediatR jako wzorcu.
- Nie wiadomo, której wersji MediatR (12 vs 13+) i jakiego szablonu Clean Architecture używa firma. Do ustalenia przed lekcją 0006 i 0009.
- Brak zweryfikowanego zasobu o Result pattern / ProblemDetails (lekcja 0010) — do wyszukania.
- Brak zasobu o testach integracyjnych z `WebApplicationFactory` (lekcja 0011) — do wyszukania.
