using Todos.DTOs;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long Mobile { get; set; }



        public UserDTO asDto => new UserDTO
        {
           Id = Id,
           Username = Username,
           Name = Name,
           Mobile = Mobile,
           Email = Email,

    };

    }
}