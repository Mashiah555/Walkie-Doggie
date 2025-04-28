using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;

namespace Walkie_Doggie.Database;

public class DogImplementation : AbstractCRUD<DogModel, string>, Interfaces.IDog
{
    public DogImplementation(FirestoreDb dbContext)
        : base(dbContext, "Dogs") { }

    public async Task<bool> HasDogAsync()
    {
        IEnumerable<DogModel> dogs = await base.GetAllAsync();

        return dogs.FirstOrDefault() != null;
    }

    public async Task<int> GetTotalWalksAsync(bool increment = false)
    {
        IEnumerable<DogModel> dogs = await base.GetAllAsync();
        DogModel dog = dogs.FirstOrDefault() ?? 
            throw new DirectoryNotFoundException("There are no saved dogs yet!");

        if (increment)
        {
            dog.TotalWalks++;
            await base.UpdateAsync(dog);
        }
        return dog.TotalWalks; // returns the newly incremented value
    }

    public override async Task AddAsync(DogModel item)
    {
        if (await HasDogAsync())
            await base.DeleteAllAsync();

        try
        {
            WalkImplementation walkImplementation = new WalkImplementation(_db);
            WalkModel lastWalk = await walkImplementation.GetLastWalkAsync();
            item.TotalWalks = lastWalk.WalkId;
        }
        catch { item.TotalWalks = 0; }

        await base.AddAsync(item);
    }
}
