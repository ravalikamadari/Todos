using Todos.DTOs;

namespace Todos.Models
{
    public record Todo
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }



    //      public TodoDTO asDto => new TodoDTO
    //     {
    //        Id = Id,
    //        UserId = UserId,
    //        Description = Description,
    //        IsCompleted = IsCompleted

    // };

    }
}