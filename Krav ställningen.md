## Krav ställningen: 

### Startsida 

* Skriv ut statistik på startsidan. Du ska se antal kunder, antal konton och summan av saldot på konton. 
* Gruppera antal kunder, antal konton och summan av saldot på konton per LAND. 
* När man klickar på tex "Norway" ska man komma till en sida där de 10 största (högst totalsaldo) kunderna i det landet visas. 
* Denna sida ska responsecachas (Vary på land) i en minut.

### Kund hantering 

* CRUD

* När en kund skapas ska de få ett nytt unikt kundnummer. 

* Det ska också automatiskt skapas ett transaktionskonto med kunden som ägare.

* Det ska gå att ta fram en kundbild genom att ange *kundnummer*. 

  * Kundbilden ska visa all information om kunden och alla kundens konton. 

  * Kundbilden ska också visa det totala saldot för kunden genom att summera saldot på kundens konton.

* Lägg till en kontobild där man kan se transaktioner för ett individuellt konto. 
  * När man klickar på ett kontonummer i kundbilden ska man komma till en kontosida som visar kontonummer och saldo samt en lista med transaktioner i descending order. 
  * Om det finns fler än 20 transaktioner ska JavaScript/AJAX användas för att ladda in ytterligare 20 transaktioner när man trycker på en knapp längst ned i listan. Trycker man igen laddas 20 till, och så vidare.

* Systemet ska också hantera insättningar, uttag och överföringar mellan konton. 
  * Det får inte bli några avrundningsfel så använd decimal. 
  * Det ska alltså inte gå att ändra saldo direkt på ett konto - alltid via en transaktion.
  * Det ska framgå tydligt om någon försöker ta ut eller överföra mer pengar än vad som finns på kontot! 

### Användare Hantering

* CRUD
* Endast Admin kommer åt denna sida

### SÖK

* Azure Cognitive Search används som sökmotor. https://docs.microsoft.com/sv-se/azure/search/search-get-started-dotnet

- Så du måste göra en batchrutin för att fylla index från databasen initialt. 

- Gör detta som en separat console applikation. Samt såklart kontinuerligt skicka över ändrad data (när man tex byter namn i admindelen). 

- Det ska gå att söka efter kund på namn och stad. 

### Sökresultatet ska vara enligt: 

* En lista ska med kundnummer och personnummer, namn, adress, city som sökresultat.
* Paginerat (50 resultat i taget och så ska man kunna bläddra till nästa/tidigare sida). Klickar man på en kund ska man komma till kundbilden.
* Sökresultatet ska vara möjligt att sortera. Första klicket på en kolumn = asc, sen desc, sen asc etc etc.

### WEB API

- Ett anrop till /api/me ska ge samma information som i kundbilden.

- Ett antop till /api/accounts/12345 ska visa transaktioner för ett konto. 
- Använd parametrar limit och offset för att begränsa antal transaktioner som hämtas. 
- Dessa värden skickas in i LINQ:s Take och Skip.

- Använd en Authorization header för att identifiera kunden. Du kan använda basic authentication eller jwt tokens.

### **Banken har också en Console app (batch som körs varje natt). Det kollar efter misstänkta transaktioner (penningtvätt tex)**

- Implementera denna så den återanvänder kod. Programmet ska

```
för varje land

​     för varje användare i det landet

​        för varje kontotransaktion för användaren kolla om den uppfyller regel för "misstänkt" (se nedan)

​    när "landet" är klart skicka en rapport (mail) till <land>@testbanken.se. Bara en lista över vilka personer och vilka kontonummer och vilka transaktionenummer
```

- Håll reda på var du slutar sas så du inte börjar från början varje dag

- ### Regler:  

  - En enskild transaktion större än 15000 kr

  - Eller totala transaktioner de senaste tre dygnen (72h) från aktuellt tidpunkt större än 23000

###  ASP.NET Core Identity

* Rollen Admin ska kunna administrera användare /att administrera användare.
* Rollen Cashier ska kunna administrera kunder och deras konton. 

### Seeda två användare

* stefan.holmberg@systementor.se och Hejsan123# och som Admin

* stefan.holmberg@nackademin.se och Hejsan123# och som Cashier



DATABAS: https://aspcodeprod.blob.core.windows.net/school-dev/BankAppDatav2%20(1).bak