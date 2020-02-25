namespace MDA.Security.Service
{
    using Models;
    using System.Collections.Generic;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IRO_SkillsService
    {
        /// <summary>
        /// Get Skills List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        [OperationContract]
        IEnumerable<RO_Skills> GetSkillsList();
    }
}