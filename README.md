##  Introduction 
This application was made by Edgar Regnault.
It aims to make inventory and  consulting blueprint easier.
All the data are stored locally on the application.

A doxygen documentation and a word file containing the plan of the application can be found in the folder "Documentation".

## Requirement to creating a functional template
-  the excel file used must contains 8 colums
-  The image used for releve (to illustrate where to read the serial number and constructor on a piece) MUST contain the name of the piece it corresponds to.
-  if multiple piece are identical but have a different name tag (a serial number is required for each), it is possible to only have one image whose name contains the piece name. Example: Piece1_Piece2_Piece3.
- at  the same row, theses colums must contains separatly the following strings:
  - "PID" for NameTag
  - "Désignation" for Description
  - "Besoin Qté Totale" used to know at which point   stop when going throught the excel file
  - "Présent" for signaling whether a piece is present "1" or absent "0"
  - "FABRICANT" for constructor
  - "N° SERIE" for serial number
  - "Commentaire" for comment
These fiels must be present or nothing is working.
A good addition in the future would be to add a setting page which lets you choose which string you want for these parameters.

## Organization of the code:

All the functions and class relative to the gestion of excel file, database, and the Datastructure are in the folder Core. It is relatively clean.
The files present are:
- DatabaseManagement.cs: it contains the class required to write and read data to a sqlite database. It uses sqlite-pcl-net libray which lets you directly store and retrieve Table of objects in the database. Though these objest must not be nested, must not contain complex types such as boolean.
- ExcelManagement.cs: it contains the classes required to read and write an excel file
- GlobalProjectData.cs: it is used to pass variables from pages to pages, store constant variables such as image path. It also some methods used to retrieve list of informations relative to file presents in the application.
- GlobalPages.cs it contains a static class used to manage navigaton throuh the pages. It also stores reference to page instances, and the URI for each page's xaml file. Each page initializes its reference when loaded.
- PiecesSections.cs: it contains two classes "Piece" and "Section". Piece contains the methods and Variable and methods to manage a Piece. Section contains // to manage a Section (which is composed of multiples Piece Objects plus other informations)
- ProjectData.cs: it mostly contains two class ProjectInfos and ProjectData. The first one contains path variables and informations relative to the project. The second one contains the first one plus a list of sections, excel file bject, database objects, and a list of the image used for sections and releve. A ProjectData oject is made for containing all the data relative to a project.

The code relative to the code behind of the pages is in MVVM>View. It musts be cleaned. Informations can be found in the doxygen.