using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    class Program
    {
        static void Main(string[] args)
        {

			Display display = new Display();
			display.ShowTracks(new List<Data>());

			/*
            Monitor monitor = new Monitor();
            monitor.SetX(0,10000);
            monitor.SetZ(500, 20000);
            monitor.SetY(0, 90000);

            Display display = new Display();
            Log log = new Log();

            Airspace airspace = new Airspace(monitor,display, log);

            CoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
            coordinate.Attach(airspace);

            TrackSimulator simulator = new TrackSimulator(coordinate, 40);
            simulator.StartSimulation();


            while (true) { }

*/
        }
    }
}
