using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public class Warning : Data
    {
        public Warning(string tag, int x, int y, int alt, string time) 
            : base(tag, x, y, alt, time)
        {
        }

        List<Data> WhoConflicting()
        {
            throw new NotImplementedException();
        }
    }
}