namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IBusinessEntityAccessByADBll
    {
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool DeleteBusinessEntityAccessByAD(int id, string userName);

        /// <summary>
        /// Get DropDown List of BusinessEntityAccessByAD
        /// </summary>
        /// <returns>DropDown List</returns>
        IEnumerable<SelectListItem> GetBusinessEntityAccessByADDropDownList();

        /// <summary>
        /// Get BusinessEntityAccessByAD For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByAD GetBusinessEntityAccessByADForCode(string code);

        /// <summary>
        /// Get BusinessEntityAccessByAD For Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Business Entity</returns>
        BusinessEntityAccessByAD GetBusinessEntityAccessByADForId(int id);

        /// <summary>
        /// Get BusinessEntityAccessByAD List
        /// </summary>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<BusinessEntityAccessByAD> GetBusinessEntityAccessByADList(IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Get BusinessEntityAccessByAD Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <param name="securityBusinessEntitiesId">Security Business Entities Id</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<BusinessEntityAccessByAD> GetBusinessEntityAccessByADPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter, int securityBusinessEntitiesId);

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool InsertBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName);

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="businessEntityAccessByAD">BusinessEntityAccessByAD Object</param>
        /// <param name="userName">Username of the Person Currently Logged onto the System</param>
        /// <returns>True on Success, False on Failure</returns>
        bool UpdateBusinessEntityAccessByAD(BusinessEntityAccessByAD businessEntityAccessByAD, string userName);
    }
}