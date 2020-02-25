namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class SecurityBusinessEntitiesDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteSecurityBusinessEntities(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new SecurityBusinessEntities { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get SecurityBusinessEntities For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public SecurityBusinessEntities GetSecurityBusinessEntitiesForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityBusinessEntitiesSet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get SecurityBusinessEntities For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public SecurityBusinessEntities GetSecurityBusinessEntitiesForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityBusinessEntitiesSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get SecurityBusinessEntities List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<SecurityBusinessEntities> GetSecurityBusinessEntitiesList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityBusinessEntitiesSet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.SecurityBusinessEntitiesSet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityBusinessEntities).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="securityBusinessEntities">SecurityBusinessEntities Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateSecurityBusinessEntities(SecurityBusinessEntities securityBusinessEntities, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(securityBusinessEntities).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}