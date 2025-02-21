# TradingSystem

System starten: 

Über Configurations (Rider) Store.WebApi Project mit Profile1 "Store1" launchen ( aktiviere Allow multiple instances!)
oder über kommandozeile in Store.WebApi Verzeichnis wechseln und dann dotnet run --property:StoreName=Store1 --launch-profile Store1

(optional für den zweiten Store)
Über Configurations (Rider) Store.WebApi Project mit Profile2 "Store2" launchen ( aktiviere Allow multiple instances!)
oder über kommandozeile in Store.WebApi Verzeichnis wechseln und dann dotnet run --property:StoreName=Store2 --launch-profile Store2

Über Configurations Enterprise.WebApi Project mit Profile Enterprise1 launchen
über kommandozeile cd Enterprise.WebApi   dotnet run --launch-profile Enterprise1

Store Frontend: 
cd Store.Angular 
npm start 

Enterprise Frontend: 
cd Enterprise.Angular
npm start

(in FostDevices) 
Terminal und Bank starten 

CashDesk Server:
cd CashDesk.Infrastructure 
dotnet run CashDesk1

kc.bat start-dev um Keylcoak zu starten

RabbitMq Broker muss local laufen


Anmeldung bei Keycloak für Frontends: 

Für den ersten Storemanager (Store1)
username: storemanager   
passwort: hello

Für den zweiten Storemanager (Store2)
username : storemanager2
passwort : hello

(damit man sich zweimal anmelden kann muss das Store Frontend auf verschiedenen Browsern am besten laufen)

Für den Enterprisemanager (Enterprise1)
username : enterprisemanager
passwort : hello









