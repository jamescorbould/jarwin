using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateNothingToSync : StateAbstract
    {
        public StateNothingToSync()
        {
            description = "Nothing to sync...";
            isRefreshRequired = false;
        }
    }
}
