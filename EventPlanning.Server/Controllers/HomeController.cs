using EventPlanning.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanning.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<EventIndexModel> Index()
        {
            return Enumerable.Empty<EventIndexModel>();
        }
    }
}
