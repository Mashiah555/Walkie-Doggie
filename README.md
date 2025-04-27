# **Walkie Doggie**

***Walkie Doggie*** is a mobile application that allows users to log and manage their pet dog activities, synced automatically across devices.
    The application is built using .NET MAUI, a cross-platform framework that enables developers to create native applications for Android, iOS, and Windows using C# and XAML.

## Installation

**Latest Release:** Version 1.1.1
Click Here to download the application for Android devices

## Database Service

### 1️⃣ Create a Project Via Firebase Console

  #### Set Up a Firebase Project
  - Go to [Firebase Console](https://console.firebase.google.com).
  - Click Create a Project → Name it (e.g. WalkieDoggieDB).
  - Click Continue until the project is created.
  #### Enable Firestore Database
  - Inside your Firebase project, go to Build → Firestore Database.
  - Click Create Database.
  - Choose Start in test mode (for development).
  - Select a location (e.g., us-central1).
  - Click Enable.
  #### Get Firebase SDK.JSON Key
  - In Project Settings → Service Accounts.
  - Click Generate New Private Key (Downloads a json file).
  - Rename the file to: firebase-adminsdk.json
  - Move the JSON file to your Resources/Raw folder in the .NET MAUI project.
  - Set the file's Build Action to Embedded Resource (Right Click the file in the Solution Explorer → Choose Properties).

### 2️⃣ Install Required Packages

  - Right Click the Dependencies in the Solution Explorer → Choose Manage NuGet Packages
  - Download the following NuGet packages:
    - FirebaseAdmin (by Google Inc.)
    - Google.Apis (by Google LLC)
    - Google.Cloud.FireStore (by Google LLC)
  
### 3️⃣ Implement the Firestore service in your project

  - Add a new folder, and name it Services
  - Inside the Services folder, add a new class called: FirebaseService.cs
  - Write [these code lines](https://github.com/Mashiah555/Walkie-Doggie/blob/master/Services/FirebaseService.cs#L1-L43) to initialize access to Firebase.
  - Replace "your-project-id" with your Firebase Project ID (found in Firebase settings).
  - Define the Firestore implementation inside every relevent page: "var firebaseService = new FirebaseService();"

## UI Enhancements

### The Community Toolkit
The .NET MAUI Community Toolkit enables the use of a more modern, unified, organized, and specific UI design.
The toolkit implements some modernized controls and components for the designing proccess, as well as predefined converters for a clean and organized coding experience.

  - Right Click the Dependencies in the Solution Explorer → Choose Manage NuGet Packages
  - Download the following NuGet package:
    - CommunityToolkit.Maui
  - **Reference the toolkit in xaml files:**
    - Add the following namespace to the required page inside it's xaml file declarations:
      - xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
    - Write "toolkit:" before stating the name of a desired toolkit component.
  - **Reference the toolkit in the code-behinds:**
    - Add the following using statement to the required page's code-behind:
      - using CommunityToolkit.Maui

### Popup Pages Plugin
The Plugin.Maui.Popup library allows the use of popups and modals in the application, which can be used for displaying information, alerts, or custom content.
  - Right Click the Dependencies in the Solution Explorer → Choose Manage NuGet Packages
  - Download the following NuGet package:
    - Plugin.Maui.Popup
  - **Reference the plugin in xaml files:**
    - Add the following namespace to the required page inside it's xaml file declarations:
      - xmlns:popup="clr-namespace:Plugin.Maui.Popup;assembly=Plugin.Maui.Popup"
    - Write "popup:" before stating the name of a desired popup component.
  - **Reference the plugin in the code-behinds:**
    - Add the following using statement to the required page's code-behind:
      - using Plugin.Maui.Popup;
  - [Click here](https://xamarincodingtutorial.blogspot.com/2022/09/pluginmauipopup.html) to learn about the plugin and how to use it functionallities.
     
## Features
  - The application features a theme-specific unified color schemes, defined in the [Colors file](https://github.com/Mashiah555/Walkie-Doggie/blob/master/Resources/Styles/Colors.xaml), located inside Resources/Styles folder.
