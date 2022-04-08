using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Todos.DTOs;
using  Todos.Models;

namespace Todos.DTOs;
public record UserDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

     [JsonPropertyName("user_name")]
    public string Username{ get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("mobile")]
    public long Mobile { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }

    // [JsonPropertyName("is_completed")]
    // public string IsCompleted { get; set; }

    // [JsonPropertyName("posts")]
    // public List<PostDTO> Post { get; internal set; }
}

public record UserCreateDTO
{

    [JsonPropertyName("user_name")]
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [JsonPropertyName("name")]
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    [JsonPropertyName("mobile")]

    [Required]
    public long Mobile { get; set; }

    [JsonPropertyName("email")]
    [MaxLength(255)]
    public string Email { get; set; }
     [JsonPropertyName("password")]
    public string Password { get; set; }

    // [JsonPropertyName("is_completed")]
    // [Required]
    // [MaxLength(6)]

    // public string IsCompleted { get; set; }

}






