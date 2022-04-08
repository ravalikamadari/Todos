using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Todos.DTOs;
using Todos.Repositories;

namespace Todos.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _user;

    public UserController(ILogger<UserController> logger,IUserRepository user,
    IConfiguration configuration)
    {
        _logger = logger;
        _user = user;
        _configuration = configuration;
    }


    [HttpPost("Sign up")]
       public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserCreateDTO Data)
    {
        var toCreateUser = new User
        {
            Name = Data.Name.Trim(),
            Username = Data.Username.Trim(),
            Password = Data.Password.Trim(),
            Email = Data.Email.Trim().ToLower(),
            // IsCompleted = Data.IsCompleted.Trim(),
            Mobile = Data.Mobile,

        };

        var createdUser = await _user.Create(toCreateUser);

        return StatusCode(StatusCodes.Status201Created, createdUser.asDto);
    }

     [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserById([FromRoute] long id)
    {
        var user = await _user.GetById(id);

        if (user is null)
            return NotFound("No user found with given id");

        var dto = user.asDto;
        return Ok(dto);
    }





     [HttpPost("login")]
    public async Task<ActionResult<UserDTO>> Login([FromBody] UserLogin userLogin)
    {
        var user = await _user.GetUsername(userLogin.Username);
        if(user == null)

            return NotFound("No user found with given username");

        if(user.Password != userLogin.Password)
            return Unauthorized("Invalid password");

        var token = Generate(user);
        return Ok(token);
    }

     private string Generate(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.MobilePhone, user.Mobile.ToString()),
            // new Claim(ClaimTypes.Password, user.Password)
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
    }



    // private ActionResult User Authenticate(UserLogin userLogin)
    // {
    //     var user = _user.GetUsername(userLogin.Username);

    //     // var currentUser = FirstOrDefault(o => o.Username.ToLower() ==
    //     // userLogin.Username.ToLower() && o.Password == userLogin.Password);

    //     if (User == null)
    //     {
    //         return NotFound("User not found");
    //     }

    //     return null;

    // }
}



