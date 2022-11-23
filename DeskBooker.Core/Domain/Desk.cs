namespace DeskBooker.Core.Domain
{
    public  class Desk
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<DeskBooking>? DeskBookings { get; set; }
    }
}
