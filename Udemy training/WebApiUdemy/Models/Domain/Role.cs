namespace WebApiUdemy.Models.Domain
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //navigation property

        public List<User_Role> UserRolse { get; set; }
    }
}
