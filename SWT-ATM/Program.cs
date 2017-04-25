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

			Display display = new Display();

            int i = 0;

            while(true)
            {
                Data data1 = new Data("testTag", i, i, i, "testTime");
                Data data2 = new Data("testTag", 1, i, 3, "testTime");
                Data data3 = new Data("testTag", 1, 2, i, i.ToString());

                List<Data> dataList = new List<Data>{data1, data2, data3};

                display.ShowTracks(dataList);


                // Just to simulate update behavior
                i++;
                int milliseconds = 500;
                Thread.Sleep(milliseconds);
            }
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
