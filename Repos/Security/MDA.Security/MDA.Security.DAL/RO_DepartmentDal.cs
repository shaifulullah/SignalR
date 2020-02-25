namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RO_DepartmentDal
    {
        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Department> GetDepartmentList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_DepartmentSet.ToList();
            }
        }

        /// <summary>
        /// Get Department List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Department> GetDepartmentListForFilter(LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_DepartmentSet.ToListResult(null, filter);
            }
        }
    }
}