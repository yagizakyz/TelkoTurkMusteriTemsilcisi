using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TTMusteriTemsilcisiAPI.Models;

namespace TTMusteriTemsilcisiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TTPanelController : ControllerBase
    {
        /// <summary>
        /// Author: Yağız AKYÜZ
        /// Date: 28.09.2025
        /// Email: akyzyagiz@gmail.com
        /// Description: Bu controllerda TelkoTürk çalışanlarının müşteri temsilcilerini kontrol edebilmesi için
        /// kayıt olamasını ve giriş yapmasını sağlıyor.
        /// </summary>

        private TTContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public TTPanelController(TTContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] TTPanelClass user)
        {
            var userWithSameData = _context.CustomerRepresentativeTable.Where(u => u.Email == user.Email).SingleOrDefault();
            if (userWithSameData != null)
            {
                return BadRequest("Girilen veriler ile daha önce bir abonelik isteği oluşturulmuş!");
            }

            var userObj = new TTPanelClass()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password)
            };
            _context.TTPanelTable.Add(userObj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, new { message = "Kayıt başarılı" });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser([FromBody] TTPanelClass user)
        {
            var userEmail = _context.TTPanelTable.FirstOrDefault(u => u.Email == user.Email);
            if (userEmail == null)
            {
                return NotFound(new { message = "Kullanıcı Bulunamadı" });
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized(new { message = "Şifre yanlış" });
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var token = _auth.GenerateAccessToken(claims);
            return Ok(new
            {
                message = "Giriş başarılı",
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_time = token.ValidFrom,
                expiration_time = token.ValidTo,
                user_id = userEmail.Id,
                user_name = userEmail.Name,
                user_surname = userEmail.Surname,
                user_email = userEmail.Email
            });
        }
    }
}
