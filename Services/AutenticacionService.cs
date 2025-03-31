using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using AdmSeriesAnimadasAPI.Models;
using AdmSeriesAnimadasAPI.Data;

namespace AdmSeriesAnimadasAPI.Services
{
    public class AutenticacionService
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AutenticacionService(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        public string Authenticate(string usuario, string password)
        {
            var user = _context.Usuarios
                .FirstOrDefault(u => u.UserName == usuario && u.Password == password);

            if (user == null) return null;

            // Generar token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Id", user.Id.ToString())
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
