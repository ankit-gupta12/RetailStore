using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SST.RS.Common.BusinessObjects;
using SSTDbContext;

namespace SST.RS.DataTier
{
    public class AppUsersData
    {
        public int GetAuthorizationPermission(AppUsers objAppUsers)
        {
            StoreDbContext connection = new StoreDbContext();
            
            return 1;
        }
    }
}
