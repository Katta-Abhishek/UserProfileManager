using UserProfileManager.Business.Interfaces;
using UserProfileManager.Data.Interfaces;
using UserProfileManager.Models;

namespace UserProfileManager.Business.Implementations;

public class UserBusiness : IUserBusiness
{
  private readonly IUserRepository _userRepository;

  public UserBusiness(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<User> LoginAsync(string uniqueName, string password)
  {
    return await _userRepository.LoginAsync(uniqueName, password);
  }

  public async Task<IEnumerable<User>> GetAllAsync()
  {
    return await _userRepository.GetAllAsync();
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await _userRepository.GetByIdAsync(id);
  }

  public async Task AddAsync(User user)
  {
    await _userRepository.AddAsync(user);
  }

  public async Task UpdateAsync(User user)
  {
    await _userRepository.UpdateAsync(user);
  }

  public async Task DeleteAsync(int id)
  {
    await _userRepository.DeleteAsync(id);
  }

  public async Task<User> CheckUniqueName(string uniqueName)
  {
    return await _userRepository.GetByUniqueNameAsync(uniqueName);
  }
}
