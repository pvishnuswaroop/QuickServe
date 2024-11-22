namespace QuickServe.DTO
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }  // Ensure this is the same as the user's ID
        public string Email { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        // Add any other properties you want to allow the user to update
    }
}
