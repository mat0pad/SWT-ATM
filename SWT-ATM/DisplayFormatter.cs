using System.Collections.Generic;
using System.Linq;

namespace SWT_ATM
{
    public class DisplayFormatter : IDisplayFormatter
    {
        private IDisplay _display;
        private IPositionCalc _calc;

        public static int Height { get; private set; }
        public static int Width { get; private set; }
       
        private List<Data> _prevList = new List<Data>();

        private readonly INotificationCenter _notificationCenter;

        public DisplayFormatter(IDisplay display, IPositionCalc calc, INotificationCenter notificationCenter, int width = 150, int height = 50)
        {
            _calc = calc;
            _notificationCenter = notificationCenter;

            Width = width;
            Height = height;
            _display = display;

            _display.BuildFrame(width, height);
        }

        // For testing
        public int GetHeight()
        {
            return Height;
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
            List<IEnumerable<string>> formattedTracks = new List<IEnumerable<string>>();

            var i = 2;
            IEnumerable<string> trackInfo;
            foreach (var track in d)
            {
                Data oldData = null;

                if (_prevList != null)
                    oldData = _prevList.FirstOrDefault(prevData => prevData.Tag == track.Tag);

                if (oldData != null && track.XCord != oldData.XCord && track.YCord != oldData.YCord)
                {
                    trackInfo = _calc.FormatTrackData(track, oldData);
                }
                else
                {
                    trackInfo = _calc.FormatTrackData(track, new Data("", 0, 0, 0, "0000000000000000"));
                }

                formattedTracks.Add(trackInfo);
            }


                lock (_prevList)
                    _prevList = d;

            _display.ShowTracks(formattedTracks);
        }

        public void ShowNotification(Data d, EventType? type)
        {
            if (d == null) return;

            var item = new List<string> { d.Tag, type.ToString() };

            _notificationCenter.EnqueNotification(item);
            _notificationCenter.SetNotificationSignalHandle();
        }

        public void ShowWarning(List<List<Data>> w)
        {
            var warningsList = new List<List<string>>();

            foreach (List<Data> wList in w)
            {
                var tmpList = new List<string>(wList.Select(data => data.Tag));
                warningsList.Add(tmpList);
            }

            _notificationCenter.EnqueWarning(warningsList);
            _notificationCenter.SetWarningsSignalHandle();
        }
    }
}