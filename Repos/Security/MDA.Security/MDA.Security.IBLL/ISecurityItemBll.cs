namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ISecurityItemBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityItem(int id, string userName);

        /// <summary>
        /// Get Available SecurityItem Page For Security Roles Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityRolesId">Security Roles Id</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForSecurityRolesId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, int companyInApplicationId);

        /// <summary>
        /// Get Available SecurityItem Page For User In Company In Application Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userInCompanyInApplicationId">User In Company In Application Id</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId, int companyInApplicationId);

        /// <summary>
        /// Get SecurityItem For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        SecurityItem GetSecurityItemForCodeAndCompanyInApplicationId(string code, int companyInApplicationId);

        /// <summary>
        /// Get SecurityItem For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityItem GetSecurityItemForId(int id);

        /// <summary>
        /// Get SecurityItem List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityItem> GetSecurityItemList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Get SecurityItem Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityItem> GetSecurityItemPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityItem(SecurityItem securityItem, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityItem(SecurityItem securityItem, string userName);
    }
}