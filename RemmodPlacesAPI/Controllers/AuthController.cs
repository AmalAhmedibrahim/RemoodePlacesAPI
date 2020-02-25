using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RemmodPlacesAPI.DTOs;
using RemmodPlacesAPI.Interfaces;
using RemmodPlacesAPI.Models;

namespace RemmodPlacesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repositry;
        private readonly IConfiguration config;
        public AuthController(IAuthRepository repositry , IConfiguration config)
        {
            this.config = config;
            this.repositry = repositry;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDTO userDTO)
        {
            userDTO.UserName = userDTO.UserName.ToLower();
            if(await this.repositry.UserExists(userDTO.UserName))
            {
                return BadRequest("UserName Is Already Exists");
            }
            else
            {
               var userToCreate = new User { UserName = userDTO.UserName };
               var createdUser = await this.repositry.Register(userToCreate, userDTO.PassWord);

                return StatusCode(200);
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var userFromRepo = await this.repositry.Login(userForLoginDTO.UserName.ToLower(), userForLoginDTO.PassWord);

            if (userFromRepo == null) // user dosn`t exisits in database
            {
                return Unauthorized();
            }
            else
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.UserName),

                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));

                // SecurityAlgorithms.HmacSha512Signature => security algo ussing to hashing 
                var Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = Credentials

                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            
        }
        public IEnumerable<string> Get()
        {
            return new string[] { "Welcome To My Project =D" };
        }





    }
}
