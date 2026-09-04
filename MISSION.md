# Mission: ASP.NET Core dla frontendowca

## Why
Wojciech pracuje jako frontend developer w zespole, którego backend jest w ASP.NET Core i używa DI, Clean Architecture / Vertical Slices, CQRS (osobne command/query handlery) i walidatorów. Chce rozumieć te pojęcia na tyle, żeby czytać kod backendu ze zrozumieniem, brać udział w dyskusjach architektonicznych i samodzielnie dopisać prosty endpoint end-to-end.

## Success looks like
- Buduje od zera prosty projekt (Helpdesk API) i potrafi wyjaśnić każdą linijkę `Program.cs`.
- Rejestruje serwis w DI i wie, kiedy użyć Singleton / Scoped / Transient i dlaczego.
- Dodaje nowy feature jako vertical slice: command + handler + validator + endpoint, bez pytania seniora, gdzie co położyć.
- Rozróżnia command od query i uzasadnia, po co je rozdzielać.
- Podłącza FluentValidation do pipeline'u tak, że niepoprawny request nie dociera do handlera.
- Czyta kod firmowy z Clean Architecture i wie, w której warstwie szukać czego.

## Constraints
- Zna JS/TS/Vue, nie zna C# ani .NET (zakładamy zero; korygować gdy okaże się inaczej).
- Lekcje krótkie (10–20 min), każda kończy się działającym przyrostem projektu.
- Maszyna: Windows 11, .NET SDK 10.0.400, VS Code, dotnet-ef zainstalowany globalnie.
- Język lekcji: polski, terminy techniczne po angielsku (tak jak w pracy).

## Out of scope
- Blazor, Razor Pages, MVC z widokami (frontend już umie).
- Aspire, Docker, chmura, CI/CD.
- Autoryzacja/Identity (być może później, po podstawach).
- Mikroserwisy, event sourcing, DDD w pełnym wydaniu.
