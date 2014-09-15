using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateNormal : StateAbstract
    {
        public StateNormal()
        {
            description = String.Empty;
            isRefreshRequired = false;
        }
    }
}
