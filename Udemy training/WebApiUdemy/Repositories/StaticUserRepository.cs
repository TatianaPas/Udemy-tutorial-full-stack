using WebApiUdemy.Models.Domain;

namespace WebApiUdemy.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
            {
            new User()
        {
            FirstName = "Read Only", LastName = "User", Email="readonly@example.com", Id=Guid.NewGuid(),
                Username= "readonly@example.com", Password="ReadOnly@user",Roles = new List<string> { "reader"}
        }, new User()
        {
            FirstName = "Read Write", LastName = "User", Email="readwrite@example.com", Id=Guid.NewGuid(),
                Username= "readwrite@example.com", Password="Readwrite@user",Roles = new List<string> { "reader", "writer"}
        }
     };


        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
            x.Password == password);
            
            return user;            
        }
    }
}
