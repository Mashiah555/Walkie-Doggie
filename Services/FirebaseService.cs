using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Grpc.Auth;
using Walkie_Doggie.Helpers;

public class FirebaseService
{
    private static FirestoreDb? _firestoreDb;
    private const string UsersCollection = "Users";
    private const string WalksCollection = "Walks";

    #region Firebase Operations
    public FirebaseService()
    {
        if (_firestoreDb == null)
        {
            _firestoreDb = Authenticate();
        }
    }

    private FirestoreDb Authenticate()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = "Walkie_Doggie.Resources.Raw.firebase-adminsdk.json";

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == null)
                throw new Exception($"Resource {resourceName} not found.");

            GoogleCredential credential = GoogleCredential.FromStream(stream);

            return new FirestoreDbBuilder
            {
                ProjectId = "walkiedoggiedb-mashiah",
                Credential = credential,
                EmulatorDetection = Google.Api.Gax.EmulatorDetection.None // Ensure it's using the live Firestore
            }.Build();
        }
    }
    #endregion Firebase Operations

    #region User CRUD Operations

    // ➤ Add a User to Firestore
    public async Task AddUserAsync(string name)
    {
        await _firestoreDb!
            .Collection(UsersCollection)
            .Document(name)
            .SetAsync(new UserModel
            {
                Name = name,
                TotalWalks = 0
            });
    }

    // ➤ Get All Users from Firestore
    public async Task<List<UserModel>> GetUsersAsync()
    {
        var users = new List<UserModel>();
        QuerySnapshot snapshot = await _firestoreDb!.Collection(UsersCollection).GetSnapshotAsync();

        foreach (var doc in snapshot.Documents)
        {
            users.Add(doc.ConvertTo<UserModel>());
        }

        return users;
    }
    #endregion User CRUD Operations

    #region Walk CRUD Operations
    // ➤ Add a Walk Record
    public async Task AddWalkAsync(string walkerName, DateTime walkTime, bool isPooped, string notes)
    {
        await _firestoreDb!
            .Collection(WalksCollection)
            .Document($"{walkerName}_{walkTime:ddMMyyyy_HHmmss}")
            .SetAsync(new WalkModel 
            {
                WalkerName = walkerName,
                WalkTime = Converters.ConvertToTimestamp(walkTime),
                IsPooped = isPooped,
                Notes = notes
            });

        // Update User's TotalWalks Count
        var userRef = _firestoreDb.Collection(UsersCollection).Document(walkerName);
        var userSnapshot = await userRef.GetSnapshotAsync();

        if (userSnapshot.Exists)
        {
            var user = userSnapshot.ConvertTo<UserModel>();
            user.TotalWalks += 1;
            await userRef.SetAsync(user);
        }
    }

    // ➤ Get All Walks from Firestore
    public async Task<List<WalkModel>> GetWalksAsync()
    {
        var walks = new List<WalkModel>();
        QuerySnapshot snapshot = await _firestoreDb!.Collection(WalksCollection).OrderBy("WalkTime").GetSnapshotAsync();

        foreach (var doc in snapshot.Documents)
        {
            walks.Add(doc.ConvertTo<WalkModel>());
        }

        return walks;
    }
    #endregion Walk CRUD Operations
}
