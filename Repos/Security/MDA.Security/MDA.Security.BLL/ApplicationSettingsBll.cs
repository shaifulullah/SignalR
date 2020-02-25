namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class ApplicationSettingsBll : IApplicationSettingsBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteApplicationSettings(int id, string userName)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.DeleteApplicationSettings(id, userName);
        }

        /// <summary>
        /// Get ApplicationSettings For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ApplicationSettings GetApplicationSettingsForId(int id)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsForId(id);
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
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsForKeyNameAndCompanyId(keyName, companyId, applicationCode);
        }

        /// <summary>
        /// Get ApplicationSettings For Key Name And Company In Application Id
        /// </summary>
        /// <param name="keyName">Key Name</param>
        /// <param name="companyInApplicationId">Company In Application Id</param>
        /// <returns>Business Entity</returns>
        public ApplicationSettings GetApplicationSettingsForKeyNameAndCompanyInApplicationId(string keyName, int companyInApplicationId)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsForKeyNameAndCompanyInApplicationId(keyName, companyInApplicationId);
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
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsList(sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Get ApplicationSettings List For Company Id And Application Code
        /// </summary>
        /// <param name="companyId">Company Id</param>
        /// <param name="applicationCode">Application Code</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ApplicationSettings> GetApplicationSettingsListForCompanyIdAndApplicationCode(int companyId, string applicationCode)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsListForCompanyIdAndApplicationCode(companyId, applicationCode);
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
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.GetApplicationSettingsPage(take, skip, sort, filter, companyInApplicationId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertApplicationSettings(ApplicationSettings applicationSettings, string userName)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.InsertApplicationSettings(applicationSettings, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="applicationSettings">ApplicationSettings Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateApplicationSettings(ApplicationSettings applicationSettings, string userName)
        {
            var applicationSettingsDal = new ApplicationSettingsDal();
            return applicationSettingsDal.UpdateApplicationSettings(applicationSettings, userName);
        }
    }
}