namespace backend.Models
{
    public class TripModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Budget { get; set; }
    }
}
