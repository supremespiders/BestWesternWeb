using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using BestWesternWeb.Models;
using BestWesternWeb.Services;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BestWesternWeb.Controllers
{
    public class User : Controller
    {
        private readonly DbContext _context;
        private readonly JwtHandler _jwt;

        public User(DbContext context, JwtHandler jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public IActionResult Index()
        {
            return Ok();
        }

        [Authorize]
        public IActionResult GetConfig()
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null)
            {
                user = new DataAccess.Models.User() { Username = "admin", Password = "admin", Config = JsonConvert.SerializeObject(new Config()) };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            return Ok(JsonConvert.DeserializeObject<Config>(user.Config));
        }

        [Authorize]
        [HttpPost]
        public IActionResult SaveConfig([FromBody] Config config)
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null) return BadRequest("no users");
            user.Config = JsonConvert.SerializeObject(config);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult Login([FromBody] Credential credential)
        {
            var user = _context.Users.FirstOrDefault();
            if (user == null)
            {
                user = new DataAccess.Models.User() { Username = "admin", Password = "admin", Config = JsonConvert.SerializeObject(new Config()) };
                _context.Users.Add(user);
                _context.SaveChanges();
            }

            user = _context.Users.FirstOrDefault(x => x.Username == credential.Username && x.Password == credential.Password);
            if (user == null) return Unauthorized(new AuthResponse { ErrorMessage = "Username and/or password is incorrect" });

            var signingCredentials = _jwt.GetSigningCredentials();
            var claims = _jwt.GetClaims(user);
            var tokenOptions = _jwt.GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new AuthResponse { IsAuthSuccessful = true, Token = token });
        }
    }
}