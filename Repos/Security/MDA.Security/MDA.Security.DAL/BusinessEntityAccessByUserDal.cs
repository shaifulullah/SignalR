namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;

    public class BusinessEntityAccessByUserDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByUser(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new BusinessEntityAccessByUser { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByUser
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByUserDropDownList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByUserSet.AsEnumerable().Where(x => x.Id != 0).OrderBy(x => x.Value)
                    .Select(x => new SelectListItem { Value = x.Id.ToString(CultureInfo.InvariantCulture), Text = x.Value }).ToList();
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByUserAccounts GetBusinessEntityAccessByUserForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByUserAccountsSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser For Value And User Account Id
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByUser GetBusinessEntityAccessByUserForValueAndUserAccountId(string value, int userAccountId, int securityBusinessEntitiesId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.BusinessEntityAccessByUserSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId)
                    .Where(x => x.LnUserAccountId == userAccountId).FirstOrDefault(x => x.Value == value);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var businessEntityAccessByUserAccountsList = db.BusinessEntityAccessByUserAccountsSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).Where(x => !x.IsRecordDeleted).ToList();
                if (businessEntityAccessByUserAccountsList != null)
                {
                    businessEntityAccessByUserAccountsList = isHideTerminated ?
                        businessEntityAccessByUserAccountsList.Where(x => (x.LnEmployeeId.HasValue && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.StatusValue == 1)).ToList() :
                        businessEntityAccessByUserAccountsList.ToList();
                }

                return businessEntityAccessByUserAccountsList.ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get BusinessEntityAccessByUser Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityAccessByUserAccounts> GetBusinessEntityAccessByUserPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var businessEntityAccessByUserAccountsList = db.BusinessEntityAccessByUserAccountsSet.Where(x => x.LnSecurityBusinessEntitiesId == securityBusinessEntitiesId).Where(x => !x.IsRecordDeleted).ToList();
                if (businessEntityAccessByUserAccountsList != null)
                {
                    businessEntityAccessByUserAccountsList = isHideTerminated ?
                        businessEntityAccessByUserAccountsList.Where(x => (x.LnEmployeeId.HasValue && x.StatusValue != -10133) || (x.LnExternalPersonId.HasValue && x.StatusValue == 1)).ToList() :
                        businessEntityAccessByUserAccountsList.ToList();
                }

                return businessEntityAccessByUserAccountsList.ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByUser).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByUser">BusinessEntityAccessByUser Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByUser(BusinessEntityAccessByUser businessEntityAccessByUser, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(businessEntityAccessByUser).State = EntityState.Modified;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}