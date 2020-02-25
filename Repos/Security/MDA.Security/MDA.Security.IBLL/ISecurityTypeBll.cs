namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ISecurityTypeBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityType(int id, string userName);

        /// <summary>
        /// Get DropDown List of SecurityType
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetSecurityTypeDropDownList();

        /// <summary>
        /// Get SecurityType For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        SecurityType GetSecurityTypeForCode(string code);

        /// <summary>
        /// Get SecurityType For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityType GetSecurityTypeForId(int id);

        /// <summary>
        /// Get SecurityType List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityType> GetSecurityTypeList(IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Get SecurityType Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityType> GetSecurityTypePage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityType(SecurityType securityType, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityType">SecurityType Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityType(SecurityType securityType, string userName);
    }
}