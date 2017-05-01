using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IDisplay
    {
        void Configure(int width, int height);

        void BuildFrame(int width, int height);

        void WriteRow(IEnumerable<string> toWrite, int seperation, int startLeft, int startTop);

        void ShowTracks(List<IEnumerable<string>> d);

        int GetRowSeperation();
    }
}