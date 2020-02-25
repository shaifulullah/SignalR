namespace MDA.Security.IDAL
{
    using Linq;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IConfigurationDal<T>
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteConfiguration(int id, string userName);

        /// <summary>
        /// Get DropDown List of Configuration
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetConfigurationDropDownList();

        /// <summary>
        /// Get Configuration For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        T GetConfigurationForCode(string code);

        /// <summary>
        /// Get Configuration For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        T GetConfigurationForId(int id);

        /// <summary>
        /// Get Configuration List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<T> GetConfigurationList(IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Get Configuration Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<T> GetConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="configuration">Configuration Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertConfiguration(T configuration, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="configuration">Configuration Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateConfiguration(T configuration, string userName);
    }
}