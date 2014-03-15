using System.IO;
using System.ServiceModel.Web;
using System.Text;
using DM.Common.Service.Interfaces;

namespace DM.Common.Service.Services
{
    public class SilverlightService : ISilverlightService
    {
        public Stream GetClientAccessPolicy()
        {
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Content-Type", "text/xml;charset=utf-8");
            }

            const string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
                                 <access-policy>
                                    <cross-domain-access>
                                    <policy>
                                        <allow-from http-request-headers=""*"">
                                        <domain uri=""*""/>
                                        </allow-from>
                                        <grant-to>
                                        <resource path=""/"" include-subpaths=""true""/>
                                        </grant-to>
                                    </policy>
                                    </cross-domain-access>
                                </access-policy>";
            var array = Encoding.Default.GetBytes(xml);
            return new MemoryStream(array);
        }
    }
}