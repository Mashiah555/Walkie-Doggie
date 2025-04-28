namespace Walkie_Doggie.Interfaces;

public interface IFeed : ICRUD<FeedModel, int>
{
    Task<FeedModel> GetLastFeedAsync(string? username = null);
}
