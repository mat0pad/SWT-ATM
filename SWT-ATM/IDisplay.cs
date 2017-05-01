using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IDisplay
    {
         void Configure(int width, int height);

         void BuildFrame(int width, int height);

         void ShowTracks(List<Data> d);

         //void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop);

         void ShowNotification(Data d, EventType type);

         void ShowWarning(List<List<Data>> w);
    }
}