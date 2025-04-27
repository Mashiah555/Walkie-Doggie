namespace Walkie_Doggie.Interfaces;

public interface ICRUD<TModel, TVariable> where TModel : class
{
    Task AddAsync(TModel item);
    Task<TModel?> GetAsync(TVariable id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<bool> UpdateAsync(TModel item);
    Task<bool> DeleteAsync(TVariable id);
    Task DeleteAll();
}
