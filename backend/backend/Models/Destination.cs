using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Location { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
