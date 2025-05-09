namespace Walkie_Doggie.Interfaces;

public interface IDog : ICRUD<DogModel, string>
{
    Task<bool> HasDogAsync();
    Task<int> GetTotalWalksAsync(bool increment = false);
    Task<DogModel?> GetAsync();
    Task AddAsync(string name, DateTime birthdate, string breed,
        double weight, int feedAmount);
    Task<bool> UpdateAsync(DateTime birthdate, string breed,
        double weight, int feedAmount);
}
