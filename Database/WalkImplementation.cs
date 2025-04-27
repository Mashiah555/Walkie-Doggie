namespace Walkie_Doggie.Database;

class WalkImplementation : Interfaces.IWalk
{
    public Task<bool> AddAsync(WalkModel item)
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

    public Task<IEnumerable<WalkModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<WalkModel?> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<WalkModel> GetLastWalkAsync(string? username = null)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(WalkModel item)
    {
        throw new NotImplementedException();
    }
}
