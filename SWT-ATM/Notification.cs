namespace SWT_ATM
{
    public class Notification : Data
    {
        public Notification(string tag, int x, int y, int alt, string time) 
            : base(tag, x, y, alt, time)
        {
        }

        public bool IsEntering { get; set; }
    }
}