using System.Reflection;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
using Microsoft.Maui.Networking;

public class FirebaseService
{
    #region Firestore Properties
    private static FirestoreDb? _firestoreDb;
    private const string UsersCollection = "Users";
    private const string WalksCollection = "Walks";
    private const string FeedsCollection = "Feeds";
    private const string DogsCollection = "Dogs";
    #endregion Firestore Properties

    #region Firebase Authentication
    public FirebaseService()
    {
        if (_firestoreDb == null)
        {
            _firestoreDb = Authenticate();
        }
    }

    private static bool IsConnected()
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            Shell.Current?.GoToAsync(nameof(MessagePopup), true);
            return false;
        }
        return true;
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
    #endregion Firebase Authentication

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
                TotalWalks = 0,
                Theme = AppTheme.Unspecified
            });
    }

    // ➤ Update a User in Firestore
    public async Task UpdateUserAsync(string name, AppTheme appTheme)
    {
        await _firestoreDb!
            .Collection(UsersCollection)
            .Document(name)
            .UpdateAsync("Theme", appTheme);
    }

    // ➤ Get All Users from Firestore
    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = new List<UserModel>();
        QuerySnapshot snapshot = await GetUsersSnapshot();

        foreach (var doc in snapshot.Documents)
            users.Add(doc.ConvertTo<UserModel>());

        return users;
    }

    // ➤ Get All Usernames from Firestore
    public async Task<List<String>> GetAllUsernamesAsync()
    {
        var users = new List<String>();
        QuerySnapshot snapshot = await GetUsersSnapshot();

        foreach (var doc in snapshot.Documents)
            users.Add(doc.ConvertTo<UserModel>().Name);

        return users;
    }

    // ➤ Searches for a User in Firestore
    public async Task<bool> HasUserAsync(string name)
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(UsersCollection)
            .WhereEqualTo("Name", name)
            .GetSnapshotAsync();
        return snapshot.Documents.Count > 0;

        //bool tr = (await GetAllUsersAsync()).Any(user => user.Name == name);
    }

    public async Task<UserModel?> GetUserAsync(string name)
    {
        var userSnapshot = await GetUserReference(name).GetSnapshotAsync();

        if (userSnapshot.Exists)
            return userSnapshot.ConvertTo<UserModel>();
        return null;
    }

    private async Task IncrementTotalWalks(string name)
    {
        var userRef = GetUserReference(name);
        var userSnapshot = await userRef.GetSnapshotAsync();

        if (userSnapshot.Exists)
        {
            var user = userSnapshot.ConvertTo<UserModel>();
            user.TotalWalks += 1;
            await userRef.SetAsync(user);
        }
    }

    private DocumentReference GetUserReference(string name)
    {
        return _firestoreDb!
            .Collection(UsersCollection)
            .Document(name);
    }

    private async Task<QuerySnapshot> GetUsersSnapshot()
    {
        return await _firestoreDb!
            .Collection(UsersCollection)
            .GetSnapshotAsync();
    }

    #endregion User CRUD Operations

    #region Walk CRUD Operations
    // ➤ Add a Walk Record
    // Throws an exception if there are no dogs saved in the database.
    public async Task AddWalkAsync(string walkerName, DateTime walkTime, bool isPooped,
        string? notes = null, string? inDebtName = null, bool? isPayback = false)
    {
        int walkId = await GetWalksIdAsync(true);
        await _firestoreDb!
            .Collection(WalksCollection)
            .Document(walkId.ToString())
            .SetAsync(new WalkModel 
            {
                WalkId = walkId,
                WalkerName = walkerName,
                WalkTime = Converters.ConvertToTimestamp(walkTime),
                IsPooped = isPooped,
                InDebtName = inDebtName,
                IsPayback = isPayback,
                Notes = notes
            });

        // Update User's TotalWalks Count
        await IncrementTotalWalks(walkerName);
    }

    // ➤ Get a Walk Record
    public async Task<WalkModel?> GetWalkAsync(int? walkId)
    {
        if (walkId == null)
            return null;

        DocumentSnapshot snapshot = await _firestoreDb!
            .Collection(WalksCollection)
            .Document(walkId!.ToString())
            .GetSnapshotAsync();

        if (snapshot.Exists)
            return snapshot.ConvertTo<WalkModel>();
        return null;
    }

    // ➤ Get the last walk from Firestore
    public async Task<WalkModel> GetLastWalkAsync(string? username = null)
    {
        Query query = _firestoreDb!
            .Collection(WalksCollection)
            .OrderByDescending("WalkTime");

        if (!string.IsNullOrEmpty(username))
            query = query.WhereEqualTo("WalkerName", username);

        QuerySnapshot snapshot = await query.Limit(1).GetSnapshotAsync();

        return snapshot.Documents.Count > 0 ?
            snapshot.Documents[0].ConvertTo<WalkModel>() :
            throw new Exception("There are no walks saved in the database yet!");
    }

    // ➤ Get All Walks from Firestore
    public async Task<List<WalkModel>> GetAllWalksAsync()
    {
        var walks = new List<WalkModel>();
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(WalksCollection)
            .OrderBy("WalkTime")
            .GetSnapshotAsync();

        foreach (var doc in snapshot.Documents)
            walks.Add(doc.ConvertTo<WalkModel>());

        return walks;
    }
    #endregion Walk CRUD Operations

    #region Feed CRUD Operations
    // ➤ Add a Feed Record
    public async Task AddFeedAsync(string feederName, DateTime feedTime, int feedAmount, string? notes)
    {
        await _firestoreDb!
            .Collection(FeedsCollection)
            .Document($"{feederName}_{feedTime:ddMMyyyy_HHmmss}")
            .SetAsync(new FeedModel
            {
                FeederName = feederName,
                FeedTime = Converters.ConvertToTimestamp(feedTime),
                FeedAmount = feedAmount,
                Notes = notes
            });
    }

    // ➤ Get the last walk from Firestore
    public async Task<FeedModel> GetLastFeedAsync()
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(FeedsCollection)
            .OrderByDescending("FeedTime")
            .Limit(1)
            .GetSnapshotAsync();

        return snapshot.Documents.Count > 0 ?
            snapshot.Documents[0].ConvertTo<FeedModel>() :
            throw new Exception("There are no feedings saved in the database yet!");
    }

    // ➤ Get All Walks from Firestore
    public async Task<List<FeedModel>> GetAllFeedsAsync()
    {
        var feeds = new List<FeedModel>();
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(FeedsCollection)
            .OrderBy("FeedTime")
            .GetSnapshotAsync();

        foreach (var doc in snapshot.Documents)
            feeds.Add(doc.ConvertTo<FeedModel>());

        return feeds;
    }
    #endregion Feed CRUD Operations

    #region Dog CRUD Operations
    public async Task<bool> AddDogAsync(string name, DateTime birthdate, string breed, 
        double weight, int feedAmount)
    {
        if (await HasDog())
            return false;

        int totalWalks;
        try
        {
            WalkModel lastWalk = await GetLastWalkAsync();
            totalWalks = lastWalk.WalkId;
        }
        catch { totalWalks = 0; }

        await _firestoreDb!
            .Collection(DogsCollection)
            .Document(name)
            .SetAsync(new DogModel
            {
                DogName = name,
                DogBirthdate = Converters.ConvertToTimestamp(birthdate),
                DogBreed = breed,
                DogWeight = weight,
                DefaultFeedAmount = feedAmount,
                TotalWalks = totalWalks
            });

        return true;
    }

    public async Task<bool> UpdateDogAsync(DateTime birthdate, string breed,
        double weight, int feedAmount)
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(DogsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Documents.Count == 0)
            return false;

        DocumentReference reference = snapshot.Documents[0].Reference;
        await reference.UpdateAsync("DogBirthdate", Converters.ConvertToTimestamp(birthdate));
        await reference.UpdateAsync("DogBreed", breed);
        await reference.UpdateAsync("DogWeight", weight);
        await reference.UpdateAsync("DefaultFeedAmount", feedAmount);

        return true;
    }

    // ➤ Get the last dog from Firestore
    public async Task<DogModel> GetDogAsync()
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(DogsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        return snapshot.Documents.Count > 0 ?
            snapshot.Documents[0].ConvertTo<DogModel>() :
            throw new Exception("There is no dog saved in the database!");
    }

    public async Task<bool> HasDog()
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(DogsCollection)
            .Limit(1)
            .GetSnapshotAsync();
        return snapshot.Documents.Count > 0;
    }

    // ➤ Get the total walks of the dog, and optionally increment it.
    // Throws an exception if there are no dogs saved in the database.
    public async Task<int> GetWalksIdAsync(bool increment = false)
    {
        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(DogsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Documents.Count == 0)
            throw new DirectoryNotFoundException("There are no dogs saved in the database yet!");

        DogModel dog = snapshot.Documents[0].ConvertTo<DogModel>();

        if (increment)
        {
            dog.TotalWalks++;
            await snapshot.Documents[0].Reference.UpdateAsync("TotalWalks", dog.TotalWalks);
        }
        return dog.TotalWalks;
    }
    #endregion Dog CRUD Operations
}
