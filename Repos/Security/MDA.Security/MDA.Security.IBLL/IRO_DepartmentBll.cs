namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IRO_DepartmentBll
    {
        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_Department> GetDepartmentList();

        /// <summary>
        /// Get Department List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_Department> GetDepartmentListForFilter(LinqFilter filter);
    }
}