namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class UserApplicationFavouritesBll : IUserApplicationFavouritesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteUserApplicationFavourites(int id, string userName)
        {
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.DeleteUserApplicationFavourites(id, userName);
        }

        /// <summary>
        /// Get Available UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserApplicationFavourites> GetAvailableUserApplicationFavouritesListForUserAccountId(int userAccountId, string applicationCode)
        {
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.GetAvailableUserApplicationFavouritesListForUserAccountId(userAccountId, applicationCode);
        }

        /// <summary>
        /// Get UserApplicationFavourites For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public UserApplicationFavourites GetUserApplicationFavouritesForId(int id)
        {
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.GetUserApplicationFavouritesForId(id);
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
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.GetUserApplicationFavouritesList(sort, filter, userAccountId);
        }

        /// <summary>
        /// Get UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesListForUserAccountId(int userAccountId)
        {
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.GetUserApplicationFavouritesListForUserAccountId(userAccountId);
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
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.GetUserApplicationFavouritesPage(take, skip, sort, filter, userAccountId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationFavourites">UserApplicationFavourites Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertUserApplicationFavourites(UserApplicationFavourites userApplicationFavourites, string userName)
        {
            var userApplicationFavouritesDal = new UserApplicationFavouritesDal();
            return userApplicationFavouritesDal.InsertUserApplicationFavourites(userApplicationFavourites, userName);
        }
    }
}