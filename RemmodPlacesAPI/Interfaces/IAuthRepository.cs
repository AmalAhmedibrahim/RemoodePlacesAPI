using RemmodPlacesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemmodPlacesAPI.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string PassWord);
        Task<User> Login(string userName, string PassWord);

        Task<Boolean> UserExists(string userName);

    }
}
