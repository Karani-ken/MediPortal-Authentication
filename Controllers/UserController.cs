using AutoMapper;
using MediPortal_AuthService.Models;
using MediPortal_AuthService.Models.Dtos;
using MediPortal_AuthService.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMessageBus _messageBus;
        private readonly IConfiguration _configuration;
        private readonly IAuthInterface _authservice;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;


        public UserController(IMapper mapper, IAuthInterface authInterface, IMessageBus message, IConfiguration configuration)
        {
            _authservice = authInterface;
            _messageBus = message;
            _configuration = configuration;
            _response = new ResponseDto();
            _mapper = mapper;
        }
        //register user
        [HttpPost("Register")]
        public async Task<ActionResult<ResponseDto>> AddUser( RegisterRequestDto registerRequest)
        {
            var result = await _authservice.RegisterUser(registerRequest);
            if (!string.IsNullOrWhiteSpace(result))
            {
                _response.IsSuccess = false;
                _response.Message = result;
                return BadRequest(_response);
            }
            //send email to the queue after registration
            var queueName = _configuration.GetSection("QueuesandTopics:RegisterUser").Get<string>();
            var message = new UserMessage()
            {
                Email = registerRequest.Email,
                Name = registerRequest.firstname,
                 Content="<p>Welcome to MediPortal.<br>"+
                 "Book an appointment with our abled doctors to get premium services.Kind Regards Karani Ken, <br> Admin</p>"
            };
            await _messageBus.PublishMessage(message, queueName);
            return Ok(_response);
        }
        //get users
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllUsers()
        {
            var users = await _authservice.GetUsers();
            if (users.Count == 0)
            {
                _response.IsSuccess = false;
                _response.Message = "Could not fetch users";
                return BadRequest(_response);
            }
            _response.obj = users;
            return Ok(_response);
        }
        //delete user
        [HttpDelete]
        public async Task<ActionResult<ResponseDto>> DeleteUser(Guid id)
        {
            var result = await _authservice.DeleteUser(id);
            if (!string.IsNullOrWhiteSpace(result))
            {
                _response.IsSuccess = false;
                _response.Message = result;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
        //update user
        [HttpPut]
        public async Task<ActionResult<ResponseDto>> UpdateUser(Guid id, RegisterRequestDto registerRequest)
        {
            var AllUsers = await _authservice.GetUsers();
            var userToUpdate = AllUsers.FirstOrDefault(u => u.Id.ToString() == id.ToString());
            if (userToUpdate == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Could not update user";
                return BadRequest(_response);
            }
            var updatedUser = _mapper.Map(registerRequest, userToUpdate);
            var res = await _authservice.UpdateUser(updatedUser);
            _response.obj = res;
            return Ok(_response);

        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseDto>> LoginUserToSystem(LoginRequestDto loginRequestDto)
        {
            var response = await _authservice.LoginUser(loginRequestDto);
            if (response.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid Credentials";
                return BadRequest(_response);
            }
            _response.obj = response;
            return Ok(_response);
        }
        [HttpPost("AssigningRole")]
        public async Task<ActionResult<ResponseDto>> AssignRole(Guid UserId, string Role)
        {
            var response = await _authservice.AssignUserRole(UserId.ToString(), Role);
             var users = await _authservice.GetUsers();
             var user = users.FirstOrDefault(u=>u.Id==UserId.ToString());
            if (!response)
            {
                
                _response.IsSuccess = false;
                _response.Message = "Error Occured";

                return BadRequest(_response);
            }
            var fullname = user.firstname + user.surname;
             var queueName = _configuration.GetSection("QueuesandTopics:RegisterUser").Get<string>();
            var message = new UserMessage()
            {
                Email = user.Email,  
                Name = fullname,
                Content="<p>Your Application to be a doctor has been Approved."+
                " Login to access the doctor's Dashboard.<br>"+
                "Welcome Dr."+fullname+"<br>Kind Regards Karani Ken, <br> Admin</p>"
            };
            await _messageBus.PublishMessage(message, queueName);
            _response.obj = response;
            return Ok(_response);
        }

        [HttpGet("GetById")]
         public async Task<ActionResult<ResponseDto>> GetUser(Guid id)
        {
            var AllUsers = await _authservice.GetUsers();
            var user =  AllUsers.FirstOrDefault(u => u.Id.ToString() == id.ToString());
            if(user == null)
            {
                 _response.IsSuccess = false;
                _response.Message = "Could not get user";
                return BadRequest(_response);
            }
            var UserToReturn = _mapper.Map<UserDto>(user);
            _response.obj = UserToReturn;
            return Ok(_response);
        }


    }
}
