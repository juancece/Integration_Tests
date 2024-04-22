using Core.Entities;

namespace Core.Interfaces;

public interface IUserRepository
{
    Task<List<UserEntity>> GetUsersAsync();
}