using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly String _tokenSecret;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

        public UserController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _userRepository = new UserRepository(context);
            _configuration = config;
            _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
        }

        // GET: api/User/GetUser/1
        [HttpGet("GetUser/{pUserId}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute] long pUserId)
        {
            User? user = await _context.User.FindAsync(pUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            
            return Ok(new ReturnUserDtoDebug(user));
        }
        
        // GET: api/User/GetUsers
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            List<User> userList = _userRepository.GetAllUsers();
            List<ReturnUserDtoDebug> userDTOs = userList.Select(user => new ReturnUserDtoDebug(user)).ToList();
            return Ok(userDTOs);
        }
        
        // POST: api/User/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO userDTO)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            
            if (await _context.User.AnyAsync(u => u.userName == userDTO.userName))
            {
                ModelState.AddModelError("UserName", "Username is already taken.");
                return BadRequest(ModelState);
            }
            
            User user = userDTO.toEntity();
            user.userId = await _context.User.AnyAsync() ? (await _context.User.MaxAsync(u => u.userId)) + 1 : 0;

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Register), new { Token = GenerateToken(userDTO) });
        }
        
        // POST: api/User/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RegisterUserDTO userDto)
        {
            User user = await _context.User.SingleOrDefaultAsync(u => u.userName == userDto.userName);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.userPassword != userDto.userPassword)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            return Ok(new { Token = GenerateToken(userDto) });
        }
        
        // POST: api/User/RefreshToken
        [HttpGet("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var user = HttpContext.User;
            
            var usernameClaim = user.Claims.FirstOrDefault(c => c.Type == "userName");
            var passwordClaim = user.Claims.FirstOrDefault(c => c.Type == "userPassword");

            if (usernameClaim == null || passwordClaim == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new { Token = GenerateToken(new RegisterUserDTO(usernameClaim.Value,  passwordClaim.Value)) });
        }

        
        private String GenerateToken([FromBody] RegisterUserDTO registerUserDto)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

            List<Claim> claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, registerUserDto.userName),
                new("userName", registerUserDto.userName),
                new("userPassword", registerUserDto.userPassword)
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifetime),
                Issuer = _configuration["JWT_SETTINGS_ISSUER"],
                Audience = _configuration["JWT_SETTINGS_AUDIENCE"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            SecurityToken token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            String jwt = jwtSecurityTokenHandler.WriteToken(token);
            
            return jwt;
        }
    }
}
