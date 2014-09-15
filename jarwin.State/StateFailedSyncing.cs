﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateFailedSyncing : StateAbstract
    {
        public StateFailedSyncing()
        {
            description = "Failed to sync...";
            isRefreshRequired = false;
        }
    }
}
