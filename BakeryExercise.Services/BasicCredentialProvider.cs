namespace BakeryExercise.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using BakeryExercise.EntityFramework;
    using Bazinga.AspNetCore.Authentication.Basic;

    public class BasicCredentialProvider : IBasicCredentialVerifier
    {
        private readonly BakeryContext bakeryContext;

        public BasicCredentialProvider(BakeryContext bakeryContext)
        {
            this.bakeryContext = bakeryContext;
        }

        public Task<bool> Authenticate(string username, string password)
        {
            var user = this.bakeryContext.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            return Task.FromResult(user != null);
        }
    }
}
