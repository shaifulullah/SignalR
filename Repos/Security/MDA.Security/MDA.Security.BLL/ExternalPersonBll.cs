namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class ExternalPersonBll : IExternalPersonBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteExternalPerson(int id, string userName)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.DeleteExternalPerson(id, userName);
        }

        /// <summary>
        /// Get Available ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalPerson> GetAvailableExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, bool isHideTerminated)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.GetAvailableExternalPersonPage(take, skip, sort, filter, isHideTerminated);
        }

        /// <summary>
        /// Get ExternalPerson For Code And External Company Id
        /// </summary>
        /// <param name="code">Code</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Business Entity</returns>
        public ExternalPerson GetExternalPersonForCodeAndExternalCompanyId(string code, int externalCompanyId)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.GetExternalPersonForCodeAndExternalCompanyId(code, externalCompanyId);
        }

        /// <summary>
        /// Get ExternalPerson For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public ExternalPerson GetExternalPersonForId(int id)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.GetExternalPersonForId(id);
        }

        /// <summary>
        /// Get ExternalPerson List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<ExternalPerson> GetExternalPersonList(IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.GetExternalPersonList(sort, filter, externalCompanyId);
        }

        /// <summary>
        /// Get ExternalPerson Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="externalCompanyId">External Company Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<ExternalPerson> GetExternalPersonPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int externalCompanyId)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.GetExternalPersonPage(take, skip, sort, filter, externalCompanyId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertExternalPerson(ExternalPerson externalPerson, string userName)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.InsertExternalPerson(externalPerson, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="externalPerson">ExternalPerson Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateExternalPerson(ExternalPerson externalPerson, string userName)
        {
            var externalPersonDal = new ExternalPersonDal();
            return externalPersonDal.UpdateExternalPerson(externalPerson, userName);
        }
    }
}