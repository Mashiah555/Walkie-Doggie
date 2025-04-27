namespace Walkie_Doggie.Interfaces;

public interface IWalk : ICRUD<WalkModel, int>
{
    Task<WalkModel> GetLastWalkAsync(string? username = null);
}
