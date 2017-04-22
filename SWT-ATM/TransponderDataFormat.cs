using System.Collections.Generic;
using System.Linq;

namespace SWT_ATM
{
	public class TransponderDataFormat : ITransponderDataFormat
    {
		public Data FormatData(string rawData)
		{
			List<string> trackList = rawData.Split(';').ToList<string>();
			return new Data(trackList[0], int.Parse(trackList[1]), int.Parse(trackList[2]), int.Parse(trackList[3]), trackList[4]);
		}
	}
}