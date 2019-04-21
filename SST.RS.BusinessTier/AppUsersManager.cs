using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SST.RS.Common.BusinessObjects;
using SST.RS.DataTier;

namespace SST.RS.BusinessTier
{
    public class AppUsersManager
    {
        public int GetAuthorizationPermission(AppUsers objAppUsers)
        {
            int objid;
            objid = new AppUsersData().GetAuthorizationPermission(objAppUsers);
            return objid;
        }
    }
}
