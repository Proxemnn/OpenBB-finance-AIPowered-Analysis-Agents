using System.Drawing;
using System.Runtime.InteropServices;


namespace Services.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public uint X;
        public uint Y;

        public static implicit operator Point(POINT point)
        {
            return new Point((int)point.X, (int)point.Y);
        }
    }
}
