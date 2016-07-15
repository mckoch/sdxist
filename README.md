# sdxist



sdxist.exe - Browser und Preprozessor für SDAW Dateien
 
sdxist.exe - Datei Browser und Decoder für den Standard Datensatz Außenwerbung

sdxist.exe eignet sich sowohl als intelligenter Dateibrowser für alle SDAW-Formate als auch als Preprozessor zur weiteren Verarbeitung von SDAW-codierten Daten, z.B. in einer Officeumgebung oder zum Datenexport für Webserver.



    erkennt und interpretiert Kopfdaten von SDAW Dateien
    decodiert STA, FRE, LWT, VMS ... in lesbare Datensätze
    öffnet alle SDAW definierten Formate
    zeigt Inhalte von STA Dateien samt zugehörigen Fotos direkt aus STA-Dateien (falls Bilder vorhanden)
    Bilderverzeichnisse berücksichtigen SDAW konforme Pfade
    zeigt zu jeder Datei die Anzahl enthaltener Datensätze, Sendernummer, Seriennummer, Datum der Erstellung
    liest direkt aus Archivdateien - kein Entzippen notwendig!
    Unterstützung von Google Maps und eigenen MapURLs
    Exportiert decodierte Datensätze zur weiteren Verarbeitung:
    Daten- und Schemaexport nach SQL, CSV, JSON.
    gleichzeitiges Laden mehrerer Dateien aus verschiedenen Verzeichnissen
    Entpacken wahlweise in temporäres Verzeichnis oder in aktuelles Arbeitsverzeichnis (automatischer Schutz vor Überschreiben, abschaltbar)
    Alle Arbeitsschritte werden im Hintergrund protokolliert.

 

Kurzanleitung
SDAW Dateien browsen

    Nach Programmstart dopppelklicken Sie bitte zunächst auf die Schaltfläche "Datenverzeichnis Auswählen".
    Navigieren Sie zu dem Verzeichnis mit (gepackten) SDAW-Dateien.
    Links unten erscheint nun eine Liste aller im Verzeichnis vorhandenen Dateien.
    Klicken Sie auf eine der Dateien in der unteren Liste. Wenn diese ein lesbares Format enthält, zeigt der rechte Bildschirmbereich die entschlüsselten Kopfdaten der Datei. Der obere linke Bilschirmbereich zeigt eine Liste der enthaltenen (noch) codierten Datenzeilen.
    Klicken Sie auf einen Datensatz. Auf der rechten Seite sehen Sie nun bequem lesbar die Details.

    Auf diese Weise können Sie sich nun beliebig durch ihr SDAW-Verzeichnis und vorliegende SDAW-Dateien bewegen.

Standortfotos und Karten anzeigen

    Doppelklicken Sie auf "Bilderverzeichnis Auswählen".
    Navigieren Sie zu dem Verzeichnis mit ihren Standortfotos (z.B. eine erstellte DVD) und bestätigen Sie ihre Auswahl.

    Beim Blättern durch die Datensätze einer STA-Datei prüft sdxist.exe automatisch sowohl "flach" im aktuellen Bildererzeichnis als auch in der SDAW konformen Pfadstruktur. Bei STA-Dateien wird automatisch die Minikarte geladen.

 

Ablage & Export

sdxist.exe ermöglicht die Zusammenstellung und Export eigener Datenauszüge.

    Um ein Element aus der Liste "SDAW Dateien" oder aus der Liste der angezeigeten Datensätze in die Ablage zu übernehmen, müssen Sie es einfach doppelt anklicken.

    Um die Ablage anzuzeigen, aktivieren Sie die Registerkarte "Ablage".

    Die kostenlose Version ist auf die Anzeige und maximalen Export der ersten 20 Datensätze limitiert. Einstellungen werden beim Verlassen des Programms nicht abgespeichert. sdxist.exe enthält keinerlei Adware oder "Nag Screens".

Entstehung

sdxist.exe wurde von Autor im Rahmen eines Drittprojektes zunächst für den eigenen (Entwickler-)Gebrauch geschrieben. Die zu Grunde liegende sdx-Engine beinhaltet über Routinen zur Codierung/Decodierung von zeilenbasierten Festformaten und ermöglicht den Umgang auch mit hohen Datenmengen.
 
Installationsvorraussetzungen

sdx.exe benötigt unter Windows das .net framework 4.5. 
Das Installationsprogramm lädt ggfls. fehlende Komponenten 
während der Installation herunter.

https://www.evernote.com/shard/s122/nl/13013414/396deb57-d12b-4a00-884a-aeb8db2579dd 
