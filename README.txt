GIT

github naam: Gilles98/Project-GP-Recht


link: https://github.com/Gilles98/Project-GP-Recht


EXTRA INFO

op het einde van veel crud operatie methodes heb ik vaak int ok staan. 
dit dient als laatste check om zeker te zijn dat het goed loopt.
staat dus niet overal.

scripts van de inserts en tabellen zitten samen in de scriptsAll file.


KORTE OMSCHRIJVING APPLICATIE

In mijn applicatie is het de bedoeling dat je volmacht hebt over een
rechtbank en alle instanties die daarmee te maken hebben zoals rechtzaken, jury's , partijen en rechters.

Ik heb besloten om mijn inserts initieel te regelen via een code first initializer klasse
hierdoor verschillen de inserts in het script met het initiële omdat ik met CRUD operaties dingen heb aangepast en omdat mijn script gegenereerd is.

Ik heb ook besloten om te werken met een treeview en user controls.
Dit zorgt ervoor dat ik mijn applicatie heel overzichtelijk kan houden en het aantal
openstaande schermen beperk.

Enkel om een rechtzaak en de partijen te beheren en bij de startview werk ik nog met aparte openstaande schermen.

........


GEEN LOGIN

Toen er mij werd meegedeeld dat dit tijdens de les wel gezegd was geweest zat ik al redelijk ver in mijn project.
Na het bespreken en herbekijken van de schetsen met mvr. Van Den Bulck is er besloten om dit niet te incorperen in de applicatie
omdat ik toen bij het maken van de schetsen niet wist dat van inloggen/registreren iets van was gezegd geweest.

Er is in samenspraak toen besloten dat dit voor mijn project dan niet meer noodzakelijk is.

in andere projecten kan ik wel aantonen dat ik de nodige kennis heb van inloggen en registreren mocht hier nu twijfel over bestaan.



OPZOEKWERK & NIEUWE TECHNIEKEN

een groot stuk dat ik nieuw heb aangeleerd is het gebruiken van een in code opgestelde treeview

 - Ik heb gebruik gemaakt van een custom uitgebreide treeview waarbij ik zelf mijn properties moest leren registreren(Dit was voor er een voorbeeld online stond!)

 - Hierarchië van de treeview items leren op te stellen in code.

 -Treeview & treeview items te doen reageren op een CRUD operatie.


Ik heb het ons toegeleerde MVVM principe uitgebreid bestudeerd door op te zoeken hoe ik MVVM kan toepassen op onderstaande zaken

 - user controls binden doormiddel van een contentcontrol in XAML(hier bestaat veel twijfel over in de community en de meningen zijn hierover verdeeld qua werkwijze)

 - Zaken die ik vorig jaar heb geleerd zoals bijvoorbeeld het gebruik van een messagebox & this.close binnen het MVVM principe

	EXTRA TOELICHTING
	Een messagebox mag niet door het viewmodel zelf worden opgeroepen
	Dit breekt het principe want doordat er bijvoorbeeld op ja of nee kan worden gedrukt gaat het viewmodel weet hebben van de view
	Via een Interface als tussenstap is dit rechtgezet!.

	De this.close methode van een view mag in een interface worden gestopt die door polymorfisme via een methode het als een event gaat invoken.

Ik heb ook geleerd om met een kalender te werken voor datum controle te doen(in webapplicaties werk ik ook met een kalender alleen is het doel anders en de code ook).

Ik heb ook opgezocht hoe ik voor het scherm rechtzaak beheren de toolbar kan verwijderen




