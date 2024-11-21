using QuickServe.Models;

namespace QuickServe.DTOs
{
    public class UserReadDTO
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }  
    }
}
