using MinAPIMusicProject.DTOs;
using MinAPIMusicProject.Models;
using MinAPIMusicProject.Requests;

namespace MinAPIMusicProject.Interfaces;

public interface IUserService
{
    Task<UserDTO> RegisterUser(CreateUserRequest request, CancellationToken cancellationToken = default);
    UserDTO LoginUser(CreateUserRequest request, CancellationToken cancellationToken = default);
    
    Task<List<UserDTO>> GetUsers(int page, int size, CancellationToken cancellationToken = default);
    Task<UserDTO> GetUserById(int id, CancellationToken cancellationToken = default);
    
    Task<UserDTO> UpdateUser(UserDTO userDto, CancellationToken cancellationToken = default);
    

    /// <summary>
    /// delete artist from database
    /// </summary>
    /// <param name="id">ID property of artist</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentNullException">throws when artist with id is not found</exception>
    Task DeleteUser(int id, CancellationToken cancellationToken = default);
    
    Task<User> GetUserWithLikedTracks(int userId, CancellationToken cancellationToken);

}
    