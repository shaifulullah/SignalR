namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class SecurityBusinessEntitiesBll : ISecurityBusinessEntitiesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityBusinessEntities(int id, string userName)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.DeleteSecurityBusinessEntities(id, userName);
        }

        /// <summary>
        /// Get SecurityBusinessEntities For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public SecurityBusinessEntities GetSecurityBusinessEntitiesForCode(string code)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.GetSecurityBusinessEntitiesForCode(code);
        }

        /// <summary>
        /// Get SecurityBusinessEntities For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityBusinessEntities GetSecurityBusinessEntitiesForId(int id)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.GetSecurityBusinessEntitiesForId(id);
        }

        /// <summary>
        /// Get SecurityBusinessEntities List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityBusinessEntities> GetSecurityBusinessEntitiesList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.GetSecurityBusinessEntitiesList(sort, filter);
        }

        /// <summary>
        /// Get SecurityBusinessEntities Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<SecurityBusinessEntities> GetSecurityBusinessEntitiesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.GetSecurityBusinessEntitiesPage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.InsertSecurityBusinessEntities(securityBusinessEntities, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName)
        {
            var securityBusinessEntitiesDal = new SecurityBusinessEntitiesDal();
            return securityBusinessEntitiesDal.UpdateSecurityBusinessEntities(securityBusinessEntities, userName);
        }
    }
}