namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ICompanyInApplicationBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteCompanyInApplication(int id, string userName);

        /// <summary>
        /// Get CompanyInApplication For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        CompanyInApplication GetCompanyInApplicationForId(int id);

        /// <summary>
        /// Get CompanyInApplication List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<CompanyInApplication> GetCompanyInApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int applicationId);

        /// <summary>
        /// Get CompanyInApplication List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<CompanyInApplication> GetCompanyInApplicationListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId);

        /// <summary>
        /// Get CompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<CompanyInApplication> GetCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int applicationId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="companyInApplication">CompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertCompanyInApplication(CompanyInApplication companyInApplication, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="companyInApplication">CompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateCompanyInApplication(CompanyInApplication companyInApplication, string userName);
    }
}