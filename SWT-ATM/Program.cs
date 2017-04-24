using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SWT_ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            IMonitor monitor = new Monitor();
            monitor.SetX(0,10000);
            monitor.SetZ(500, 20000);
            monitor.SetY(0, 90000);

            IDisplay display = new Display();
            ILog log = new Log();

            Airspace airspace = new Airspace(monitor,display, log);

            var simulationThread = new Thread(() => SimulationThread(airspace));
            simulationThread.Start();

            while (true) { }

        }


        public static void SimulationThread(Airspace airspace)
        {
            ICoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
            coordinate.Attach(airspace);

            TrackSimulator simulator = new TrackSimulator(coordinate, 40);
            simulator.StartSimulation();
        }
    }
}
