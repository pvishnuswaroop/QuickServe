using System;

namespace QuickServe.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }  // Primary Key
        public string Token { get; set; }  // The refresh token value
        public string Username { get; set; }  // The username associated with the token
        public DateTime ExpiryDate { get; set; }  // The expiration date of the refresh token
    }
}
