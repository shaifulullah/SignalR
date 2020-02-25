namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RO_EmployeeDetailsDal
    {
        /// <summary>
        /// Get EmployeeDetails For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public RO_EmployeeDetails GetEmployeeDetailsForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_EmployeeDetailsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get EmployeeDetails List For Is Hide Terminated And Filter
        /// </summary>
        /// <param name="isHideTerminated">Hide Terminated Employees</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_EmployeeDetails> GetEmployeeDetailsListForIsHideTerminatedAndFilter(bool isHideTerminated, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                var rO_EmployeeDetailsList = db.RO_EmployeeDetailsSet.ToList();
                if (rO_EmployeeDetailsList != null)
                {
                    rO_EmployeeDetailsList = isHideTerminated ? rO_EmployeeDetailsList.Where(x => x.StatusValue != -10133).ToList() : rO_EmployeeDetailsList.ToList();
                }

                return rO_EmployeeDetailsList.ToListResult(null, filter);
            }
        }
    }
}