# Droga B wybrana samodzielnie, z własnym wariantem sygnatury

Wojciech naprawił captive dependency drogą B (dane żądania jako argument metody, nie przez konstruktor), ale przekazał cały serwis `ICurrentUser`, nie `string user.Name` jak w lekcji. Zweryfikowane na działającej aplikacji: licznik przeżywa żądania (3 → 6 po trzech POST-ach), `/lifetimes` zachowuje się poprawnie w obu osiach. Reguła „stan długowieczny w polu, dane żądania parametrem” jest opanowana na tyle, że stosuje ją w wariancie, którego nie pokazano.

## Evidence

- `RecordCreated(ICurrentUser user)` zamiast `RecordCreated(string user)`; `TicketMetrics` bez zależności w konstruktorze.
- `GET /metrics` po serii POST-ów: `{"created":6}` — singleton, licznik nie resetuje się między żądaniami.

## Implications

- **Wariant jest bezpieczny, ale węziej niż wygląda.** Przekazanie scoped serwisu jako argumentu nie tworzy captive dependency tylko dlatego, że singleton nigdzie nie zapisuje tej referencji. Jedno przypisanie do pola i problem wraca — tym razem bez wyjątku przy starcie, bo walidacja kontenera patrzy wyłącznie na konstruktory. Warto o tym wspomnieć, gdy pojawi się pierwszy handler z zależnościami (lekcja 0005).
- Do rozważenia przy Clean Architecture (0009): przekazywanie `ICurrentUser` w głąb wiąże warstwę domenową z abstrakcją webową; `string`/`UserId` nie wiąże. To dobry konkret do rozmowy o kierunku zależności.
