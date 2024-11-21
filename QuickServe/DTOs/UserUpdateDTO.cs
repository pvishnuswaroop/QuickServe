namespace QuickServe.DTOs
{
    public class UserUpdateDTO
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }  
    }
}
