using System.ServiceModel;

namespace DM.Common.Service.Interfaces
{
    [ServiceContract]
    public interface ICommandService
    {
        [OperationContract]
        string Invoke(string request);
    }
}