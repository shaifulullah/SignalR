namespace MDA.Core.Service.Validators
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IdentityModel.Selectors;

    public class ClientValidator : UserNamePasswordValidator
    {
        public override void Validate(string name, string password)
        {
            var clientCredentials = (NameValueCollection)ConfigurationManager.GetSection("clientCredentials");

            var tempPassword = clientCredentials[name];
            if (name == null || password == null)
            {
                throw new UnauthorizedAccessException("Invalid User Name or Password");
            }

            if (tempPassword != password)
            {
                throw new UnauthorizedAccessException("Invalid User Name or Password");
            }
        }
    }
}