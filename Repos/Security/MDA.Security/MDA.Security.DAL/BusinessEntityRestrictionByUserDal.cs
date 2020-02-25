namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByUserDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByUser(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityRestrictionByUser { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByUserDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByUserSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByUserAccounts GetBusinessEntityRestrictionByUserForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByUserAccountsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByUser GetBusinessEntityRestrictionByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityRestrictionByUserSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).Where(x => x.LnUserAccountId == userAccountId).FirstOrDefault(x => x.Value == value);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var businessEntityRestrictionByUserAccountsList = db.BusinessEntityRestrictionByUserAccountsSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).Where(x => !x.IsRecordDeleted).ToList();
                if (businessEntityRestrictionByUserAccountsList != null)
                {
                    businessEntityRestrictionByUserAccountsList = isHideTerminated ?
                        businessEntityRestrictionByUserAccountsList.Where(x => (x.LnEmployeeId.HasValue && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.StatusValue == 1)).ToList() :
                        businessEntityRestrictionByUserAccountsList.ToList();
                }

                return businessEntityRestrictionByUserAccountsList.ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityRestrictionByUserAccounts> GetBusinessEntityRestrictionByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var businessEntityRestrictionByUserAccountsList = db.BusinessEntityRestrictionByUserAccountsSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).Where(x => !x.IsRecordDeleted).ToList();
                if (businessEntityRestrictionByUserAccountsList != null)
                {
                    businessEntityRestrictionByUserAccountsList = isHideTerminated ?
                        businessEntityRestrictionByUserAccountsList.Where(x => (x.LnEmployeeId.HasValue && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.StatusValue == 1)).ToList() :
                        businessEntityRestrictionByUserAccountsList.ToList();
                }

                return businessEntityRestrictionByUserAccountsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByUser).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByUser">BusinessEntityRestrictionByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByUser(BusinessEntityRestrictionByUser businessEntityRestrictionByUser, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityRestrictionByUser).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}