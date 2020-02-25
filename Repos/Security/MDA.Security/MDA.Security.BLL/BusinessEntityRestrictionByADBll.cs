namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByADBll : IBusinessEntityRestrictionByADBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool DeleteBusinessEntityRestrictionByAD(int id, string userName)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.DeleteBusinessEntityRestrictionByAD(id, userName);
        }

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        public IEnumerable<SelectListItem> GetBusinessEntityRestrictionByADDropDownList()
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.GetBusinessEntityRestrictionByADDropDownList();
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForCode(string code)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.GetBusinessEntityRestrictionByADForCode(code);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        public BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForId(int id)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.GetBusinessEntityRestrictionByADForId(id);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.GetBusinessEntityRestrictionByADList(sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Get BusinessEntityRestrictionByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.GetBusinessEntityRestrictionByADPage(take, skip, sort, filter, securityBusinessEntitiesId);
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool InsertBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.InsertBusinessEntityRestrictionByAD(businessEntityRestrictionByAD, userName);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        public bool UpdateBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName)
        {
            var businessEntityRestrictionByADDal = new BusinessEntityRestrictionByADDal();
            return businessEntityRestrictionByADDal.UpdateBusinessEntityRestrictionByAD(businessEntityRestrictionByAD, userName);
        }
    }
}