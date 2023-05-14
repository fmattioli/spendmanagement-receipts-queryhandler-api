using System.Text.Json;

namespace API.Filters.Models
{
    public record Error
    {
        public string? StatusCode { get; set; }
        public string? Message { get; set;}
        public string? StackTrace { get; set;}
    }
}
