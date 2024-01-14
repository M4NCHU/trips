namespace backend.DTOs
{
    public class VisitPlaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
        public IFormFile ImageFile { get; set; }
        public int DestinationId { get; set; }
    }
}
