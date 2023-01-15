# MTCG-MonsterTradingCardGame

Protokoll

Der Code ist aufgeteilt in BL, DAL, http und Model, wobei letzteres keinen Inhalt hat. In http ist alles drinnen was etwas mit http zu tun hat wie HttpProcessor, HttpRequest, HttpResponse und der HttpServer. Auch sind die Controller für die weiteren Funktionen in der http Ordner. Die Controller erben von einem IController welcher eine Run Methode verlangt. Der Controller schaut dann was für eine Request gesendet wurde und gibt an seine Logik Klasse weiter, welche sich im BusinessLayer befindet. Die Logik Klassen greifen auf andere Logik Klassen zu und auf ihr eigenes Repository, welches auf die Datenbank zugreift.
Große Probleme hatte ich beim Erstellen eines Kampfes. Das Warten auf eine andere Anfrage war eine Herausforderung. Das Problem wurde mit einer BattleUtils Klasse gelöst welche ein ManualResetEventSlim sowie dazugehörige Player und das BattleLog enthält. Diese werden in einer static List<BattleUtils> gespeichert. 
Die Unit Tests testen einfache Funktionen darauf, ob diese auch ihre Aufgabe machen. Es werden User erstellt, User gelöscht und auch Userinformationen bearbeitet. Auch wird der Code auf einige Exceptions getestet.

Ich habe an dem Projekt 80-90 Stunden gearbeitet und es fehlen leider noch ein paar Funktionalitäten.
Es gibt eine Datenbank SQL Datei mit den CREATE befehlen für die Tabellen.
Als Unique Feature könnte man die Möglichkeit sehen User zu löschen. Außerdem kann man sich ausloggen und die Passwörter werden gehasht.
