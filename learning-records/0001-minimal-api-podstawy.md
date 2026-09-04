# Lekcja 0001 zaliczona: anatomia Program.cs i minimal API

Wojciech zbudował Helpdesk od zera, dodał `/tickets` i `/tickets/{id}`, i zweryfikowałem empirycznie (curl na działającej instancji): `/` → `text/plain`, `/tickets` → JSON w camelCase, `/tickets/abc` → 400 z `BadHttpRequestException: Failed to bind parameter "int id"`. Granica `builder.Build()` i binding parametru trasy po nazwie są opanowane — nie trzeba do nich wracać.

## Evidence

- `project/Helpdesk/Program.cs` kompiluje się bez ostrzeżeń (`dotnet build`, 0 Ostrzeżeń na czystej kopii).
- Zadanie 2 rozwiązane lepiej niż w treści lekcji: wyciągnął tablicę do zmiennej `tickets` poza lambdę, zamiast duplikować literał w każdym handlerze.
- Użył LINQ (`Where`, lambda `x => x.Id == id`) bez instrukcji — transfer z JS `Array.filter` zadziałał.

## Implications

- **Do doszlifowania w lekcji 0002/0003**: `Where` zamiast `FirstOrDefault` — `/tickets/1` zwraca tablicę jednoelementową, a `/tickets/99` pustą tablicę ze statusem 200 zamiast 404. Semantyka „jeden zasób vs kolekcja" i `TypedResults.Ok/NotFound` to naturalny materiał na lekcję 0003.
- **Most do lekcji 0002 (DI)**: zmienna `tickets` domknięta w lambdzie to closure — jedna instancja na cały proces, czyli de facto ręcznie zrobiony Singleton. To gotowy punkt wyjścia do wyjaśnienia, po co w ogóle kontener DI i czym się różni Singleton od Scoped.
- Deferred execution LINQ (`Where` nie wykonuje się w miejscu wywołania) jeszcze nie omawiane — do lekcji o EF Core (0008), gdzie `IQueryable` czyni z tego pułapkę.
