namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IExternalPersonBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteExternalPerson(int id, string userName);

        /// <summary>
        /// Get Available ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<ExternalPerson> GetAvailableExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated);

        /// <summary>
        /// Get ExternalPerson For Code And External Company Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Business Entity</returns>
        ExternalPerson GetExternalPersonForCodeAndExternalCompanyId(string code, int externalCompanyId);

        /// <summary>
        /// Get ExternalPerson For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        ExternalPerson GetExternalPersonForId(int id);

        /// <summary>
        /// Get ExternalPerson List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<ExternalPerson> GetExternalPersonList(IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId);

        /// <summary>
        /// Get ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<ExternalPerson> GetExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertExternalPerson(ExternalPerson externalPerson, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateExternalPerson(ExternalPerson externalPerson, string userName);
    }
}