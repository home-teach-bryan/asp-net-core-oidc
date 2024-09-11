using AspNetCoreJwt.Models;

namespace AspNetCoreJwt.Service;

public interface IUserService
{
    (bool isValid, User user) IsValid(string name, string password);
}