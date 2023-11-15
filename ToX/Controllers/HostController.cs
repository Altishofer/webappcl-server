using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToX.DTOs;
using ToX.Models;
using ToX.Services;
using Host = ToX.Models.Host;

namespace ToX.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly HostService _hostService;
        private readonly IConfiguration _configuration;

        public HostController(ApplicationContext context, IConfiguration config)
        {
            _context = context;
            _configuration = config;
            _hostService = new HostService(_context, _configuration);
        }
        
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterHostDTO hostDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The format of the credentials are not valid");
            }
            if (await _hostService.HostExistsByHostName(hostDTO.hostName))
            {
                return BadRequest("Hostname is already taken, please choose another one");
            }

            Host host = await _hostService.CreateHost(hostDTO);
            return CreatedAtAction(nameof(Register), new {token = _hostService.GenerateToken(hostDTO), id = host.hostId }); 
        }
        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RegisterHostDTO hostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The format of the credentials were not valid");
            }

            Host? authHost = await _hostService.GetHostOrNull(hostDto);
            if (authHost == null || authHost.hostPassword != hostDto.hostPassword)
            {
                return Unauthorized("User with given credentials was not found");
            }

            return Ok(new {token = _hostService.GenerateToken(hostDto), id = authHost.hostId});
        }
        
        [HttpGet("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("The token could not be validated");
            }

            return Ok(_hostService.GenerateToken(claimHost));
        }
    }
}
