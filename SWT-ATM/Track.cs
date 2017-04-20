using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace SWT_ATM
{
    public class Track : Subject<Track>, ITransponderReceiver
    {
        //public ITransponderReceiver Receiver { get; set; }

        public event EventHandler<RawTransponderDataEventArgs> TransponderDataReady;

        public Track()
        {
            // Create transponder receiver
            //Receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            TransponderDataReady += DataReady;

        }

        static void DataReady(object sender, EventArgs e)
        {

            Console.WriteLine("The threshold was reached.");
        }

    }
}
