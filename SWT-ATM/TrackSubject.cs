using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    abstract class TrackSubject : ISubject
    {

        private List<int> list;

        public void notify()
        {
            throw new NotImplementedException();
        }

        public void attach()
        {
            throw new NotImplementedException();
        }

        public void deAttach()
        {
            throw new NotImplementedException();
        }
    }
}
