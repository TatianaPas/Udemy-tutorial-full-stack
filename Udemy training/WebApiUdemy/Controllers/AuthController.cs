using Microsoft.AspNetCore.Mvc;
using WebApiUdemy.Repositories;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenhandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenhandler)
        {
            this.userRepository = userRepository;
            this.tokenhandler = tokenhandler;
        }

        public IUserRepository UserRepository { get; }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
        {
            //Validate the incoming request

            //Check if user is authenticated
            //check username and password
            var user = await userRepository.AuthenticateAsync(
                loginRequest.Username, loginRequest.Password);

            if(user!=null)
            {
                //Generate JWT Token
               var token = await tokenhandler.CreatTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("Username or Password incorrect");
            
        }
           
    }
}
