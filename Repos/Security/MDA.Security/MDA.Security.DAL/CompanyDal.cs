namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class CompanyDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteCompany(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new Company { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get Available Company Page For Application
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="applicationId">Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Company> GetAvailableCompanyPageForApplication(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int applicationId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.Where(x => !db.CompanyInApplicationSet.Where(y => y.LnCompanyId == x.Id).Any(y => y.LnApplicationId == applicationId))
                    .Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Get DropDown List of Company
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetCompanyDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Description)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get DropDown List of Company For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetCompanyDropDownListForUserAccountId(int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.Where(x => x.Id != 0)
                    .Where(x => db.UserInCompanyInApplicationSet.Where(y => y.CompanyInApplicationObj.LnCompanyId == x.Id).Any(y => y.LnUserAccountId == userAccountId))
                    .AsEnumerable().OrderBy(x => x.Description).Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Description }).ToList();
            }
        }

        /// <summary>
        /// Get Company For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Company GetCompanyForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.FirstOrDefault(x => x.Code == code);
            }
        }

        /// <summary>
        /// Get Company For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public Company GetCompanyForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<Company> GetCompanyList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.Where(x => x.Id != 0).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get Company Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Company> GetCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.CompanySet.Where(x => x.Id != 0).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertCompany(Company company, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(company).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateCompany(Company company, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(company).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}