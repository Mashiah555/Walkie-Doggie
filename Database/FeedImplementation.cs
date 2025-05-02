using Google.Api.Gax.ResourceNames;
using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;

namespace Walkie_Doggie.Database;

public class FeedImplementation : GenericCRUD<FeedModel, int>, Interfaces.IFeed
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

    public async Task AddAsync(string feederName, DateTime feedTime, int feedAmount, string? notes)
    {
        await base.AddAsync(new FeedModel
            {
                FeederName = feederName,
                FeedTime = Converters.ConvertToTimestamp(feedTime),
                FeedAmount = feedAmount,
                Notes = notes
            });
    }
}
