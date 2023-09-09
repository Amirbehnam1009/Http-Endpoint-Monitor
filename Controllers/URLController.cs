using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    public class URLHelper
    {
        public string Address { get; set; }
        public int Threshold { get; set; }
    }
    [Route("[controller]/[action]")]
    [Authorize]
    public class URLController : Controller
    {
        [NonAction]
        public int GetUserID()
        {
            var x = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id");
            return int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value!);
        }
        public DB Db { get; }
        public URLController(DB db)
        {
            Db = db;
        }
        [HttpGet]
        public IActionResult GetList()
        {
            var userId = GetUserID();
            var urls = Db.URLs.Where(c => c.UserId == userId).ToList();
            return StatusCode(StatusCodes.Status200OK, urls);
        }
        [HttpGet]
        public IActionResult GetAlerts()
        {
            var userId = GetUserID();
            var urls = Db.URLs.Where(c => c.UserId == userId).ToList();
            var allUrlsIds = urls.Select(c => c.Id).ToList();
            var allres = Db.Requests.Where(c => allUrlsIds.Contains(c.URLId) && (c.Result >= 300 || c.Result < 200)).ToList();
            
            var res = allres.GroupBy(c => c.URLId).Select(c => new
            {
                URLId = c.Key,
                Count = c.Count(),
                Threshold = urls.FirstOrDefault(d => d.Id == c.Key).Threshold,
                Address = urls.FirstOrDefault(d=>d.Id == c.Key).Address
            }).Where(c=> c.Count>=c.Threshold).ToList();


            return StatusCode(StatusCodes.Status200OK, res);
        }
        [HttpGet]
        public IActionResult GetAllTries()
        {
            var userId = GetUserID();
            var urls = Db.URLs.Where(c => c.UserId == userId).ToList();
            var allUrlsIds = urls.Select(c => c.Id).ToList();
            var reqs = Db.Requests.Where(c => allUrlsIds.Contains(c.URLId));
            return StatusCode(StatusCodes.Status200OK, reqs);
        }
        [HttpPost]
        public IActionResult Post([FromBody] URLHelper h)
        {
            var id = GetUserID();
            if (Db.URLs.Where(c => c.UserId == id).Count() > 20)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "شما نمیتوانید بیش از 20 آدرس داشته باشید");
            }

            var url = new URL()
            {
                Address = h.Address,
                UserId = GetUserID(),
                Threshold = h.Threshold
            };
            var urls = Db.URLs.Add(url);
            Db.SaveChanges();
            return StatusCode(StatusCodes.Status200OK, "با موفقیت انجام شد");
        }
    }

}
