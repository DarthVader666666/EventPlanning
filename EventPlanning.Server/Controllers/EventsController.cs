using AutoMapper;
using EventPlanning.Bll.Interfaces;
using EventPlanning.Bll.Services;
using EventPlanning.Data.Entities;
using EventPlanning.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EventPlanning.Server.Controllers
{
    [EnableCors("AllowClient")]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserEvent> _userEventRepository;
        private readonly EmailSender _emailSender;
        private readonly IMapper _mapper;

        public EventsController(IMapper mapper, IRepository<Event> eventRepository, IRepository<User> userRepository, 
            IRepository<UserEvent> userEventRepository, EmailSender emailSender)
        {
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _userEventRepository = userEventRepository;
            _emailSender = emailSender;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/events/")]
        public async Task<IActionResult> Index()
        {
            var events = await _eventRepository.GetListAsync();
            var mappedEvents = _mapper.Map<IEnumerable<Event>, IEnumerable<EventIndexModel>>(events);
            return Ok(mappedEvents);
        }

        [HttpGet]
        [Route("/events/{eventId:int}")]
        public async Task<EventIndexModel> GetEvent([FromRoute] int? eventId)
        {
            var eventEntity = await _eventRepository.GetAsync(eventId);
            var mappedEvent = _mapper.Map<Event, EventIndexModel>(eventEntity);

            return mappedEvent;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Create(EventCreateModel model)
        {
            try
            {
                var newEvent = _mapper.Map<EventCreateModel, Event>(model);
                await _eventRepository.CreateAsync(newEvent);
            }
            catch (SqlException)
            {
                return BadRequest("Error while creating event");
            }

            return Ok("Event created");
        }

        [HttpPost]
        [Route("participate")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Participate(EventConfirm model)
        {
            var user = await _userRepository.GetAsync(model.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            var userEvent = new UserEvent()
            {
                UserId = (int)user.UserId!,
                EventId = (int)model.EventId!
            };

            if (!await _userEventRepository.ExistsAsync(userEvent))
            {
                await _userEventRepository.CreateAsync(userEvent);
            }

            var url = Request.GetDisplayUrl().Replace("participate/", "confirm");

            var result = await _emailSender.SendEmailAsync(model.Email, "Thank you! Event participation confirmed!",
                $"{url}?userId={userEvent.UserId}&eventId={userEvent.EventId}");

            if (result.HasCompleted)
            {
                return Ok("Email sent");
            }
            else
            {
                return BadRequest("Error while sending email");
            }
            
        }

        [HttpGet]
        [Route("confirm")]
        public async Task<IActionResult> Confirm([FromQuery] int? userId, [FromQuery]int? eventId)
        {
            var userEvent = await _userEventRepository.GetAsync(new Tuple<int?, int?>(userId, eventId));

            if (userEvent == null)
            {
                return BadRequest("User or event not found");
            }

            userEvent.EmailConfirmed = true;

            await _userEventRepository.UpdateAsync(userEvent);

            return Ok();
        }
    }
}
