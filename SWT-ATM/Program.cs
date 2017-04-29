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

            int i = 1; // Kan bruges til at opdatere position på nedenstående data
            Data data1 = new Data("timeTest1", i, i, 1, "20170830205453166");
            Data data2 = new Data("timeTest2", i, i, 1, "20170830205453166");
            Data data3 = new Data("timeTest3", i, i, 1, "20170830205453166");
            Data data4 = new Data("timeTest4", i, i, 1, "20170830205453166");

            // Notifications, fjernes efter 5 sek
            display.ShowNotification(data1, EventType.LEAVING);
            display.ShowNotification(data2, EventType.LEAVING);
            display.ShowNotification(data3, EventType.ENTERING);


            // Warnings, modtager liste indeholdende liste af dem der konflikter "lige nu", samt tilhørende liste af eventtype
            display.ShowWarning(new List<List<Data>> { new List<Data> { data2, data2, data2, data2 } }, new List<EventType>{EventType.CONFLICTING });
            Thread.Sleep(3000);
            display.ShowWarning(new List<List<Data>> { new List<Data> { data2, data2, data2, data2 }, new List<Data> { data2, data2, data2}}, new List<EventType> { EventType.CONFLICTING, EventType.CONFLICTING_ENTERING});
            Thread.Sleep(3000);
            display.ShowWarning(new List<List<Data>>(), null); // Fjerner alle warnings

            while (true) ; // Istedet for thrad join

            /*while (true)
            {
                // Simulate data
                String time = "201708302054" + string.Format("{0:00}", i) + "166";
                Data data1 = new Data("testTag", i, i, 1, time);

                // Add data to list
                List<Data> dataList = new List<Data>{data1};

                // Give list to display
                //display.ShowTracks(dataList);


                // Just to simulate update behavior
                i++;

                    //display.SetSize(150, 30);
                    int milliseconds = 200;
                    Thread.Sleep(milliseconds);

            }*/


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
