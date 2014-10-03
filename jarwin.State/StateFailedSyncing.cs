using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateFailedSyncing : StateAbstract
    {
        public StateFailedSyncing(bool isRefreshRequiredIn)
        {
            description = "Failed to sync...";
            isRefreshRequired = isRefreshRequiredIn;
        }
    }
}
