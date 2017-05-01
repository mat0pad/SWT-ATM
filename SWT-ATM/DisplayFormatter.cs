using System.Collections.Generic;
using System.Linq;

namespace SWT_ATM
{
    public class DisplayFormatter : IDisplayFormatter
    {
        private IDisplay _display;

        public static int Height { get; private set; }
        public static int Width { get; private set; }

        public DisplayFormatter(IDisplay display, int width = 150, int height = 50)
        {
            _display = display;

            Width = width;
            Height = height;

            _display.BuildFrame(width, height);
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            _display.BuildFrame(Width, Height);
        }

        public void SetWidth(int width)
        {
            Width = width;
            _display.BuildFrame(Width, Height);
        }

        public void SetHeight(int height)
        {
            Height = height;
            _display.BuildFrame(Width, Height); 
        }

        public void ShowTracks(List<Data> d)
        {
            _display.ShowTracks(d);
        }

        public void ShowNotification(Data d, EventType type)
        {
            _display.ShowNotification(new List<string> { d.Tag, type.ToString() });
        }

        public void ShowWarning(List<List<Data>> w)
        {
            var warningsList = new List<List<string>>();

            foreach (List<Data> wList in w)
            {
                var tmpList = new List<string>(wList.Select(data => data.Tag));
                warningsList.Add(tmpList);
            }

            _display.ShowWarning(warningsList);
        }
    }
}