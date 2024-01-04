using AutoMapper;
using EvaluationPartyProduct.DTO;
using EvaluationPartyProduct.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EvaluationPartyProduct.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private EvaluationDbContext context;
        private readonly IMapper mapper;

        public AuthController(IMapper mapper, EvaluationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }
        public async Task<bool> checkUserExists(UserDTO userDTO)
        {
            var user = await context.TblUsers.FirstOrDefaultAsync(i => i.Username == userDTO.Username && i.Password == userDTO.Password);
            if (user == null) return false;
            return true;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserDTO userDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await checkUserExists(userDTO)) return Conflict("Duplicate data is not allowed.");
            var user = mapper.Map<TblUser>(userDTO);
            context.Add(user);
            await context.SaveChangesAsync();
            return Ok("Registered Successfully");
        }
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO userDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await context.TblUsers.FirstOrDefaultAsync(i => i.Username == userDTO.Username && i.Password == userDTO.Password);
            if (user == null) return BadRequest("User Not Found");
            String token = CreateToken(user);
            return Ok(token);
        }
        private string CreateToken(TblUser user)
        {
            List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);
            // Create a JWT token with claims, expiration, and signing credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
