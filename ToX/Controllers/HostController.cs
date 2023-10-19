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
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "The credentials are not valid." });
            }
            if (await _hostService.HostExistsByHostName(hostDTO.hostName))
            {
                ModelState.AddModelError("HostName", "Hostname is already taken.");
                return BadRequest(ModelState);
            }

            await _hostService.CreateHost(hostDTO);
            return CreatedAtAction(nameof(Register), new { Token = _hostService.GenerateToken(hostDTO) });
        }
        
        // POST: api/Host/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] RegisterHostDTO hostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The format of the credentials were not valid");
            }

            Host? authHost = await _hostService.GetHostOrNull(hostDto);
            if (authHost == null)
            {
                return Unauthorized("User was not found with given credentials");
            }
            
            return Ok(new { Token = _hostService.GenerateToken(hostDto) });
        }
        
        // POST: api/Host/RefreshToken
        [HttpGet("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            Host? claimHost = await _hostService.VerifyHost(HttpContext.User);
            if (claimHost == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            return Ok(new { Token = _hostService.GenerateToken(claimHost) });
        }
    }
}
