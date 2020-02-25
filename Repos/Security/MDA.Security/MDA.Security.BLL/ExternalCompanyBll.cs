namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class ExternalCompanyBll : IExternalCompanyBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteExternalCompany(int id, string userName)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.DeleteExternalCompany(id, userName);
        }

        /// <summary>
        /// Get ExternalCompany For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public ExternalCompany GetExternalCompanyForCode(string code)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.GetExternalCompanyForCode(code);
        }

        /// <summary>
        /// Get ExternalCompany For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ExternalCompany GetExternalCompanyForId(int id)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.GetExternalCompanyForId(id);
        }

        /// <summary>
        /// Get ExternalCompany List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ExternalCompany> GetExternalCompanyList(IEnumerable<Sort> sort, LinqFilter filter)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.GetExternalCompanyList(sort, filter);
        }

        /// <summary>
        /// Get ExternalCompany Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalCompany> GetExternalCompanyPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.GetExternalCompanyPage(take, skip, sort, filter);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertExternalCompany(ExternalCompany externalCompany, string userName)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.InsertExternalCompany(externalCompany, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalCompany">ExternalCompany Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateExternalCompany(ExternalCompany externalCompany, string userName)
        {
            var externalCompanyDal = new ExternalCompanyDal();
            return externalCompanyDal.UpdateExternalCompany(externalCompany, userName);
        }
    }
}