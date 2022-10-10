using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiUdemy.Models.Domain
{
    public class UserNZ
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [NotMapped]
        public List<string> Roles { get; set; }

        //navigation property

        public List<User_Role> UserRoles { get; set; }

    }
}
