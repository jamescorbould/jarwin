using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateRefreshRequired : StateAbstract
    {
        public StateRefreshRequired()
        {
            description = "Sync completed, new content downloaded...";
            isRefreshRequired = true;
        }
    }
}
