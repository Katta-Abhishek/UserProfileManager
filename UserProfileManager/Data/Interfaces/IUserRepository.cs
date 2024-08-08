using UserProfileManager.Models;

namespace UserProfileManager.Data.Interfaces;

public interface IUserRepository
{
  Task<User> LoginAsync(string username, string password);
  Task<User> GetByIdAsync(int id);
  Task<User> GetByUniqueNameAsync(string uniqueName);
  Task<IEnumerable<User>> GetAllAsync();
  Task AddAsync(User user);
  Task UpdateAsync(User user);
  Task DeleteAsync(int id);
}
