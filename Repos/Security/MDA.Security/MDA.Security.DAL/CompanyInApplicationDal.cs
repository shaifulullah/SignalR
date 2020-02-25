namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class CompanyInApplicationDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteCompanyInApplication(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new CompanyInApplication { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get CompanyInApplication For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public CompanyInApplication GetCompanyInApplicationForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanyInApplicationSet.Include(x => x.CompanyObj).FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get CompanyInApplication List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<CompanyInApplication> GetCompanyInApplicationList(IEnumerable<Sort> sort, LinqFilter filter, int applicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanyInApplicationSet.Include(x => x.CompanyObj).Where(x => x.LnApplicationId == applicationId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get CompanyInApplication List For Application Code And User Account Id
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<CompanyInApplication> GetCompanyInApplicationListForApplicationCodeAndUserAccountId(string applicationCode, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanyInApplicationSet.Include(x => x.CompanyObj).Include(x => x.ApplicationObj).Where(x => x.ApplicationObj.Code == applicationCode)
                    .Where(x => db.UserInCompanyInApplicationSet.Where(y => y.LnUserAccountId == userAccountId).Any(y => y.CompanyInApplicationObj.LnCompanyId == x.LnCompanyId)).ToList();
            }
        }

        /// <summary>
        /// Get CompanyInApplication Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<CompanyInApplication> GetCompanyInApplicationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int applicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanyInApplicationSet.Include(x => x.CompanyObj).Where(x => x.LnApplicationId == applicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="companyInApplication">CompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertCompanyInApplication(CompanyInApplication companyInApplication, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(companyInApplication).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="companyInApplication">CompanyInApplication Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateCompanyInApplication(CompanyInApplication companyInApplication, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(companyInApplication).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}