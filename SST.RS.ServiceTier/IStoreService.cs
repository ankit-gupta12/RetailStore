using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SST.RS.Common.BusinessObjects;
using System.ServiceModel.Web;


namespace SST.RS.ServiceTier
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IStoreService
    {
        [OperationContract(Name = "GetAuthorizationPermission")]
        [WebInvoke(Method ="Get",RequestFormat =WebMessageFormat.Json,ResponseFormat =WebMessageFormat.Json,UriTemplate ="",BodyStyle =WebMessageBodyStyle.Wrapped)]
        [return:MessageParameter(Name ="IsUserHavePermission")]
        int GetAuthorizationPermission(AppUsers objAppUsers);

    } 
}
