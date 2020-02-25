namespace MDA.Security.Service
{
    using Models;
    using System.ServiceModel;

    [ServiceContract(Namespace = "MDA.Security.Service")]
    public interface IServiceDetails
    {
        /// <summary>
        /// Get ServiceDetails
        /// </summary>
        /// <returns>Business Entity</returns>
        [OperationContract]
        ServiceDetails GetServiceDetails();
    }
}