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
            Monitor monitor = new Monitor();
            Display display = new Display();
            Log log = new Log();

            Airspace airspace = new Airspace(monitor,display, log);

            CoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
            coordinate.Attach(airspace);

            TrackSimulator simulator = new TrackSimulator(coordinate);
            simulator.StartSimulation();


            while (true) { }


        }
    }
}
