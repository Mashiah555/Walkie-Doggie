using Google.Cloud.Firestore;
using System.Text;
using System.Text.Json;
using Walkie_Doggie.Services;

namespace Walkie_Doggie.Database;

public class DbInsights
{
    #region Properties
    private const int BytesLimit = 1000000000; // 1 GB free cloud storage space limit
    private const int MetadataBytes = 32; // Estimated 32 bytes of metadata per document
    public static int DbBytes { get; private set; } = GetEstimatedBytesAsync().Result;
    #endregion Properties

    public async Task<bool> FreeStorageSpaceAsync(bool forceCleanup = false, int maxPercentage = 90)
    {
        /* NOTICE:
         * This method is used to free up storage space of the database,
         * in case it is full or almost full, or the user forced cleanup.
         * BE AWARE that documents will be deleted from the database permanentely!!
         * Documents will be deleted according to the algorithm's rules,
         * as it is the least harmful way to free up space.
         * Notice the algorithm will not spare any targeted document!
         */

        if (maxPercentage <= 0 || maxPercentage >= 95)
            maxPercentage = 90; // Set a default value if out of range

        // returns indication of whether the method even tried to clean up space
        if (!forceCleanup && !(await IsDbFullAsync(maxPercentage)))
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

        int excessiveBytes = (await GetEstimatedBytesAsync()) -
            (int)((double)BytesLimit * (double)maxPercentage / 100.0);
        List<WalkModel> walks = (await DbService.Walks.GetAllAsync())
            .OrderBy(walk => walk.WalkTime).ToList();
        List<FeedModel> feeds = (await DbService.Feeds.GetAllAsync())
            .OrderBy(feed => feed.FeedTime).ToList();

        while (await IsDbFullAsync(maxPercentage))
        {
            if (walks.Count > 0)
            {
                WalkModel walk = GetFirst(walks);
                walks.Remove(walk);
                await DbService.Walks.DeleteAsync(walk.WalkId);
            }
            else if (feeds.Count > 0)
            {
                FeedModel feed = GetFirst(feeds);
                feeds.Remove(feed);
                await DbService.Feeds.DeleteAsync(feed.FeedId);
            }
            else
            {
                break; // No more documents to delete
            }
        }

        return true;
    }

    public bool IsDbFullAsync(int percentageLimit = 95)
    {
        return (double)DbBytes >= (double)BytesLimit * (double)percentageLimit / 100.0;
    }

    private async Task<int> GetEstimatedBytesAsync()
    {
        return
            GetByteCount(await DbService.Dogs.GetAllAsync()) +
            GetByteCount(await DbService.Walks.GetAllAsync()) +
            GetByteCount(await DbService.Users.GetAllAsync()) +
            GetByteCount(await DbService.Feeds.GetAllAsync());
    }

    private int GetByteCount<T>(IEnumerable<T> items)
    {
        return items.Sum(item => Encoding.UTF8
            .GetByteCount(JsonSerializer.Serialize(item)) + MetadataBytes);
    }
}
