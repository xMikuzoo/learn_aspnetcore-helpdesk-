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
