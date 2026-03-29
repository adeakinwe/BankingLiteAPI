using BankingLite.Api.Interface;
using BankingLite.Api.Models.DTOs.Request;
using BankingLite.Api.Models.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingLite.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        public AuthController(IUserRepo userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("FullName, Email and Password are required.");

            try
            {
                var user = await _userRepo.Register(request);

                return CreatedAtAction(nameof(Register), new { user.Id }, new { user.Id, user.FullName, user.Email });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and Password are required.");

            var user = await _userRepo.Login(request);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = GenerateJwtToken(user);

            return Ok(new Authresponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Token = token
            });
        }

        private string GenerateJwtToken(UserResponse user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secret = jwtSettings.GetValue<string>("Key") ?? throw new InvalidOperationException("Jwt:Key must be configured");
            var issuer = jwtSettings.GetValue<string>("Issuer") ?? "BankingLiteAPI";
            var audience = jwtSettings.GetValue<string>("Audience") ?? "BankingLiteAPI";
            var expires = DateTime.UtcNow.AddHours(1);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}