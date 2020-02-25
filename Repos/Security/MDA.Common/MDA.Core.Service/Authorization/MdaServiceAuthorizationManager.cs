namespace MDA.Core.Service.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.IdentityModel.Claims;
    using System.IdentityModel.Policy;
    using System.Linq;
    using System.Security.Principal;

    public class AuthorizationPolicy : IAuthorizationPolicy
    {
        /// <summary>
        /// Implement Interface Member of IAuthorizationPolicy
        /// </summary>
        public string Id
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Implement Interface Member of IAuthorizationPolicy
        /// </summary>
        public ClaimSet Issuer
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets Trusted Servers List
        /// </summary>
        protected string[] TrustedServers
        {
            get
            {
                return Array.ConvertAll(ConfigurationManager.AppSettings["TrustedServers"].Split(','), x => x.Trim().ToUpper());
            }
        }

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <param name="evaluationContext">Evaluation Context</param>
        /// <param name="state">State</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool Evaluate(EvaluationContext evaluationContext, ref object state) // TODO Hani... find a permanent solution
        {
            evaluationContext.Properties["Principal"] = new ServicePrincipal(GetClientIdentity(evaluationContext));

            //var endpoint = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name];
            //var clientIP = ((RemoteEndpointMessageProperty)endpoint).Address;
            //var clientName = Dns.GetHostEntry(clientIP).HostName.ToUpper();

            //var isValid = TrustedServers.Contains(clientName);
            //if (isValid)
            //{
            return true;
            //}

            //throw new FaultException(string.Format("Authorization failed for client {0}", clientName));
        }

        /// <summary>
        /// Get Client Identity
        /// </summary>
        /// <param name="evaluationContext">Evaluation Context</param>
        /// <returns>Client Identity</returns>
        private IIdentity GetClientIdentity(EvaluationContext evaluationContext)
        {
            object value;
            if (!evaluationContext.Properties.TryGetValue("Identities", out value))
            {
                throw new Exception("No Identity found");
            }

            var identities = value as IList<IIdentity>;
            if (identities == null || identities.Count <= 0)
            {
                throw new Exception("No Identity found");
            }

            return identities[0];
        }
    }

    public class ServicePrincipal : IPrincipal
    {
        /// <summary>
        /// Initializes a new instance of the ServicePrincipal class.
        /// </summary>
        /// <param name="clientIdentity">Client Identity</param>
        public ServicePrincipal(IIdentity clientIdentity)
        {
            Identity = clientIdentity;
        }

        /// <summary>
        /// Gets or sets Identity
        /// </summary>
        public IIdentity Identity { get; set; }

        /// <summary>
        /// Gets IPrincipal.Identity
        /// </summary>
        IIdentity IPrincipal.Identity
        {
            get { return Identity; }
        }

        /// <summary>
        /// Gets or sets Roles
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// Checks if a Role is Authorized to perform an Action
        /// </summary>
        /// <param name="role">Role</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool IsInRole(string role)
        {
            var clientRoles = (NameValueCollection)ConfigurationManager.GetSection("clientRoles");
            Roles = Array.ConvertAll(clientRoles[Identity.Name].Split(','), x => x.Trim().ToUpper());

            return Roles.Contains(role);
        }
    }
}