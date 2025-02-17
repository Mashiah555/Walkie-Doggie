# Walkie Doggie

A .NET MAUI based C# application that allows the users to log and manage their pet dog activities and sync changes across devices

## Database Service

### 1️⃣ Firebase Cloud Firestore Service

  #### Set Up a Firebase Project
  - Go to [Firebase Console](https://console.firebase.google.com).
  - Click Create a Project → Name it (e.g., WalkieDoggieDB).
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
  - *add a link to the following code lines:
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

public class FirebaseService
{
    private static FirestoreDb _firestoreDb;
    private const string CollectionName = "Walks";

    public FirebaseService()
    {
        if (_firestoreDb == null)
        {
            _firestoreDb = Authenticate();
        }
    }

    private FirestoreDb Authenticate()
    {
        var assembly = IntrospectionExtensions.GetTypeInfo(typeof(FirebaseService)).Assembly;
        var stream = assembly.GetManifestResourceStream("YourNamespace.Resources.Raw.firebase-adminsdk.json");

        if (stream == null)
            throw new Exception("Firebase credentials file not found.");

        var credential = GoogleCredential.FromStream(stream);
        FirestoreDb db = FirestoreDb.Create("your-project-id", credential);
        return db;
    }
}

  - Replace "your-project-id" with your Firebase Project ID (found in Firebase settings).
  - Define the Firestore implementation inside every relevent page: "var firebaseService = new FirebaseService();"
