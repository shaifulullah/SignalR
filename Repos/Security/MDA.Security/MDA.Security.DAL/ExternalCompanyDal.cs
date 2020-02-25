namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ExternalCompanyDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteExternalCompany(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new ExternalCompany { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get ExternalCompany For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public ExternalCompany GetExternalCompanyForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalCompanySet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get ExternalCompany For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ExternalCompany GetExternalCompanyForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalCompanySet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get ExternalCompany List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ExternalCompany> GetExternalCompanyList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalCompanySet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get ExternalCompany Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalCompany> GetExternalCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalCompanySet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertExternalCompany(ExternalCompany externalCompany, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(externalCompany).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateExternalCompany(ExternalCompany externalCompany, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(externalCompany).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}