# CRUD domknięty; Id wyliczane z listy, recykling zaakceptowany

Lekcja 0003 zaliczona w całości: DTO w ciele żądania, `TypedResults` z `Results<Ok<Ticket>, NotFound>`, `MapGroup`, `{id:int}`, 201/204/404, `lock` na całym store i `Interlocked.Increment` w metrykach. Kolizja Id po `DELETE` naprawiona samodzielnie — wybrana wersja wyliczająca (`Max + 1`), nie licznik-pole.

## Evidence

- 30 równoległych POST-ów → 32 zgłoszenia, 32 unikalne Id, `/metrics` = 33. Lock i Interlocked działają.
- `GenerateId()` wydzielone do osobnej metody, początkowo `public`; po uwadze poprawione na `private`.
- `Interlocked.Increment` przyjęty dopiero po rozpisaniu przeplotu dwóch wątków — sam zapis `_created++` nie wyglądał podejrzanie.
- Zweryfikowany skutek wybranej wersji: `DELETE /tickets/4` → następny POST znowu dostaje 4.

## Implications

- **Recykling Id jest teraz w projekcie faktem, nie przeoczeniem** — świadomy wybór po pokazaniu obu wariantów. Warto do niego wrócić przy EF Core (0008), gdzie `IDENTITY` narzuci wariant przeciwny; to będzie naturalny moment na „dlaczego baza robi inaczej”.
- `Max()` liczy się wewnątrz `lock` — O(n) w sekcji krytycznej. Nieistotne teraz, ale dobry konkret na rozmowę o kosztach lock-owania, gdyby wypadła.
- **Wzorzec dydaktyczny do powtarzania:** przy zadaniu z wyborem trzeba pokazać obie wersje w całości i różnicę w *zachowaniu*, nie tylko w składni. Pierwsze podejście („dwie możliwości, zastanów się”) nie wystarczyło; rozpisanie tabeli przeplotu i scenariusza z recyklingiem odblokowało decyzję od razu.
- Poziom C#: składnia nie jest już wąskim gardłem. Wąskim gardłem jest to, czego nie widać w kodzie — atomowość, współbieżność, kontrakt wobec klienta HTTP. Kolejne lekcje mogą iść szybciej po składni, wolniej po konsekwencjach.
