namespace MDA.Security.Service
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IRO_EmployeeAddInfoService
    {
        /// <summary>
        /// Get EmployeeAddInfo List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<RO_EmployeeAddInfo> GetEmployeeAddInfoListForFilter(LinqFilter filter);
    }
}