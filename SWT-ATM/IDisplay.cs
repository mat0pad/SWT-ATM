using System.Collections.Generic;

namespace SWT_ATM
{
    public interface IDisplay
    {
        void ShowNormal(Data d);
        void ShowNotification(Notification n);
        void ShowWarning(List<Warning> w);
    }
}