using Google.Cloud.Firestore;
using Walkie_Doggie.Helpers;
using Walkie_Doggie.Services;

namespace Walkie_Doggie.Database;

public class DogImplementation : GenericCRUD<DogModel, string>, Interfaces.IDog
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
            await DeleteAllAsync();

        try
        {
            WalkModel lastWalk = await DbService.Walks.GetLastWalkAsync();
            item.TotalWalks = lastWalk.WalkId;
        }
        catch { item.TotalWalks = 0; }

        await base.AddAsync(item);
    }

    public async Task AddAsync(string name, DateTime birthdate, string breed, double weight, int feedAmount)
    {
        await AddAsync(new DogModel
        {
            DogName = name,
            DogBirthdate = Converters.ConvertToTimestamp(birthdate),
            DogBreed = breed,
            DogWeight = weight,
            DefaultFeedAmount = feedAmount,
            TotalWalks = 0
        });
    }
}
