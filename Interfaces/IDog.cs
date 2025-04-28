namespace Walkie_Doggie.Interfaces;

public interface IDog : ICRUD<DogModel, string>
{
    Task<bool> HasDogAsync();
    Task<int> GetTotalWalksAsync(bool increment = false);
}
