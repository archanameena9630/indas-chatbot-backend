
namespace JobCardBackend.Models
{
    public class User
    {
       public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string Password { get; set; } = "";
         
    }
}
