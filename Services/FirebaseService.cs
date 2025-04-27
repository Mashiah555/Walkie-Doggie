using System.Reflection;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;

public class FirebaseService
{
    #region Firestore Properties
    private static FirestoreDb? _firestoreDb;
    private const string UsersCollection = "Users";
    private const string WalksCollection = "Walks";
    private const string FeedsCollection = "Feeds";
    private const string DogsCollection = "Dogs";
    private const string ConfigsCollection = "Configurations";
    #endregion Firestore Properties

    #region Firebase Authentication
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
    #endregion Firebase Authentication

    #region User CRUD Operations

    // ➤ Add a User to Firestore
    public async Task AddUserAsync(string name)
    {
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

        await _firestoreDb!
            .Collection(UsersCollection)
            .Document(name)
            .UpdateAsync("Theme", appTheme);
    }

    // ➤ Get All Users from Firestore
    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        await NetworkService.NetworkCheck();

        List<UserModel> users = new();
        QuerySnapshot snapshot = await GetUsersSnapshot();

        foreach (var doc in snapshot.Documents)
            users.Add(doc.ConvertTo<UserModel>());

        return users;
    }

    // ➤ Get All Usernames from Firestore
    public async Task<List<String>> GetAllUsernamesAsync()
    {
        return (await GetAllUsersAsync())
            .Select(user => user.Name).ToList();
    }

    // ➤ Searches for a User in Firestore
    public async Task<bool> HasUserAsync(string name)
    {
        return await GetUserAsync(name) != null;
    }

    public async Task<UserModel?> GetUserAsync(string name)
    {
        await NetworkService.NetworkCheck();

        var userSnapshot = await GetUserReference(name).GetSnapshotAsync();

        if (userSnapshot.Exists)
            return userSnapshot.ConvertTo<UserModel>();
        return null;
    }

    private async Task IncrementTotalWalks(string name)
    {
        await NetworkService.NetworkCheck();

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
        //WARNING! Notice:
        //This method should only be called after verifying a secure
        //connection to the database, as it does not check for network access.
        //In order to use this method, be sure to first call NetworkCheck(),
        //as it is awaited unlike this method.

        return _firestoreDb!
            .Collection(UsersCollection)
            .Document(name);
    }

    private async Task<QuerySnapshot> GetUsersSnapshot()
    {
        //WARNING! Notice:
        //This method should only be called after verifying a secure
        //connection to the database, as it does not check for network access.
        //In order to use this method, be sure to first call NetworkCheck(),
        //as it is unnecessary to call in here again.

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
        await NetworkService.NetworkCheck();

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
    public async Task<WalkModel?> GetWalkAsync(int walkId)
    {
        await NetworkService.NetworkCheck();

        DocumentSnapshot snapshot = await _firestoreDb!
            .Collection(WalksCollection)
            .Document(walkId.ToString())
            .GetSnapshotAsync();

        if (snapshot.Exists)
            return snapshot.ConvertTo<WalkModel>();
        return null;
    }

    // ➤ Get the last walk from Firestore
    public async Task<WalkModel> GetLastWalkAsync(string? username = null)
    {
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

        List<WalkModel> walks = new();
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
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

        List<FeedModel> feeds = new();
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
        //WARNING! Notice:
        //This method should only be called when verifying a secure
        //connection to the database, as it does not check for network access.
        //In order to user this method, make sure it first calls HasDog(),
        //as it contains a check for network unlike this method.

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
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

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
        await NetworkService.NetworkCheck();

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

    #region Configuration CRUD Operations
    private async Task<bool> CreateConfigAsync()
    {
        /*WARNING! Notice:
        This method should only be called when verifying a secure
        connection to the database, as it does not check for network access.
        In order to user this method, make sure it first calls HasConfig(),
        as it contains a check for network unlike this method.*/

        int.TryParse(AppInfo.BuildString, out int build);

        if (build == 0) // Int parsing failed
            return false;

        await _firestoreDb!
            .Collection(ConfigsCollection)
            .Document(AppInfo.BuildString)
            .SetAsync(new ConfigurationModel
            {
                LatestBuild = build
            });

        return true;
    }

    public async Task<bool> UpdateConfigAsync()
    {
        await NetworkService.NetworkCheck();

        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(ConfigsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        if (snapshot.Documents.Count == 0)
            return await CreateConfigAsync();

        int.TryParse(AppInfo.BuildString, out int build);
        if (build == 0) // Int parsing failed
            return false;

        DocumentReference reference = snapshot.Documents[0].Reference;
        await reference.UpdateAsync("LatestBuild", build);

        return true;
    }

    // ➤ Get the configuration values from Firestore
    public async Task<ConfigurationModel> GetConfigAsync()
    {
        await NetworkService.NetworkCheck();

        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(ConfigsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        return snapshot.Documents.Count > 0 ?
            snapshot.Documents[0].ConvertTo<ConfigurationModel>() :
            throw new Exception("There is no config file saved in the database!");
    }

    public async Task<bool> HasConfig()
    {
        await NetworkService.NetworkCheck();

        QuerySnapshot snapshot = await _firestoreDb!
            .Collection(ConfigsCollection)
            .Limit(1)
            .GetSnapshotAsync();

        return snapshot.Documents.Count > 0;
    }
    #endregion Configuration CRUD Operations
}
