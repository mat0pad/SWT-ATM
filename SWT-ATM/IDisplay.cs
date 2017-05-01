using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IDisplay
    {
         void Configure(int width, int height);

         void BuildFrame(int width, int height);

         //void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop);

         void ShowNotification(List<string> list);

         void ShowWarning(List<List<string>> w);

        void ShowTracks(List<Data> d);
    }
}