using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace SWT_ATM
{
    public class TrackSimulator
    {
        private List<ITransponderReceiver> _list;
        private static ICoordinateMapper _mapper;

        public TrackSimulator(ICoordinateMapper mapper, int numOfPlanes)
        {
            _mapper = mapper; 

            // Create transponder receiver
            _list = new List<ITransponderReceiver>();

           for (int i = 0; i < numOfPlanes+1; i++)
                _list.Add(TransponderReceiverFactory.CreateTransponderDataReceiver());
        }

        public void StartSimulation()
        {
            foreach (var item in _list)
                 item.TransponderDataReady += OnDataReceieved;  
        }

        public static void OnDataReceieved(object sender, RawTransponderDataEventArgs e)
        {
            if (e.TransponderData.Count > 1)
                _mapper.MapTrack(e.TransponderData[0]);
        }

    }
}
