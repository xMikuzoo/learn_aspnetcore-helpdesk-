# Indeks warunkowy napisany samodzielnie; migracja padła na danych, nie na schemacie

Zadanie z lekcji 0011 (indeks złożony + `HasFilter`) wykonane samodzielnie po jednej podpowiedzi o składni. Blokada nie dotyczyła pojęcia, tylko **granicy C#/SQL**: „nwm jaki ten hasfilter napisać”. Po rozbiciu na dwa kroki (jaki SQL ma powstać → jak go wpisać w string) napisane bez dalszej pomocy.

Migracja wywaliła się na `SQLite Error 19: 'FOREIGN KEY constraint failed'` — z powodu, którego nie było w lekcji: baza użytkownika miała wiersz utworzony curlem w lekcji 0009, spoza `HasData`.

## Evidence

- Napisany indeks: `HasIndex(t => new { t.Title, t.RequesterId }).IsUnique().HasFilter("\"Status\" <> 'Resolved'")` — kolejność kolumn odwrotna niż w lekcji (`Title` pierwsze), poprawna; z cudzysłowami wokół nazwy kolumny, wariant odporniejszy.
- Stan bazy przed poprawką: `Tickets` = jeden wiersz `(3, 'Drukarka nie drukuje', 'high')`, seedy 1 i 2 nieobecne. `AddColumn … DEFAULT 0` + `UpdateData` tylko dla `Id` 1 i 2 → wiersz 3 z `RequesterId = 0` → FK bez celu.
- Rollback zadziałał w całości: `__EFMigrationsHistory` dalej sam `InitialCreate`, brak nowych kolumn, brak `ef_temp_Tickets`, `__EFMigrationsLock` pusty.
- Po dopisaniu dwóch `migrationBuilder.Sql(...)` (backfill `RequesterId` i `Status`) migracja przeszła; pełny ruch HTTP na prawdziwym projekcie: 409 duplikat wojtka / 201 ania z tym samym tytułem / 404 nieznany login / 204 resolve / 201 ten sam tytuł po resolve.

## Implications

- **Luka dydaktyczna po mojej stronie, nie po jego.** Zweryfikowałem lekcję na kopii projektu z bazą zbudowaną od zera z migracji — czyli na jedynym stanie, w którym ten błąd nie występuje. Wniosek na przyszłość: **przy migracjach weryfikować na bazie użytkownika, nie na czystej**; różnica „schemat vs dane” to dokładnie to, co odróżnia naukę od produkcji.
- Trafiło do lekcji jako pełna sekcja (*data migration*), do ściągi jako osobna tabela pułapek, do checklisty jako `c9`. Zostawiam to jako materiał do interleavingu — reguła „dodaj kolumnę → uzupełnij dane → nałóż ograniczenie” wróci przy Clean Architecture (0012), gdzie migracje przenoszą się do osobnego projektu.
- **Wzorzec blokady do zapamiętania:** użytkownik nie zatrzymał się na koncepcie (rozumiał, czego chce od indeksu), tylko na przejściu między językami — SQL w stringu C#, escapowanie cudzysłowów. To trzeci raz, gdy blokada jest składniowa, a nie pojęciowa (por. [[0005-slices-i-interfejs-vs-klasa]] — sygnatura, nie mechanizm). Przy kolejnych zadaniach z surowym SQL-em (`FromSqlRaw`, `HasComputedColumnSql`) od razu podawać docelowy SQL osobno, a dopiero potem jego opakowanie w C#.
- Nie pojawił się problem ze zrozumieniem indeksu częściowego jako pojęcia — pytanie „chcę unikalność względem stanu encji” użytkownik sformułował sam, zanim poznał nazwę. Tempo można utrzymać; kolejny krok to Clean Architecture.
