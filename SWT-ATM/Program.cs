using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SWT_ATM
{
    class Program
    {
        static void Main(string[] args)
        {

            Monitor monitor = new Monitor();
             monitor.SetX(0, 10000);
             monitor.SetZ(500, 20000);
             monitor.SetY(0, 90000);

             IDisplay display = new Display();

             ILog log = new Log();

             Airspace airspace = new Airspace(monitor, display, log);

            var simThread = new Thread(() => SimulationThread(airspace));
            simThread.Start();
        }

        public static void SimulationThread(Airspace airspace)
        {
            CoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
            coordinate.Attach(airspace);

            TrackSimulator simulator = new TrackSimulator(coordinate, 200);
            simulator.StartSimulation();

        }

    }
}
