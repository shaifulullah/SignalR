namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityAccessByADBll : IBusinessEntityAccessByADBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityAccessByAD(int id, string userName)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.DeleteBusinessEntityAccessByAD(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityAccessByADDropDownList()
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.GetBusinessEntityAccessByADDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByAD GetBusinessEntityAccessByADForCode(string code)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.GetBusinessEntityAccessByADForCode(code);
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityAccessByAD GetBusinessEntityAccessByADForId(int id)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.GetBusinessEntityAccessByADForId(id);
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityAccessByAD> GetBusinessEntityAccessByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.GetBusinessEntityAccessByADList(sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityAccessByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityAccessByAD> GetBusinessEntityAccessByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.GetBusinessEntityAccessByADPage(take, skip, sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.InsertBusinessEntityAccessByAD(businessEntityAccessByAD, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName)
        {
            var businessEntityAccessByADDal = new BusinessEntityAccessByADDal();
            return businessEntityAccessByADDal.UpdateBusinessEntityAccessByAD(businessEntityAccessByAD, userName);
        }
    }
}