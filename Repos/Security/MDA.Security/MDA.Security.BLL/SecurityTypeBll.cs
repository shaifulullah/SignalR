namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class SecurityTypeBll : ISecurityTypeBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityType(int id, string userName)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.DeleteSecurityType(id, userName);
        }

        /// <summary>
        /// Get DropDown List of SecurityType
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetSecurityTypeDropDownList()
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.GetSecurityTypeDropDownList();
        }

        /// <summary>
        /// Get SecurityType For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public SecurityType GetSecurityTypeForCode(string code)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.GetSecurityTypeForCode(code);
        }

        /// <summary>
        /// Get SecurityType For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityType GetSecurityTypeForId(int id)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.GetSecurityTypeForId(id);
        }

        /// <summary>
        /// Get SecurityType List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityType> GetSecurityTypeList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.GetSecurityTypeList(sort, filter);
        }

        /// <summary>
        /// Get SecurityType Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityType> GetSecurityTypePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.GetSecurityTypePage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityType(SecurityType securityType, string userName)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.InsertSecurityType(securityType, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityType(SecurityType securityType, string userName)
        {
            var securityTypeDal = new SecurityTypeDal();
            return securityTypeDal.UpdateSecurityType(securityType, userName);
        }
    }
}