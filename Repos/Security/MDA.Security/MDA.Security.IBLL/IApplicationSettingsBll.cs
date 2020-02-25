namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IApplicationSettingsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteApplicationSettings(int id, string userName);

        /// <summary>
        /// Get ApplicationSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        ApplicationSettings GetApplicationSettingsForId(int id);

        /// <summary>
        /// Get ApplicationSettings For Key Name And Company Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Business Entity</returns>
        ApplicationSettings GetApplicationSettingsForKeyNameAndCompanyId(string keyName, int companyId, string applicationCode);

        /// <summary>
        /// Get ApplicationSettings For Key Name And Company In Application Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        ApplicationSettings GetApplicationSettingsForKeyNameAndCompanyInApplicationId(string keyName, int companyInApplicationId);

        /// <summary>
        /// Get ApplicationSettings List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<ApplicationSettings> GetApplicationSettingsList(IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Get ApplicationSettings List For Company Id And Application Code
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<ApplicationSettings> GetApplicationSettingsListForCompanyIdAndApplicationCode(int companyId, string applicationCode);

        /// <summary>
        /// Get ApplicationSettings Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<ApplicationSettings> GetApplicationSettingsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int companyInApplicationId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertApplicationSettings(ApplicationSettings applicationSettings, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateApplicationSettings(ApplicationSettings applicationSettings, string userName);
    }
}