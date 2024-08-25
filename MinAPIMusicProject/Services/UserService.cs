using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinAPIMusicProject.Data;
using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Interfaces;
using MinAPIMusicProject.Models;
using MinAPIMusicProject.Requests;

namespace MinAPIMusicProject.Services;

public class UserService : IUserService
{
    private readonly MusicContext _context;
    private readonly IMapper _mapper;

    public UserService(MusicContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UserDTO> RegisterUser(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // валідація вводу
        if (request.Login == null || string.IsNullOrEmpty(request.Login.Trim()) || request.Login.Length < 4 || 
                                                          request.Password == null || string.IsNullOrEmpty(request.Password.Trim()) || request.Password.Length < 4)
            throw new ArgumentException();
        
        // перевірка існування користувача з таким самим ім'ям
        if (_context.Users.Any(u => u.Login == request.Login))
            throw new InvalidOperationException("User with the same login already exists.");
    
        var hashedPassword = HashPassword(request.Password);
    
        // створення нового користувача
        var newUser = new User
        {
            Login = request.Login,
            Password = hashedPassword,
        };
    
        var userEntry  = _context.Users.Add(newUser);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDTO>(userEntry.Entity ); 
    }
    
    // метод хешування пароля
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public UserDTO LoginUser(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        // валідація вводу
        if (request.Login == null || string.IsNullOrEmpty(request.Login.Trim()) || request.Login.Length < 4 || 
            request.Password == null || string.IsNullOrEmpty(request.Password.Trim()) || request.Password.Length < 4)
            throw new ArgumentException();
        
        // перевірка користувача на наявність в базі
        var user = _context.Users.FirstOrDefault(u => u.Login == request.Login);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid nickname.");
        
        // перевірка пароля
        if (!VerifyPassword(request.Password, user.Password))
            throw new UnauthorizedAccessException("Invalid password.");
        
        return _mapper.Map<UserDTO>(user);
    }
    
    // метод перевірки пароля
    private bool VerifyPassword(string inputPassword, string storedHashedPassword)
    {
        var hashedInputPassword = HashPassword(inputPassword);
        return hashedInputPassword == storedHashedPassword;
    }

    public async Task<List<UserDTO>> GetUsers(int page, int size, CancellationToken cancellationToken = default)
    {
        return await _context.Users.Skip(page * size)
            .Take(size)
            .Select(x => _mapper.Map<UserDTO>(x))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDTO> GetUserById(int id, CancellationToken cancellationToken = default)
    {
        var userFromDb = await _context.Users.FindAsync(new object[]{id}, cancellationToken: cancellationToken);
        return _mapper.Map<UserDTO>(userFromDb);
    }

    public async Task<UserDTO> UpdateUser(UserDTO userDto, CancellationToken cancellationToken = default)
    {
        userDto.Password = HashPassword(userDto.Password);
        
        var entity = _context.Update(_mapper.Map<User>(userDto));
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<UserDTO>(entity.Entity);
    }

    public async Task DeleteUser(int id, CancellationToken cancellationToken = default)
    {
        var userfromDb = await _context.Users.FindAsync(new object[]{id}, cancellationToken: cancellationToken);

        if (userfromDb == null)
        {
            throw new ArgumentNullException(nameof(userfromDb) + " is null");
        }
        
        _context.Remove(userfromDb);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    
    public async Task<User> GetUserWithLikedTracks(int userId, CancellationToken cancellationToken)
    {
        var userFromDb = await _context.Users.Include(u => u.LikedTracks)
            .ThenInclude(lt => lt.Track)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        return userFromDb;
    }
    
}