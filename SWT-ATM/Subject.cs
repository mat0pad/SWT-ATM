using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public abstract class Subject<T>
    {
        private List<IObserver<T>> List;

        protected Subject()
        {
            List = new List<IObserver<T>>();
        }

        public void Notify(T subject)
        {
            foreach (var item in List)
            {
               item.Update(subject);
            }
        }

        public void Attach(IObserver<T> observer)
        {
            List.Add(observer);
        }

        public void Deattach(IObserver<T> observer)
        {
            List.Remove(observer);
        }
    }
}
