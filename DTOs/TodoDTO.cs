using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Todos.DTOs;
using  Todos.Models;

namespace Todos.DTOs;
public record TodoDTO
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

     [JsonPropertyName("user_id")]
    public long UserId{ get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }

    // [JsonPropertyName("posts")]
    // public List<PostDTO> Post { get; internal set; }
}

public record TodoCreateDTO
{
    //  [JsonPropertyName("user_id")]
    // public long UserId{ get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    // [JsonPropertyName("is_completed")]
    // public bool IsCompleted { get; set; }



}

public record TodoUpdateDTO
{


    // [JsonPropertyName("description")]
    // public string Description { get; set; }

    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }



}