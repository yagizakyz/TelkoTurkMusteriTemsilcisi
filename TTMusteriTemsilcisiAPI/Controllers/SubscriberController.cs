using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTMusteriTemsilcisiAPI.Models;

namespace TTMusteriTemsilcisiAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        /// <summary>
        /// Author: Yağız AKYÜZ
        /// Date: 28.09.2025
        /// Email: akyzyagiz@gmail.com
        /// Description: Bu controllerda abone kaydı ve listeleme işlemleri yapılmaktadır.
        /// </summary>

        private TTContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public SubscriberController(TTContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        // Abone ekleme
        [HttpPost]
        public IActionResult Post([FromBody] SubscriberClass subscriberObj)
        {
            SubscriberClass subscriber = new SubscriberClass
            {
                CustomerRepresentativeID = subscriberObj.CustomerRepresentativeID,
                Name = subscriberObj.Name,
                Surname = subscriberObj.Surname,
                TCKN = subscriberObj.TCKN,
                Email = subscriberObj.Email,
                PhoneNumber = subscriberObj.PhoneNumber,
                Address = subscriberObj.Address,
                City = subscriberObj.City,
                District = subscriberObj.District,
                AddedDate = DateTime.Now,
                ProcessFinished = false
            };
            
            _context.SubscriberTable.Add(subscriber);
            _context.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // Abone işlemlerini bitirme
        [HttpPut("{id}")]
        public IActionResult ProcessFinishedTrue(int id, [FromBody] SubscriberClass subscriberObj)
        {
            var subs = _context.SubscriberTable.FirstOrDefault(u => u.Id == id);

            if (subs == null)
            {
                return NotFound(new { message = "Kayıt bulunamadı." });
            }

            subs.ProcessFinished = true;

            _context.SaveChanges();

            return Ok(new { message = "Kayıt güncelleme başarılı." });
        }

        // Tüm aboneleri listeleme
        [HttpGet]
        public IActionResult GetAllSubscribers()
        {
            var customerRepresentativeObj = (from subs in _context.SubscriberTable
                                             join cr in _context.CustomerRepresentativeTable on subs.CustomerRepresentativeID equals cr.Id
                                             select new
                                             {
                                                 Id = subs.Id,
                                                 Name = subs.Name,
                                                 Surname = subs.Surname,
                                                 TCKN = subs.TCKN,
                                                 Email = subs.Email,
                                                 PhoneNumber = subs.PhoneNumber,
                                                 Address = subs.Address,
                                                 City = subs.City,
                                                 District = subs.District,
                                                 AddedDate = subs.AddedDate,
                                                 ProcessFinished = subs.ProcessFinished,
                                                 CustomerRepresentativeName = cr.Name,
                                                 CustomerRepresentativeSurname = cr.Surname,
                                                 CustomerRepresentativePhoneNumber = cr.PhoneNumber
                                             });
            return Ok(customerRepresentativeObj);
        }

        // Müşteri temsilcisine göre aboneleri listeleme
        [HttpGet("{CustomerRepresentativeID}")]
        public IActionResult GetCRSubscribers(int CustomerRepresentativeID)
        {
            var customerRepresentativeObj = (from subs in _context.SubscriberTable
                         join cr in _context.CustomerRepresentativeTable on subs.CustomerRepresentativeID equals cr.Id
                         where subs.CustomerRepresentativeID == CustomerRepresentativeID
                         select new
                         {
                             Id = subs.Id,
                             Name = subs.Name,
                             Surname = subs.Surname,
                             TCKN = subs.TCKN,
                             Email = subs.Email,
                             PhoneNumber = subs.PhoneNumber,
                             Address = subs.Address,
                             City = subs.City,
                             District = subs.District,
                             AddedDate = subs.AddedDate,
                             ProcessFinished = subs.ProcessFinished,
                             CustomerRepresentativeName = cr.Name,
                             CustomerRepresentativeSurname = cr.Surname,
                             CustomerRepresentativePhoneNumber = cr.PhoneNumber
                         });
            return Ok(customerRepresentativeObj);
        }

        // Telefon numarasına göre abone listeleme
        [HttpGet("{PhoneNumber}")]
        public IActionResult GetSubscribersByPhoneNumber(string PhoneNumber)
        {
            var customerRepresentativeObj = (from subs in _context.SubscriberTable
                                             join cr in _context.CustomerRepresentativeTable on subs.CustomerRepresentativeID equals cr.Id
                                             where subs.PhoneNumber == PhoneNumber
                                             select new
                                             {
                                                 Id = subs.Id,
                                                 Name = subs.Name,
                                                 Surname = subs.Surname,
                                                 TCKN = subs.TCKN,
                                                 Email = subs.Email,
                                                 PhoneNumber = subs.PhoneNumber,
                                                 Address = subs.Address,
                                                 City = subs.City,
                                                 District = subs.District,
                                                 AddedDate = subs.AddedDate,
                                                 ProcessFinished = subs.ProcessFinished,
                                                 CustomerRepresentativeName = cr.Name,
                                                 CustomerRepresentativeSurname = cr.Surname,
                                                 CustomerRepresentativePhoneNumber = cr.PhoneNumber
                                             });
            return Ok(customerRepresentativeObj);
        }

        // İşlemleri bitmemiş (ProcessFinished false olan) aboneleri listeleme
        [HttpGet]
        public IActionResult GetAllPF_False()
        {
            var customerRepresentativeObj = (from subs in _context.SubscriberTable
                                             join cr in _context.CustomerRepresentativeTable on subs.CustomerRepresentativeID equals cr.Id
                                             where subs.ProcessFinished == false
                                             orderby subs.AddedDate ascending
                                             select new
                                             {
                                                 Id = subs.Id,
                                                 Name = subs.Name,
                                                 Surname = subs.Surname,
                                                 TCKN = subs.TCKN,
                                                 Email = subs.Email,
                                                 PhoneNumber = subs.PhoneNumber,
                                                 Address = subs.Address,
                                                 City = subs.City,
                                                 District = subs.District,
                                                 AddedDate = subs.AddedDate,
                                                 ProcessFinished = subs.ProcessFinished,
                                                 CustomerRepresentativeName = cr.Name,
                                                 CustomerRepresentativeSurname = cr.Surname,
                                                 CustomerRepresentativePhoneNumber = cr.PhoneNumber
                                             });
            return Ok(customerRepresentativeObj);
        }

        // İşlemleri bitmiş (ProcessFinished true olan) aboneleri listeleme
        public IActionResult GetAllPF_True()
        {
            var customerRepresentativeObj = (from subs in _context.SubscriberTable
                                             join cr in _context.CustomerRepresentativeTable on subs.CustomerRepresentativeID equals cr.Id
                                             where subs.ProcessFinished == true
                                             orderby subs.AddedDate ascending
                                             select new
                                             {
                                                 Id = subs.Id,
                                                 Name = subs.Name,
                                                 Surname = subs.Surname,
                                                 TCKN = subs.TCKN,
                                                 Email = subs.Email,
                                                 PhoneNumber = subs.PhoneNumber,
                                                 Address = subs.Address,
                                                 City = subs.City,
                                                 District = subs.District,
                                                 AddedDate = subs.AddedDate,
                                                 ProcessFinished = subs.ProcessFinished,
                                                 CustomerRepresentativeName = cr.Name,
                                                 CustomerRepresentativeSurname = cr.Surname,
                                                 CustomerRepresentativePhoneNumber = cr.PhoneNumber
                                             });
            return Ok(customerRepresentativeObj);
        }
    }
}
