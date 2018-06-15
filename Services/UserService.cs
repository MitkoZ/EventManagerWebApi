using DataAccess.Models;
using Repositories;
using Services.Interfaces;

namespace Services
{
    public class UserService : BaseService<User, UserRepository>
    {
        public UserService(IValidationDictionary validationDictionary, UserRepository repository) : base(validationDictionary, repository)
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
