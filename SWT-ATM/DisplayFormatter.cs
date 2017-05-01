namespace SWT_ATM
{
    public class DisplayFormatter : IDisplayFormatter
    {
        private IDisplay _display;

        public static int Height { get; private set; }

        private int _width;

        DisplayFormatter(IDisplay display, int width = 150, int height = 50)
        {
            _display = display;

            _width = width;
            Height = height;
        }

        public void SetSize(int width, int height)
        {
            _width = width;
            Height = height;
            _display.BuildFrame(_width, Height);
        }

        public void SetWidth(int width)
        {
            _width = width;
            _display.BuildFrame(_width, Height);
        }

        public void SetHeight(int height)
        {
            Height = height;
            _display.BuildFrame(_width, Height); 
        }
    }
}