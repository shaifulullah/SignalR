namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface ISecurityBusinessEntitiesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteSecurityBusinessEntities(int id, string userName);

        /// <summary>
        /// Get SecurityBusinessEntities For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        SecurityBusinessEntities GetSecurityBusinessEntitiesForCode(string code);

        /// <summary>
        /// Get SecurityBusinessEntities For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        SecurityBusinessEntities GetSecurityBusinessEntitiesForId(int id);

        /// <summary>
        /// Get SecurityBusinessEntities List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<SecurityBusinessEntities> GetSecurityBusinessEntitiesList(IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Get SecurityBusinessEntities Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<SecurityBusinessEntities> GetSecurityBusinessEntitiesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName);
    }
}