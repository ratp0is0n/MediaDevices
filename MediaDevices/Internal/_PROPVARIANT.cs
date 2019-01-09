using System.Runtime.InteropServices;

namespace MediaDevices.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROPVARIANT
    {
        public ushort vt;
        public byte wReserved1;
        public byte wReserved2;
        public uint wReserved3;
        public PROPVARIANT_INNER inner;
    }
}
