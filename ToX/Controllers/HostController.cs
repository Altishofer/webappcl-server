﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.Models;
using ToX.Repositories;
using ToX.Services;
using Host = ToX.Models.Host;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly HostRepository _hostRepository;
        private readonly HostService _hostService;
        private readonly IConfiguration _configuration;
        private readonly String _tokenSecret;
        private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

        public HostController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _hostRepository = new HostRepository(context);
            _configuration = config;
            _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
            _hostService = new HostService(_context);
        }
        
        // GET: api/Host/GetHosts
        [HttpGet("GetHosts")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHosts()
        {
            List<Host> hostList = await _hostService.GetAllHosts();
            List<ReturnHostDtoDebug> hostDTOs = hostList.Select(host => new ReturnHostDtoDebug(host)).ToList();
            return Ok(hostDTOs);
        }
        
        // POST: api/Host/Register
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterHostDTO hostDTO)
        {
            if (!ModelState.IsValid){return BadRequest(ModelState);}
            
            if (await _hostService.HostExistsByHostName(hostDTO.hostName))
            {
                ModelState.AddModelError("HostName", "Hostname is already taken.");
                return BadRequest(ModelState);
            }

            await _hostService.CreateHost(hostDTO);
            return CreatedAtAction(nameof(Register), new { Token = GenerateToken(hostDTO) });
        }
        
        // POST: api/Host/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RegisterHostDTO hostDto)
        {
            Host? host = await _hostService.GetHostOrNull(hostDto);
            if (host == null)
            {
                return NotFound("Host not found.");
            }

            if (host.hostPassword != hostDto.hostPassword)
            {
                return Unauthorized("Invalid credentials.");
            }
            
            return Ok(new { Token = GenerateToken(hostDto) });
        }
        
        // POST: api/Host/RefreshToken
        [HttpGet("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var host = HttpContext.User;
            
            var hostnameClaim = host.Claims.FirstOrDefault(c => c.Type == "hostName");
            var passwordClaim = host.Claims.FirstOrDefault(c => c.Type == "hostPassword");

            if (hostnameClaim == null || passwordClaim == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new { Token = GenerateToken(new RegisterHostDTO(hostnameClaim.Value,  passwordClaim.Value)) });
        }

        
        private String GenerateToken([FromBody] RegisterHostDTO registerHostDto)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_tokenSecret);

            List<Claim> claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, registerHostDto.hostName),
                new("hostName", registerHostDto.hostName),
                new("hostPassword", registerHostDto.hostPassword)
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
