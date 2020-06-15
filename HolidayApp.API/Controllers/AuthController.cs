using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using HolidayApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using HolidayApp.API.Dtos;
using HolidayApp.API.Data;
using Microsoft.AspNetCore.Identity;

namespace HolidayApp.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private IConfiguration Configuration { get; set; }
        private readonly IMapper _mapper;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public AuthController(IConfiguration config, IMapper mapper, UserManager<User> userManager,
        SignInManager<User> signInManager)
        {
            _mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            Configuration = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var userToCreate = _mapper.Map<User>(userForRegisterDto);
            var result = await userManager.CreateAsync(userToCreate,userForRegisterDto.Password);
            //var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);
            if(result.Succeeded){
                return Ok(result);
                //return CreatedAtRoute("GetUser", new {controller = "Users", id = userToCreate.Id}, userToReturn);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var user = await userManager.FindByNameAsync(userForLoginDto.Username);
            if (user != null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);
                if (result.Succeeded)
                {
                    var appUser = _mapper.Map<UserForListDto>(user);
                    return Ok(new
                    {
                        token = GenerateJwtToken(user),
                        user = appUser
                    });
                }
                return Unauthorized();
            }
            else
            {
                return BadRequest(new { message = "Username is incorrect." });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Jwt:Secret"]));


            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

    }
}