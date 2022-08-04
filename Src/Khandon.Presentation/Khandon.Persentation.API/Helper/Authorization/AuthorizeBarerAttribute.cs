using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Khandon.Persentation.API.Helper.Authorization
{
    public class AuthorizeBarerAttribute: Attribute, IAuthorizeData
    {
        public string? Policy { get; set; }
        //
        // Summary:
        //     Gets or sets a comma delimited list of roles that are allowed to access the resource.
        public string? Roles { get; set; }
        //
        // Summary:
        //     Gets or sets a comma delimited list of schemes from which user information is
        //     constructed.
        public string? AuthenticationSchemes { get; set; } = JwtBearerDefaults.AuthenticationScheme;
    }
}
