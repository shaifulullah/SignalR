namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IRO_EmployeeBll
    {
        /// <summary>
        /// Get Available Employees Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<RO_Employee> GetAvailableEmployeesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

        /// <summary>
        /// Get Employee For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        RO_Employee GetEmployeeForId(int id);

        /// <summary>
        /// Get Employee List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_Employee> GetEmployeeListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter);
    }
}