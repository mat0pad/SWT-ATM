using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWT_ATM
{
    public class Display : IDisplay
    {
		public void ShowTracks(List<Data> d)
        {
			buildFrame();
			//Console.SetCursorPosition(0, 2);
			//Console.Write("test");
        }

		private void buildFrame() 
		{
			int width = 70;
			int height = 30;

			for (int i = 0; i < width; i++)
				Console.Write("-");

			Console.SetCursorPosition(0, 1);
			for (int i = 0; i < height; i++)
			{ 
				Console.WriteLine("|");
			}

			Console.SetCursorPosition(width,1);
			for (int i = 0; i < height; i++)
			{ 
				Console.WriteLine("|");
			}
		}

		private void initFrame(int height)
		{
			
		}

        public void ShowNotification(Notification n)
        {
            throw new NotImplementedException();
        }

        public void ShowWarning(List<Warning> w)
        {
            throw new NotImplementedException();
        }
    }
}
