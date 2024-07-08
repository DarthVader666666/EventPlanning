using AutoMapper;
using EventPlanning.Bll.Interfaces;
using EventPlanning.Data.Entities;
using EventPlanning.Server.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EventPlanning.Server.Controllers
{
    [EnableCors("AllowClient")]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public EventsController(IMapper mapper, IRepository<Event> eventRepository, ILogger<EventsController> logger)
        {
            _logger = logger;
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<EventIndexModel>> Index()
        {
            var events = await _eventRepository.GetListAsync();
            var mappedEvents = _mapper.Map<IEnumerable<Event>, IEnumerable<EventIndexModel>>(events);
            return mappedEvents;
        }

        [HttpPost]
        public async Task Create(EventCreateModel model)
        {
            try
            {
                var newEvent = _mapper.Map<EventCreateModel, Event>(model);
                await _eventRepository.CreateAsync(newEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating event");
            }
        }
    }
}
