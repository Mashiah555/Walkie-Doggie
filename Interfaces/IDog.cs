namespace Walkie_Doggie.Interfaces;

public interface IDog : ICRUD<DogModel, string>
{
    Task<bool> HasDogAsync();
    Task<IEnumerable<string>> GetWalksIdAsync();
}
