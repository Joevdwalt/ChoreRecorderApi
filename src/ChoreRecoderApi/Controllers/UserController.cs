using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChoreRecorderApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using ChoreRecorderApi.Model;
using AutoMapper;
using ChoreRecorderApi.Helpers;
using System.Diagnostics;
using ChoreRecoderApi.Model;

namespace ChoreRecorderApi.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private IOptions<AppSettings> _appSettings;
        public UserController(IUserService userService,
                              IMapper mapper,
                              IOptions<AppSettings> appsettings
        )
        {
            this._userService = userService;
            this._mapper = mapper;
            this._appSettings = appsettings;
        }

        [HttpGet("getall")]
        public string GetUsers()
        {
            return "asdfdsd";

        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(registerUserDto);

            try
            {
                if (string.Compare(registerUserDto.Password, registerUserDto.ConfirmPassword) == 1)
                {
                    throw new AppException("Password and confirmed password should be the same");
                }

                if (string.IsNullOrEmpty(registerUserDto.FirstName))
                {
                    throw new ApplicationException("Firstname cannot be empty");
                }

                if (string.IsNullOrEmpty(registerUserDto.LastName))
                {
                    throw new ApplicationException("Lastname cannot be empty");
                }

                if (string.IsNullOrEmpty(registerUserDto.Username))
                {
                    throw new ApplicationException("Username cannot be empty");
                }

                if (!this.IsValidEmail(registerUserDto.Username))
                {
                    throw new ApplicationException("Username must be a valid email address");
                }

                // save 
                await _userService.Create(user, registerUserDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }


        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}