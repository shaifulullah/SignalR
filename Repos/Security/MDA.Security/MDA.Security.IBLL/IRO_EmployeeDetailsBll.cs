namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IRO_EmployeeDetailsBll
    {
        /// <summary>
        /// Get EmployeeDetails For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        RO_EmployeeDetails GetEmployeeDetailsForId(int id);

        /// <summary>
        /// Get EmployeeDetails List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_EmployeeDetails> GetEmployeeDetailsListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter);
    }
}