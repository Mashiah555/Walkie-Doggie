namespace Walkie_Doggie.Interfaces;

public interface IFeed : ICRUD<FeedModel, int>
{
    Task<FeedModel> GetLastFeedAsync(string? username = null);
    Task AddAsync(string feederName, DateTime feedTime, int feedAmount, string? notes);
}
