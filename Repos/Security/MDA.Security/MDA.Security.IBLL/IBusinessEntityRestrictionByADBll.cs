namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityRestrictionByADBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityRestrictionByAD(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityRestrictionByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityRestrictionByADDropDownList();

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForCode(string code);

        /// <summary>
        /// Get BusinessEntityRestrictionByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityRestrictionByAD GetBusinessEntityRestrictionByADForId(int id);

        /// <summary>
        /// Get BusinessEntityRestrictionByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityRestrictionByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<BusinessEntityRestrictionByAD> GetBusinessEntityRestrictionByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityRestrictionByAD">BusinessEntityRestrictionByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityRestrictionByAD(BusinessEntityRestrictionByAD businessEntityRestrictionByAD, string userName);
    }
}