namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface ISecurityRolesService
    {
        /// <summary>
        /// Get SecurityRolesDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<SecurityRolesDetails> GetSecurityRolesDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode);

        /// <summary>
        /// Get SecurityRolesDetailsWithUserName List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<SecurityRolesDetailsWithUserName> GetSecurityRolesDetailsWithUserNameListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode);

        /// <summary>
        /// Get SecurityRoles List For Application Code And Company Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="companyId">Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<SecurityRoles> GetSecurityRolesListForApplicationCodeAndCompanyId(string applicationCode, int companyId);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityRoles">SecurityRoles Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        [OperationContract]
        bool UpdateSecurityRoles(SecurityRoles securityRoles, string userName);
    }
}