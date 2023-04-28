namespace UserManagement.Models
{
    public class UserShift
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal TotalTime { get; set; }
    }
}
