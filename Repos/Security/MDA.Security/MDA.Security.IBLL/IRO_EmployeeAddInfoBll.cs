namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IRO_EmployeeAddInfoBll
    {
        /// <summary>
        /// Get EmployeeAddInfo List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_EmployeeAddInfo> GetEmployeeAddInfoListForFilter(LinqFilter filter);
    }
}