# Notes

## Preferencje użytkownika
- Pisze po polsku; lekcje po polsku, terminy po angielsku.
- Chce budować projekt od zera, stopniowo: minimal API → DI → slices → command/query handlery → walidator → EF Core → clean arch.
- Frontend (Vue/TS). Analogie do świata JS/TS pomagają (DI ≈ provide/inject, middleware ≈ Express, handler ≈ store action).

## Decyzje kursu
- Projekt: **Helpdesk API** (zgłoszenia/tickets). Uzasadnienie: naturalne komendy (CreateTicket, AssignTicket, CloseTicket), zapytania (GetTicket, ListTickets), reguły biznesowe (przejścia statusów), walidacja. Domenę można zmienić — zapytać jeśli użytkownik ma lepszy pomysł.
- Kod projektu żyje w `./project/Helpdesk/`. Lekcje w `./lessons/`, ściągi w `./reference/`.
- Kolejność: najpierw hand-rolled dispatcher (żeby zrozumieć mechanizm), dopiero potem biblioteka.
- Biblioteka mediatora (lekcja 6): MediatR >= 13 jest komercyjny (RPL-1.5 / Community < 5M USD przychodu; nauka = OK). Przed lekcją 6 ZAPYTAĆ: jakiej wersji MediatR używa firma (12.x Apache vs 13+)? Jeśli MediatR — uczyć MediatR (bo to czytają w pracy). Alternatywa MIT o tym samym API: martinothamar/Mediator.
- Przed lekcją 9 ZAPYTAĆ: czy firmowe repo przypomina szablon jasontaylordev czy ardalis.
- Odpowiedzi użytkownika (2026-09-04): (a) mediator — używamy oryginalnego MediatR (14.x, Community/nauka = darmowe) po własnym dispatcherze; ustalić w firmie wersję (12 vs 13+). (b) .NET 10 — tak, cały kurs. (c) Kontrolery — nie przechodzimy; jedna krótka lekcja „ten sam slice jako kontroler” po lekcji 4, żeby czytać firmowy kod; jeśli firma używa kontrolerów, przesunąć ją wcześniej. (d) Firmowy zespół zna Milana Jovanovicia i Nicka Chapsasa → styl: minimal API + vertical slices + MediatR + FluentValidation + Result pattern. Dodać Chapsasa do RESOURCES po weryfikacji (research).
- Terminologia: trzymać się `reference/glossary.html`.

## Roadmapa (wstępna, do rewizji po każdej lekcji)
1. Minimal API: anatomia Program.cs, pierwszy endpoint.
2. DI: interfejs + implementacja, lifetimes (Singleton/Scoped/Transient).
3. In-memory repo + CRUD endpoints, TypedResults, route groups.
4. Vertical slices: folder per feature.
5. Command vs Query: własne ICommandHandler/IQueryHandler.
6. Dispatcher/mediator: własny → biblioteka.
7. FluentValidation + pipeline behavior.
8. EF Core + SQLite + migracje.
9. Clean Architecture: warstwy, kierunek zależności, porównanie ze slices.
10. Result pattern / ProblemDetails / obsługa błędów.
11. Testy: xUnit + WebApplicationFactory.

## Log sesji
- 2026-09-04: workspace założony. Research gotowy (research/aspnet-learning-path.md, 60 źródeł). RESOURCES.md, glossary, assets, lekcja 0001 napisane i zweryfikowane na SDK 10.0.400. Lekcja 0001 otwarta w przeglądarce; czekam na wynik.
- 2026-09-04: lekcja 0001 zaliczona i zweryfikowana curl-em (LR-0001). Poprawiona ścieżka workspace w lekcji 0001 (było `C:\REPOS\asp`). Dodany `.vscode/settings.json` (formatOnSave + codeActionsOnSave) — bez tego `.editorconfig` nie odpalał się przy zapisie. Uwaga na przyszłość: Roslyn formatter NIE poprawia wcięć w `new[] { … }` (sprawdzone empirycznie) — nie obiecywać w lekcjach, że format-on-save wyrówna array initializer.
- Do lekcji 0003: `TypedResults.Ok/NotFound`, `FirstOrDefault` vs `Where`, 404 dla nieistniejącego zasobu.
- 2026-09-04: lekcja 0002 (DI + lifetimes) napisana. Nowe: `reference/di-lifetimes.html` (ściąga), link z glossary. Cały kod lekcji zweryfikowany na .NET 10.0.400 w scratchpadzie: wydruk `/lifetimes` w lekcji to prawdziwy output, tak samo treść wyjątku captive dependency. Potwierdzone: walidacja scope'ów leci z `builder.Build()` tylko przy ASPNETCORE_ENVIRONMENT=Development. Potwierdzone: `ITicketStore` jako Scoped → POST zwraca 200 z nowym id, GET go nie widzi (cicha utrata danych) — to jest pointa zadania 3.
- TODO(human) czeka w `project/Helpdesk/Program.cs`: wybór lifetime dla ITicketStore.
- 2026-09-04 (lekcja 0002, po feedbacku): użytkownik słusznie zakwestionował `TicketAuditor` — klasa bez celu, więc lifetime nierozstrzygalny. Krok 4 przepisany na `TicketMetrics` (Singleton, licznik od startu) + `ICurrentUser` (Scoped, nagłówek X-User przez IHttpContextAccessor). Wszystkie trzy stany zweryfikowane realnym uruchomieniem: captive → crash w Build(); droga A (metryki Scoped) → konsola dwa razy „1”, `/metrics` = 0; droga B (user parametrem) → `/metrics` = 3, poprawni użytkownicy. Nowy quiz q5 o regule A vs B, nowa sekcja w ściądze („Jak to naprawić — w tej kolejności”).
- **ZASADA PROJEKTOWANIA LEKCJI**: każdy przykładowy serwis musi mieć oczywistą rolę wynikającą z nazwy klasy i metody. Żadnych wydmuszek typu `Describe()` zwracającego opis samego siebie — blokują decyzję, której uczymy.
- Dług do spłacenia: thread-safety (`lock`/`Interlocked`/`Concurrent*`) — wspomnieć w 0003, rozwinąć w 0008 przy DbContext.
- 2026-09-04: lekcja 0002 zaliczona i zestageowana (LR-0003). Użytkownik wybrał drogę B, ale z sygnaturą `RecordCreated(ICurrentUser)` zamiast `string`.
- 2026-09-04: lekcja 0003 (CRUD + TypedResults + MapGroup + lock) napisana; nowa ściąga `reference/minimal-api-crud.html`. Cały kod zweryfikowany uruchomieniem: 201 z `Location: /tickets/3`, PUT/DELETE 204, powtórny DELETE 404, `{id:int}` zmienia 400→404 dla `/tickets/abc`, POST bez body → 400. Potwierdzone: `System.Threading.Lock` i `Results<Ok<Ticket>, NotFound>` kompilują się na .NET 10.0.400.
- TODO(human) lekcji 0003 w `InMemoryTicketStore.Add`: `_tickets.Count + 1` daje duplikaty Id po Delete (zweryfikowane: dwa zgłoszenia z id 3). Dług thread-safety z LR-0002 spłacony w kroku 5 lekcji 0003.
- 2026-09-04: user dodał `CLAUDE.md` — zakaz komentarzy w kodzie, wyjątki: krótki `/// <summary>` (jedno zdanie) i dokładnie jeden `// TODO(human)` naraz. Kod kursu doprowadzony do tej reguły. **Wszystkie przyszłe lekcje muszą to respektować: wyjaśnienia idą do `lessons/`, nie do plików .cs.**
