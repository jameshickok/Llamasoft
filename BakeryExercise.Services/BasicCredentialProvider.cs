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
            // From what I can tell, there is no encryption of the password here. That creates a security vulnerability.
            //TODO: Encrypt the password/use another provider like ASP Identity.
            //Also, two factor authentication would help make this more secure.
            //Using [ValidateAntiforgeryToken] along with an anti-forgery token will help prevent cross-site attacks.
            var user = this.bakeryContext.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
            return Task.FromResult(user != null);
        }
    }
}
