namespace ApartmentManagement.Entities
{
    public class Building
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
