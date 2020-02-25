namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ExternalPersonDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteExternalPerson(int id, string userName)
        {
            var externalPerson = GetExternalPersonForId(id);
            using (var db = new ApplicationDBContext())
            {
                if (externalPerson.IsActive)
                {
                    externalPerson.IsActive = false;

                    db.Entry(externalPerson).State = EntityState.Modified;
                    return (db.SaveChanges(userName) > 0);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Get Available ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalPerson> GetAvailableExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var externalPersonList = db.ExternalPersonSet.Where(x => x.Id != 0)
                    .Where(x => !db.UserAccountSet.Where(y => y.LnExternalPersonId != 0).Where(y => y.LnEmployeeId.HasValue)
                        .Any(y => y.LnExternalPersonId.Value == x.Id)).ToList();

                if (externalPersonList != null)
                {
                    externalPersonList = isHideTerminated ? externalPersonList.Where(x => x.IsActive).ToList() : externalPersonList.ToList();
                }

                return externalPersonList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get ExternalPerson For Code And External Company Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Business Entity</returns>
        public ExternalPerson GetExternalPersonForCodeAndExternalCompanyId(string code, int externalCompanyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalPersonSet.Where(x => x.LnExternalCompanyId == externalCompanyId).FirstOrDefault(x => x.PersonCode == code);
            }
        }

        /// <summary>
        /// Get ExternalPerson For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ExternalPerson GetExternalPersonForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalPersonSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get ExternalPerson List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ExternalPerson> GetExternalPersonList(IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalPersonSet.Where(x => x.LnExternalCompanyId == externalCompanyId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalPerson> GetExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ExternalPersonSet.Where(x => x.LnExternalCompanyId == externalCompanyId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertExternalPerson(ExternalPerson externalPerson, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(externalPerson).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateExternalPerson(ExternalPerson externalPerson, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(externalPerson).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}