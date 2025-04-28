using Walkie_Doggie.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Reflection;

namespace Walkie_Doggie.Database;

public static class DbService
{
    private static FirestoreDb _db = Authenticate();
    public static IUser Users { get; }
    public static IWalk Walks { get; }
    public static IFeed Feeds { get; }
    public static IDog Dogs { get; }

    static DbService()
    {
        Dogs = new DogImplementation(_db);
        Users = new UserImplementation(_db);
        Walks = new WalkImplementation(_db);
        Feeds = new FeedImplementation(_db);
    }

    private static FirestoreDb Authenticate()
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
}
