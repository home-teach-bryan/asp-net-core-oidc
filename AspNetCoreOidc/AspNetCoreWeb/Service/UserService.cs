﻿using AspNetCoreWeb.Models;

namespace AspNetCoreWeb.Service;

public class UserService : IUserService
{
    private List<User> _users = new List<User>
    {
        new User { Id = Guid.NewGuid(), Name = "SuperAdmin", Password = "SuperAdmin", Roles = new[] { "Admin", "User" } },
        new User { Id = Guid.NewGuid(), Name = "Admin", Password = "Admin", Roles = new[] { "Admin" } },
        new User { Id = Guid.NewGuid(), Name = "User", Password = "User", Roles = new[] { "User" } },
        new User { Id = Guid.NewGuid(), Name = "Employee", Password = "Employee", Roles = new[] { "Employee" }},  
        new User { Id = Guid.NewGuid(), Name = "Employee2", Password = "Employee2", Roles = new[] { "Employee" }}  
    };

    public (bool isValid, User user) IsValid(string name, string password)
    {
        var user = _users.FirstOrDefault(item => item.Name == name && item.Password == password);
        if (user == null)
        {
            return (false, new User());
        }
        return (true, user);
    } 

}