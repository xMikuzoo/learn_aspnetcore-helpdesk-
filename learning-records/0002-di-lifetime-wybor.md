# Wybór lifetime'u przez konsekwencję dla stanu, nie przez regułkę

Wojciech uzasadnił Singleton dla `ITicketStore` tak: „jedna 'baza danych' w obrębie całej aplikacji; gdyby było Scoped, każdy request dodania zwracałby, że utworzył id 3, a nic by się nie dodawało”. To nie jest powtórzona definicja — to opis zaobserwowanego zachowania, z poprawnie przewidzianym objawem (POST zwraca sukces z id 3, GET nic nie widzi). Rozumowanie „gdzie fizycznie mieszka stan → jak długo musi żyć instancja” jest opanowane; nie trzeba go powtarzać w kolejnych lekcjach.

## Implications

- **Nie doszedł wątek thread-safety.** Zadanie prosiło o rozważenie, czy współdzielona `List<T>` w singletonie to problem — komentarz tego nie tknął. To samo dotyczy `_created++` w `TicketMetrics` (odczyt-modyfikacja-zapis, nie jest atomowy). Do domknięcia: krótko przy okazji lekcji 0003 (`lock` / `Interlocked` / `Concurrent*`), pełniej przy EF Core (0008), gdzie `DbContext` jest jawnie nie-thread-safe i to jest oficjalny powód, dla którego jest Scoped.
- **Zgłosił zasadną reklamację dydaktyczną**: klasa `TicketAuditor` z lekcji 0002 nie miała żadnego celu („ani nazwa klasy, ani nazwa funkcji, ani zwracany string nie mówi, do czego to służy”), więc nie dało się zdecydować o jej lifetimie. Miał rację — bez roli serwisu decyzja o czasie życia jest nierozstrzygalna, bo lifetime wynika z zadania serwisu. Lekcja przepisana: `TicketMetrics` (licznik od startu, musi być Singleton) + `ICurrentUser` (dane żądania, musi być Scoped).
- **Wniosek na przyszłe lekcje**: każdy przykładowy serwis musi mieć nazwę i metodę, z których wprost wynika jego rola. Sztuczne klasy-wydmuszki blokują dokładnie tę decyzję, której chcemy uczyć. Zapisane też w [[NOTES.md]].
