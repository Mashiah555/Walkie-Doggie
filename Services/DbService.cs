using Walkie_Doggie.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System.Reflection;
using System.Text;
using Walkie_Doggie.Database;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Text.Json;

namespace Walkie_Doggie.Services;

public static class DbService
{
    private static FirestoreDb _db = Authenticate();
    public static IUser Users { get; }
    public static IWalk Walks { get; }
    public static IFeed Feeds { get; }
    public static IDog Dogs { get; }

    public static class Operations
    {
        #region Properties
        private const int MaxBytes = 1000000000; // 1 GB free cloud storage space limit
        private const int MetadataBytes = 32; // Estimated 32 bytes of metadata per document
        public static int DbBytes { get; private set; } = GetEstimatedBytesAsync().Result;
        #endregion Properties

        public static async Task<bool> FreeStorageSpaceAsync(bool forceCleanup = false, int maxPercentage = 90)
        {
            /* NOTICE:
             * This method is used to free up storage space of the database,
             * in case it is full or almost full, or the user forced cleanup.
             * BE AWARE that documents will be deleted from the database permanentely!!
             * Documents will be deleted according to the algorithm's rules,
             * as it is the least harmful way to free up space.
             * Notice the algorithm will not spare any targeted document!
             */

            try
            {
                if (maxPercentage <= 0 || maxPercentage >= 95)
                    maxPercentage = 90; // Set a default value if out of range

                // returns indication of whether the method even tried to clean up space
                if (!forceCleanup && !IsDbFullAsync(maxPercentage))
                    return false;
                /* Although IsDbFullAsync() set maxPercentage to 95% by default,
                 * the default at this method is 90%.
                 * This is so since this method is called upon app startup,
                 * and those 5% are needed for the app to start properly in edge cases.
                 */

                /* The Algorithm Guidelines:
                 * The algorith will skip Dogs and Users collections, 
                 * as they are essential for the app to function properly.
                 * The algorithm will focus only on Walks and Feeds collections, 
                 * deleting their oldest documents first,
                 * until the database is below maxPercentage of it's storage limit.
                 */

                int excessiveBytes = (await
                    GetEstimatedBytesAsync()) - (int)GetBytesLimit(maxPercentage);
                int cleanedBytes = 0;

                List<WalkModel> walks = (await DbService.Walks.GetAllAsync())
                    .OrderBy(walk => walk.WalkTime).ToList();
                List<FeedModel> feeds = (await DbService.Feeds.GetAllAsync())
                    .OrderBy(feed => feed.FeedTime).ToList();

                while (cleanedBytes < excessiveBytes && (walks.Count > 0 || feeds.Count > 0))
                {
                    bool hasWalks = walks.Count > 0;
                    bool hasFeeds = feeds.Count > 0;

                    WalkModel walk = hasWalks ? walks[0] : WalkModel.Default;
                    FeedModel feed = hasFeeds ? feeds[0] : FeedModel.Default;

                    if (!hasWalks || (hasFeeds && feed.FeedTime <= walk.WalkTime))
                    {
                        await DbService.Feeds.DeleteAsync(feed);
                        feeds.RemoveAt(0);
                        cleanedBytes += GetByteCount(feed);
                    }
                    else
                    {
                        await DbService.Walks.DeleteAsync(walk);
                        walks.RemoveAt(0);
                        cleanedBytes += GetByteCount(walk);
                    }
                }

                // Update the database size after cleanup
                DbBytes -= cleanedBytes;

                // Inform the user about the cleanup
                if (forceCleanup)
                    await Toast.Make($"נוקו {cleanedBytes} ג\"ב מהאחסון של מאגר המידע",
                        ToastDuration.Long).Show();
                else
                    await Toast.Make("המערכת פינתה מקום במאגר למידע חדש").Show();
            }
            catch { return false; }
            return true;
        }

        public static bool IsDbFullAsync(int percentageLimit = 95)
        {
            return DbBytes >= GetBytesLimit(percentageLimit);
        }

        #region Private Helper Methods
        private static async Task<int> GetEstimatedBytesAsync()
        {
            return
                GetByteCount(await DbService.Dogs.GetAllAsync()) +
                GetByteCount(await DbService.Walks.GetAllAsync()) +
                GetByteCount(await DbService.Users.GetAllAsync()) +
                GetByteCount(await DbService.Feeds.GetAllAsync());
        }

        private static double GetBytesLimit(int percentageLimit)
        {
            return (double)MaxBytes * ((double)percentageLimit / 100.0);
        }
        private static int GetByteCount<T>(IEnumerable<T> items)
        {
            return items.Sum(item => Encoding.UTF8
                .GetByteCount(JsonSerializer.Serialize(item)) + MetadataBytes);
        }
        private static int GetByteCount<T>(T item)
        {
            return Encoding.UTF8.GetByteCount(JsonSerializer.Serialize(item)) + MetadataBytes;
        }
        #endregion Private Helper Methods
    }

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
