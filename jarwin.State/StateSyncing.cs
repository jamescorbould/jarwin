﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarwin.State
{
    public class StateSyncing : StateAbstract
    {
        public StateSyncing()
        {
            description = "Syncing...";
            isRefreshRequired = false;
        }
    }
}
