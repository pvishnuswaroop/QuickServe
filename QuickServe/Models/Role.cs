namespace QuickServe.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }

}
