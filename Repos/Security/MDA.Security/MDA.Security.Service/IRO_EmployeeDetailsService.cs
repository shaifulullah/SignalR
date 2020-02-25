namespace MDA.Security.Service
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IRO_EmployeeDetailsService
    {
        /// <summary>
        /// Get EmployeeDetails List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<RO_EmployeeDetails> GetEmployeeDetailsListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter);
    }
}