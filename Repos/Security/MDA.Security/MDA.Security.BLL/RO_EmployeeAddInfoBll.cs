namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_EmployeeAddInfoBll : IRO_EmployeeAddInfoBll
    {
        /// <summary>
        /// Get EmployeeAddInfo List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_EmployeeAddInfo> GetEmployeeAddInfoListForFilter(LinqFilter filter)
        {
            var rO_EmployeeAddInfoDal = new RO_EmployeeAddInfoDal();
            return rO_EmployeeAddInfoDal.GetEmployeeAddInfoListForFilter(filter);
        }
    }
}