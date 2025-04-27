namespace Walkie_Doggie.Database;

class FeedImplementation : Interfaces.IFeed
{
    public Task<bool> AddAsync(FeedModel item)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FeedModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FeedModel?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<FeedModel> GetLastFeedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(FeedModel item)
    {
        throw new NotImplementedException();
    }
}
