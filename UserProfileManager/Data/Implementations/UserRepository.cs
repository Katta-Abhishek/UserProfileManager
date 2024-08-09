using System.Text;
using Microsoft.EntityFrameworkCore;
using UserProfileManager.Data.Interfaces;
using UserProfileManager.Models;

namespace UserProfileManager.Data.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(ApplicationDbContext context, ILogger<UserRepository> logger)
    {
      _context = context;
      _logger = logger;
    }

    public async Task<User> LoginAsync(string uniqueName, string password)
    {
      try
      {
        string encodedPassword = EncodeToBase64(password);

        IEnumerable<User> users = await _context.Users
            .Where(u => u.UniqueName == uniqueName && u.Password == encodedPassword)
            .ToListAsync();

        if (users.Any()) return users.First();
        throw new UnauthorizedAccessException("Invalid Credentials");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error during login attempt.");
        throw;
      }
    }

    public async Task<User> GetByIdAsync(int id)
    {
      try
      {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id) ?? throw new KeyNotFoundException($"Error retrieving user by Id: {id}");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error retrieving user by Id: {id}");
        throw;
      }
    }

    public async Task<User> GetByUniqueNameAsync(string uniqueName)
    {
      try
      {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.UniqueName == uniqueName) ?? throw new KeyNotFoundException($"Error retrieving user by uniqueName: {uniqueName}");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error retrieving user by uniqueName: {uniqueName}");
        throw;
      }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      try
      {
        return await _context.Users
            .AsNoTracking()
            .ToListAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error retrieving all users.");
        throw;
      }
    }

    public async Task AddAsync(User user)
    {
      try
      {
        // Check if email already exists
        bool emailExists = await _context.Users
            .AnyAsync(u => u.Email == user.Email);

        if (emailExists)
        {
          throw new InvalidOperationException("A user with this email already exists.");
        }

        // Encode the password to base64 before storing
        user.Password = EncodeToBase64(user.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error adding a new user.");
        throw;
      }
    }

    public async Task UpdateAsync(User user)
    {
      try
      {
        // Check if email exists for other users
        var existingUser = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == user.Email && u.Id != user.Id);

        if (existingUser != null)
        {
          throw new InvalidOperationException("A user with this email already exists.");
        }

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error updating the user.");
        throw;
      }
    }

    public async Task DeleteAsync(int id)
    {
      try
      {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
          _context.Users.Remove(user);
          await _context.SaveChangesAsync();
        }
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Error deleting user with Id: {id}");
        throw;
      }
    }

    private string EncodeToBase64(string text)
    {
      if (string.IsNullOrEmpty(text)) return string.Empty;
      var plainTextBytes = Encoding.UTF8.GetBytes(text);
      return Convert.ToBase64String(plainTextBytes);
    }
  }
}
