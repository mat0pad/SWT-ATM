using System.Collections.Generic;
using System.Linq;

namespace SWT_ATM
{
    public class TransponderDataFormat
    {
		List<Data> FormatData(List<string> rawData)
		{
			List<Data> dataList = new List<Data>();

			foreach (string s in rawData)
			{ 
				List<string> trackList = s.Split(';').ToList<string>();
				dataList.Add(new Data (trackList[0], int.Parse(trackList[1]), int.Parse(trackList[2]), int.Parse(trackList[3]), trackList[4]));
			}

			return dataList;
		}
    }

	public class Data
	{
		public readonly string tag;
		public readonly int x_cord;
		public readonly int y_cord;
		public readonly int altitude;
		public readonly string timestamp;

		public Data(string tag, int x, int y, int alt, string time)
		{
			this.tag = tag;
			x_cord = x;
			y_cord = y;
			altitude = alt;
			timestamp = time;
		}
	}
}