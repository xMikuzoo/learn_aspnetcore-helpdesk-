# Slices przeniesione samodzielnie; sygnatura rozszerzenia wybrana typem konkretnym

Refactor lekcji 0004 zrobiony w całości bez wsparcia: `git mv` zachował historię plików, namespace'y dopisane, `TicketsFeature` i `DiagnosticsFeature` napisane w konwencji `Add*`/`Map*` zwracającej argument. Zadanie 5 (drugi feature bez wzoru) wykonane — z jednym odchyleniem: `MapDiagnostics(this WebApplication app)` zamiast `IEndpointRouteBuilder`, i bez `MapGroup`.

## Evidence

- Build 0 błędów, 0 ostrzeżeń; `/tickets`, POST 201 z `Location`, `/tickets/metrics` 200, DELETE nieistniejącego 404 — wszystko dalej działa.
- `/diagnostics/lifetimes` → 404; endpoint faktycznie wisi pod `/lifetimes`.
- `CommonServices.cs`: `using Helpdesk.Common;` bez `namespace Helpdesk.Common;` — klasa w globalnym namespace, kompiluje się „przypadkiem”.
- Klasa `TicketFeature` w pliku `TicketsFeature.cs`.

## Implications

- **Wzorzec „para Add/Map zwracająca argument” jest opanowany.** Odchylenie dotyczy nie mechanizmu, tylko wyboru typu parametru — czyli tego, że interfejs kupuje elastyczność, której w danym momencie nie widać. Quiz q3 lekcji 0004 pytał dokładnie o to; pytanie w quizie nie wystarczyło, żeby przeniosło się na własny kod pisany bez wzoru.
- **Wniosek dydaktyczny:** przy pisaniu „bez wzoru” podawać w zadaniu sygnaturę docelową, jeśli jej wybór jest pointą. Sam quiz nie działa jako transfer.
- Namespace jeszcze nie jest odruchem — to pierwszy projekt użytkownika z nietrywialną strukturą. Warto wracać do tego przy każdym nowym pliku przez najbliższe 2–3 lekcje, aż przestanie być pomijany.
- Nie pojawił się żaden problem ze zrozumieniem samych metod rozszerzających ani `git mv`. Tempo można utrzymać.
