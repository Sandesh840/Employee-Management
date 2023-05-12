namespace UserManagement.Models
{
    public class UserShiftLog
    {
        public int Id { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public int UserId { get; set; }
        public User User {get; set;}
        public int UserShiftId { get; set; }   
        public UserShift UserShift { get; set; }
    }
}
