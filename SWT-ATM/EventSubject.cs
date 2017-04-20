using System;
using System.Collections.Generic;

namespace SWT_ATM
{
    public abstract class EventSubject : ISubject<IEventObserver, EventSubject>
    {
        private List<IEventObserver> list;

        protected EventSubject()
        {
            list = new List<IEventObserver>();
        }

        public void Attach(IEventObserver item)
        {
            list.Add(item);
        }

        public void Deattach(IEventObserver item)
        {
            list.Remove(item);
        }

        public void Notify(EventSubject subject)
        {
            foreach (var item in list)
            {
                item.Update(subject);
            }
        }
    }
}