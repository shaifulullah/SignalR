namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_DepartmentBll : IRO_DepartmentBll
    {
        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Department> GetDepartmentList()
        {
            var rO_DepartmentDal = new RO_DepartmentDal();
            return rO_DepartmentDal.GetDepartmentList();
        }

        /// <summary>
        /// Get Department List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Department> GetDepartmentListForFilter(LinqFilter filter)
        {
            var rO_DepartmentDal = new RO_DepartmentDal();
            return rO_DepartmentDal.GetDepartmentListForFilter(filter);
        }
    }
}