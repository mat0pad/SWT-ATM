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
        private ITransponderReceiver _transponderReceiver;
        private static ICoordinateMapper _mapper;

        public TrackSimulator(ICoordinateMapper mapper, ITransponderReceiver receiver)
        {
            _mapper = mapper;

            // Create transponder receiver
            _transponderReceiver = receiver;

            //TransponderReceiverFactory.CreateTransponderDataReceiver();
        }


        public void StartSimulation()
        {
            _transponderReceiver.TransponderDataReady += OnDataReceieved;  
        }

        public void OnDataReceieved(object sender, RawTransponderDataEventArgs e)
        {
            if (e.TransponderData.Count > 0)
            {
               /* foreach (var item in e.TransponderData)
                {
                    Console.WriteLine(item);
                }
                */
                
                _mapper.MapTrack(e.TransponderData);

            
            }
        }

    }
}
