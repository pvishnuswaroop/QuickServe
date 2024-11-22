namespace QuickServe.DTO
{
    public class UpdateUser
    {
        public int Id { get; set; }  // Ensure this property exists
        public string Email { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
    }
}
