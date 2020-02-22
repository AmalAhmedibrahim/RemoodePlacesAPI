using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RemmodPlacesAPI.Models;
using RemodePlacesAPI.Data;

namespace RemmodPlacesAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }
        public async Task<User> Login(string userName, string PassWord)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                return null;
            }
            else if(!VerifyPassWordHash(PassWord,user.PasswordHash,user.PasswordSalt))
            {
                return null;
            }
            else
            {
                return user;
            }


        }

      

        public async Task<User> Register(User user, string passWord)
        {
            byte[] passWordHash, passWordSalt;

            CreatePassWordHash(passWord, out passWordHash, out passWordSalt);

            user.PasswordHash = passWordHash;
            user.PasswordSalt = passWordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
           if(await this.context.Users.AnyAsync(u=>u.UserName==userName))
            {
                return true;
            }
            else
            {
                return false;

            }
        }


        // *******************   private Methods  *********************** //
       private void CreatePassWordHash(string passWord , out byte[] passWordHash, out byte[] passWordSalt)
       {
            // use using to call dispose function after finsih 
            using (var hmac= new System.Security.Cryptography.HMACSHA512())
            {
                passWordSalt = hmac.Key; // radomly generated key 
                passWordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord));
            }
       }
        private bool VerifyPassWordHash(string passWord, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               
               var ComputedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord));

                for (int i = 0; i < ComputedHash.Length; i++)
                {
                    if(ComputedHash[i]!=passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
