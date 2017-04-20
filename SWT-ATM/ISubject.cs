using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public interface ISubject<I, S>
    {
        void Notify(S item);
        void Attach(I item);
        void Deattach(I item);
    }
}
