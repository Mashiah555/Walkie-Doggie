# **Walkie Doggie**

A .NET MAUI based C# application that allows the users to log and manage their pet dog activities and sync changes across devices

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

## Design Patterns & UI Enhancements

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
     
## Features
  - The application features a theme-specific unified color schemes, defined in the [Colors file](https://github.com/Mashiah555/Walkie-Doggie/blob/master/Resources/Styles/Colors.xaml), located inside Resources/Styles folder.
