namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_EmployeeBll : IRO_EmployeeBll
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
        public DataSourceResult<RO_Employee> GetAvailableEmployeesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            var rO_EmployeeDal = new RO_EmployeeDal();
            return rO_EmployeeDal.GetAvailableEmployeesPage(take, skip, sort, filter, isHideTerminated);
        }

        /// <summary>
        /// Get Employee For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public RO_Employee GetEmployeeForId(int id)
        {
            var rO_EmployeeDal = new RO_EmployeeDal();
            return rO_EmployeeDal.GetEmployeeForId(id);
        }

        /// <summary>
        /// Get Employee List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Employee> GetEmployeeListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            var rO_EmployeeDal = new RO_EmployeeDal();
            return rO_EmployeeDal.GetEmployeeListForIsHideTerminatedAndFilter(isHideTerminated, filter);
        }
    }
}