using InventoryService.Models;
using Microsoft.Extensions.Options;
using System.DirectoryServices;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace InventoryService.Services
{
    [SupportedOSPlatform("windows")]
    public class LdapService : ILdapService
    {
        private const string EmailAttribute = "mail";
        private const string UserIdAttribute = "uid";
        private const string UserNameAttribute = "givenName";

        private readonly LdapConfig config;

        public LdapService(IOptions<LdapConfig> config)
        {
            this.config = config.Value;
        }
        public User SearchUser(string userId)
        {
            //whitelist validation for vulnerability test
            if (Regex.IsMatch(userId, "^[a-zA-Z][a-zA-Z0-9]*$"))
            {
                using (DirectoryEntry entry = new DirectoryEntry(config.Path))
                {
                    entry.AuthenticationType = AuthenticationTypes.Anonymous;
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = "(&(" + UserIdAttribute + "=" + userId + "))";
                        searcher.PropertiesToLoad.Add(EmailAttribute);
                        searcher.PropertiesToLoad.Add(UserIdAttribute);
                        searcher.PropertiesToLoad.Add(UserNameAttribute);
                        var result = searcher.FindOne();
                        if (result != null)
                        {
                            var email = result.Properties[EmailAttribute];
                            var uid = result.Properties[UserIdAttribute];
                            var uname = result.Properties[UserNameAttribute];

                            return new User
                            {
                                Email = email == null || email.Count <= 0 ? null : email[0].ToString(),
                                UserId = uid == null || uid.Count <= 0 ? null : uid[0].ToString(),
                                UserName = uname == null || uname.Count <= 0 ? null : uname[0].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
