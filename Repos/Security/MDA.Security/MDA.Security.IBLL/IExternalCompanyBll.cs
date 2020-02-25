namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IExternalCompanyBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteExternalCompany(int id, string userName);

        /// <summary>
        /// Get ExternalCompany For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        ExternalCompany GetExternalCompanyForCode(string code);

        /// <summary>
        /// Get ExternalCompany For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        ExternalCompany GetExternalCompanyForId(int id);

        /// <summary>
        /// Get ExternalCompany List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<ExternalCompany> GetExternalCompanyList(IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Get ExternalCompany Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<ExternalCompany> GetExternalCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertExternalCompany(ExternalCompany externalCompany, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateExternalCompany(ExternalCompany externalCompany, string userName);
    }
}