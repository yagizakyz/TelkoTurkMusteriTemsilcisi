using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlTypes;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TTMusteriTemsilcisiAPI.Models;

namespace TTMusteriTemsilcisiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerRepresentativeController : ControllerBase
    {
        /// <summary>
        /// Author: Yağız AKYÜZ
        /// Date: 28.09.2025
        /// Email: akyzyagiz@gmail.com
        /// Description: Bu controllerda müşteri temsilcilerinin kayıt ve giriş işlemleri yapılmaktadır.
        /// bunun yanında müşteri temsilcilerinin listeleme işlemi yapılmaktadır.
        /// </summary>

        private TTContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public CustomerRepresentativeController(TTContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        // Kullanıcı kayıt
        [AllowAnonymous]
        [HttpPost]
        public IActionResult RegisterUser([FromBody] CustomerRepresentativeClass user)
        {
            var userWithSameData = _context.CustomerRepresentativeTable.Where(u => u.Email == user.Email || u.PhoneNumber == user.PhoneNumber || u.TCKN == user.TCKN).SingleOrDefault();
            if (userWithSameData != null)
            {
                return BadRequest("Girilen veriler ile daha önce bir abonelik isteği oluşturulmuş!");
            }

            var userObj = new CustomerRepresentativeClass()
            {
                Name = user.Name,
                Surname = user.Surname,
                TCKN = user.TCKN,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                MembershipDate = DateTime.Now
            };
            _context.CustomerRepresentativeTable.Add(userObj);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, new { message = "Kayıt başarılı" });
        }

        // Kullanıcı girişi
        [AllowAnonymous]
        [HttpPost]
        public IActionResult LoginUser([FromBody] CustomerRepresentativeClass user)
        {
            var userEmail = _context.CustomerRepresentativeTable.FirstOrDefault(u => u.Email == user.Email);
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
                user_email = userEmail.Email,
                user_phoneNumber = userEmail.PhoneNumber,
                user_tckn = userEmail.TCKN
            });
        }

        //Bütün müşteri temsilcilerini listeleme
        [HttpGet]
        public IActionResult GetAllCR()
        {
            var customerRepresentativeObj = (from cr in _context.CustomerRepresentativeTable
                                             select new
                                             {
                                                 Id = cr.Id,
                                                 Name = cr.Name,
                                                 Surname = cr.Surname,
                                                 TCKN = cr.TCKN,
                                                 Email = cr.Email,
                                                 PhoneNumber = cr.PhoneNumber,
                                                 MembershipDate = cr.MembershipDate,
                                                 MembershipEndDate = cr.MembershipEndDate
                                             });
            return Ok(customerRepresentativeObj);
        }

        //Telefon numarasına göre müşteri temsilcisi listeleme
        [HttpGet("{PhoneNumber}")]
        public IActionResult GetCRByPhoneNumber(string PhoneNumber)
        {
            var customerRepresentativeObj = (from cr in _context.CustomerRepresentativeTable
                                             where cr.PhoneNumber == PhoneNumber
                                             select new
                                             {
                                                 Id = cr.Id,
                                                 Name = cr.Name,
                                                 Surname = cr.Surname,
                                                 TCKN = cr.TCKN,
                                                 Email = cr.Email,
                                                 PhoneNumber = cr.PhoneNumber,
                                                 MembershipDate = cr.MembershipDate,
                                                 MembershipEndDate = cr.MembershipEndDate
                                             });
            return Ok(customerRepresentativeObj);
        }
    }
}
