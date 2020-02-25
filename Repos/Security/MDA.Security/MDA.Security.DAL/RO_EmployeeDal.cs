namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RO_EmployeeDal
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
            using (var db = new ApplicationDBContext())
            {
                var rO_EmployeeList = db.RO_EmployeeSet
                    .Where(x => !db.UserAccountDetailsSet.Where(y => y.LnEmployeeId != 0).Where(y => y.LnEmployeeId.HasValue).Any(y => y.LnEmployeeId.Value == x.Id)).ToList();

                if (rO_EmployeeList != null)
                {
                    rO_EmployeeList = isHideTerminated ? rO_EmployeeList.Where(x => x.StatusValue != -10133).ToList() : rO_EmployeeList.ToList();
                }

                return rO_EmployeeList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get Employee For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public RO_Employee GetEmployeeForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_EmployeeSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get Employee List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Employee> GetEmployeeListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                var rO_EmployeeList = db.RO_EmployeeSet.ToList();
                if (rO_EmployeeList != null)
                {
                    rO_EmployeeList = isHideTerminated ? rO_EmployeeList.Where(x => x.StatusValue != -10133).ToList() : rO_EmployeeList.ToList();
                }

                return rO_EmployeeList.ToListResult(null, filter);
            }
        }
    }
}