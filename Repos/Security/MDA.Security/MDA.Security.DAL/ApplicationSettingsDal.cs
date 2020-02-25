namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class ApplicationSettingsDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteApplicationSettings(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new ApplicationSettings { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get ApplicationSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ApplicationSettings GetApplicationSettingsForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get ApplicationSettings For Key Name And Company Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        public ApplicationSettings GetApplicationSettingsForKeyNameAndCompanyId(string keyName, int companyId, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.Include(x => x.CompanyInApplicationObj.ApplicationObj).Where(x => x.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.CompanyInApplicationObj.ApplicationObj.Code == applicationCode).FirstOrDefault(x => x.KeyName.ToUpper() == keyName);
            }
        }

        /// <summary>
        /// Get ApplicationSettings For Key Name And Company In Application Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public ApplicationSettings GetApplicationSettingsForKeyNameAndCompanyInApplicationId(string keyName, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).FirstOrDefault(x => x.KeyName == keyName);
            }
        }

        /// <summary>
        /// Get ApplicationSettings List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ApplicationSettings> GetApplicationSettingsList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get ApplicationSettings List For Company Id And Application Code
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ApplicationSettings> GetApplicationSettingsListForCompanyIdAndApplicationCode(int companyId, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.Include(x => x.CompanyInApplicationObj.ApplicationObj).Where(x => x.CompanyInApplicationObj.LnCompanyId == companyId)
                    .Where(x => x.CompanyInApplicationObj.ApplicationObj.Code == applicationCode).ToList();
            }
        }

        /// <summary>
        /// Get ApplicationSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ApplicationSettings> GetApplicationSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ApplicationSettingsSet.Where(x => x.LnCompanyInApplicationId == companyInApplicationId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertApplicationSettings(ApplicationSettings applicationSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(applicationSettings).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateApplicationSettings(ApplicationSettings applicationSettings, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(applicationSettings).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}