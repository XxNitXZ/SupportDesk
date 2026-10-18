# SupportDesk

SupportDesk är en grafisk applikation för hantering av kunder och supportärenden.

Applikationen är byggd i C# med WinUI och använder lokal JSON-lagring.

Syftet med SupportDesk är att ge ett mindre IT-företag ett enkelt sätt att registrera kunder, skapa supportärenden, tilldela handläggare och följa ett ärende från registrering till lösning.

---

## Gruppmedlemmar

- Farhad Amiri
- Lily Esther Boycen
- Nitin Kumar
- Stelian Arama

---

## Funktioner

SupportDesk innehåller bland annat följande funktioner:

- Registrera kunder
- Visa registrerade kunder
- Uppdatera kunduppgifter
- Skapa supportärenden
- Automatiskt skapa unika id:n
- Automatiskt registrera datum och tid
- Prioritet: Low, Normal och High
- Status: New, In Progress och Resolved
- Tilldela och byta handläggare
- Ändra titel och beskrivning
- Lägga till kommentarer
- Visa tidigare kommentarer i tidsordning
- Söka på ärendetitel
- Söka på kundnamn
- Case-insensitive sökning
- Filtrera ärenden efter status
- Kombinera sökning och statusfilter
- Dashboard med antal ärenden per status
- Lokal JSON-lagring
- Validering
- Felhantering
- Enhetstester
- Integrationstester

---

# Bygga och starta applikationen

## Visual Studio

1. Klona eller ladda ner repositoryt.
2. Öppna solution-filen i Visual Studio.
3. Kontrollera att `SupportDesk.Presentation` är valt som startup project.
4. Bygg solutionen.
5. Starta applikationen med den gröna Play-knappen.

---

## Terminal

Från repositoryts rot:

```bash
dotnet restore
```

Bygg solutionen:

```bash
dotnet build
```

---

# Köra tester

## Visual Studio

Öppna:

**Test → Test Explorer**

Välj sedan:

**Run All Tests**

## Terminal

Från repositoryts rot:

```bash
dotnet test
```

---

# Projektstruktur

Lösningen är uppdelad i fyra projekt med olika ansvarsområden.

## SupportDesk.Domain

Domain innehåller verksamhetens modeller och regler.

Exempel:

- `Customer`
- `SupportTicket`
- `Comment`
- `TicketStatus`
- `TicketPriority`

Domänobjekten innehåller även metoder som skyddar verksamhetsreglerna.

Exempel på sådana metoder är:

- `AssignTechnician()`
- `ChangeStatus()`
- `AddComment()`

Domain ansvarar inte för gränssnitt eller JSON-lagring.

---

## SupportDesk.Application

Application samordnar applikationens funktioner.

Här finns bland annat:

- `CustomerService`
- `TicketService`
- `ICustomerRepository`
- `ITicketRepository`

Services hämtar information genom repository-interface, använder domänobjekten för verksamhetsregler och ber sedan repositoryt att spara ändringarna.

Application känner inte till hur informationen faktiskt sparas i JSON.

---

## SupportDesk.Infrastructure

Infrastructure ansvarar för lagringen.

Här finns bland annat:

- `JsonCustomerRepository`
- `JsonTicketRepository`
- `JsonFileHandler`

Repositories implementerar repository-interface från Application.

`JsonFileHandler` ansvarar för själva läsningen och skrivningen av JSON-filer.

---

## SupportDesk.Presentation

Presentation är WinUI-delen som användaren arbetar med.

Här finns bland annat:

- Pages
- ViewModels
- XAML
- Navigation
- Dialoger
- Dependency Injection-konfiguration

Presentation använder services från Application och arbetar inte direkt mot JSON-filer.

---

# Exempel på anropsflöde

När användaren exempelvis öppnar ett supportärende, ändrar information och trycker på **Save Changes** går anropet ungefär så här:

```text
View
  ↓
ViewModel
  ↓
TicketService ─────→ SupportTicket (Domain)
  ↓
Repository
  ↓
JsonFileHandler
  ↓
JSON
```

`TicketService` använder alltså `SupportTicket` i Domain för att utföra ändringar och kontrollera verksamhetsregler.

Exempelvis kan servicen använda domänmetoder som:

```text
AssignTechnician()
ChangeStatus()
AddComment()
```

När ändringen är giltig fortsätter `TicketService` till repositoryt som sparar informationen.

Ett mer konkret exempel kan därför beskrivas så här:

```text
TicketDetailsPage
       ↓
TicketDetailsViewModel
       ↓
TicketService ─────→ SupportTicket
       ↓
JsonTicketRepository
       ↓
JsonFileHandler
       ↓
tickets.json
```

Det gör att gränssnitt, verksamhetsregler och lagring hålls separerade.

---

# JSON-lagring

SupportDesk använder lokal JSON-lagring.

Information som sparas är bland annat:

- Kunder
- Supportärenden
- Kundkopplingar
- Handläggare
- Status
- Prioritet
- Kommentarer
- Id
- Skapandedatum och tid

När applikationen startas läses tidigare sparad information tillbaka.

Applikationen kan även starta om JSON-filerna ännu inte finns.

---

# Repositories och filsökvägar

Repositories använder normalt applikationens vanliga JSON-filer.

Exempel:

```csharp
public JsonCustomerRepository(
    JsonFileHandler jsonFileHandler,
    string? filePath = null)
{
    _jsonFileHandler = jsonFileHandler;
    _filePath = filePath ?? DataPaths.CustomerFilePath;
}
```

Om ingen `filePath` skickas in används den vanliga sökvägen:

```text
DataPaths.CustomerFilePath
```

Det gör att den vanliga applikationen fungerar som tidigare.

Vid integrationstester kan vi däremot skicka in en egen temporär sökväg så att testerna inte påverkar applikationens vanliga data.

---

# Felhantering

SupportDesk hanterar fel som kan uppstå vid JSON-lagring.

Exempel på situationer som hanteras:

- Filen finns inte
- Filen är tom
- JSON-formatet är felaktigt
- Informationen i filen är ogiltig
- Filen kan inte läsas
- Filen kan inte sparas

Om en JSON-fil innehåller felaktig JSON får användaren ett begripligt felmeddelande istället för att applikationen avslutas oväntat.

En misslyckad inläsning behandlas inte som en vanlig tom lista.

Det minskar risken att befintlig information skrivs över med tom data.

---

# Verksamhetsregler och validering

Applikationen innehåller flera regler för att förhindra felaktig information.

Exempel:

- Kundens namn måste vara ifyllt
- Kundens e-postadress måste vara ifylld
- E-postadressen kontrolleras med enkel formatvalidering
- Ett supportärende måste ha titel
- Ett supportärende måste ha beskrivning
- Ett supportärende måste vara kopplat till en registrerad kund
- Obligatoriska textfält får inte bara bestå av blanksteg
- In Progress kräver en handläggare
- Resolved kräver en handläggare
- Kommentarer får inte vara tomma
- Endast tillåtna statusar används
- Endast tillåtna prioriteter används
- Id och skapandetid bevaras när information uppdateras

Viktiga verksamhetsregler ligger i Domain och Application och inte enbart i gränssnittet.

---

# Tester

Projektet innehåller både enhetstester och integrationstester.

## Enhetstester

Enhetstesterna testar verksamhetslogiken utan att använda applikationens vanliga JSON-filer.

Vi testar bland annat att:

- Ett giltigt ärende skapas med status New
- Ett ärende med tom titel avvisas
- Ett ärende utan handläggare inte kan ändras till In Progress
- En giltig statusändring fungerar när en handläggare finns
- Sökningen fungerar oberoende av stora och små bokstäver
- Statusfiltret endast visar ärenden med vald status

---

## Integrationstester

Integrationstesterna använder den riktiga JSON-lagringen.

Tester använder egna temporära filer för att inte påverka applikationens vanliga data.

Vi testar bland annat att:

- En kund kan sparas och läsas tillbaka
- Ett supportärende kan sparas och läsas tillbaka
- Kundens id bevaras
- Ärendets id bevaras
- Kundkopplingen bevaras
- Handläggaren bevaras
- Status bevaras
- Kommentarer bevaras
- Felaktig JSON rapporteras på ett hanterbart sätt
- Felaktig JSON inte automatiskt skrivs över med tom information

Efter testerna tas de temporära testfilerna bort.

---

# Manuell kontroll

Följande funktioner har kontrollerats manuellt:

- [x] Starta applikationen
- [x] Registrera en kund
- [x] Visa registrerade kunder
- [x] Uppdatera kunduppgifter
- [x] Skapa ett supportärende
- [x] Kontrollera att ett nytt ärende får status New
- [x] Öppna ett supportärende
- [x] Ändra titel och beskrivning
- [x] Tilldela handläggare
- [x] Ändra prioritet
- [x] Ändra status till In Progress
- [x] Lägga till kommentar
- [x] Ändra status till Resolved
- [x] Söka efter ärenden
- [x] Söka efter kundnamn
- [x] Filtrera efter status
- [x] Kombinera sökning och statusfilter
- [x] Kontrollera Dashboard
- [x] Starta om applikationen
- [x] Kontrollera att kunder finns kvar
- [x] Kontrollera att ärenden finns kvar
- [x] Kontrollera att handläggare och status finns kvar
- [x] Kontrollera att kommentarer finns kvar

---

# Arbetsfördelning

Arbetet delades huvudsakligen upp efter funktioner.

Olika gruppmedlemmar arbetade med olika delar av applikationen, exempelvis:

- Kundhantering
- Skapa supportärenden
- Ärendehantering
- Dashboard
- Sökning och filtrering
- JSON-lagring
- Validering
- Tester

Vi utgick från en gemensam projektstruktur i Visual Studio så att alla arbetade enligt samma struktur.

Gemensamma delar som modeller, interfaces, navigation och projektstruktur diskuterades och samordnades inom gruppen.

---

# Git och GitHub

Projektet utvecklades i ett gemensamt GitHub-repository.

Under arbetet använde vi:

- Separata branches
- Egna commits
- Pull requests
- Code review
- Merge av färdiga funktioner

Det gjorde att flera personer kunde arbeta parallellt på olika delar av applikationen.

Efter att ändringar slagits ihop kontrollerades att den gemensamma versionen fortfarande kunde byggas och köras.

---

# Kända begränsningar

Den nuvarande versionen är byggd för lokal användning av en användare åt gången.

Följande funktioner ingår inte:

- Databas
- Server
- Inloggning
- Flera samtidiga användare
- Administration av handläggare
- Radering av kunder
- Synkronisering mellan flera datorer

Handläggare används istället från en fördefinierad lista.

---

# AI-användning

AI-verktyg har använts som stöd under utvecklingen.

AI har bland annat använts för:

- Förklaringar av C# och .NET
- Felsökning
- Kodförslag
- Förklaringar av arkitektur
- Förslag på tester
- Hjälp med dokumentation

AI-genererade förslag har inte behandlats som automatiskt korrekta.

Förslag har granskats och kontrollerats genom:

- Manuell testning
- Enhetstester
- Integrationstester
- Kodgranskning
- Körning av den färdiga applikationen

Gruppen ansvarar för den kod som finns i den slutliga lösningen och ska kunna förklara hur den fungerar.

---

# Sammanfattning

SupportDesk samlar hela supportflödet i en applikation.

```text
Registrera kund
      ↓
Skapa ärende
      ↓
Tilldela handläggare
      ↓
Ändra status
      ↓
Lägg till kommentarer
      ↓
Sök och filtrera
      ↓
Lös ärendet
```

Applikationen använder bland annat:

- C#
- WinUI
- DDD
- Clean Architecture
- Service Pattern
- Repository Pattern
- Dependency Injection
- JSON-lagring
- Enhetstester
- Integrationstester

Målet är att ge supportpersonalen ett enkelt och tydligt sätt att följa ett supportärende från registrering till lösning.
