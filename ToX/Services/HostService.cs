using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ToX.DTOs;
using ToX.Repositories;

namespace ToX.Services;

using System;
using System.Threading.Tasks;
using ToX.Models;

public class HostService
{
    private readonly ApplicationContext _context;
    private readonly HostRepository _hostRepository;
    private readonly IConfiguration _configuration;
    private readonly String _tokenSecret;
    private static readonly TimeSpan tokenLifetime = TimeSpan.FromHours(24);

    public HostService(ApplicationContext context, IConfiguration configuration)
    {
        _context = context;
        _hostRepository = new HostRepository(_context);
        _configuration = configuration;
        _tokenSecret = _configuration["JWT_SETTINGS_KEY"];
    }

    public async Task<bool> HostExistsByHostName(string hostName)
    {
        return await _hostRepository.HostExistsByHostName(hostName);
    }
    
    public async Task<Host> CreateHost(RegisterHostDTO hostDto)
    {
        Host host = hostDto.toEntity();
        host.hostId = await _hostRepository.NextHostId();
        return await _hostRepository.SaveHost(host);
    }
    
    public async Task<Host?> GetHostOrNull(RegisterHostDTO hostDto)
    {
        return await _hostRepository.GetHostByHostName(hostDto.hostName);
    }
    
    public async Task<List<Host>> GetAllHosts()
    {
        return await _hostRepository.GetAllHosts();
    }

    public async Task<Host?> VerifyHost(ClaimsPrincipal context)
    {
        string? hostnameClaim;
        string? passwordClaim;
        string? idClaim;
        try
        {
            hostnameClaim = context.Claims.FirstOrDefault(c => c.Type == "hostName").Value;
            passwordClaim = context.Claims.FirstOrDefault(c => c.Type == "hostPassword").Value;
            //idClaim = context.Claims.FirstOrDefault(c => c.Type == "hostId").Value;
        } catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }

        RegisterHostDTO claimDto = new RegisterHostDTO(hostnameClaim, passwordClaim);

        Host? host = await GetHostOrNull(claimDto);

        return host ?? null ;
    }

    public async Task<string> GenerateToken(Host host)
    {
        return await GenerateToken(new RegisterHostDTO(host.hostName, host.hostPassword));
    }
    
    public void Delete(Host host)
    {
        _hostRepository.DeleteHost(host);
    }
    
    public async Task<string> GenerateToken(RegisterHostDTO registerHostDto)
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
