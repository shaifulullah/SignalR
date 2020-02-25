namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ICompanyBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteCompany(int id, string userName);

        /// <summary>
        /// Get Available Company Page For Application
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<Company> GetAvailableCompanyPageForApplication(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int applicationId);

        /// <summary>
        /// Get DropDown List of Company
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetCompanyDropDownList();

        /// <summary>
        /// Get DropDown List of Company For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetCompanyDropDownListForUserAccountId(int userAccountId);

        /// <summary>
        /// Get Company For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        Company GetCompanyForCode(string code);

        /// <summary>
        /// Get Company For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        Company GetCompanyForId(int id);

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<Company> GetCompanyList(IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Get Company Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<Company> GetCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertCompany(Company company, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateCompany(Company company, string userName);
    }
}