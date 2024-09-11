using AspNetCoreWeb.Models;

namespace AspNetCoreWeb.Service;

public interface IUserService
{
    (bool isValid, User user) IsValid(string name, string password);
}