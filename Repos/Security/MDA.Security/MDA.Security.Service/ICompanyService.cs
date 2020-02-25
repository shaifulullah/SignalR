namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.Web.Mvc;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface ICompanyService
    {
        /// <summary>
        /// Get DropDown List of Company
        /// </summary>
        /// <returns>DropDown List</returns>
        [OperationContract]
        IEnumerable<SelectListItem> GetCompanyDropDownList();

        /// <summary>
        /// Get Company For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        [OperationContract]
        Company GetCompanyForCode(string code);
    }
}