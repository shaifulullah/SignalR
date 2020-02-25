namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IUserApplicationFavouritesBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteUserApplicationFavourites(int id, string userName);

        /// <summary>
        /// Get Available UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserApplicationFavourites> GetAvailableUserApplicationFavouritesListForUserAccountId(int userAccountId, string applicationCode);

        /// <summary>
        /// Get UserApplicationFavourites For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        UserApplicationFavourites GetUserApplicationFavouritesForId(int id);

        /// <summary>
        /// Get UserApplicationFavourites List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesList(IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Get UserApplicationFavourites List For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<UserApplicationFavourites> GetUserApplicationFavouritesListForUserAccountId(int userAccountId);

        /// <summary>
        /// Get UserApplicationFavourites Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<UserApplicationFavourites> GetUserApplicationFavouritesPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int userAccountId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="userApplicationFavourites">UserApplicationFavourites Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertUserApplicationFavourites(UserApplicationFavourites userApplicationFavourites, string userName);
    }
}