using Google.Cloud.Firestore;

namespace Walkie_Doggie.Database;

public class FeedImplementation : AbstractCRUD<FeedModel, int>, Interfaces.IFeed
{
    public FeedImplementation(FirestoreDb dbContext)
        : base(dbContext, "Feeds") { }

    public async Task<FeedModel> GetLastFeedAsync(string? username = null)
    {
        IEnumerable<FeedModel> feeds = await base.GetAllAsync();
        if (!string.IsNullOrEmpty(username))
            feeds = feeds.Where(feed => feed.FeederName == username);

        FeedModel? latestFeed= feeds
            .OrderByDescending(feed => feed.FeedTime).FirstOrDefault();

        return latestFeed ??
            throw new Exception("There are no saved feeds yet!");
    }
}
