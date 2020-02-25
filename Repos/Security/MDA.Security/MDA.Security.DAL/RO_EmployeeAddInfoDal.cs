namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_EmployeeAddInfoDal
    {
        /// <summary>
        /// Get EmployeeAddInfo List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_EmployeeAddInfo> GetEmployeeAddInfoListForFilter(LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_EmployeeAddInfoSet.ToListResult(null, filter);
            }
        }
    }
}