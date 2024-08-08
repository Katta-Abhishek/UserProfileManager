using UserProfileManager.Models;

namespace UserProfileManager.Business.Interfaces;

public interface IUserBusiness
{
  Task<User> CheckUniqueName(string uniqueName);
  Task<User> LoginAsync(string uniqueName, string password);
  Task<IEnumerable<User>> GetAllAsync();
  Task<User?> GetByIdAsync(int id);
  Task AddAsync(User user);
  Task UpdateAsync(User user);
  Task DeleteAsync(int id);
}
