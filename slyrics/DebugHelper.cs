using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slyrics
{
    class DebugHelper
    {
        public static bool InDebug()
        {
            return System.Diagnostics.Debugger.IsAttached;
        }
    }
}
