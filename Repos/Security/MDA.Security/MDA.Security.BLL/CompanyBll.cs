namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class CompanyBll : ICompanyBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteCompany(int id, string userName)
        {
            var companyDal = new CompanyDal();
            return companyDal.DeleteCompany(id, userName);
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
            var companyDal = new CompanyDal();
            return companyDal.GetAvailableCompanyPageForApplication(take, skip, sort, filter, applicationId);
        }

        /// <summary>
        /// Get DropDown List of Company
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetCompanyDropDownList()
        {
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyDropDownList();
        }

        /// <summary>
        /// Get DropDown List of Company For User Account Id
        /// </summary>
        /// <param name="userAccountId">User Account Id</param>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetCompanyDropDownListForUserAccountId(int userAccountId)
        {
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyDropDownListForUserAccountId(userAccountId);
        }

        /// <summary>
        /// Get Company For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Company GetCompanyForCode(string code)
        {
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyForCode(code);
        }

        /// <summary>
        /// Get Company For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public Company GetCompanyForId(int id)
        {
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyForId(id);
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<Company> GetCompanyList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyList(sort, filter);
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
            var companyDal = new CompanyDal();
            return companyDal.GetCompanyPage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertCompany(Company company, string userName)
        {
            var companyDal = new CompanyDal();
            return companyDal.InsertCompany(company, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="company">Company Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateCompany(Company company, string userName)
        {
            var companyDal = new CompanyDal();
            return companyDal.UpdateCompany(company, userName);
        }
    }
}