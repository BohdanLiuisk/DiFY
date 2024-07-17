using System.Text.Json.Serialization;

namespace Dify.Entity.WebApi.Models;

public class ContactModelTest
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("created_by")]
    public ContactModelTest CreatedBy { get; set; }
    
    [JsonPropertyName("parent_contact")]
    public ContactModelTest ParentContact { get; set; }
}
