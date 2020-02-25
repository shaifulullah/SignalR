namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserApplicationFavouritesDal
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserApplicationFavourites(int id, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(new UserApplicationFavourites { Id = id }).State = EntityState.Deleted;
                return (db.SaveChanges(userName) > 0);
            }
        }

        /// <summary>
        /// Get Available UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserApplicationFavourites> GetAvailableUserApplicationFavouritesListForUserAccountId(int userAccountId, string applicationCode)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationFavouritesSet.Include(x => x.ApplicationObj).Where(x => x.ApplicationObj.Code != applicationCode)
                    .Where(x => x.LnUserAccountId == userAccountId).OrderBy(x => x.ApplicationObj.Code).Take(5).Skip(0).ToList();
            }
        }

        /// <summary>
        /// Get UserApplicationFavourites For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserApplicationFavourites GetUserApplicationFavouritesForId(int id)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationFavouritesSet.FirstOrDefault(x => x.Id == id);
            }
        }

        /// <summary>
        /// Get UserApplicationFavourites List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationFavouritesSet.Include(x => x.ApplicationObj).Where(x => x.LnUserAccountId == userAccountId).ToListResult(sort, filter);
            }
        }

        /// <summary>
        /// Get UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesListForUserAccountId(int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationFavouritesSet.Include(x => x.ApplicationObj)
                    .Where(x => x.LnUserAccountId == userAccountId).OrderBy(x => x.ApplicationObj.Code).ToList();
            }
        }

        /// <summary>
        /// Get UserApplicationFavourites Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<UserApplicationFavourites> GetUserApplicationFavouritesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.UserApplicationFavouritesSet.Include(x => x.ApplicationObj).Where(x => x.LnUserAccountId == userAccountId).ToDataSourceResult(take, skip, sort, filter);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationFavourites">UserApplicationFavourites Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserApplicationFavourites(UserApplicationFavourites userApplicationFavourites, string userName)
        {
            using (var db = new ApplicationDBContext())
            {
                db.Entry(userApplicationFavourites).State = EntityState.Added;
                return (db.SaveChanges(userName) > 0);
            }
        }
    }
}