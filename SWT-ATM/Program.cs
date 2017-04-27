using System;
using System.Collections.Generic;
using System.Linq;
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

			Display display = new Display();

            int i = 1;

            Data data = new Data("timeTest", i, i, 1, "20170830205453166");

            display.ShowNotification(data, EventType.ENTERING);

            while (true)
            {
                // Simulate data
                String time = "201708302054" + string.Format("{0:00}", i) + "166";
                Data data1 = new Data("testTag", i, i, 1, time);

                // Add data to list
                List<Data> dataList = new List<Data>{data1};

                // Give list to display
                display.ShowTracks(dataList);


                // Just to simulate update behavior
                i++;



                while (true)
                {
                    display.SetSize(150, 30);
                    int milliseconds = 200;
                    Thread.Sleep(milliseconds);
                }
                        

                if (i == 20)
                    break; //display.SetWidth(120);

                if (i == 100)
                    i = 0;

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
