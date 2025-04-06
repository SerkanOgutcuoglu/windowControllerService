using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowControlService
{
    internal class WindowCommands
    {
        public const int SW_MINIMIZE = 6;
        public const int SW_MAXIMIZE = 3;
        public const int SW_RESTORE = 9;
        public const uint WM_CLOSE = 0x0010;
    }
}
