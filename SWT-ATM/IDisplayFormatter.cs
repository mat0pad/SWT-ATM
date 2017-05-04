using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IDisplayFormatter
    {
        void SetSize(int width, int height);
        void SetWidth(int width);
        void SetHeight(int height);
        int GetHeight();

        void ShowTracks(List<Data> d);
        void ShowNotification(Data d, EventType? type);
        void ShowWarning(List<List<Data>> w);
    }
}