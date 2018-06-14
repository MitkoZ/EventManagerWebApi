using DataAccess.Models;
using Repositories.Helpers;
using System.Linq;

namespace Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public User GetUserByNameAndPassword(string username, string password)
        {
            User user = base.DBSet.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                PasswordManager passManager = new PasswordManager();
                bool isValidPassoword = passManager.IsPasswordMatch(password, user.PasswordHash, user.PasswordSalt);
                if (isValidPassoword == false)
                {
                    user = null;
                }
            }
            return user;
        }

        public int RegisterUser(User user, string password)
        {
            string salt, hash;
            PasswordManager passManager = new PasswordManager();
            hash = passManager.GeneratePasswordHash(password, out salt);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;

            return base.Create(user);
        }

    }
}
