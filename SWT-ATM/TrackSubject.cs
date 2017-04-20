using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public abstract class TrackSubject : ISubject<ITrackObserver,TrackSubject>
    {

        private List<ITrackObserver> list;

        protected TrackSubject()
        {
            list = new List<ITrackObserver>();
        }

        public void Notify(TrackSubject subject)
        {
            foreach (var item in list)
            {
               item.Update(subject);
            }
        }

        public void Attach(ITrackObserver track)
        {
            list.Add(track);
        }

        public void Deattach(ITrackObserver track)
        {
            list.Remove(track);
        }
    }
}
