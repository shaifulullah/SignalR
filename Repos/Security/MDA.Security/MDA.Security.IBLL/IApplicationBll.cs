namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IApplicationBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteApplication(int id, string userName);

        /// <summary>
        /// Get DropDown List of Application
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetApplicationDropDownList(int userAccountId);

        /// <summary>
        /// Get Application For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        Application GetApplicationForCode(string code);

        /// <summary>
        /// Get Application For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        Application GetApplicationForId(int id);

        /// <summary>
        /// Get Application List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<Application> GetApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Get Application Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<Application> GetApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Get Available Application Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<Application> GetAvailableApplicationPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertApplication(Application application, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateApplication(Application application, string userName);
    }
}