# CLAUDE.md

## Styl kodu

**Zakaz komentarzy w kodzie.** Kod ma się tłumaczyć nazwami. Bez `// tworzymy serwis`,
bez nagłówków sekcji, bez TODO-notatek. Wyjaśnienia idą do lekcji w `lessons/`, nie do repo.

Wyjątek: krótki XML doc (`/// <summary>`), jedno zdanie, nie akapit. Nad interfejsem — co to za
kontrakt i po co się go wstrzykuje. Nad implementacją — tylko gdy nazwa nie oddaje, że coś jest
uproszczone/udawane na potrzeby kursu (`HeaderCurrentUser` bierze usera z nagłówka `X-User`
zamiast z ciasteczka/tokenu). Wzór: `project/Helpdesk/ICurrentUser.cs`.

Wyjątek 2: `// TODO(human)` — marker miejsca, które ma napisać user (dokładnie jeden naraz w repo).
