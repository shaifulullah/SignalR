namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_EmployeeDetailsBll : IRO_EmployeeDetailsBll
    {
        /// <summary>
        /// Get EmployeeDetails For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public RO_EmployeeDetails GetEmployeeDetailsForId(int id)
        {
            var rO_EmployeeDetailsDal = new RO_EmployeeDetailsDal();
            return rO_EmployeeDetailsDal.GetEmployeeDetailsForId(id);
        }

        /// <summary>
        /// Get EmployeeDetails List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_EmployeeDetails> GetEmployeeDetailsListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            var rO_EmployeeDetailsDal = new RO_EmployeeDetailsDal();
            return rO_EmployeeDetailsDal.GetEmployeeDetailsListForIsHideTerminatedAndFilter(isHideTerminated, filter);
        }
    }
}