namespace Walkie_Doggie.Database;

class DogImplementation : Interfaces.IDog
{
    public Task<bool> AddAsync(DogModel item)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAll()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DogModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DogModel?> GetAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetWalksIdAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> HasDogAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(DogModel item)
    {
        throw new NotImplementedException();
    }
}
