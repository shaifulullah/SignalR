namespace MDA.Security.Service
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IApplicationService
    {
        /// <summary>
        /// Get Application For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        Application GetApplicationForCode(string code);

        /// <summary>
        /// Get Available Application Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        DataSourceResult<Application> GetAvailableApplicationPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);
    }
}