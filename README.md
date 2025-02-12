<!-- PROJECT LOGO -->
<br />
<div align="center">
  <a href="https://github.com/NoahAldahan/FISA_A3_GL_Aldahan">
    <img src="Documentation/img/saveIcon.png" alt="EasySave" width="80" height="80">
  </a>

<h3 align="center">EasySave</h3>

#### **1. Objectif du Logiciel**
EasySave 1.0 est une application console développée avec .Net Core. Son objectif est de permettre la gestion et l’exécution de travaux de sauvegarde (backup) de manière simple et efficace, tout en garantissant une compatibilité pour des utilisateurs anglophones et francophones.

---

#### **2. Fonctionnalités Principales**

##### **2.1 Gestion des Travaux de Sauvegarde**
- Possibilité de créer **jusqu'à 5 travaux de sauvegarde**.
- Définition d’un travail de sauvegarde par :
  - **Nom** du travail.
  - **Répertoire source**.
  - **Répertoire cible**.
  - **Type de sauvegarde** :
    - Sauvegarde complète.
    - Sauvegarde différentielle.
- Prise en charge des répertoires situés sur :
  - Disques locaux.
  - Disques externes.
  - Lecteurs réseaux.

##### **2.2 Exécution des Sauvegardes**
- Exécution à la demande d’un ou plusieurs travaux de sauvegarde.
- Exécution séquentielle de l’ensemble des travaux.
- Commandes utilisables en ligne de commande pour l’exécution automatique :
  - **Exemple 1** : `1-3` pour exécuter les sauvegardes 1 à 3.
  - **Exemple 2** : `1;3` pour exécuter les sauvegardes 1 et 3.

##### **2.3 Sauvegarde Complète des Répertoires**
- Tous les fichiers et sous-répertoires d’un répertoire source sont inclus dans la sauvegarde.

---

#### **3. Journalisation et Suivi des Sauvegardes**

##### **3.1 Fichier Log Journalier**
- Écriture en temps réel des actions réalisées dans un fichier log journalier au format JSON.
- Contenu minimal pour chaque action :
  - **Horodatage**.
  - **Nom de sauvegarde**.
  - **Adresse complète** du fichier source (format UNC).
  - **Adresse complète** du fichier de destination (format UNC).
  - **Taille du fichier**.
  - **Temps de transfert** en millisecondes (valeur négative si erreur).
- Le fichier doit permettre une lecture facile via Notepad, avec des retours à la ligne entre les éléments JSON.

##### **3.2 Fichier d'État en Temps Réel**
- Enregistrement en temps réel de l’état des travaux dans un fichier unique au format JSON.
- Informations minimales enregistrées pour chaque travail :
  - **Nom du travail**.
  - **Horodatage** de la dernière action.
  - **État** (ex. : Actif, Non Actif...).
  - Si actif :
    - **Nombre total de fichiers** éligibles.
    - **Taille totale** des fichiers à transférer.
    - **Progression**.
    - **Nombre de fichiers restants**.
    - **Taille des fichiers restants**.
    - **Adresse complète** du fichier source en cours.
    - **Adresse complète** du fichier de destination.

---

#### **4. Contraintes Techniques**

##### **4.1 Compatibilité et Configuration**
- Les emplacements des fichiers (log journalier et état) doivent être compatibles avec les serveurs clients. Les emplacements temporaires comme `c:\temp\` sont proscrits.
- Format JSON obligatoire pour tous les fichiers (log, état, et configurations éventuelles).

##### **4.2 Modularité (facultatif)**
- Développement de la fonctionnalité de journalisation (log) sous forme de **Dynamic Link Library (DLL)**.
- La DLL doit rester compatible avec la version 1.0, même lors d’évolutions futures.

##### **4.3 Pagination (facultatif)**
- Pagination des fichiers JSON pour faciliter la lecture rapide.

---

#### **5. Évolutions Futures**
Si la version 1.0 est jugée satisfaisante, une **version 2.0** avec une interface graphique (architecture MVVM) sera développée.

---
#### **5. Contraintes**
- **Outils** :
  - Visual studio 2022
  - PlantUML
  - WPF
  - Package nuget
  - Pipeline (déploiement / test)
  - Github : 
    - Convention de commit :
      - type : correspondant à une information sur le type de rajout ou de décrément de
        contenu dans le commit (par exemple fix, feat, test, init, docs, etc),
      - sujet (scope) : équivaut à l’information de la modification effectuée (style, logic,
        structure, documentation,...)
      - description : détails des modifications comme dans un commit sans convention
    - Workflow :

      ![workflow](img/workflow.png)

      Chaque développeur travaille sur sa propre branche. Lorsqu'il effectue un commit pour 
      ajouter une nouvelle fonctionnalité, corriger un bug ou toute autre modification, un merge 
      est effectué vers une branche TEST dédiée au livrable en cours, par exemple, `TEST-lv1`. Une fois les modifications
      validées, elles sont ensuite poussées sur une branche MAIN spécifique au même livrable, par exemple, `lv1`.

      Lorsqu'un nouveau livrable débute, une nouvelle paire de branches est créée : une branche TEST 
      pour les développements (`TEST-lv2`) et une branche MAIN pour les livraisons finales (`lv2`).
      Ces nouvelles branches partent de l'état final de la branche MAIN du livrable précédent (par exemple, `lv1`), 
      garantissant ainsi une continuité et une base de travail stable pour le nouveau cycle de développement.

- **Langages et Frameworks** :
  - C#
  - Dotnet 8.0
  - Architecture logiciel extensible
  - Convention de nommage :
    - Les conventions de nommage dans un projet C# sont essentielles pour garantir la lisibilité et la cohérence de notre code. 
    Concernant les conventions générales de programmation, on utilise le PascalCase et le camelCase. 
    - Le PascalCase est utilisé pour les noms publics dont les noms de classe, les méthodes ou encore les propriétés.
    - Le camelCase est utilisé pour les noms privés, locaux ou encore les paramètres.




<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributors">Contributors</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project


EasySave is a backup software developed as part of a structured project within ProSoft. The project simulates an accelerated development cycle, covering multiple versions of the software. The goal is to design, implement, and document a robust and maintainable backup solution using C# and .NET 8.0, while adhering to best practices in version control, code quality, and user documentation. The software is designed to be distributed to clients, requiring well-structured UI/UX and efficient backup functionality.


### Built With

- Visual Studio
- C# 
- .NET


<!-- GETTING STARTED -->
## Getting Started


### Installation

To use the application :

Download the exe of the version you want in the releases and execute it.

To access and modify source code :
- Visual Studio 2022
```
Download and install from this link
https://visualstudio.microsoft.com/fr/
When installing Visual Studio, make sure to check :
- Multiplatform development
- .NET native
- kit SDK .NET
- .NET Framework
```

- System.Text.Json
```
In Visual Studio, open Project, Manage NuGetPackages
Search for System.Text.JSON and install it
```
- DotNetEnv
```
In Visual Studio, open Project, Manage NuGetPackages
Search for DotNetEnv and install it
```


<!-- USAGE EXAMPLES -->
## Usage

Here are the options you might find in the CLI application :
![Main menu](Documentation/img/MainMenu.png)
![Save menu](Documentation/img/SaveMenu.png)

_For a step by step explanation, please refer to the [user guide](Documentation/UserGuide.pdf), ([guide utilisateur](Documentation/GuideUtilisateur.pdf))_


<!-- ROADMAP -->
## Roadmap

- [ ] Version 1
    - [ ] Version 1.1
- [ ] Version 2
- [ ] Version 3

## Contributors

* [Romain](https://github.com/Romain68)
* [Jean](https://github.com/Yamigiri1)
* [Mattéo](https://github.com/Mattbalaise)
* [Maxime Noah](https://github.com/NoahAldahan)
