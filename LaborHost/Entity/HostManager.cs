using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Host.Entity
{
    public partial class HostManager
    {
        private List<HostModel> stack;
        private static HostManager instance;
        private HostManager()
        {
            stack = new List<HostModel>() { new HostModel("Lab. Mobile", "") };
        }

        public static HostManager getInstance()
        {
            if (instance == null)
            {
                instance = new HostManager();
            }
            return instance;
        }

        internal void each(Action<HostModel> callback)
        {
            stack.ForEach(callback);
        }

    }
}
