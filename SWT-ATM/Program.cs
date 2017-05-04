using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TransponderReceiver;

namespace SWT_ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            Monitor monitor = new Monitor();
            monitor.SetX(0, 100000);   // 0 - 10.000
            monitor.SetZ(500, 20000); // 500 - 20.000
            monitor.SetY(0, 900000);   // 0 - 90.000
            
            IPositionCalc calc = new PositionCalc();

            IDisplay display = new Display();
            INotificationCenter notificationCenter = new NotificationCenter(display);

            IDisplayFormatter formatter = new DisplayFormatter(display, calc, notificationCenter);

            ILog log = new Log();

            Airspace airspace = new Airspace(monitor, formatter, log);

            var simThread = new Thread(() => SimulationThread(airspace));
            simThread.Start();
        }

        public static void SimulationThread(Airspace airspace)
        {
            CoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
            coordinate.Attach(airspace);

            TrackSimulator simulator = new TrackSimulator(coordinate, TransponderReceiverFactory.CreateTransponderDataReceiver());
            simulator.StartSimulation();

        }

    }
}
