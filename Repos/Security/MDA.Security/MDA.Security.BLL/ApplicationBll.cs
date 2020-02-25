namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ApplicationBll : IApplicationBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteApplication(int id, string userName)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.DeleteApplication(id, userName);
        }

        /// <summary>
        /// Get DropDown List of Application
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetApplicationDropDownList(int userAccountId)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetApplicationDropDownList(userAccountId);
        }

        /// <summary>
        /// Get Application For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Application GetApplicationForCode(string code)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetApplicationForCode(code);
        }

        /// <summary>
        /// Get Application For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public Application GetApplicationForId(int id)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetApplicationForId(id);
        }

        /// <summary>
        /// Get Application List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<Application> GetApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetApplicationList(sort, filter, userAccountId);
        }

        /// <summary>
        /// Get Application Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Application> GetApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetApplicationPage(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Get Available Application Page For User Account Id
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Application> GetAvailableApplicationPageForUserAccountId(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.GetAvailableApplicationPageForUserAccountId(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertApplication(Application application, string userName)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.InsertApplication(application, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="application">Application Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateApplication(Application application, string userName)
        {
            var applicationDal = new ApplicationDal();
            return applicationDal.UpdateApplication(application, userName);
        }
    }
}