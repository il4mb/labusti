
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;



namespace Host
{
    internal class Remote
    {

        [DllImport("paexec/PAExec.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wmain(int a, int b);



    }
}
