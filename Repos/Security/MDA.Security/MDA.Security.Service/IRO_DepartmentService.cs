namespace MDA.Security.Service
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IRO_DepartmentService
    {
        /// <summary>
        /// Get Department List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<RO_Department> GetDepartmentList();

        /// <summary>
        /// Get Department List For Filter
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<RO_Department> GetDepartmentListForFilter(LinqFilter filter);
    }
}