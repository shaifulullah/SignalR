namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class SecurityItemBll : ISecurityItemBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityItem(int id, string userName)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.DeleteSecurityItem(id, userName);
        }

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
        public DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForSecurityRolesId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityRolesId, int companyInApplicationId)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetAvailableSecurityItemPageForSecurityRolesId(take, skip, sort, filter, securityRolesId, companyInApplicationId);
        }

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
        public DataSourceResult<SecurityItem> GetAvailableSecurityItemPageForUserInCompanyInApplicationId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userInCompanyInApplicationId, int companyInApplicationId)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetAvailableSecurityItemPageForUserInCompanyInApplicationId(take, skip, sort, filter, userInCompanyInApplicationId, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityItem For Code And Company In Application Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public SecurityItem GetSecurityItemForCodeAndCompanyInApplicationId(string code, int companyInApplicationId)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetSecurityItemForCodeAndCompanyInApplicationId(code, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityItem For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityItem GetSecurityItemForId(int id)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetSecurityItemForId(id);
        }

        /// <summary>
        /// Get SecurityItem List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityItem> GetSecurityItemList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetSecurityItemList(sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Get SecurityItem Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityItem> GetSecurityItemPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.GetSecurityItemPage(take, skip, sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityItem(SecurityItem securityItem, string userName)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.InsertSecurityItem(securityItem, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityItem">SecurityItem Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityItem(SecurityItem securityItem, string userName)
        {
            var securityItemDal = new SecurityItemDal();
            return securityItemDal.UpdateSecurityItem(securityItem, userName);
        }
    }
}