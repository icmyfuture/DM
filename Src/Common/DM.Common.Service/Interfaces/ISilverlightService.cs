using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DM.Common.Service.Interfaces
{
    [ServiceContract]
    public interface ISilverlightService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/clientaccesspolicy.xml")]
        Stream GetClientAccessPolicy();
    }
}