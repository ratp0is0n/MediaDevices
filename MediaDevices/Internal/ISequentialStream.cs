using System;
using System.Runtime.InteropServices;

namespace MediaDevices.Internal
{
    [System.Runtime.InteropServices.Guid("0C733A30-2A1C-11CE-ADE5-00AA0044773D")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISequentialStream
    {
        void RemoteRead(
            [Out] out byte pv, 
            [In] uint cb, 
            [Out] out uint pcbRead);

        void RemoteWrite(
            [In] ref byte pv, 
            [In] uint cb, 
            [Out] out uint pcbWritten);
    }
}
