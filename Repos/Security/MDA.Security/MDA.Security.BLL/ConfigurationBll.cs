namespace MDA.Security.BLL
{
    using IBLL;
    using IDAL;
    using Linq;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ConfigurationBll<T> : IConfigurationBll<T>
    {
        /// <summary>
        /// Initializes a new instance of the ConfigurationBll class.
        /// </summary>
        public ConfigurationBll()
        {
            var configurationDal = string.Format("MDA.Security.DAL.{0}Dal, MDA.Security.DAL", typeof(T).Name);
            var configurationDalType = Type.GetType(configurationDal, true);

            ConfigurationDal = (IConfigurationDal<T>)Activator.CreateInstance(configurationDalType);
        }

        /// <summary>
        /// Gets or sets ConfigurationDal
        /// </summary>
        private IConfigurationDal<T> ConfigurationDal { get; set; }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteConfiguration(int id, string userName)
        {
            return ConfigurationDal.DeleteConfiguration(id, userName);
        }

        /// <summary>
        /// Get DropDown List of Configuration
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetConfigurationDropDownList()
        {
            return ConfigurationDal.GetConfigurationDropDownList();
        }

        /// <summary>
        /// Get Configuration For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public T GetConfigurationForCode(string code)
        {
            return ConfigurationDal.GetConfigurationForCode(code);
        }

        /// <summary>
        /// Get Configuration For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public T GetConfigurationForId(int id)
        {
            return ConfigurationDal.GetConfigurationForId(id);
        }

        /// <summary>
        /// Get Configuration List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<T> GetConfigurationList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            return ConfigurationDal.GetConfigurationList(sort, filter);
        }

        /// <summary>
        /// Get Configuration Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<T> GetConfigurationPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            return ConfigurationDal.GetConfigurationPage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="configuration">Configuration Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertConfiguration(T configuration, string userName)
        {
            return ConfigurationDal.InsertConfiguration(configuration, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="configuration">Configuration Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateConfiguration(T configuration, string userName)
        {
            return ConfigurationDal.UpdateConfiguration(configuration, userName);
        }
    }
}