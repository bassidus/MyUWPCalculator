# MyUWPCalculator

Inlämningsuppgift 1 - "Miniräknare"

Följande uppgift ska primärt göras som en konsollapplikation i C#.

Vill du hellre göra det som en UWP-applikation går det också bra.

Målet är att skriva en specialiserad "miniräknare" som förutom vanliga matematiska uttryck
också hjälper till med enhetsomvandling och mer specialiserade uträkningar:

- Plus, minus, gånger, delat med (gör det enkelt och ta en operation/operand i taget, som i exemplet nedan)
- Omvandla Celsius till Fahrenheit (och tvärtom)
- Hantera Newtons andra lag (F = m*a) med kommandot "Newton".
- Om "MARCUS" skrivs in någonstans så ska det behandlas som talet 42.
- Kommandot "lista" ska skriva ut någon slags lista med alla uträkningar som gjorts.
- Avsluta applikationen om användaren skriver "quit".

Inlämningen ska också innehålla enhetstester. Dela upp koden i (åter-)användbara metoder för
att göra det lättare! Kommentera/dokumentera vid behov.
Notera att eventuella fel som kan uppstå (formatering, konvertering, etc.) ska hanteras -
programmet ska inte krascha.

Exempel på användning (det behöver inte se ut exakt såhär):

(Rader som börjar med > har användaren skrivit in)
```
 > 5
 > +
 > MARCUS
 47
 > *
 > 2
 94
 > -
 > xyz
 Förlåt? Försök igen.
 > 25
 > C
 78 grader Fahrenheit.
 > Newton
 m=?
 > 10
 a=?
 > 3.5
 F=35
 > oiewuraslkdfjasdilojk
 Förlåt? Försök igen.
 > lista
 5+MARCUS*2 => 94
 25 C => 77 F
 oiewuraslkdfjasdilojk => ???
```
- Jobba ensam eller i par, men se då till att båda skriver kod och förstår allt.
- Ta hjälp av internet eller varandra, men kopiera inte.
- Var beredda på att svara på frågor om koden.
- Kommentera gärna koden med tankar, motiveringar, eller idéer du hellre genomfört men
inte lyckades lista ut.

Skapa en zip-fil med projektet och lämna in via Learnpoint.

Lycka till! :)
