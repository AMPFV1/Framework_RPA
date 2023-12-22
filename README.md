# Framework_RPA
---
Framework für den Einsatz von Robotic Process Automation zur Testautomatisierung
---
Mit diesem open-source-Framework ist es möglich eine RPA Software für die Testautomatisierung einzusetzen. 
Als RPA Software können beispielsweise unter anderem [OpenRPA](https://github.com/open-rpa/openrpa) oder [taskt](https://github.com/saucepleez/taskt) implementiert werden.

Das Framework bietet zur einfacheren Bedienung eine grafische Benutzeroberfläche. Es unterstützt bei der Verwaltung der Testfälle und Testdaten, es übernimmt die Steuerung der einzelnen Softwareroboter und bietet am Ende jedes Testdurchlaufes eine Übersicht über die Ergebnisse anhand eines Soll- und Ist-Vergleiches sowie ein Fortschrittsprotokoll zur einfachen Fehlererkennung.

## Einstellungen

Bei der erstmaligen Verwendung dieses Frameworks müssen zu Beginn einige Einstellungen vorgenommen werden. Das kann auf 2 Arten geschehen. Entweder man passt die default Einträge bei der Funktion setDefaultValues() in der Klasse RegistrySettings.cs an oder nach dem ersten Start können die Einträge direkt in der Registry geändert werden.

```c#
public static void setDefaultValues()
        {
            defaultValues.Clear();
            defaultValues.Add("Pfad_Ergebnisse", @"D:\Daten\Framework_RPA\Ergebnisse");
            defaultValues.Add("Pfad_Testfälle", @"D:\Daten\Framework_RPA\Testfälle");
            defaultValues.Add("RPA_Software", "");
            defaultValues.Add("Max_Dauer", "900000");
            SetAllRegitryKeys();
        }
```
Die Einträge werden in der Registry unter diesen Pfad gespeichert:
```
HKEY_CURRENT_USER\SOFTWARE\Framework_RPA
```

Hier eine Auflistung der erfordlichen Einstellungen mit einer Beschreibung:
|Name|Wert|Beschreibung|
|---|-----|------------|
|Pfad_Ergebnisse|D:\Daten\Framework_RPA\Ergebnisse|Ablagepfad für die Ergebnisse|
|Pfad_Testfälle|D:\Daten\Framework_RPA\Testfälle|Ablagepfad für die Dateien der Testfälle|
|RPA_Software|RPASoftware.exe|Pfad und die Bezeichnung der ausführbaren Datei der RPA-Software|
|Max_Dauer|900000|Wartezeit des Frameworks bis der Testfall beendet wird

## RPA Erweiterungen
[!IMPORTANT]
Der Einsatz einer RPA Software gemeinsam mit dem Framework ist nur möglich wenn die Funktionen der RPA Software mit DLL erweitert werden können.


Die Funktionen werden durch die Testautomation.dll zur Verfügung gestellt bzw. befindet sich dieses Projekt in dem Ordner [Testprotokoll](https://github.com/AMPFV1/Framework_RPA/tree/795a964780ef6ddd9ce27d29d677f32da1415b55/Testprotokoll). Die 
Einbindung dieser Funktionen kann über zwei Arten erfolgen. Entweder als eine 
Windows-Workflow-Foundation-Aktivität oder direkt über den Aufruf der Funktionen 
aus der Klasse RPATestautomation. In der Abbildung 15 sind die oberen fünf Klassen 
die Aktivitäten und die untere Klasse beinhaltet die Funktionen. Jede Aktivität ruft 
über die Methode Execute die jeweilige zugehörige Methode aus der Klasse 
RPATestautomation auf und übergibt ihr die erforderlichen Parameter. Die 
erforderlichen Parameter sind bei den Methoden der Klasse RPATestautomation gleich 
wie bei der jeweiligen Aktivität.

### RPA_Close – CloseRPA
Im Zuge der Entwicklung des Prototyps wurde festgestellt, dass sich nicht alle 
Softwareroboter am Ende des Prozesses automatisch oder durch eine eigene Funktion 
schließen lassen. Daher war die Zurverfügungstellung einer Funktion, welche die RPASoftware schließt, erforderlich.
### TestcaseInfos – GetValueFromTestcaseXML
Die durch das Framework erstellten Informationen für den jeweiligen Testfall, in Form 
einer XML-Datei, sind zum Teil auch für den jeweiligen Softwareroboter von Interesse. 
Daher muss jede RPA-Software auf diese XML zugreifen und die jeweiligen 
Informationen auslesen können. Die Methode liefert immer nur einen Wert zurück,
wodurch der Aufruf für jeden erforderlichen Wert notwendig ist.
### SetError – SetError
Auch beim Softwareroboter kann es passieren, dass ein Fehler im Prozess auftritt und er 
den Testfall nicht bis zum Ende durchlaufen kann. In einem solchen Fall wird diese 
Methode aufgerufen und in der XML der Eintrag Error gesetzt. Diese Methode wird vor 
allem beim Abfangen von Ausnahmen durch Try and Catch eingesetzt.
### ProgressProtocol – WriteProgress
Diese Methode fügt der XML im Ergebnisordner Fortschrittsinformationen hinzu. Diese 
können individuell durch die jeweiligen Anwendenden im Testprozess gesetzt und durch 
einen individuellen Text ergänzt werden. Auch diese Funktion kann beliebig oft durch 
den Softwareroboter aufgerufen werden. 
### ResultProtocol – WriteResult
Wird bei einem Testdurchlauf ein Testfall gestartet, wird durch das Framework im 
Ordner Ergebnisse eine XML mit dem Namen des Testfalles erstellt. Durch diese 
Methode wird dieser XML das Testergebnis Soll, Ist und ein Text hinzugefügt. Da es bei 
einem Testfall mehrere Testergebnisse geben kann, ist der Aufruf dieser Funktion 
beliebig oft möglich.

## Testfälle
Ein neuer Testfall kann über die Schaltfläche "Testfall erstellen" angelegt werden.
Bei der Anlage eines neuen Testfalls ist die Bezeichnung für den Testfall, der Ablagebereich und eventuelle Übergabeparameter für den Start der RPA Software erforderlich anzugeben.

Zu jeden Testfall kann eine Beschreibung hinzugefügt werden. Testdaten können vom Framework anhand von einer Excel-Datei hinzugefügt werden.

Alle Testfälle und Bereiche werden in der Mitte des Hauptfensters angezeigt. Die Testfälle können über die Checkbox ausgewählt werden. Dabei können mehrere Testfälle gleichzeitig für einen Testdurchlauf ausgewählt werden.

## Testdurchlauf
Wurden Testfälle ausgewählt kann links zum Testdurchlauf navigiert werden. Im Fenster für den Testdurchlauf werden nochmal alle ausgewählten Testfälle und einige Systeminformationen angezeigt. Jeder Testdurchlauf kann beschrieben werden. 

Mit der Schaltfläche "Starten", welche sich im Menüband befindet, wird der Testdurchlauf gestartet. 

## Reporting

Nach Abschluss des Testdurchlaufes wird das Ergebnis angezeigt. Alle Ergebnisse werden links in der Übersicht mit dem Datum und der Uhrzeit aufgelistet und können auch jederzeit geöffnet werden.

Die Übersich zum Ergebnis zeigt die Dauer mit Sart und Ende, sowie einzelnen Testfälle und das Ergebnis OK, FAIL oder Error. Über den Link Details können die Detailergebnisse zum jeweiligen Testfall geöffnet werden.
