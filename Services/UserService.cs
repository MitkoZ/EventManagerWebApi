using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Repositories;
using Services.Interfaces;
using System.Collections.Generic;

namespace Services
{
    public class UserService : BaseService<User, UserRepository>
    {
        public UserService(UserRepository repository) : base(repository)
        {
        }

        public int RegisterUser(User userDb, string password)
        {
            return repository.RegisterUser(userDb, password);
        }

        public User GetUserByNameAndPassword(string username, string password)
        {
            return repository.GetUserByNameAndPassword(username, password);
        }
    }
}
