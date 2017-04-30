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
            /*
            List<String> list = new List<string>();
            list.Add("dd");
            list.Add("dd");
            list.Add("DD");

            for (int i = list.Count - 1; i >= 0; i--)
            {
                Console.WriteLine(i);
            }
            */









            /*IDisplay display = new Display();
            IMonitor monitor = new Monitor();

            int i = 1; // Kan bruges til at opdatere position på nedenstående data
            Data data1 = new Data("timeTest1", i, i, 1, "20170830205453166");
            Data data2 = new Data("timeTest2", i, i, 1, "20170830205453166");
            Data data3 = new Data("timeTest3", i, i, 1, "20170830205453166");
            Data data4 = new Data("timeTest4", i, i, 1, "20170830205453166");

            // Brug af Notifications, fjernes efter 5 sek

            
           

            Thread t = new Thread(() =>
            {
                for (int j = 0; j < 80; j++)
                {
                    display.ShowNotification(data1, EventType.LEAVING);
                    Thread.Sleep(10);
                }
            });

            t.Start();



            for (int j = 0; j < 80; j++)
            {
                display.ShowNotification(data2, EventType.LEAVING);
                Thread.Sleep(j + 20);
            }


            /*
       // Brug af Warnings, modtager liste indeholdende liste af dem der konflikter "lige nu"
       data1 = new Data("1", 0, 0, 500, "");
       data2 = new Data("2", 4999, 4999, 799, "");
       data3 = new Data("3", 5000, 5000, 800, "");

      var list = new List<Data> { data1, };
      monitor.SetShareList(ref list);
      var testConflicts = monitor.GetAllConflicts();


      //display.ShowTracks(list);
      //display.ShowTracks(list);


    display.ShowWarning(testConflicts);
    Thread.Sleep(3000);
    display.ShowWarning(new List<List<Data>> { new List<Data> { data2, data2, data2, data2 }, new List<Data> { data2, data2, data2 } }); // Viser nye warnings
    Thread.Sleep(3000);
    display.ShowWarning(new List<List<Data>>()); // Fjerner alle warnings
    */


             Monitor monitor = new Monitor();
             monitor.SetX(0, 10000);
             monitor.SetZ(500, 20000);
             monitor.SetY(0, 90000);

             IDisplay display = new Display();

             ILog log = new Log();

             Airspace airspace = new Airspace(monitor, display, log);

             CoordinateMapper coordinate = new CoordinateMapper(new TransponderDataFormat());
             coordinate.Attach(airspace);

             TrackSimulator simulator = new TrackSimulator(coordinate, 200);
             simulator.StartSimulation();//*/


        }
    }
}
