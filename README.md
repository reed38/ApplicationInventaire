

##  Introduction 
Cette application a pour but de faciliter l'accès aux plan et le suivi des manquants d'un montage.

## Autre
Ce dossier contient les éléments n'étant pas directement en lien avec le code/du projet.
### ReleaseApp
Contient la release de l'application. Cette version est indépendante du framework .NET et peut donc simplement
être copiée sur un oridinateur, elle fonctionnera immédiatement.

### DemoRessources
Contient des ressources pour effectuer une démo de l'application. Des infos supplémentaires sur ces ressources peuvent être trouvé 
dans le .md situé dans ce dossier.
### Documentation 
Contient la documentation utilisateur et la documentation programmeur.
### Code
Documentation programeur. Une documentation doxygen et le doxyfile utilisé pour la générer y sont présents.
### Utilisateur
Contient le PDF du manuel d'utilisateur.
## Core
Contient les classes utilisée pour gérer la structure de données de l'application et des Templates.

- DatabaseManagement.cs: Classes et méthodes utilisées pour lire et écrire dans une base de données sqlite les structures de données utilisés par l'application. La library utilisée pour cela est 
- ExcelManagement.cs: Classes et méthodes utilisées pour lire et écrire un Document Excel. La library utilisée pour cela est NPOI.
- GlobalProjectData.cs: Contient les variables/ méthodes globales, classe statique.
- GlobalPages.cs: Contient la classe utilisée pour gérer la navigation entre les pages de l'application.
- PiecesSections.cs: Contient les classes "Piece" et "Section".
- ProjectData.cs: Contient principalement la classe utilisée instancier un template. Cette classe est un agrégat des classes contenues dans les fichiers précédents.



## MVVM
Interface graphique de l'application
### Model
### View
Contient le code xaml et le code behind des pages de l'application.
## Ressources
Ressources utilisées par l'application. Contient seulement des images pour le moment.
## Theme
Contient les thèmes d'éléments utilisés dans le code xaml (ex apparence boutton).
