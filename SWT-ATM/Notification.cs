namespace SWT_ATM
{
    public class Notification : Data
    {
        public Notification(Data d, bool isEntering) 
            : base(d.Tag, d.XCord, d.YCord, d.Altitude, d.Timestamp)
        {
            IsEntering = isEntering;
        }

        public bool IsEntering { get; set; }
    }
}