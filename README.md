# Solvify

Solve word puzzles with ease. Don't think too hard, let the computer do the work for you. No AI, just a simple word solver.

## How it works

### DeductWordService

The `DeductWordService` class is responsible for solving the word puzzle where you search for one word with a given number of characters with a limited amount of tries.

## Todos

1. Die Programmlogik und UI (bzw. CLI Logik) sollte in verschiedene Projekte getrennt werden.
1. Es sollten verschiedene Implementierung der Programmlogik möglch sein, um diese zu benchmarken.
1. Neue Implementierung: Wenn Buchstaben samt Positionen bekannt sind, sollten sie nicht bei jedem Guess wiederholt werden, solange es noch unbekannte Buchstaben gibt. Damit sinkt nämlich die Anzahl der Buchstaben, die pro Guess verifiziert werden können.
    - Das bedeutet: Worte, die bekannte Buchstaben samt Positionen beinhalten sind sollten nicht vorgeschlagen werden - eher Worte, die mehr unbekannte Buchstaben beinhalten.
    - Wenn aufgrund der bekannte Buchstaben samt Positionen eigentlich nur noch ein (oder anderer Schwellenwert) Wort in Frage kommt, muss dies gewählt werden.
    - Beispiel: links (-*--+). Jetzt sollten wörter, bei denen I an zweiter stelle steht nicht ausgegeben werden, solange man noch wörter hat, die mehr buchstaben oder mehr positionen aufdecken können.
    - Ein Wort mit i an zweiter stelle sollte auch keinen malus erhalten, aber halt auch keinen bonus, solange es noch gute alternative vorschläge gibt.
1. Neue Implementierung: Die ausgeschlossenen Buchstaben/Positionen sollten beim Suchen nach neuen Wortvorschlägen berücksichtigt werden.
1. Benchmark/Test: Schauen, welche Implementierung wie Lange (wie viele Versuche) für die Lösung eines bestimmten wortes benötigt.
1. Beim Start der Anwendung sollte ein kurzer Text zur Verwendung angezeigt werden
1. Es sollten die nächsten 5 wortvorschläge ausgegeben werden (numeriert). Bei der Eingabe muss dann die wortvorschlagnummer vorangestellt werden, falls es nicht der erste vorschlag ist. die anderen vorschläge werden als nicht gültig interpretiert.
1. Alternative GUI TUI
1. Alternative GUI Blazor UI
1. Alternative GUI Web UI
1. Verwaltung von verschiedenen Spiele-Seiten
    - Auswahl
    - Links zum Spiel
    - Spielregeln je Site / Unterschiedliche Lösungs-Implementierungen
    - Sammlung invalider Wörter
    - Speichern vergangener Sessions
    - Auswertungen: Eie viele Guesses im Durchschnitt etc., Wie viele invalide Wörter etc.