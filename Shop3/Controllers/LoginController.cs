using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shop3.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Shop3.Models.ViewModel;
using System.Collections.Generic;

namespace Shop3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly UserManager<Usuario> _userManager;
        public LoginController(IConfiguration config, UserManager<Usuario> userManager)
        {
            _config = config;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginVM usuarioLoginVM)
        {
            if (ModelState.IsValid)
            {
                var PasswordHash = new PasswordHasher<Usuario>();

                var existingUser = _userManager.Users.SingleOrDefault(u => u.Email == usuarioLoginVM.Email);

                if (existingUser == null)
                {
                    // We dont want to give to much information on why the request has failed for security reasons
                    return NotFound("No se encuentra el usuario con email: " + usuarioLoginVM.Email);
                }

                // Now we need to check if the user has inputed the right password

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, usuarioLoginVM.Password);

                if (isCorrect)
                {

                    //var asdarols = await _userManager.AddToRoleAsync(existingUser, "ADMINISTRADOR");

                    return await generateJwtAsync(existingUser);
                }
            }
            return BadRequest("Credencial invalida. Vuelva a intentarlo");
        }

        private async Task<IActionResult> generateJwtAsync(Usuario existingUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), /* Insert user id here */
                 new Claim(JwtRegisteredClaimNames.Email, existingUser.Email),
                 new Claim(JwtRegisteredClaimNames.Sub, existingUser.UserName),
             };

            var roles = await _userManager.GetRolesAsync(existingUser);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var token = new JwtSecurityToken
           (
               claims: claims,
               signingCredentials: creds,
               expires: DateTime.Now.AddMinutes(50), /* Token expire time */
               issuer: _config["Jwt:Issuer"],
               audience: _config["Jwt:Audience"]
           );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

    }


}
