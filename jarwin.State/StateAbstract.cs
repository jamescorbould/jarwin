using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public abstract class StateAbstract
    {
        public string description { get; set; }
        public bool isRefreshRequired { get; set; }
    }
}
